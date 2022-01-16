using System;
using System.Collections.Generic;
using System.Text;

namespace MW.Diagnostics
{
	/// <summary>Provides a stacktrace for code.</summary>
	public static class Stacktrace
	{

		/// <summary>Stacktrace using <see cref="Log.Print"/> with <see cref="EVerbosity"/> verbosity.</summary>
		/// <param name="verbosity">The <see cref="EVerbosity"/> verbosity of the <see cref="Log.Print"/>.</param>
		public static void Here(EVerbosity verbosity = EVerbosity.Log)
		{
			switch (verbosity)
			{
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
		public static void Here(string sMessage, EVerbosity verbosity = EVerbosity.Log)
		{
			string stacktrace = sMessage + ' ' + Environment.StackTrace;

			switch (verbosity)
			{
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
}
