using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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

		/// <summary>Writes string lines to a file asynchronously.</summary>
		/// <decorations decor="public static async Task"></decorations>
		/// <param name="Path">The path of the file to write to.</param>
		/// <param name="NameOfFile">The name of the file to write to, including it's extension.</param>
		/// <param name="Mode"><see cref="EWriteMode"/> append to the file (if it exists), or overwrite the file regardless of it's existing contents.</param>
		/// <param name="Encoding">The type of <see cref="Encoding"/> to write as.</param>
		/// <param name="Lines">The lines to write.</param>
		/// <returns>The asynchronous operation.</returns>
		public static async Task AsyncWriteToFile(string Path, string NameOfFile, EWriteMode Mode, Encoding Encoding, params string[] Lines)
		{
			// Construct the file.
			string PathAndName = Path + NameOfFile;
			using StreamWriter File = new StreamWriter(PathAndName, Mode == EWriteMode.Append, Encoding);

			foreach (string Line in Lines)
				await File.WriteLineAsync(Line);

			File.Close();
		}

		/// <summary>Writes string lines to a file.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Path">The path of the file to write to.</param>
		/// <param name="NameOfFile">The name of the file to write to, including it's extension.</param>
		/// <param name="Mode"><see cref="EWriteMode"/> append to the file (if it exists), or overwrite the file regardless of it's existing contents.</param>
		/// <param name="Encoding">The type of <see cref="Encoding"/> to write as.</param>
		/// <param name="Lines">The lines to write.</param>
		public static void WriteToFile(string Path, string NameOfFile, EWriteMode Mode, Encoding Encoding, params string[] Lines)
		{
			// Construct the file.
			string PathAndName = Path + NameOfFile;
			using StreamWriter File = new StreamWriter(PathAndName, Mode == EWriteMode.Append, Encoding);

			foreach (string Line in Lines)
				File.WriteLine(Line);

			File.Close();
		}

		/// <summary>Reads contents from a file into a <see cref="List{T}"/> of <see cref="string"/>s.</summary>
		/// <decorations decor="public static bool"></decorations>
		/// <param name="Path">The path of the file to read from.</param>
		/// <param name="NameOfFile">The name of the file to read from, including it's extension.</param>
		/// <param name="ContentsInFile">The out <see cref="List{T}"/> of <see cref="string"/>s of the contents from the file at path.</param>
		/// <returns>True if NameOfFile at Path was read with no errors. False if NameOfFile at Path does not exist, or is unable to be read.</returns>
		public static bool ReadFromFile(string Path, string NameOfFile, out MArray<string> ContentsInFile)
		{
			ContentsInFile = new MArray<string>();

			// Open the file.
			using StreamReader StreamReader = new StreamReader(Path + NameOfFile);

			try
			{
				string Line;

				// Add every line until EOF.
				while ((Line = StreamReader.ReadLine()) != null)
					ContentsInFile.Push(Line);
			}
			catch (IOException E)
			{
				Log.E($"IO Exception occurred when trying to read {NameOfFile} at {Path}\n{E}");
				return false;
			}
			catch (Exception E)
			{
				Log.E("File could not be read!\n" + E);
				return false;
			}
			finally
			{
				// Close the file.
				StreamReader.Close();
			}

			return true;
		}

		/// <summary>Whether or not a file exists.</summary>
		/// <decorations decor="public static bool"></decorations>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool FileExists(string Path, string NameOfFile)
		{
			return File.Exists(Path + NameOfFile);
		}
	}

	/// <summary>The behaviour in which to write to a file.</summary>
	public enum EWriteMode
	{
		/// <summary>Append to the end of a file.</summary>
		Append,
		/// <summary>Make or write to a file, regardless of it's existing contents.</summary>
		Overwrite
	}
}

