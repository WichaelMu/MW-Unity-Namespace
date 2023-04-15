using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using MW.Extensions;
using MW.Conversion;
using UnityEngine;
using UE = UnityEngine;
using UObject = UnityEngine.Object;

namespace MW.Console
{
	/// <summary>A Console Debugger for debugging games during runtime.</summary>
	/// <decorations decor="public abstract class : MonoBehaviour"></decorations>
	public abstract class MConsole : MonoBehaviour
	{
		/// <summary>The <see cref="Type"/>to get the <see cref="Assembly"/> of the Unity Game where <see cref="ExecAttribute"/>s are defined.</summary>
		/// <docs>The Type to get the Assembly of the Unity Game where ExecAttributes are defined.</docs>
		/// <decorations decor="public abstract Type[]"></decorations>
		public abstract Type[] ExecTypes { get; }
		/// <summary>The <see cref="KeyCode"/> to show <see cref="OnGUI"/>.</summary>
		/// <docs>The KeyCode to show the Console GUI.</docs>
		/// <decorations decor="public virtual KeyCode"></decorations>
		public virtual KeyCode ShowConsoleKey { get; set; } = KeyCode.BackQuote;

		/// <summary>Exec attributes and their reflected functions.</summary>
		/// <decorations decor="protected Dictionary&lt; string, MethodExec&lt; MethodInfo, ExecAttribute &gt;&gt;"></decorations>
		protected Dictionary<string, MethodExec<MethodInfo, ExecAttribute>> Funcs;

		/// <summary>True to show the MConsole GUI.</summary>
		/// <decorations decor="protected bool"></decorations>
		protected bool bShowConsole = false;
		/// <summary>The raw string input given by the developer using MConsole's GUI.</summary>
		/// <decorations decor="protected string"></decorations>
		protected string RawInput;
		/// <summary>The previous Exec'd functions called by MConsole.</summary>
		/// <decorations decor="protected MArray&lt;string&gt;"></decorations>
		protected MArray<string> PreviousInputs;
		int InputsIndex = 0;

		protected const char kTargetGameObjectIdentifier = '@';
		protected const char kGameObjectByNameIdentifier = '#';
		protected const char kArrayDeclaration = '{';
		protected const char kArrayTermination = '}';

		StringBuilder OutputLog;

		bool bShowBuiltIn = true;

		/// <summary>Finds and constructs the Console and its ExecAttributes.</summary>
		/// <decorations decor="public virtual void"></decorations>
		public virtual void Awake()
		{
			PreviousInputs = new MArray<string>();
			OutputLog = new StringBuilder();
			WriteDefaultMessage();

			Funcs = new Dictionary<string, MethodExec<MethodInfo, ExecAttribute>>();
			BindingFlags MethodFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;

			MArray<Type> Types = new MArray<Type>(ExecTypes);
			Types.Push(typeof(BuiltInExecFunctions));

			foreach (Type T in Types)
			{
				IEnumerable<MethodInfo> Methods = T.Assembly.GetTypes()
					.SelectMany(Type => Type.GetMethods(MethodFlags));

				foreach (MethodInfo Method in Methods)
				{
					ExecAttribute Command = (ExecAttribute)Attribute.GetCustomAttribute(Method, typeof(ExecAttribute));

					if (Command == null)
						continue;

					if (Funcs.ContainsKey(Method.Name))
						MConsoleErrorHander.NotifyDuplicateFunction(Method, Funcs[Method.Name].Method);

					Funcs.Add(Method.Name, new MethodExec<MethodInfo, ExecAttribute>(Method, Command));

					if (Command.bExecOnAwake)
						Exec(Command.GameObjectTargetsByName, Method.Name, Command.ExecParams);
				}
			}
		}

		/// <summary>Shows the Console on the in-game viewport.</summary>
		/// <decorations decor="public virtual void"></decorations>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual void ShowConsole()
		{
			bShowConsole = !bShowConsole;
			RawInput = "";
		}

