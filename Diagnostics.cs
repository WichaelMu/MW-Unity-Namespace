

namespace MW.Diagnostics {

	/// <summary>UnityEngine.Debug.[MW.Development.Diagnostics.Stacktrace.EVerbosity]</summary>
	public enum EVerbosity { Log, Warning, Error };

	/// <summary>Track execution time of code.</summary>
	public class Stopwatch {

		readonly System.Diagnostics.Stopwatch sw;

		/// <summary>Creates a new Stopwatch.</summary>
		/// <param name="bStartOnCreation">Immediately starting timing upon instantiating?</param>
		public Stopwatch(bool bStartOnCreation = false) {
			sw = new System.Diagnostics.Stopwatch();

			if (bStartOnCreation) {
				sw.Start();
			}
		}

		/// <summary>Start Stopwatch.</summary>
		public void Start() => sw.Start();

		/// <summary>Stop Stopwatch and get the elapsed time.</summary>
		public long Stop() {
			sw.Stop();
			return Time();
		}

		/// <summary>Restarts Stopwatch and get the time before restarting.</summary>
		public long Restart() {
			long now = Time();
			sw.Restart();

			return now;
		}

		/// <summary>Get the current elapsed time.</summary>
		public long Time() => sw.ElapsedMilliseconds;

		/// <summary>Get the current elapsed time in seconds.</summary>
		public long TimeInSeconds() => ToSeconds(Time());

		/// <summary>Converts milliseconds to seconds.</summary>
		/// <param name="lMilliseconds">Milliseconds to convert.</param>
		public static long ToSeconds(long lMilliseconds) => lMilliseconds * (long).001;
	}

	/// <summary>Provides a stacktrace for code.</summary>
	public static class Stacktrace {

		/// <summary>Stacktrace using UnityEngine.Debug with verbosity.</summary>
		/// <param name="verbosity">The verbosity of the UnityEngine.Debug.</param>
		public static void Here(EVerbosity verbosity = EVerbosity.Log) {
			switch (verbosity) {
				case EVerbosity.Log:
					UnityEngine.Debug.Log(System.Environment.StackTrace);
					return;
				case EVerbosity.Error:
					UnityEngine.Debug.LogError(System.Environment.StackTrace);
					return;
				case EVerbosity.Warning:
					UnityEngine.Debug.LogWarning(System.Environment.StackTrace);
					return;
			}
		}

		/// <summary>Stacktrace using UnityEngine.Debug with verbosity and a message.</summary>
		/// <param name="sMessage">The message to show.</param>
		/// <param name="verbosity">The verbosity og the UnityEngine.Debug.</param>
		public static void Here(string sMessage, EVerbosity verbosity = EVerbosity.Log) {
			string stacktrace = sMessage + ' ' + System.Environment.StackTrace;

			switch (verbosity) {
				case EVerbosity.Log:
					UnityEngine.Debug.Log(stacktrace);
					return;
				case EVerbosity.Error:
					UnityEngine.Debug.LogError(stacktrace);
					return;
				case EVerbosity.Warning:
					UnityEngine.Debug.LogWarning(stacktrace);
					return;
			}
		}
	}

	public static class Debug {
		/// <summary>UnityEngine.Logs every object.</summary>
		/// <param name="debug">The list of objects to log.</param>
		public static void Log(params object[] debug) {
			for (int i = 0; i < debug.Length; ++i) {
				UnityEngine.Debug.Log(debug[i].ToString() + ' ');
			}
		}

		/// <summary>UnityEngine.LogErrors every object.</summary>
		/// <param name="debug">The list of objects to log.</param>
		public static void LogError(params object[] debug) {
			for (int i = 0; i < debug.Length; ++i) {
				UnityEngine.Debug.LogError(debug[i].ToString() + ' ');
			}
		}

		/// <summary>UnityEngine.LogWarnings every object.</summary>
		/// <param name="debug">The list of objects to log.</param>
		public static void LogWarning(params object[] debug) {
			for (int i = 0; i < debug.Length; ++i) {
				UnityEngine.Debug.LogWarning(debug[i].ToString() + ' ');
			}
		}
	}

	public static class Editor {

		/// <summary>Switches the Unity Editor to Edit mode.</summary>
		public static void Terminate() {
			UnityEditor.EditorApplication.ExitPlaymode();
		}
	}
}
