using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using MW.Diagnostics;
using MW.Extensions;
using UnityEngine;
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
		/// <summary>The previous Exec'd function called by MConsole.</summary>
		/// <decorations decor="protected string"></decorations>
		protected string PreviousInput;

		const char kTargetGameObjectIdentifier = '@';
		const char kGameObjectByNameIdentifier = '#';

		/// <summary>Finds and constructs the Console and its ExecAttributes.</summary>
		/// <decorations decor="public virtual void"></decorations>
		public virtual void Awake()
		{
			Funcs = new Dictionary<string, MethodExec<MethodInfo, ExecAttribute>>();
			BindingFlags MethodFlags = BindingFlags.Public | BindingFlags.Static;

			MArray<Type> Types = new MArray<Type>(ExecTypes);
			Types.Push(typeof(BuiltInExecFunctions));

			foreach (Type T in ExecTypes)
			{
				IEnumerable<MethodInfo> Methods = T.Assembly.GetTypes()
					.SelectMany(Type => Type.GetMethods(MethodFlags));

				foreach (MethodInfo Method in Methods)
				{
					ExecAttribute Command = (ExecAttribute)Attribute.GetCustomAttribute(Method, typeof(ExecAttribute));

					if (Command == null)
						continue;

					Funcs.Add(Method.Name, new MethodExec<MethodInfo, ExecAttribute>(Method, Command));

					if (Command.bExecOnAwake)
					{
						Exec(Command.GameObjectTargetsByName, Method.Name, Command.ExecParams);
					}

					if (Command.bHideInConsole)
					{
						Funcs.Remove(Method.Name);
					}
				}
			}
		}

		/// <summary>Shows the Console on the in-game viewport.</summary>
		/// <decorations decor="public virtual void"></decorations>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual void ShowConsole()
		{
			bShowConsole = !bShowConsole;
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

				PreviousInput = RawInput;

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
			if (Funcs.ContainsKey(MethodName))
			{
				try
				{
					MethodExec<MethodInfo, ExecAttribute> Func = Funcs[MethodName];
					MethodInfo Method = Func.Method;
					Type DeclaringType = Method.DeclaringType;


					ParameterInfo[] MethodParams = Method.GetParameters();
					object[] ExecParameters = new object[MethodParams.Length];

					// Convert to correct Parameter types declared by the Exec function.
					for (int RawParamIndex = 0, ExecParamIndex = 0; RawParamIndex < RawParams.Length; ++ExecParamIndex)
						GetCustomParameterType(RawParams, ref RawParamIndex, ref ExecParameters[ExecParamIndex], MethodParams[ExecParamIndex].ParameterType);

					if (Func.Method.IsStatic)
					{
						Method.Invoke(null, ExecParameters);
					}
					else
					{
						// If we have targets, invoke MethodName on their respective components.
						if (Targets != null && Targets.Length != 0)
						{
							foreach (string Target in Targets)
							{
								if (string.IsNullOrEmpty(Target))
									break;

								Func.Method.Invoke(GetTargetFromString(Target).GetComponent(DeclaringType), ExecParameters);
							}
						}
						// Otherwise, we FindObjectsOfType.
						else
						{
							UObject[] ObjectTargets = FindObjectsOfType(DeclaringType);
							int Length = ObjectTargets.Length;

							StringBuilder AllObjectsOfType = new StringBuilder();
							AllObjectsOfType.Append($"{Method.Name} () was executed on {Length} GameObject{(Length == 1 ? "" : "s")}:").AppendLine();

							for (int i = 0; i < Length; ++i)
							{
								UObject ObjectOfType = ObjectTargets[i];
								ExecuteOnTarget(ObjectOfType, Method, DeclaringType, ExecParameters);
								AllObjectsOfType.Append($"{ObjectOfType.name}").AppendLine();
							}
						}
					}
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

					Log.E($"Failed to execute {MethodName} ({ErrorBuilder}) - {E.Message}\n{E}");
				}
			}
			else
			{
				Debug.LogError($"Unknown Command: {MethodName}");
			}
		}

		static void ExecuteOnTarget(UObject ObjectTarget, MethodInfo Method, Type DeclaringType, object[] ExecParameters)
		{
			Method.Invoke(Convert.ChangeType(ObjectTarget, DeclaringType), ExecParameters);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		GameObject GetTargetFromString(string Target)
		{
			Target = Target.Trim();

			if (string.IsNullOrEmpty(Target))
				return null;

			return GameObject.Find(Target);
		}

		void GetCustomParameterType(object[] RawParams, ref int ParamIndex, ref object TargetObject, Type ExecParameterType)
		{
			if (ParamIndex < 0 || ParamIndex >= RawParams.Length)
				throw new ArgumentOutOfRangeException($"Parameter Index is out of range! Expected 0 <= {nameof(ParamIndex)} ({ParamIndex}) < {nameof(RawParams)}.Length ({RawParams.Length})!");

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
			}
			else if (ExecParameterType == typeof(GameObject) || ExecParameterType == typeof(Transform)) // GameObject or Transform.
			{
				string StringValue = RawParams[ParamIndex].Cast<string>();

				if (StringValue[0] != kGameObjectByNameIdentifier)
					throw new ArgumentException($"GameObject and Transform [Exec] Function Parameters must be prefixed with {kGameObjectByNameIdentifier}!");

				string GameObjectName = StringValue.TrimStart(kGameObjectByNameIdentifier);

				GameObject FindResult = GetTargetFromString(GameObjectName);

				if (!FindResult)
					throw new NullReferenceException($"Could not find GameObject with name: '{GameObjectName}'");

				if (ExecParameterType == typeof(Transform))
					TargetObject = FindResult.transform;
				else
					TargetObject = FindResult;
			}
			else if (typeof(MonoBehaviour).IsAssignableFrom(ExecParameterType)) // Components.
			{
				string StringValue = RawParams[ParamIndex].Cast<string>();

				if (StringValue.Length < 2 || StringValue[0] != kGameObjectByNameIdentifier)
					throw new ArgumentException($"[Exec] Function Parameters referencing a {nameof(MonoBehaviour)} must be prefixed with '{kGameObjectByNameIdentifier}'!");

				string ComponentTarget = StringValue.TrimStart(kGameObjectByNameIdentifier);

				if (string.IsNullOrEmpty(ComponentTarget))
					throw new NullReferenceException("The Target GameObject name is contains no identifier after a valid prefix!");

				GameObject GameObjectWithComponent = GetTargetFromString(ComponentTarget);

				if (!GameObjectWithComponent)
					throw new NullReferenceException($"Could not find GameObject with name: '{ComponentTarget}'!");

				TargetObject = GameObjectWithComponent.GetComponent(ExecParameterType);

				if (TargetObject == null)
					throw new NullReferenceException($"GameObject: '{GameObjectWithComponent.name}' doesn't have an attached {ExecParameterType}");
			}
			else if (ExecParameterType.IsPrimitive)
			{
				HandlePrimitiveParameter(ref TargetObject, RawParams[ParamIndex], ExecParameterType);
			}
			else
			{
				HandleCustomParameter(RawParams, ref ParamIndex, ref TargetObject, ExecParameterType);
			}

			++ParamIndex;
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
		public virtual void HandleCustomParameter(object[] RawParameters, ref int ParamIndex, ref object TargetObject, Type ExecParameterType)
		{
			throw new NotImplementedException($"{nameof(MConsole)} does not natively support type: {ExecParameterType}. Override {nameof(HandleCustomParameter)}, and do not call base, to support it.");
		}

		Vector2 Scroll;

		/// <summary>Draws the Console to the in-game viewport.</summary>
		/// <decorations decor="public virtual void"></decorations>
		public virtual void OnGUI()
		{
			if (!bShowConsole)
			{
				return;
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

			if (Event.current.Equals(Event.KeyboardEvent("Up")))
			{
				RawInput = PreviousInput;
			}

			float Y = 0f;

			if (Funcs.Count > 0)
			{
				float FuncsHeight = Screen.height * .75f;

				GUI.Box(new Rect(0, Y, Screen.width, FuncsHeight), "");
				Rect ExecList = new Rect(0, 0, Screen.width - 30, 20 * Funcs.Count);

				GUI.backgroundColor = Color.white;
				Scroll = GUI.BeginScrollView(new Rect(0, Y + 5, Screen.width, FuncsHeight), Scroll, ExecList);

				int i = 0;
#pragma warning disable UNT0018 // System.Reflection features in performance critical messages. MethodInfo is already cached in Funcs by Start().
				foreach (KeyValuePair<string, MethodExec<MethodInfo, ExecAttribute>> Func in Funcs)
				{
					StringBuilder ParamsBuilder = new StringBuilder();
					foreach (ParameterInfo Param in Func.Value.Method.GetParameters())
					{
						string[] ParamSplit = Param.ParameterType.Name.Split('.');
						ParamsBuilder.Append(ParamSplit[ParamSplit.Length - 1]).Append(' ').Append(Param.Name).Append(", ");
					}

					string Text = $"{Func.Value.Method.Name} ({ParamsBuilder.ToString().TrimEnd(',', ' ')}) - {Func.Value.Exec.Description}";

					Rect TextRect = new Rect(5, 20 * i++, ExecList.width - 100, 20);

					GUI.Label(TextRect, Text);
				}
#pragma warning restore UNT0018 // System.Reflection features in performance critical messages. MethodInfo is already cached in Funcs by Start().

				GUI.EndScrollView();

				Y += FuncsHeight;
			}

			GUI.Box(new Rect(0, Y, Screen.width, 30), "");
			GUI.backgroundColor = Color.white;

			GUI.SetNextControlName("Exec Text Field");
			RawInput = GUI.TextField(new Rect(10f, Y + 5f, Screen.width - 20f, 20f), RawInput);
			GUI.FocusControl("Exec Text Field");
		}
	}
}
