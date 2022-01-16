using System;

namespace MW.Diagnostics {

	/// <summary>UnityEngine.Debug.Log<see cref="EVerbosity"/>().</summary>
	public enum EVerbosity { Log, Warning, Error };

	public class Log {
		/// <summary><see cref="UnityEngine.Debug.Log(object)"/> every object.</summary>
		/// <param name="debug">The list of <see cref="object"/>s to log separated by a space.</param>
		public static void Print(params object[] debug) {
			string print = "";
			for (int i = 0; i < debug.Length; ++i) {
				print += debug[i].ToString() + ' ';
			}

			UnityEngine.Debug.Log(print);
		}

		/// <summary><see cref="UnityEngine.Debug.LogError(object)"/> every object.</summary>
		/// <param name="debug">The list of <see cref="object"/>s to log separated by a space.</param>
		public static void PrintError(params object[] debug) {
			string print = "";
			for (int i = 0; i < debug.Length; ++i) {
				print += debug[i].ToString() + ' ';
			}

			UnityEngine.Debug.LogError(print);
		}

		/// <summary><see cref="UnityEngine.Debug.LogWarning(object)"/> every object.</summary>
		/// <param name="debug">The list of <see cref="object"/>s to log separated by a space.</param>
		public static void PrintWarning(params object[] debug) {
			string print = "";
			for (int i = 0; i < debug.Length; ++i) {
				print += debug[i].ToString() + ' ';
			}

			UnityEngine.Debug.LogWarning(print);
		}
	}
}
