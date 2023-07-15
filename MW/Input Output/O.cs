#if STANDALONE
using System;
using CSConsole = System.Console;
#endif // STANDALONE
using MW.Diagnostics;

namespace MW.IO
{
	/// <summary></summary>
	/// <decorations decor="public static class"></decorations>
	public static class O
	{
		/// <summary>Identical to <see cref="Log.P(object[])"/>.</summary>
		/// <docs>Identical to Log.P(object[]).</docs>
		/// <decorations decor="public static void"></decorations>
		/// <param name="args">The list of objects to log separated by a space.</param>
		public static void Out(params object[] args)
		{
			Log.P(args);
		}

#if STANDALONE
		/// <summary>
		/// Writes <paramref name="Content"/> followed by a line terminator, to the standard output stream.
		/// <br></br><br></br>
		/// Essentially a replacement for <see cref="CSConsole.WriteLine(string)"/>, with Colour and line clearing functionality.
		/// </summary>
		/// <param name="Content">The string value of what will be printed to the Console.</param>
		/// <param name="FColour">The colour of the font.<br>Default is <see cref="ConsoleColor.Gray"/>.</br></param>
		/// <param name="BColour">The colour of the console behind the font.<br>Default is <see cref="ConsoleColor.Black"/>.</br></param>
		public static void Print(string Content, ConsoleColor FColour = ConsoleColor.Gray, ConsoleColor BColour = ConsoleColor.Black)
		{
			// Set Colours.
			SetColours(FColour, BColour);

			// Clear line and Write Content.
			CSConsole.WriteLine(Content);

			// Revert to defaults.
			ResetColours();
		}

		/// <summary>Set the <see cref="CSConsole.ForegroundColor"/> and <see cref="CSConsole.BackgroundColor"/>.</summary>
		/// <param name="FColour">The colour of the font.</param>
		/// <param name="BColour">The colour of the console behind the font.<br>Default is <see cref="ConsoleColor.Black"/>.</br></param>
		public static void SetColours(ConsoleColor FColour, ConsoleColor BColour = ConsoleColor.Black)
		{
			CSConsole.ForegroundColor = FColour;
			CSConsole.BackgroundColor = BColour;
		}

		/// <summary>Reset <see cref="CSConsole.ForegroundColor"/> and <see cref="CSConsole.BackgroundColor"/> to default values.</summary>
		public static void ResetColours()
		{
			SetColours(ConsoleColor.Gray);
		}
#endif // STANDALONE
	}
}