		void BuildExec()
		{
			if (!string.IsNullOrEmpty(RawInput))
			{
				string[] Split = RawInput.Split(' ');
				int ArgC = Split.Length;
				MArray<object> ArgV = new MArray<object>();
				MArray<string> TargetsArgV = new MArray<string>();
				string Func = Split[0];

				for (int Arg = 1; Arg < ArgC; ++Arg)
				{
					if (string.IsNullOrEmpty(Split[Arg]))
						continue;

					if (Split[Arg][0] == kTargetGameObjectIdentifier)
					{
						TargetsArgV.Push(Split[Arg].Substring(1));
					}
					else
					{
						ArgV.Push(Split[Arg]);
					}
				}

				Exec(TargetsArgV, Func, ArgV);

				InputsIndex = PreviousInputs.Num;
				PreviousInputs.Push(RawInput);
			}

			RawInput = "";
		}

		/// <summary>Executes <paramref name="MethodName"/> with <paramref name="RawParams"/>.</summary>
		/// <docs>Executes MethodName with RawParams on Targets (if any).</docs>
		/// <remarks>If no Targets are specified, MethodName will execute on all GameObjects of its declaring type.</remarks>
		/// <decorations decor="public void"></decorations>
		/// <param name="Targets">The names of GameObjects to execute MethodName on. If null or empty, Object.FindObjectOfType will be used instead.</param>
		/// <param name="MethodName">The name of the method to execute. (This is case-sensitive)</param>
		/// <param name="RawParams">The parameters to pass to the method.</param>
		public void Exec(string[] Targets, string MethodName, params object[] RawParams)
		{
			if (CheckInternallyDefinedCommands(MethodName, RawParams))
				return;

			if (!Funcs.ContainsKey(MethodName))
			{
				WriteToOutput($"-- Unknown Exec Function -- '{MethodName}'", MConsoleColourLibrary.Red);
				SuggestExecFunctions(MethodName);
				return;
			}

			MethodExec<MethodInfo, ExecAttribute> Func = Funcs[MethodName];

			ExecAttribute Exec = Func.Exec;
			if (Exec.bRequireTarget && Targets.Length == 0)
			{
				WriteToOutput($"-- Target Required -- Exec '{MethodName}' requires a target to be Executed.\n" +
					$"Prefix Targets with '{kTargetGameObjectIdentifier}' before their name in the Editor hierarchy.", MConsoleColourLibrary.Red);
			}

			MethodInfo Method = Func.Method;
			Type DeclaringType = Method.DeclaringType;

			ParameterInfo[] MethodParams = Method.GetParameters();
			int RawParamsArgC = RawParams.Length;

			bool bContainsArray = false;
			foreach (ParameterInfo Param in MethodParams)
			{
				Type ParamType = Param.ParameterType;
				if (ParamType.IsArray || ParamType == typeof(MVector) || ParamType == typeof(Vector3) || ParamType == typeof(MRotator))
				{
					bContainsArray = true;
					break;
				}
			}

			if (!bContainsArray && MethodParams.Length != RawParamsArgC)
			{
				WriteToOutput($"-- Parameter Mismatch -- Exec: '{MethodName}' requires {MethodParams.Length} parameter{(MethodParams.Length == 1 ? "" : "s")}, " +
					$"but {RawParamsArgC} {(RawParamsArgC == 1 ? "was" : "were")} given.", MConsoleColourLibrary.Yellow);
				return;
			}

			object[] ExecParameters = new object[MethodParams.Length];

			// Convert to correct Parameter types declared by the Exec function.
			for (int RawParamIndex = 0, ExecParamIndex = 0; RawParamIndex < RawParamsArgC; ++ExecParamIndex)
				if (!GetCustomParameterType(RawParams, ref RawParamIndex, ref ExecParameters[ExecParamIndex], MethodParams[ExecParamIndex].ParameterType))
					return;

			// Remove Array Declaration and Terminations from being passed in and executed.
			MArray<object> StrippedArray = new MArray<object>(ExecParameters);
			StrippedArray.PullAll(kArrayDeclaration);
			StrippedArray.PullAll(kArrayTermination);
			ExecParameters = StrippedArray.TArray();

			try
			{
				if (Func.Method.IsStatic)
				{
					Method.Invoke(null, ExecParameters);
				}
				else
				{
					// If we have targets, invoke MethodName on their respective components.
					if (Targets != null && Targets.Length != 0)
					{
						StringBuilder ExecTargets = new StringBuilder();
						ExecTargets.Append($"{MethodName} was Executed with ({new MArray<object>(ExecParameters).Print(Separator: ", ")}) on: ");

						foreach (string Target in Targets)
						{
							if (string.IsNullOrEmpty(Target))
								break;

							if (GetTargetFromString(Target, out GameObject TargetObject))
							{
								Func.Method.Invoke(TargetObject.GetComponent(DeclaringType), ExecParameters);
								ExecTargets.Append($"'{Target}' ");
							}
						}

						WriteToOutput(ExecTargets);
					}
					// Otherwise, we FindObjectsOfType.
					else
					{
						UObject[] ObjectTargets = FindObjectsOfType(DeclaringType);
						int Length = ObjectTargets.Length;

						StringBuilder AllObjectsOfType = new StringBuilder();
						AllObjectsOfType.Append($"{Method.Name} () was executed on {Length} GameObject{(Length == 1 ? "" : "s")}:");

						for (int i = 0; i < Length; ++i)
						{
							UObject ObjectOfType = ObjectTargets[i];
							ExecuteOnTarget(ObjectOfType, Method, DeclaringType, ExecParameters);
							AllObjectsOfType.Append($"'{ObjectOfType.name}' ");
						}

						WriteToOutput(AllObjectsOfType);
					}
				}

				ScrollOutputLog.y = GetOutputLogHeight(out _);
			}
			catch (Exception E)
			{
				StringBuilder ErrorBuilder = new StringBuilder();

				for (int i = 0; i < RawParams.Length; ++i)
				{
					if (RawParams[i] == null)
						continue;

					ErrorBuilder.Append(RawParams[i].ToString().TrimStart(kGameObjectByNameIdentifier, kTargetGameObjectIdentifier));
					if (i != RawParams.Length - 1)
						ErrorBuilder.Append(", ");
				}

				WriteToOutput($"Failed to execute {MethodName} ({ErrorBuilder}) - {E.Message}", MConsoleColourLibrary.Red);
			}
		}

