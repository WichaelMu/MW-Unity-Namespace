﻿using UnityEngine;

namespace MW.IO
{
	/// <summary>Mouse Input and Keyboard Input, based on <see cref="Input"/>.</summary>
	/// <docs>Mouse Input and Keyboard Input.</docs>
	/// <decorations decor="public static class"></decorations>
	public static class I
	{
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
	}
}