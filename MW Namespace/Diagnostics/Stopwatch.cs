﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MW.Diagnostics
{
	/// <summary>Track execution time of code.</summary>
	public class Stopwatch
	{
		public delegate void Print (params object[] args);

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

		/// <summary>Stop Stopwatch and get the elapsed Time.</summary>
		public long Stop()
		{
			sw.Stop();
			return Time();
		}

		/// <summary>Restarts Stopwatch and get the Time before restarting.</summary>
		public long Restart()
		{
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
}