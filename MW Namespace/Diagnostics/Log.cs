using System;

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
		/// <summary>UnityEngine.Debug.Log(object) every object with object.ToString().</summary>
		/// <param name="args">The list of objects to log separated by a space.</param>
		public static void P(params object[] args)
		{
			string print = "";
			foreach (object arg in args)
				print += arg.ToString() + ' ';

			UnityEngine.Debug.Log(print);
		}

		/// <summary>UnityEngine.Debug.LogError(object) every object with object.ToString().</summary>
		/// <param name="args">The list of objects to log separated by a space.</param>
		public static void E(params object[] args)
		{
			string print = "";
			foreach (object arg in args)
				print += arg.ToString() + ' ';

			UnityEngine.Debug.LogError(print);
		}

		/// <summary>UnityEngine.Debug.LogWarning(object) every object with object.ToString().</summary>
		/// <param name="args">The list of objects to log separated by a space.</param>
		public static void W(params object[] args)
		{
			string print = "";
			foreach (object arg in args)
				print += arg.ToString() + ' ';

			UnityEngine.Debug.LogWarning(print);
		}
	}
}
