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
		/// <param name="debug">The list of objects to log separated by a space.</param>
		public static void P(params object[] debug)
		{
			string print = "";
			for (int i = 0; i < debug.Length; ++i)
			{
				print += debug[i].ToString() + ' ';
			}

			UnityEngine.Debug.Log(print);
		}

		/// <summary>UnityEngine.Debug.LogError(object) every object with object.ToString().</summary>
		/// <param name="debug">The list of objects to log separated by a space.</param>
		public static void E(params object[] debug)
		{
			string print = "";
			for (int i = 0; i < debug.Length; ++i)
			{
				print += debug[i].ToString() + ' ';
			}

			UnityEngine.Debug.LogError(print);
		}

		/// <summary>UnityEngine.Debug.LogWarning(object) every object with object.ToString().</summary>
		/// <param name="debug">The list of objects to log separated by a space.</param>
		public static void W(params object[] debug)
		{
			string print = "";
			for (int i = 0; i < debug.Length; ++i)
			{
				print += debug[i].ToString() + ' ';
			}

			UnityEngine.Debug.LogWarning(print);
		}
	}
}
