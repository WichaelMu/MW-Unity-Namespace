using System;
using System.Diagnostics;
using System.Reflection;
using MW.Easing;
using MW.Extensions;
using UnityEditor;
using UnityEngine;

namespace MW.Diagnostics
{
	/// <summary>Provides a stacktrace for code.</summary>
	/// <decorations decor="public static class"></decorations>
	public static class Stacktrace
	{
		/// <summary>Gets the current stack trace information.</summary>
		/// <decorations decor="public static string"></decorations>
		/// <returns>StacktraceInfo struct.</returns>
		public static StacktraceInfo Here() => StacktraceInfo.Construct(Environment.StackTrace);

		/// <summary>Stacktrace using <see cref="Log.P(object[])"/> with <see cref="EVerbosity"/>.</summary>
		/// <docs>Stacktrace using Log.P with EVerbosity verbosity.</docs>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Verbosity">The EVerbosity verbosity of the Log.P.</param>
		public static void Here(EVerbosity Verbosity = EVerbosity.Log)
		{
			switch (Verbosity)
			{
				case EVerbosity.Log:
					Log.P(Here());
					return;
				case EVerbosity.Error:
					Log.E(Here());
					return;
				case EVerbosity.Warning:
					Log.W(Here());
					return;
			}
		}

		/// <summary>Stacktrace using <see cref="Log.P(object[])"/> with <see cref="EVerbosity"/> and a string message.</summary>
		/// <docs>Stacktrace using Log.P with EVerbosity verbosity and a string message.</docs>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Message">The string message to show.</param>
		/// <param name="Verbosity">The EVerbosity of the Log.P.</param>
		public static void Here(string Message, EVerbosity Verbosity = EVerbosity.Log)
		{
			string stacktrace = Message + ' ' + Here();

			switch (Verbosity)
			{
				case EVerbosity.Log:
					Log.P(stacktrace);
					return;
				case EVerbosity.Error:
					Log.E(stacktrace);
					return;
				case EVerbosity.Warning:
					Log.W(stacktrace);
					return;
			}
		}
	}

	/// <summary>Information regarding a Stacktrace.</summary>
	/// <decorations decors="public struct"></decorations>
	public struct StacktraceInfo
	{
		/// <summary>The string value of the stacktrace.</summary>
		/// <decorations decors="public string"></decorations>
		public string Stacktrace;
		/// <summary>The Type of the calling Class.</summary>
		/// <decorations decors="public Type"></decorations>
		public Type Class;
		/// <summary>The Method or Function that called this Stacktrace.</summary>
		/// <decorations decors="public MethodInfo"></decorations>
		public MethodInfo Function;

		internal static StacktraceInfo Construct(string Stacktrace)
		{
			StacktraceInfo RetVal;
			RetVal.Stacktrace = Stacktrace;

			StackFrame F = new StackFrame(2, true);
			RetVal.Function = (MethodInfo)F.GetMethod();
			RetVal.Class = RetVal.Function.DeclaringType;

			return RetVal;
		}

		/// <summary>Implicit string conversion to the string value of the stacktrace.</summary>
		/// <decorations decors="public static implicit operator string"></decorations>
		public static implicit operator string(StacktraceInfo StacktraceInfo) => StacktraceInfo.Stacktrace;
	}
}
