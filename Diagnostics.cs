

namespace MW.Development {

	public static class Diagnostics {

		public class Stopwatch {

			readonly System.Diagnostics.Stopwatch sw;

			public Stopwatch (bool bStart = false) {
				sw = new System.Diagnostics.Stopwatch();

				if (bStart) {
					sw.Start();
				}
			}

			public void Start() {
				sw.Start();
			}

			public long Stop() {
				sw.Stop();
				return sw.ElapsedMilliseconds;
			}

			public long Restart() {
				long now = sw.ElapsedMilliseconds;
				sw.Restart();

				return now;
			}

			public long Time() {
				return sw.ElapsedMilliseconds;
			}
		}

		public static class Stacktrace {

			public enum EVerbosity { Log, Warning, Error };

			public static void Trace(EVerbosity verbosity = EVerbosity.Log) {
				switch (verbosity) {
					case EVerbosity.Log:
						UnityEngine.Debug.Log(System.Environment.StackTrace);
						return;
					case EVerbosity.Error:
						UnityEngine.Debug.LogError(System.Environment.StackTrace);
						return;
					case EVerbosity.Warning:
						UnityEngine.Debug.LogError(System.Environment.StackTrace);
						return;
				}
			}
		}
	}

}
