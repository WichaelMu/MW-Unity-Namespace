using System;
using System.Collections.Generic;
using System.Text;

namespace MW.Diagnostics
{
	/// <summary>Provides a stacktrace for code.</summary>
	public static class Stacktrace
	{

		/// <summary>Stacktrace using <see cref="Log.P"/> with <see cref="EVerbosity"/> verbosity.</summary>
		/// <param name="verbosity">The <see cref="EVerbosity"/> verbosity of the <see cref="Log.P"/>.</param>
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

		/// <summary>Stacktrace using <see cref="Log.P"/> with <see cref="EVerbosity"/> verbosity and a <see cref="string"/> message.</summary>
		/// <param name="sMessage">The <see cref="string"/> message to show.</param>
		/// <param name="verbosity">The <see cref="EVerbosity"/> of the <see cref="Log.P"/>.</param>
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
