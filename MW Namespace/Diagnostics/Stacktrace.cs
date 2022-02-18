using System;
using System.Collections.Generic;
using System.Text;

namespace MW.Diagnostics
{
	/// <summary>Provides a stacktrace for code.</summary>
	public static class Stacktrace
	{

		/// <summary>Stacktrace using Log.P with EVerbosity verbosity.</summary>
		/// <param name="verbosity">The EVerbosity verbosity of the Log.P.</param>
		public static void Here(EVerbosity verbosity = EVerbosity.Log)
		{
			switch (verbosity)
			{
				case EVerbosity.Log:
					Log.P(Environment.StackTrace);
					return;
				case EVerbosity.Error:
					Log.E(Environment.StackTrace);
					return;
				case EVerbosity.Warning:
					Log.W(Environment.StackTrace);
					return;
			}
		}

		/// <summary>Stacktrace using Log.P with EVerbosity verbosity and a string message.</summary>
		/// <param name="sMessage">The string message to show.</param>
		/// <param name="verbosity">The EVerbosity of the Log.P.</param>
		public static void Here(string sMessage, EVerbosity verbosity = EVerbosity.Log)
		{
			string stacktrace = sMessage + ' ' + Environment.StackTrace;

			switch (verbosity)
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
}
