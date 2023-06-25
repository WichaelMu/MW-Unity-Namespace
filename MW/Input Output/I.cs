#if STANDALONE
using System;
using MW.Diagnostics;
#else
using UnityEngine;
#endif // RELEASE

namespace MW.IO
{
#if RELEASE
	/// <summary>Mouse Input and Keyboard Input, based on <see cref="Input"/>.</summary>
	/// <docs>Mouse Input and Keyboard Input.</docs>
	/// <decorations decor="public static class"></decorations>
#else
	/// <summary>Standard input helper class.</summary>
#endif // RELEASE
	public static class I
	{
#if RELEASE
		/// <decorations decor="public static bool"></decorations>
		/// <param name="MouseButton">The EButton press to listen for.</param>
		/// <param name="bHold">Whether or not to check if this button is held down.</param>
		/// <param name="bUp">Whether or not to check if this button is released.</param>
		/// <returns>If MouseButton was clicked or held.</returns>
		public static bool Click(EButton MouseButton, bool bHold = false, bool bUp = false)
		{
			switch (MouseButton)
			{
				case EButton.LeftMouse:
					if (bUp)
						return Input.GetMouseButtonUp(0);

					if (bHold)
						return Input.GetMouseButton(0);
					return Input.GetMouseButtonDown(0);
				case EButton.RightMouse:
					if (bUp)
						return Input.GetMouseButtonUp(1);

					if (bHold)
						return Input.GetMouseButton(1);
					return Input.GetMouseButtonDown(1);
				case EButton.MiddleMouse:
					if (bUp)
						return Input.GetMouseButtonUp(2);

					if (bHold)
						return Input.GetMouseButton(2);
					return Input.GetMouseButtonDown(2);
			}

			return false;
		}

		/// <decorations decor="public static bool"></decorations>
		/// <param name="KeyStroke">The KeyCode that was pressed on the keyboard.</param>
		/// <param name="bHold">Whether or not to check if this button is held down.</param>
		/// <param name="bUp">Whether or not to check if this button is released.</param>
		/// <returns>If KeyStroke was pressed or Held.</returns>
		public static bool Key(KeyCode KeyStroke, bool bHold = false, bool bUp = false)
		{
			if (bUp)
				return Input.GetKeyUp(KeyStroke);

			if (bHold)
				return Input.GetKey(KeyStroke);
			return Input.GetKeyDown(KeyStroke);
		}

		/// <summary>Identical to <see cref="Input.anyKey"/>.</summary>
		/// <docs>Identical to Input.anyKey.</docs>
		/// <decorations decor="public static bool"></decorations>
		/// <returns>True if a key or a mouse button was pressed.</returns>
		public static bool Any()
		{
			return Input.anyKey;
		}
#else
		/// <summary>Reads a line from the <see cref="Console"/>.</summary>
		/// <returns>The line that was read.</returns>
		public static string String() { return System.Console.ReadLine(); }

		/// <summary>Displays a message before reading a line from the <see cref="Console"/>.</summary>
		/// <param name="Message">The message to display before reading a line.</param>
		/// <returns>The line that was read.</returns>
		public static string String(string Message)
		{
			O.Out(Message);

			return String();
		}

		/// <summary>Reads an <see cref="int"/> from the <see cref="Console"/>.</summary>
		/// <remarks><see cref="FormatException"/> if the input is malinformed.</remarks>
		/// <returns>The <see cref="int"/> that was read.</returns>
		public static int Int() { return Convert.ToInt32(String()); }

		/// <summary>Displays a message before reading an <see cref="int"/> from the <see cref="Console"/>.</summary>
		/// <remarks><see cref="FormatException"/> if the input is malinformed.</remarks>
		/// <param name="Message">The message to display before reading a line.</param>
		/// <returns>The <see cref="int"/> that was read.</returns>
		public static int Int(string Message)
		{
			O.Out(Message);

			return Int();
		}

		/// <summary>Reads a <see cref="float"/> from the <see cref="Console"/>.</summary>
		/// <remarks><see cref="FormatException"/> if the input is malinformed.</remarks>
		/// <returns>The <see cref="float"/> that was read.</returns>
		public static float Float() { return float.Parse(String()); }

		/// <summary>Displays a message before reading a <see cref="float"/> from the <see cref="Console"/>.</summary>
		/// <remarks><see cref="FormatException"/> if the input is malinformed.</remarks>
		/// <param name="Message">The message to display before reading a line.</param>
		/// <returns>The <see cref="float"/> that was read.</returns>
		public static float Float(string Message)
		{
			O.Out(Message);

			return Float();
		}

		/// <summary>Reads a <see cref="double"/> from the <see cref="Console"/>.</summary>
		/// <remarks><see cref="FormatException"/> if the input is malinformed.</remarks>
		/// <returns>The <see cref="double"/> that was read.</returns>
		public static double Double() { return double.Parse(String()); }

		/// <summary>Displays a message before reading a <see cref="double"/> from the <see cref="Console"/>.</summary>
		/// <remarks><see cref="FormatException"/> if the input is malinformed.</remarks>
		/// <param name="Message">The message to display before reading a line.</param>
		/// <returns>The <see cref="double"/> that was read.</returns>
		public static double Double(string Message)
		{
			O.Out(Message);

			return Double();
		}

		/// <summary>Removes any leading and trailing white-space from <paramref name="ToTrim"/>.</summary>
		/// <param name="ToTrim">The string to trim.</param>
		public static void Trim(ref string ToTrim)
		{
			ToTrim = ToTrim.Trim();
		}

		/// <summary>Reads any key as a stub input.</summary>
		public static void Any() => System.Console.ReadKey();

		/// <summary>Reads the next <see cref="ConsoleKey"/>.</summary>
		public static ConsoleKey Key() => System.Console.ReadKey(intercept: true).Key;

		/// <summary>Reads the next <see cref="char"/>.</summary>
		public static char Char(out char C) => C = System.Console.ReadKey(intercept: true).KeyChar;
#endif // RELEASE
	}
}
