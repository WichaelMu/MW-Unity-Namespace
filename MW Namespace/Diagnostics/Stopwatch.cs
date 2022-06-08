namespace MW.Diagnostics
{
	/// <summary>Track execution time of code.</summary>
	public class Stopwatch
	{
		readonly System.Diagnostics.Stopwatch sw;

		/// <summary>Creates a new Stopwatch.</summary>
		/// <param name="bStartOnCreation">Immediately starting timing upon instantiating?</param>
		public Stopwatch(bool bStartOnCreation = true)
		{
			sw = new System.Diagnostics.Stopwatch();

			if (bStartOnCreation)
			{
				sw.Start();
			}
		}

		/// <summary>Start Stopwatch.</summary>
		public void Start()
		{
			sw.Start();
		}

		/// <summary>Stop Stopwatch and get the elapsed <see cref="Time"/>.</summary>
		/// <docs>Stop Stopwatch and get the elapsed Time.</docs>
		/// <ret>The elapsed time before stopping.</ret>
		/// <returns>The elapsed <see cref="Time"/> before stopping.</returns>
		public long Stop()
		{
			sw.Stop();
			return Time();
		}

		/// <summary>Restarts Stopwatch and get the <see cref="Time"/> before restarting.</summary>
		/// <docs>Restarts Stopwatch and get the Time before restarting.</docs>
		/// <returns>The elapsed time before restarting.</returns>
		public long Restart()
		{
			long now = Time();
			sw.Restart();

			return now;
		}

		/// <summary>Get the current elapsed time.</summary>
		public long Time() => sw.ElapsedMilliseconds;

		/// <summary>Get the current elapsed <see cref="Time"/> in seconds.</summary>
		/// <docs>Get the current elapsed time in seconds.</docs>
		public long TimeInSeconds() => ToSeconds(Time());

		/// <summary>Converts milliseconds to seconds.</summary>
		/// <param name="Milliseconds">Milliseconds to convert.</param>
		public static long ToSeconds(long Milliseconds) => Milliseconds * (long).001;
	}
}
