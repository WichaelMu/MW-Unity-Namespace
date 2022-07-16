using System.Text;
using UnityEngine;

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
		/// <summary><see cref="Debug.Log(object)"/> every object with <see cref="object.ToString()"/>.</summary>
		/// <docs>Debug.Log(object) every object with object.ToString().</docs>
		/// <decorations decor="public static void"></decorations>
		/// <param name="args">The list of objects to log separated by a space.</param>
		public static void P(params object[] args)
		{
			string print = "";
			foreach (object arg in args)
				print += arg.ToString() + ' ';

			Debug.Log(print);
		}

		/// <summary><see cref="Debug.LogError(object)"/> every object with <see cref="object.ToString()"/>.</summary>
		/// <docs>Debug.LogError(object) every object with object.ToString().</docs>
		/// <decorations decor="public static void"></decorations>
		/// <param name="args">The list of objects to log separated by a space.</param>
		public static void E(params object[] args)
		{
			string print = "";
			foreach (object arg in args)
				print += arg.ToString() + ' ';

			Debug.LogError(print);
		}

		/// <summary><see cref="Debug.LogWarning(object)"/> every object with <see cref="object.ToString()"/>.</summary>
		/// <docs>Debug.LogWarning(object) every object with object.ToString().</docs>
		/// <decorations decor="public static void"></decorations>
		/// <param name="args">The list of objects to log separated by a space.</param>
		public static void W(params object[] args)
		{
			string print = "";
			foreach (object arg in args)
				print += arg.ToString() + ' ';

			Debug.LogWarning(print);
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

		/// <summary>Prints Content to the Console with Colour and Verbosity.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Content">The string to colour and print to the console.</param>
		/// <param name="Colour">The colour to display Content.</param>
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
	}
}
