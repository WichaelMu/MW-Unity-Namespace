using System;

namespace MW.Diagnostics {

	/// <summary>UnityEngine.Debug.Log<see cref="EVerbosity"/>().</summary>
	public enum EVerbosity { Log, Warning, Error };

	/// <summary>Track execution time of code.</summary>
	public class Stopwatch {

		readonly System.Diagnostics.Stopwatch sw;

		/// <summary>Creates a new Stopwatch.</summary>
		/// <param name="bStartOnCreation">Immediately starting timing upon instantiating?</param>
		public Stopwatch(bool bStartOnCreation = true) {
			sw = new System.Diagnostics.Stopwatch();

			if (bStartOnCreation) {
				sw.Start();
			}
		}

		/// <summary>Start Stopwatch.</summary>
		public void Start() { 
			sw.Start();
		}

		/// <summary>Stop Stopwatch and get the elapsed <see cref="Time"/>.</summary>
		public long Stop() {
			sw.Stop();
			return Time();
		}

		/// <summary>Restarts Stopwatch and get the <see cref="Time"/> before restarting.</summary>
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

		/// <summary>Stacktrace using <see cref="Log.Print"/> with <see cref="EVerbosity"/> verbosity.</summary>
		/// <param name="verbosity">The <see cref="EVerbosity"/> verbosity of the <see cref="Log.Print"/>.</param>
		public static void Here(EVerbosity verbosity = EVerbosity.Log) {
			switch (verbosity) {
				case EVerbosity.Log:
					Log.Print(Environment.StackTrace);
					return;
				case EVerbosity.Error:
					Log.PrintError(Environment.StackTrace);
					return;
				case EVerbosity.Warning:
					Log.PrintWarning(Environment.StackTrace);
					return;
			}
		}

		/// <summary>Stacktrace using <see cref="Log.Print"/> with <see cref="EVerbosity"/> verbosity and a <see cref="string"/> message.</summary>
		/// <param name="sMessage">The <see cref="string"/> message to show.</param>
		/// <param name="verbosity">The <see cref="EVerbosity"/> of the <see cref="Log.Print"/>.</param>
		public static void Here(string sMessage, EVerbosity verbosity = EVerbosity.Log) {
			string stacktrace = sMessage + ' ' + Environment.StackTrace;

			switch (verbosity) {
				case EVerbosity.Log:
					Log.Print(stacktrace);
					return;
				case EVerbosity.Error:
					Log.PrintError(stacktrace);
					return;
				case EVerbosity.Warning:
					Log.PrintWarning(stacktrace);
					return;
			}
		}
	}

	public static class Log {
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
