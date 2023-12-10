#if STANDALONE

using MW;
using MW.Console;

class MAPI_MConsole : MConsole
{
	// Override ExecTypes to bring in new functions.
	public override Type[] ExecTypes => new Type[] { typeof(MAPI_MConsole) };

	static void Main(string[] Args)
	{
		MAPI_MConsole M = new MAPI_MConsole();
		bool bArgsAreEmpty = Args.Length == 0;
		M.Exec(bArgsAreEmpty
			? new string[] { "__HELP__" } // Show the __HELP__ message if no arguments are provided.
			: Args
		);

		// Show examples if no arguments are provided.
		if (bArgsAreEmpty)
		{
			// Toggle the printing of Built-In Exec Functions in __LIST__.
			// M.Exec(new string[] { "__TOGGLE_BUILTIN__" });
			M.Exec(new string[] { "__LIST__" });
		}
	}

	////////////////////////////////////////////////////////////////////////////////
	// Example [Exec] Functions. These are not affected by __TOGGLE_BUILTIN__.
	////////////////////////////////////////////////////////////////////////////////

	// MAPI PrintString "This is a message"
	// > This is a message
	[Exec("Prints a string.")]
	public static void PrintString(string String) { Console.WriteLine(String); }

	// MAPI PrintAdd 5 9
	// PrintAdd returned: 14
	[Exec("Computes the addition of two integers.")]
	public static int PrintAdd(int A, int B) { return A + B; }

	// MAPI PrintMax { -4.2 6.6 4.992 4831.486 762.0014 .42 }
	// PrintMax returned: 4831.486
	[Exec("Returns the maximum out of a set of floats.")]
	public static float PrintMax(params float[] F) { return FMath.Max(F); }
}

#else

#error RUN MAPI IN A STANDALONE CONFIGURATION.

#endif // STANDALONE
