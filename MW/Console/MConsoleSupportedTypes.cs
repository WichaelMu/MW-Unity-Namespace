using System;
using System.Collections.Generic;
using MW.Extensions;
using MV = MW.MVector;
using MR = MW.MRotator;
using System.Linq;
#if RELEASE
using System.Text;
using UnityEngine;
using V3 = UnityEngine.Vector3;
#endif

namespace MW.Console
{
	internal delegate bool SupportedExecTypeFunction(object[] RawParams, ref int ParamIndex, ref object TargetObject, Type ExecParameterType);
#if RELEASE
	delegate GameObject FindTargetFunction(string Target);
#endif // RELEASE
	delegate void ErrorFunction(string ErrorMessage,
#if RELEASE
		MV Colour
#else
		ConsoleColor Colour
#endif // RELEASE
		);

	struct MConsoleSettings
	{
		public char TargetGameObjectIdentifier;
		public char GameObjectByNameIdentifier;
		public char ArrayDeclaration;
		public char ArrayTermination;
		public char StringDeclaration;

#if RELEASE
		public FindTargetFunction GetTargetFromString;
#endif // RELEASE
		public ErrorFunction ThrowError;
		public SupportedExecTypeFunction GetCustomParameterType;

		public MConsole Console;
	}

	internal class MConsoleSupportedTypes
	{
		internal Dictionary<Type, SupportedExecTypeFunction> SupportedTypes;
		internal MConsoleSettings Settings;

		public MConsoleSupportedTypes(MConsoleSettings Settings)
		{
			SupportedTypes = new Dictionary<Type, SupportedExecTypeFunction>();
			this.Settings = Settings;

			MConsoleArrayPrimitiveTranslator.Initialise();
			MapSupportedTypes();
		}

		public bool HasDefined(Type Type, out SupportedExecTypeFunction Exec)
		{
			if (SupportedTypes.ContainsKey(Type))
			{
				Exec = SupportedTypes[Type];
				return true;
			}

			Exec = null;
			return false;
		}

		void MapSupportedTypes()
		{
			SupportedTypes.Add(typeof(MV), MVector);
			SupportedTypes.Add(typeof(MR), MRotator);
#if RELEASE
			SupportedTypes.Add(typeof(V3), Vector3);
			SupportedTypes.Add(typeof(GameObject), GameObjectOrTransform);
			SupportedTypes.Add(typeof(Transform), GameObjectOrTransform);
#endif // RELEASE
			SupportedTypes.Add(typeof(string), String);
		}

		internal void ThrowError(string Message) => Settings.ThrowError(Message, MConsoleColourLibrary.Red);

		internal bool MVector(object[] RawParams, ref int ParamIndex, ref object TargetObject, Type ExecParameterType)
		{
			if (ParamIndex + 2 < RawParams.Length)
			{
				MV RetVal;
				RetVal.X = RawParams[ParamIndex++].Cast<float>();
				RetVal.Y = RawParams[ParamIndex++].Cast<float>();
				RetVal.Z = RawParams[ParamIndex].Cast<float>();

				TargetObject = RetVal;
			}
			else
			{
				ThrowError($"{nameof(MV)} requires 3 float parameters.");
				return false;
			}

			++ParamIndex;
			return true;
		}
#if RELEASE
		internal bool Vector3(object[] RawParams, ref int ParamIndex, ref object TargetObject, Type ExecParameterType)
		{
			if (ParamIndex + 2 < RawParams.Length)
			{
				V3 RetVal;
				RetVal.x = RawParams[ParamIndex++].Cast<float>();
				RetVal.y = RawParams[ParamIndex++].Cast<float>();
				RetVal.z = RawParams[ParamIndex].Cast<float>();

				TargetObject = RetVal;
			}
			else
			{
				ThrowError($"{nameof(V3)} requires 3 float parameters.");
				return false;
			}

			++ParamIndex;
			return true;
		}
#endif // RELEASE

