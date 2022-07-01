﻿

namespace MW.Diagnostics
{

	/// <summary>UnityEngine.Debug.LogEVerbosity().</summary>
	public enum EVerbosity
	{
		/// <summary></summary>
		Log,
		/// <summary></summary>
		Warning,
		/// <summary></summary>
		Error
	};

	/// <summary>Write to the console.</summary>
	public class Log
	{
		/// <summary><see cref="UnityEngine.Debug.Log(object)"/> every object with <see cref="object.ToString()"/>.</summary>
		/// <docs>UnityEngine.Debug.Log(object) every object with object.ToString().</docs>
		/// <param name="args">The list of objects to log separated by a space.</param>
		public static void P(params object[] args)
		{
			string print = "";
			foreach (object arg in args)
				print += arg.ToString() + ' ';

			UnityEngine.Debug.Log(print);
		}

		/// <summary><see cref="UnityEngine.Debug.LogError(object)"/> every object with <see cref="object.ToString()"/>.</summary>
		/// <docs>UnityEngine.Debug.LogError(object) every object with object.ToString().</docs>
		/// <param name="args">The list of objects to log separated by a space.</param>
		public static void E(params object[] args)
		{
			string print = "";
			foreach (object arg in args)
				print += arg.ToString() + ' ';

			UnityEngine.Debug.LogError(print);
		}

		/// <summary><see cref="UnityEngine.Debug.LogWarning(object)"/> every object with <see cref="object.ToString()"/>.</summary>
		/// <docs>UnityEngine.Debug.LogWarning(object) every object with object.ToString().</docs>
		/// <param name="args">The list of objects to log separated by a space.</param>
		public static void W(params object[] args)
		{
			string print = "";
			foreach (object arg in args)
				print += arg.ToString() + ' ';

			UnityEngine.Debug.LogWarning(print);
		}

		public static void Auto(string Content, EVerbosity Verbosity)
		{
			switch (Verbosity)
			{
				case EVerbosity.Log:
					P(Content);
					break;
				case EVerbosity.Warning:
					W(Content);
					break;
				case EVerbosity.Error:
					E(Content);
					break;
				default:
					P(Content);
					break;
			}
		}
	}
}
