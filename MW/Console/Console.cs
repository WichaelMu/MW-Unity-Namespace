using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using MW.IO;
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

		protected Dictionary<string, MethodExec<MethodInfo, ExecAttribute>> Funcs;

		protected bool bShowConsole = false;
		protected string RawInput;
		protected string PreviousInput;

		const char kTargetGameObjectIdentifier = '@';
		const char kGameObjectByNameIdentifier = '#';

		/// <summary>Finds and constructs the Console and its ExecAttributes.</summary>
		/// <decorations decor="public virtual void"></decorations>
		public virtual void Awake()
		{
			Funcs = new Dictionary<string, MethodExec<MethodInfo, ExecAttribute>>();
			BindingFlags MethodFlags = BindingFlags.Public | BindingFlags.Static;

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

					ParameterInfo[] MethodParams = Func.Method.GetParameters();
					object[] ExecParameters = new object[MethodParams.Length];

					// Convert to correct Parameter types declared by the Exec function.
					for (int RawParamIndex = 0, ExecParamIndex = 0; RawParamIndex < RawParams.Length; ++ExecParamIndex)
						GetCustomParameterType(RawParams, ref RawParamIndex, ref ExecParameters[ExecParamIndex], MethodParams[ExecParamIndex].ParameterType);

					if (Func.Method.IsStatic)
					{
						Func.Method.Invoke(null, ExecParameters);
					}
					else
					{
						if (Targets != null && Targets.Length != 0)
						{
							foreach (string Target in Targets)
							{
								if (string.IsNullOrEmpty(Target))
									break;

								Func.Method.Invoke(GetTargetFromString(Target).GetComponent(Func.Method.DeclaringType), ExecParameters);
							}
						}
						else
						{
							UObject ObjectTarget = FindObjectOfType(Func.Method.DeclaringType);
							if (ObjectTarget)
							{
								Func.Method.Invoke(Convert.ChangeType(ObjectTarget, Func.Method.DeclaringType), ExecParameters);
								O.Out($"Exec: {Func.Method.Name} on {ObjectTarget.name}");
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		GameObject GetTargetFromString(string Target)
		{
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

				GameObject FindResult = GameObject.Find(GameObjectName);

				if (!FindResult)
					throw new NullReferenceException($"Could not find GameObject with name: {GameObjectName}");

				if (ExecParameterType == typeof(Transform))
					TargetObject = FindResult.transform;
				else
					TargetObject = FindResult;
			}
			else if (typeof(MonoBehaviour).IsAssignableFrom(ExecParameterType)) // Components.
			{
				string StringValue = RawParams[ParamIndex].Cast<string>();

				if (StringValue.Length <= 2 || StringValue[0] != kGameObjectByNameIdentifier)
					throw new ArgumentException($"[Exec] Function Parameters referencing a {nameof(MonoBehaviour)} must be prefixed with '{kGameObjectByNameIdentifier}'!");

				string ComponentTarget = StringValue.TrimStart(kGameObjectByNameIdentifier);

				if (string.IsNullOrEmpty(ComponentTarget))
					throw new NullReferenceException("The Target GameObject name is contains no identifier after a valid prefix!");

				GameObject ComponentResult = GameObject.Find(ComponentTarget);

				if (!ComponentResult)
					throw new NullReferenceException($"Could not find GameObject with name: {ComponentTarget}!");

				TargetObject = ComponentResult.GetComponent(ExecParameterType);

				if (TargetObject == null)
					throw new NullReferenceException($"GameObject: {ComponentResult.name} doesn't have an attached {ExecParameterType}");
			}
			else
			{
				HandlePrimitiveParameter(ref TargetObject, RawParams[ParamIndex], ExecParameterType);
			}

			++ParamIndex;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void HandlePrimitiveParameter(ref object TargetObject, object RawParameter, Type ParameterType)
			=> TargetObject = Convert.ChangeType(RawParameter, ParameterType);

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
						ParamsBuilder.Append(ParamSplit[ParamSplit.Length - 1]).Append(" ").Append(Param.Name).Append(", ");
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