		internal bool MRotator(object[] RawParams, ref int ParamIndex, ref object TargetObject, Type ExecParameterType)
		{
			if (ParamIndex + 2 < RawParams.Length)
			{
				MR RetVal;
				RetVal.Pitch = RawParams[ParamIndex++].Cast<float>();
				RetVal.Yaw = RawParams[ParamIndex++].Cast<float>();
				RetVal.Roll = RawParams[ParamIndex].Cast<float>();

				TargetObject = RetVal;
			}
			else
			{
				ThrowError($"{nameof(MR)} requires 3 float parameters.");
				return false;
			}

			++ParamIndex;
			return true;
		}
#if RELEASE
		internal bool GameObjectOrTransform(object[] RawParams, ref int ParamIndex, ref object TargetObject, Type ExecParameterType)
		{
			string StringValue = RawParams[ParamIndex].Cast<string>();

			if (StringValue[0] != Settings.GameObjectByNameIdentifier)
			{
				ThrowError($"GameObject and Transform [Exec] Function Parameters must be prefixed with {Settings.GameObjectByNameIdentifier}!");
				return false;
			}

			string GameObjectName = StringValue.TrimStart(Settings.GameObjectByNameIdentifier)
								.Replace("##", " ");

			GameObject FindResult = Settings.GetTargetFromString(GameObjectName);

			if (!FindResult)
			{
				ThrowError($"Could not find GameObject with name: '{GameObjectName}'");
				return false;
			}

			if (ExecParameterType == typeof(Transform))
				TargetObject = FindResult.transform;
			else
				TargetObject = FindResult;

			++ParamIndex;
			return true;
		}

		internal bool MonoBehaviourOrComponents(object[] RawParams, ref int ParamIndex, ref object TargetObject, Type ExecParameterType)
		{
			string StringValue = RawParams[ParamIndex].Cast<string>();

			if (StringValue.Length < 2 || StringValue[0] != Settings.GameObjectByNameIdentifier)
			{
				ThrowError($"[Exec] Function Parameters referencing a {nameof(MonoBehaviour)} must be prefixed with '{Settings.GameObjectByNameIdentifier}'!");
				return false;
			}

			string ComponentTarget = StringValue.TrimStart(Settings.GameObjectByNameIdentifier);

			if (string.IsNullOrEmpty(ComponentTarget))
			{
				ThrowError("The Target GameObject name is contains no identifier after a valid prefix!");
				return false;
			}

			GameObject GameObjectWithComponent = Settings.GetTargetFromString(ComponentTarget);

			if (!GameObjectWithComponent)
			{
				ThrowError($"Could not find GameObject with name: '{ComponentTarget}'!");
				return false;
			}

			TargetObject = GameObjectWithComponent.GetComponent(ExecParameterType);

			if (TargetObject == null)
			{
				ThrowError($"GameObject: '{GameObjectWithComponent.name}' doesn't have an attached {ExecParameterType}");
				return false;
			}

			++ParamIndex;
			return true;
		}
#endif // RELEASE

		internal bool String(object[] RawParams, ref int ParamIndex, ref object TargetObject, Type ExecParameterType)
		{
			string StringValue = RawParams[ParamIndex].Cast<string>();

			TargetObject = StringValue;

			++ParamIndex;
			return true;
		}

#if RELEASE
		internal bool GetQuotedString(string StringValue, string[] Arguments, ref int ParamIndex, out string QuotedString)
		{
			if (StringValue[0] != Settings.StringDeclaration)
			{
				QuotedString = StringValue;
				return false;
			}

			int OriginalParamIndex = ParamIndex;
			QuotedString = StringValue;
			StringBuilder QuotedStringBuilder = new StringBuilder();
			if (StringValue.GetLastChar() == Settings.StringDeclaration)
			{
				QuotedString = StringValue.Substring(1, StringValue.GetLastIndex() - 1);
				return true;
			}

			QuotedStringBuilder.Append(StringValue.MakeStringStart(1));

			++ParamIndex;
			bool bFoundStringTermination = false;
			for (; ParamIndex < Arguments.Length; ++ParamIndex)
			{
				StringValue = Arguments[ParamIndex];
				if (string.IsNullOrEmpty(StringValue))
					continue;

				if (StringValue.GetLastChar() == Settings.StringDeclaration)
				{
					QuotedStringBuilder.Append(StringValue.MakeStringEnd(0));
					bFoundStringTermination = true;
					break;
				}
				else
				{
					QuotedStringBuilder.Append(' ').Append(StringValue);
				}
			}

			if (bFoundStringTermination)
			{
				QuotedString = QuotedStringBuilder.ToString();
			}
			else
			{
				ParamIndex = OriginalParamIndex;
			}

			return bFoundStringTermination;
		}

