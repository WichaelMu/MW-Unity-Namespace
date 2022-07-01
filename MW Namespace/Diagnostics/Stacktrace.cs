using System;

namespace MW.Diagnostics
{
	/// <summary>Provides a stacktrace for code.</summary>
	public static class Stacktrace
	{
		public static string Here() => Environment.StackTrace;

		/// <summary>Stacktrace using <see cref="Log.P(object[])"/> with <see cref="EVerbosity"/>.</summary>
		/// <docs>Stacktrace using Log.P with EVerbosity verbosity.</docs>
		/// <param name="Verbosity">The EVerbosity verbosity of the Log.P.</param>
		public static void Here(EVerbosity Verbosity = EVerbosity.Log)
		{
			switch (Verbosity)
			{
				case EVerbosity.Log:
					Log.P(Here());
					return;
				case EVerbosity.Error:
					Log.E(Here());
					return;
				case EVerbosity.Warning:
					Log.W(Here());
					return;
			}
		}

		/// <summary>Stacktrace using <see cref="Log.P(object[])"/> with <see cref="EVerbosity"/> and a string message.</summary>
		/// <docs>Stacktrace using Log.P with EVerbosity verbosity and a string message.</docs>
		/// <param name="Message">The string message to show.</param>
		/// <param name="Verbosity">The EVerbosity of the Log.P.</param>
		public static void Here(string Message, EVerbosity Verbosity = EVerbosity.Log)
		{
			string stacktrace = Message + ' ' + Here();

			switch (Verbosity)
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
