using System.Text;
using MW;
using UnityEngine;

namespace MTest.Output
{
	public static class O
	{
		const string kFloatingPointAccuracy = "F3";

		public static void Failed(int TestNumber, string Expected, string Result, string Function, params object[] Params)
		{
			string ErrorMessage = $"Failed {TestNumber}: {Function}(";

			StringBuilder SB = new StringBuilder();
			for (byte i = 0; i < Params.Length; ++i)
			{
				SB.Append(Params[i].ToString());

				if (i < Params.Length - 1)
					SB.Append(", ");
			}

			ErrorMessage += SB.ToString();
			ErrorMessage += $") Expected: {Expected}. Returned: {Result}.";

			Line(ErrorMessage, 2);
		}

		public static void Failed(int TestNumber, float Expected, float Result, string Function, params object[] Params)
		{
			string ErrorMessage = $"Failed {TestNumber}: {Function}(";

			StringBuilder SB = new StringBuilder();
			for (byte i = 0; i < Params.Length; ++i)
			{
				SB.Append(Params[i].ToString());

				if (i < Params.Length - 1)
					SB.Append(", ");
			}

			ErrorMessage += SB.ToString();
			ErrorMessage += $") Expected: {Expected.ToString(kFloatingPointAccuracy)}F. Returned: {Result.ToString(kFloatingPointAccuracy)}F.";

			Line(ErrorMessage, 2);
		}

		public static void Failed(int TestNumber, int Expected, int Result, string Function, params object[] Params)
		{
			string ErrorMessage = $"Failed {TestNumber}: {Function}(";

			StringBuilder SB = new StringBuilder();
			for (byte i = 0; i < Params.Length; ++i)
			{
				SB.Append(Params[i].ToString());

				if (i < Params.Length - 1)
					SB.Append(", ");
			}

			ErrorMessage += SB.ToString();
			ErrorMessage += $") Expected: {Expected}. Returned: {Result}.";

			Line(ErrorMessage, 2);
		}

		public static void Failed(int TestNumber, Vector3 Expected, MVector Result, string Function, params object[] Params)
		{
			string ErrorMessage = $"Failed {TestNumber}: {Function}(";

			StringBuilder SB = new StringBuilder();
			for (byte i = 0; i < Params.Length; ++i)
			{
				SB.Append(Params[i].ToString());

				if (i < Params.Length - 1)
					SB.Append(", ");
			}

			ErrorMessage += SB.ToString();
			ErrorMessage += $") Expected: x: {Expected.x}, y: {Expected.y}, z: {Expected.z}. Returned: {Result}.";

			Line(ErrorMessage, 2);
		}

		public static void Write(object O, EVerbosity Verbosity = EVerbosity.Message)
		{
			Console.Write(EvaluateVerbosity(ref Verbosity) + O);
		}

		public static void Line(object O, EVerbosity Verbosity = EVerbosity.Message)
		{
			Console.WriteLine(EvaluateVerbosity(ref Verbosity) + O);
		}

		static string EvaluateVerbosity(ref EVerbosity Verbosity)
		{
			return Verbosity switch
			{
				EVerbosity.Error => $"Error: ",
				EVerbosity.Message => "",
				EVerbosity.Warning => $"Warning: ",
				// Can't happen.
				_ => $"Error: Failed to parse Verbosity."
			};
		}
		public static void Write(object O, byte Verbosity)
		{
			Console.Write(EvaluateVerbosity(ref Verbosity) + O);
		}

		public static void Line(object O, byte Verbosity)
		{
			Console.WriteLine(EvaluateVerbosity(ref Verbosity) + O);
		}

		static string EvaluateVerbosity(ref byte Verbosity)
		{
			return Verbosity switch
			{
				2 => $"Error: ",
				0 => "",
				1 => $"Warning: ",
				_ => $"Error: Failed to parse Verbosity from byte: " + Verbosity
			};
		}
	}

	public enum EVerbosity : byte
	{
		Message,
		Warning,
		Error
	}
}
