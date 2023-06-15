﻿using System;
using System.Collections.Generic;
using MW.Extensions;
using MV = MW.MVector;
using MR = MW.MRotator;
using UnityEngine;
using V3 = UnityEngine.Vector3;

namespace MW.Console
{
	internal delegate bool SupportedExecTypeFunction(object[] RawParams, ref int ParamIndex, ref object TargetObject, Type ExecParameterType);
	delegate GameObject FindTargetFunction(string Target);
	delegate void ErrorFunction(string ErrorMessage, MV Colour);

	struct MConsoleSettings
	{
		public char TargetGameObjectIdentifier;
		public char GameObjectByNameIdentifier;
		public char ArrayDeclaration;
		public char ArrayTermination;

		public FindTargetFunction GetTargetFromString;
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
			SupportedTypes.Add(typeof(V3), Vector3);
			SupportedTypes.Add(typeof(MR), MRotator);
			SupportedTypes.Add(typeof(GameObject), GameObjectOrTransform);
			SupportedTypes.Add(typeof(Transform), GameObjectOrTransform);
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

		internal bool String(object[] RawParams, ref int ParamIndex, ref object TargetObject, Type ExecParameterType)
		{
			TargetObject = RawParams[ParamIndex].Cast<string>();

			++ParamIndex;
			return true;
		}

		internal bool Primitive(object[] RawParams, ref int ParamIndex, ref object TargetObject, Type ExecParameterType)
		{
			MConsole.HandlePrimitiveParameter(ref TargetObject, RawParams[ParamIndex], ExecParameterType);

			++ParamIndex;
			return true;
		}

		internal bool Array(object[] RawParams, ref int ParamIndex, ref object TargetObject, Type ExecParameterType)
		{
			if (RawParams[ParamIndex].Cast<string>()[0] != Settings.ArrayDeclaration)
			{
				ThrowError($"To parse arrays into Exec functions, they must be wrapped with {Settings.ArrayDeclaration} and {Settings.ArrayTermination}");
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
	}
}