		static void ExecuteOnTarget(UObject ObjectTarget, MethodInfo Method, Type DeclaringType, object[] ExecParameters)
		{
			Method.Invoke(ObjectTarget.Cast(DeclaringType), ExecParameters);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		GameObject GetTargetFromString(string Target)
		{
			Target = Target.Trim();

			if (string.IsNullOrEmpty(Target))
			{
				WriteToOutput($"Target is empty!", MConsoleColourLibrary.Yellow);
				return null;
			}

			return GameObject.Find(Target);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected bool GetTargetFromString(string Target, out GameObject TargetObject)
		{
			TargetObject = GetTargetFromString(Target);
			if (!TargetObject)
				WriteToOutput($"Could not find Target named: '{Target}'.", MConsoleColourLibrary.Yellow);

			return TargetObject;
		}

		bool GetCustomParameterType(object[] RawParams, ref int ParamIndex, ref object TargetObject, Type ExecParameterType)
		{
			if (ParamIndex < 0 || ParamIndex >= RawParams.Length)
			{
				WriteToOutput($"Parameter Index is out of range! Expected 0 <= {nameof(ParamIndex)} ({ParamIndex}) < {nameof(RawParams)}.Length ({RawParams.Length})!", MConsoleColourLibrary.Red);
				return false;
			}

			// Test against known types.
			if (ExecParameterType == typeof(MVector)) // MVector.
			{
				if (ParamIndex + 2 < RawParams.Length)
				{
					MVector RetVal;
					RetVal.X = RawParams[ParamIndex++].Cast<float>();
					RetVal.Y = RawParams[ParamIndex++].Cast<float>();
					RetVal.Z = RawParams[ParamIndex].Cast<float>();

					TargetObject = RetVal;
				}
				else
				{
					WriteToOutput($"{nameof(MVector)} requires 3 float parameters.", MConsoleColourLibrary.Red);
					return false;
				}
			}
			else if (ExecParameterType == typeof(Vector3)) // Vector3.
			{
				if (ParamIndex + 2 < RawParams.Length)
				{
					Vector3 RetVal;
					RetVal.x = RawParams[ParamIndex++].Cast<float>();
					RetVal.y = RawParams[ParamIndex++].Cast<float>();
					RetVal.z = RawParams[ParamIndex].Cast<float>();

					TargetObject = RetVal;
				}
				else
				{
					WriteToOutput($"{nameof(Vector3)} requires 3 float parameters.", MConsoleColourLibrary.Red);
					return false;
				}
			}
			else if (ExecParameterType == typeof(MRotator)) // MRotator.
			{
				if (ParamIndex + 2 < RawParams.Length)
				{
					MRotator RetVal;
					RetVal.Pitch = RawParams[ParamIndex++].Cast<float>();
					RetVal.Yaw = RawParams[ParamIndex++].Cast<float>();
					RetVal.Roll = RawParams[ParamIndex].Cast<float>();

					TargetObject = RetVal;
				}
				else
				{
					WriteToOutput($"{nameof(MRotator)} requires 3 float parameters.", MConsoleColourLibrary.Red);
					return false;
				}
			}
			else if (ExecParameterType == typeof(GameObject) || ExecParameterType == typeof(Transform)) // GameObject or Transform.
			{
				string StringValue = RawParams[ParamIndex].Cast<string>();

				if (StringValue[0] != kGameObjectByNameIdentifier)
				{
					WriteToOutput($"GameObject and Transform [Exec] Function Parameters must be prefixed with {kGameObjectByNameIdentifier}!", MConsoleColourLibrary.Red);
					return false;
				}

				string GameObjectName = StringValue.TrimStart(kGameObjectByNameIdentifier)
									.Replace("##", " ");

				GameObject FindResult = GetTargetFromString(GameObjectName);

				if (!FindResult)
				{
					WriteToOutput($"Could not find GameObject with name: '{GameObjectName}'", MConsoleColourLibrary.Red);
					return false;
				}

				if (ExecParameterType == typeof(Transform))
					TargetObject = FindResult.transform;
				else
					TargetObject = FindResult;
			}
			else if (typeof(MonoBehaviour).IsAssignableFrom(ExecParameterType) || typeof(UE.Behaviour).IsAssignableFrom(ExecParameterType)) // Components.
			{
				string StringValue = RawParams[ParamIndex].Cast<string>();

				if (StringValue.Length < 2 || StringValue[0] != kGameObjectByNameIdentifier)
				{
					WriteToOutput($"[Exec] Function Parameters referencing a {nameof(MonoBehaviour)} must be prefixed with '{kGameObjectByNameIdentifier}'!", MConsoleColourLibrary.Red);
					return false;
				}

				string ComponentTarget = StringValue.TrimStart(kGameObjectByNameIdentifier);

				if (string.IsNullOrEmpty(ComponentTarget))
				{
					WriteToOutput("The Target GameObject name is contains no identifier after a valid prefix!", MConsoleColourLibrary.Red);
					return false;
				}

				GameObject GameObjectWithComponent = GetTargetFromString(ComponentTarget);

				if (!GameObjectWithComponent)
				{
					WriteToOutput($"Could not find GameObject with name: '{ComponentTarget}'!", MConsoleColourLibrary.Red);
					return false;
				}

				TargetObject = GameObjectWithComponent.GetComponent(ExecParameterType);

				if (TargetObject == null)
				{
					WriteToOutput($"GameObject: '{GameObjectWithComponent.name}' doesn't have an attached {ExecParameterType}", MConsoleColourLibrary.Red);
					return false;
				}
			}
			else if (ExecParameterType == typeof(string)) // String.
			{
				TargetObject = RawParams[ParamIndex].Cast<string>();
			}
			else if (ExecParameterType.IsPrimitive) // Any other primitive.
			{
				HandlePrimitiveParameter(ref TargetObject, RawParams[ParamIndex], ExecParameterType);
			}
			else if (ExecParameterType.IsArray)
			{
				if (RawParams[ParamIndex].Cast<string>()[0] != kArrayDeclaration)
				{
					WriteToOutput($"To parse arrays into Exec functions, they must be wrapped with {kArrayDeclaration} and {kArrayTermination}", MConsoleColourLibrary.Red);
					return false;
				}

				ParamIndex++;
				MArray<object> Array = new MArray<object>();
				Type ElementType = ExecParameterType.GetElementType();

				do
				{
					object Element = default;
					if (!GetCustomParameterType(RawParams, ref ParamIndex, ref Element, ElementType))
						return false;

					Array.Push(Element);
				} while (RawParams[ParamIndex].Cast<string>()[0] != kArrayTermination);

				++ParamIndex;
				return MConsoleArrayParser.Convert(this, ref TargetObject, ElementType, Array);
			}
			else // Any custom type.
			{
				++ParamIndex;
				return HandleCustomParameter(RawParams, ref ParamIndex, ref TargetObject, ExecParameterType);
			}

			++ParamIndex;
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void HandlePrimitiveParameter(ref object TargetObject, object RawParameter, Type ParameterType)
			=> TargetObject = Convert.ChangeType(RawParameter, ParameterType);

		/// <summary>Handles custom parameter types for MConsole to parse and execute methods and functions.</summary>
		/// <decorations decor="public virtual void"></decorations>
		/// <remarks>
		/// Natively supported types are:<br></br>
		/// <list type="bullet">
		/// <item><see cref="MVector"/> and <see cref="Vector3"/>.</item>
		/// <item><see cref="MRotator"/>.</item>
		/// <item><see cref="GameObject"/> and <see cref="Transform"/>, given a GameObject Target by hierarchy-name reference.</item>
		/// <item>Any <see cref="MonoBehaviour"/> component, given a GameObject Target by hierarchy-name reference.</item>
		/// <item>All primitive types.</item>
		/// </list><br></br>
		/// <b>** DO NOT CALL THIS BASE METHOD IF YOU ARE OVERRIDING **</b>
		/// </remarks>
		/// <docremarks>
		/// Natively supported types are:&lt;br&gt;
		/// MVector and Vector3.&lt;br&gt;
		/// MRotator.&lt;br&gt;
		/// GameObject and Transform, given a GameObject Target by hierarchy-name reference.&lt;br&gt;
		/// Any MonoBehaviour component, given a GameObject Target by hierarchy-name reference.&lt;br&gt;
		/// All primitive types.&lt;br&gt;&lt;br&gt;
		/// &lt;span style="color:red;"&gt;Do not call the base method if you are overriding.&lt;/span&gt;
		/// </docremarks>
		/// <param name="RawParameters">The Parameters entered into as RawInput to the MConsole GUI.</param>
		/// <param name="ParamIndex">The current index of the Parameters array that requires custom parsing.</param>
		/// <param name="TargetObject">The fully parsed custom type parameter as an object.</param>
		/// <param name="ExecParameterType">The [Exec] function's required parameter type.</param>
		/// <exception cref="NotImplementedException">Occurs when MConsole does not natively support ExecParameterType.</exception>
		/// <returns>True if the Custom Parameter was properly handled.</returns>
		public virtual bool HandleCustomParameter(object[] RawParameters, ref int ParamIndex, ref object TargetObject, Type ExecParameterType)
		{
			WriteToOutput($"{nameof(MConsole)} does not natively support type: {ExecParameterType}. Override {nameof(HandleCustomParameter)}, and do not call base, to support it.", MConsoleColourLibrary.Yellow);
			return false;
		}

		public virtual bool HandleCustomArrayType(MConsole Console, ref object TargetObject, Type ElementType, MArray<object> Elements)
		{
			WriteToOutput($"{nameof(MConsole)} does not natively support the element type: {ElementType}. Override {nameof(HandleCustomArrayType)}, and do not call base, to support it.", MConsoleColourLibrary.Yellow);

			StringBuilder CodeSnippet = new StringBuilder();
			CodeSnippet.AppendLine()
				.Append('\t').Append("MArray<T> Array = new MArray<T>();").AppendLine()
				.Append("\t\t").Append("foreach (object Element in Elements)").AppendLine()
				.Append("\t\t\t").Append("Array.Push(Element.Cast<T>());").AppendLine()
				.Append('\t').Append("return Array.TArray();").AppendLine();

			WriteToOutput($"To support arrays of custom types, follow and adapt the following example: {CodeSnippet}");
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual void WriteToOutput(string Output)
		{
			OutputLog.Append(Output).Append('\n');
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual void WriteToOutput(StringBuilder Output)
		{
			OutputLog.Append(Output).Append('\n');
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void WriteToOutput(string Output, MVector Colour)
		{
			OutputLog.Append(Colour).Append('\\');
			WriteToOutput(Output);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void WriteToOutput(StringBuilder Output, MVector Colour)
		{
			OutputLog.Append(Colour).Append('\\');
			WriteToOutput(Output);
		}

		const float kConsoleFontHeight = 20f;
		float GetOutputLogHeight(out string[] Output)
		{
			Output = OutputLog.ToString().Split('\n');
			return kConsoleFontHeight * Output.Length + 10;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void WriteDefaultMessage()
		{
			WriteToOutput("-- MW Unity Namespace - MConsole --", MConsoleColourLibrary.Green);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void WriteHelpMessage()
		{
			WriteToOutput("-- Help --", MConsoleColourLibrary.LimeGreen);
			WriteToOutput($"\t{nameof(MConsole)} is a developer tool for debugging and arbitrary code execution during runtime.", MConsoleColourLibrary.LimeGreen);
			WriteToOutput($"\tAbove are a list of [Exec] functions that you can execute at will, with most supported parameter types in Unity and the MW Namespace.", MConsoleColourLibrary.LimeGreen);
			WriteToOutput($"\tSome [Exec] functions are 'Built-In' and can be hidden by executing '__TOGGLE_BUILTIN__' in the text area.", MConsoleColourLibrary.LimeGreen);
			WriteToOutput($"\tTo add your own functions here, simply add 'using MW.Console;' and mark your methods and functions with the [Exec] attribute.", MConsoleColourLibrary.LimeGreen);
			WriteToOutput($"\t\tOnly public, static, and instance functions are included. Private [Exec] functions are ignored.", MConsoleColourLibrary.Yellow);
			WriteToOutput("");
			WriteToOutput($"\tThere are also a few functions that are 'Internal'.", MConsoleColourLibrary.LimeGreen);
			WriteToOutput($"\t__CLEAR__ - Clears the output.", MConsoleColourLibrary.LimeGreen);
			WriteToOutput($"\t__SET_RATIO__ - Sets the ratio for the Console. It accepts values between .15 to .85 as a percentage of your screen's height. Default is {kDefaultConsoleRatio}.", MConsoleColourLibrary.LimeGreen);
			WriteToOutput($"\t__TOGGLE_BUILTIN__ - Shows and hides Built-In [Exec] Functions. They can still be executed regardless of being hidden.", MConsoleColourLibrary.LimeGreen);
			WriteToOutput($"\t__HELP__, ?, -h, and --help - Shows this help message.", MConsoleColourLibrary.LimeGreen);
			WriteToOutput("");
			WriteToOutput($"\tWhen making a game, you will eventually need to make your own types and aren't natively supported by {nameof(MConsole)}.", MConsoleColourLibrary.LimeGreen);
			WriteToOutput($"\tCustom parameters can be handled by overriding certain functions, such as {nameof(HandleCustomParameter)} for function parameters or {nameof(HandleCustomArrayType)} for arrays.", MConsoleColourLibrary.LimeGreen);
			WriteToOutput("");
		}

		protected virtual string GetPersistentOutput()
			=> $"FPS: {Utils.FPS():D3} | Delta Time: {Time.deltaTime:F3} | Exec __HELP__ for Help |";

		bool CheckInternallyDefinedCommands(string Command, object[] Parameters)
		{
			switch (Command)
			{
				case "__CLEAR__":
					OutputLog.Clear();
					WriteDefaultMessage();
					break;
				case "__SET_RATIO__":
					if (Parameters != null && Parameters.Length != 0)
					{
						if (Parameters[0].Is(out float InRatio))
						{
							Utils.Clamp(ref InRatio, .15f, .85f);
							ConsoleRatio = InRatio;
						}
						else
						{
							WriteToOutput($"__SET_RATIO__ invoked, expected a float.", MConsoleColourLibrary.Red);
							ConsoleRatio = kDefaultConsoleRatio;
						}
					}
					else
					{
						ConsoleRatio = kDefaultConsoleRatio;
					}
					break;
				case "__TOGGLE_BUILTIN__":
					bShowBuiltIn = !bShowBuiltIn;
					break;
				case "__HELP__":
				case "?":
				case "-h":
				case "--help":
					WriteHelpMessage();
					break;
				default:
					return false;
			}

			return true;
		}

		void SuggestExecFunctions(string AttemptedExecName)
		{
			StringBuilder SimilarExecs = new StringBuilder();

			foreach (KeyValuePair<string, MethodExec<MethodInfo, ExecAttribute>> Exec in Funcs)
				if (Utils.Compare(AttemptedExecName, Exec.Key) > .8f)
					SimilarExecs.Append('\t').Append(Exec.Key).Append('\n');

			if (SimilarExecs.Length > 0)
			{
				WriteToOutput($"\tWe found some similar Exec functions.", MConsoleColourLibrary.Purple);
				WriteToOutput(SimilarExecs, MConsoleColourLibrary.Purple);
			}
		}

		const float kDefaultConsoleRatio = .75f;
		float ConsoleRatio = kDefaultConsoleRatio;
		Vector2 Scroll;
		Vector2 ScrollOutputLog;
		float t = 0f;
		bool bHelpMessageHasBeenShown = false;

		/// <summary>Draws the Console to the in-game viewport.</summary>
		/// <decorations decor="public virtual void"></decorations>
		public virtual void OnGUI()
		{
			if (!bShowConsole)
			{
				InputsIndex = PreviousInputs.Num - 1;
				t = 0f;
				return;
			}

			if (string.IsNullOrEmpty(RawInput))
				t += Time.deltaTime;

			if (t > 5f && !bHelpMessageHasBeenShown)
			{
				bHelpMessageHasBeenShown = true;
				WriteHelpMessage();
			}

			if (Event.current.Equals(Event.KeyboardEvent("Return")))
			{
				BuildExec();
				return;
			}

			if (Event.current.Equals(Event.KeyboardEvent(ShowConsoleKey.ToString())))
			{
				bShowConsole = false;
				return;
			}

			if (!PreviousInputs.IsEmpty())
			{
				if (Event.current.Equals(Event.KeyboardEvent("Up"))) // Going to earliest-entered.
				{
					if (!string.IsNullOrEmpty(RawInput))
						InputsIndex--;
					Utils.ClampMin(ref InputsIndex, 0);
					RawInput = PreviousInputs[InputsIndex];
				}
				else if (Event.current.Equals(Event.KeyboardEvent("Down"))) // Going to latest-entered.
				{
					if (!string.IsNullOrEmpty(RawInput))
						InputsIndex++;
					Utils.ClampMax(ref InputsIndex, PreviousInputs.Num - 1);
					RawInput = PreviousInputs[InputsIndex];
				}
			}

			float Y = 0f;

			if (Funcs.Count > 0)
			{
				float FuncsHeight = Screen.height * ConsoleRatio;

				GUI.Box(new Rect(0, Y, Screen.width, FuncsHeight), "");
				Rect ExecList = new Rect(0, 0, Screen.width - 30, 20 * Funcs.Count);

				Scroll = GUI.BeginScrollView(new Rect(0, Y + 5, Screen.width, FuncsHeight), Scroll, ExecList);

				int i = 0;
#pragma warning disable UNT0018 // System.Reflection features in performance critical messages. MethodInfo is already cached in Funcs by Awake().
				foreach (KeyValuePair<string, MethodExec<MethodInfo, ExecAttribute>> Func in Funcs)
				{
					if (Func.Value.Exec.bHideInConsole)
						continue;

					if (Func.Value.Exec.bIsBuiltIn)
					{
						if (!bShowBuiltIn)
							continue;

						GUI.contentColor = MConsoleColourLibrary.BuiltIn;
					}
					else
					{
						GUI.contentColor = MConsoleColourLibrary.White;
					}

					StringBuilder ParamsBuilder = new StringBuilder();
					foreach (ParameterInfo Param in Func.Value.Method.GetParameters())
					{
						string[] ParamSplit = Param.ParameterType.Name.Split('.');
						ParamsBuilder.Append(ParamSplit[ParamSplit.Length - 1]).Append(' ').Append(Param.Name).Append(", ");
					}

					string Text = $"{Func.Value.Method.Name} ({ParamsBuilder.ToString().TrimEnd(',', ' ')}) - {Func.Value.Exec.Description}";

					Rect TextRect = new Rect(5, 20 * i++, ExecList.width - 100, kConsoleFontHeight);


					GUI.Label(TextRect, Text);
				}
#pragma warning restore UNT0018 // System.Reflection features in performance critical messages. MethodInfo is already cached in Funcs by Awake().

				Y += FuncsHeight;

				GUI.EndScrollView();

				float OutputLogHeight = Screen.height * (1f - ConsoleRatio);
				float OutputScrollHeight = OutputLogHeight - 33;

				GUI.Box(new Rect(0, Y + kConsoleFontHeight, Screen.width, OutputLogHeight), "");
				Rect OutputLogList = new Rect(0, 0, Screen.width - 30, GetOutputLogHeight(out string[] Output));
				ScrollOutputLog = GUI.BeginScrollView(new Rect(0, Y + 25, Screen.width, OutputScrollHeight), ScrollOutputLog, OutputLogList);

				i = 0;
				for (; i < Output.Length; i++)
				{
					Rect OutputTextRect = new Rect(5, 20 * i, OutputLogList.width - 100, kConsoleFontHeight);
					string StringValue = Output[i];
					string[] ColourSeparator = StringValue.Split('\\');

					if (ColourSeparator.Length > 1 && MVector.TryParse(ColourSeparator[0], out MVector OutputColour))
					{
						GUI.contentColor = OutputColour;
						GUI.Label(OutputTextRect, ColourSeparator[1]);
					}
					else
					{
						GUI.contentColor = MConsoleColourLibrary.White;
						GUI.Label(OutputTextRect, StringValue);
					}
				}

				Rect PersistentRect = new Rect(5, 20 * (i - 1), OutputLogList.width - 100, kConsoleFontHeight);
				GUI.contentColor = MConsoleColourLibrary.Green;
				GUI.Label(PersistentRect, GetPersistentOutput());

				GUI.EndScrollView();
			}

			GUI.backgroundColor = GUI.contentColor = GUI.color = MConsoleColourLibrary.Purple;

			GUI.SetNextControlName("Exec Text Field");
			RawInput = GUI.TextField(new Rect(2.5f, Y, Screen.width, kConsoleFontHeight), RawInput);
			GUI.FocusControl("Exec Text Field");
		}

		class MConsoleArrayParser
		{
			internal static bool Convert(MConsole Console, ref object TargetObject, Type ElementType, MArray<object> Elements)
			{
				bool bSuccessfulConversion = true;

				if (ElementType == typeof(MVector))
				{
					TargetObject = Make<MVector>(Elements);
				}
				else if (ElementType == typeof(Vector3))
				{
					TargetObject = Make<Vector3>(Elements);
				}
				else if (ElementType == typeof(MRotator))
				{
					TargetObject = Make<MRotator>(Elements);
				}
				else if (ElementType == typeof(GameObject))
				{
					TargetObject = Make<GameObject>(Elements);
				}
				else if (ElementType == typeof(Transform))
				{
					TargetObject = Make<Transform>(Elements);
				}
				else if (ElementType == typeof(MonoBehaviour))
				{
					TargetObject = Make<MonoBehaviour>(Elements);
				}
				else if (ElementType == typeof(UE.Behaviour))
				{
					TargetObject = Make<UE.Behaviour>(Elements);
				}
				else if (ElementType == typeof(string))
				{
					TargetObject = Make<string>(Elements);
				}
				else if (ElementType.IsPrimitive)
				{
					TargetObject = Elements.TArray();
				}
				else
				{
					bSuccessfulConversion = Console.HandleCustomArrayType(Console, ref TargetObject, ElementType, Elements);

					if (TargetObject == null)
						Console.WriteToOutput($"The type: '{ElementType.Name}' cannot be parsed into an Array.", MConsoleColourLibrary.Red);
				}

				return bSuccessfulConversion;
			}

			static T[] Make<T>(MArray<object> Elements)
			{
				MArray<T> Array = new MArray<T>();
				foreach (object Element in Elements)
					Array.Push(Element.Cast<T>());
				return Array.TArray();
			}
		}
	}

	internal class MConsoleErrorHander
	{
		internal static void NotifyDuplicateFunction(MethodInfo Method, MethodInfo Existing)
		{
			throw new ArgumentException($"An [Exec] Function named: '{Method.Name}' already exists! '{nameof(MConsole)} does not support method overloading." +
				$"Or, you may be trying to add assemblies with duplicate function names into {nameof(MConsole.ExecTypes)}.\n\t" +
				$"First appearance was in the assembly: '{Existing.GetType().Assembly}', this appearance is in the assembly: '{Method.GetType().Assembly}'.");
		}
	}

	internal class MConsoleColourLibrary
	{
		static Color red = Colour.ColourHex("#FF4444");
		static Color yel = Colour.ColourHex("#FFD344");
		static Color gre = Colour.ColourHex("#95FF44");
		static Color lim = Colour.ColourHex("#26F04B");
		static Color pur = Colour.ColourHex("#BEB7FF");

		static Color bla = Colour.ColourHex("#444444");
		static Color whi = Colour.ColourHex("#FFFFFF");

		static Color bti = Colour.ColourHex("#44FF44");

		internal static Color Red => red;
		internal static Color Yellow => yel;
		internal static Color Green => gre;
		internal static Color LimeGreen => lim;
		internal static Color Purple => pur;

		internal static Color Black => bla;
		internal static Color White => whi;

		internal static Color BuiltIn = bti;
	}
}