		internal bool ValidateQuotationMarks(string[] Arguments)
		{
			int QuoteCount = 0;
			foreach (string Argument in Arguments)
				QuoteCount += Argument.Count(C => C == Settings.StringDeclaration);
			return QuoteCount % 2 == 0;
		}
#endif // RELEASE

		internal static bool Primitive(object[] RawParams, ref int ParamIndex, ref object TargetObject, Type ExecParameterType)
		{
			MConsole.HandlePrimitiveParameter(ref TargetObject, RawParams[ParamIndex], ExecParameterType);

			++ParamIndex;
			return true;
		}

		internal bool Array(object[] RawParams, ref int ParamIndex, ref object TargetObject, Type ExecParameterType)
		{
			if (RawParams[ParamIndex].Cast<string>()[0] != Settings.ArrayDeclaration)
			{
				ThrowError($"To parse arrays into Exec functions, they must be wrapped with '{Settings.ArrayDeclaration}' and '{Settings.ArrayTermination}'!");
				return false;
			}

			ParamIndex++;
			MArray<object> Array = new MArray<object>();
			Type ElementType = ExecParameterType.GetElementType();

			do
			{
				object Element = default;
				if (!Settings.GetCustomParameterType(RawParams, ref ParamIndex, ref Element, ElementType))
					return false;

				Array.Push(Element);
			} while (RawParams[ParamIndex].Cast<string>()[0] != Settings.ArrayTermination);

			++ParamIndex;
			return MConsoleArrayParser.Convert(this, ref TargetObject, ElementType, Array);
		}

		internal bool ValidateArrayDeclarations(string[] Arguments)
		{
			int ArrayDeclarationCount = 0;
			int ArrayTerminationCount = 0;
			foreach (string Argument in Arguments)
			{
				ArrayDeclarationCount += Argument.Count(C => C == Settings.ArrayDeclaration);
				ArrayTerminationCount += Argument.Count(C => C == Settings.ArrayTermination);
			}

			return ArrayDeclarationCount == ArrayTerminationCount;
		}
	}

	internal class MConsoleArrayPrimitiveTranslator
	{
		delegate void PrimitiveTranslationFunc(object[] Elements, ref object TargetObject);
		static Dictionary<Type, PrimitiveTranslationFunc> PrimitiveTranslation;

		MConsoleArrayPrimitiveTranslator()
		{
			PrimitiveTranslation = new Dictionary<Type, PrimitiveTranslationFunc>
			{
				{ typeof(bool), Translate<bool> },
				{ typeof(byte), Translate<byte> },
				{ typeof(sbyte), Translate<sbyte> },
				{ typeof(short), Translate<short> },
				{ typeof(ushort), Translate<ushort> },
				{ typeof(int), Translate<int> },
				{ typeof(long), Translate<long> },
				{ typeof(ulong), Translate<ulong> },
				{ typeof(IntPtr), Translate<IntPtr> },
				{ typeof(UIntPtr), Translate<UIntPtr> },
				{ typeof(char), Translate<char> },
				{ typeof(double), Translate<double> },
				{ typeof(float), Translate<float> }
			};
		}

		internal static void Initialise()
		{
			_ = new MConsoleArrayPrimitiveTranslator();
		}

		internal static bool Translate(object[] Elements, ref object TargetObject, Type ElementType)
		{
			if (PrimitiveTranslation.ContainsKey(ElementType))
			{
				PrimitiveTranslation[ElementType](Elements, ref TargetObject);
				return true;
			}

			return false;
		}

		static void Translate<T>(object[] Elements, ref object TargetObject)
		{
			MArray<T> Translation = new MArray<T>();
			foreach (object Element in Elements)
				Translation.Push(Element.Cast<T>());

			TargetObject = Translation.TArray();
		}
	}
}
