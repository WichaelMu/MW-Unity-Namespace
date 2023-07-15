using System;
using System.Text;
#if RELEASE
using UnityEngine;
#else
using MW.IO;
#endif // RELEASE

namespace MW.Diagnostics
{

	/// <summary>Debug.LogEVerbosity().</summary>
	/// <decorations decor="public enum"></decorations>
	public enum EVerbosity
	{
		/// <summary></summary>
		Log,
		/// <summary></summary>
		Warning,
		/// <summary></summary>
		Error
	};

	/// <summary>Write to the console.</summary>
	/// <decorations decor="public class"></decorations>
	public class Log
	{
		/// <summary>Prints every object with <see cref="object.ToString()"/> to the console as output.</summary>
		/// <docs>Prints every object with object.ToString() to the console as output.</docs>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Args">The list of objects to log separated by a space.</param>
		public static void P(params object[] Args)
		{
			StringBuilder SB = new StringBuilder();
			foreach (object arg in Args)
				SB.Append(arg.ToString()).Append(' ');
#if RELEASE
			Debug.Log(SB.ToString());
#else
			O.Print(SB.ToString());
#endif // RELEASE
        }

		/// <summary>Prints every object with <see cref="object.ToString()"/> to the console as output.</summary>
		/// <docs>Prints every object with object.ToString() to the console as output.</docs>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Args">The list of objects to log separated by a space.</param>
		public static void E(params object[] Args)
		{
			StringBuilder SB = new StringBuilder();
			foreach (object Arg in Args)
				SB.Append(Arg.ToString()).Append(' ');

#if RELEASE
			Debug.LogError(SB.ToString());
#else
			O.Print(SB.ToString(), ConsoleColor.Red, ConsoleColor.Black);
#endif // RELEASE
		}

		/// <summary>Prints every object with <see cref="object.ToString()"/> to the console as output.</summary>
		/// <docs>Prints every object with object.ToString() to the console as output.</docs>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Args">The list of objects to log separated by a space.</param>
		public static void W(params object[] Args)
		{
			StringBuilder SB = new StringBuilder();
			foreach (object Arg in Args)
				SB.Append(Arg.ToString()).Append(' ');

#if RELEASE
			Debug.LogWarning(SB.ToString());
#else
			O.Print(SB.ToString(), ConsoleColor.Yellow, ConsoleColor.Black);
#endif // RELEASE
		}

		/// <summary>Prints Content to the Console with Verbosity.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Content">The string to print to the console.</param>
		/// <param name="Verbosity">The verbosity to print Content with.</param>
		public static void Auto(string Content, EVerbosity Verbosity)
		{
			switch (Verbosity)
			{
				case EVerbosity.Log:
					P(Content);
					break;
				case EVerbosity.Warning:
					W(Content);
					break;
				case EVerbosity.Error:
					E(Content);
					break;
				default:
					P(Content);
					break;
			}
		}

#if RELEASE
		/// <summary>Prints Content to the Console with Colour and Verbosity.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Content">The string to colour and print to the console.</param>
		/// <param name="Colour">The colour to display Content in RGB.</param>
		/// <param name="Verbosity">The verbosity to print Content with.</param>
		public static void Colourise(string Content, MVector Colour, EVerbosity Verbosity = EVerbosity.Log)
		{
			StringBuilder SB = new StringBuilder();
			SB.Append("<color=");
			SB.Append(Conversion.Colour.RGBToHex((byte)Colour.X, (byte)Colour.Y, (byte)Colour.Z));
			SB.Append(">");
			SB.Append(Content);
			SB.Append("</color>");

			Auto(SB.ToString(), Verbosity);
		}
#endif // RELEASE
	}
}
