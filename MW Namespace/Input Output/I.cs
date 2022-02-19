﻿using MW.Enums;
using UnityEngine;

namespace MW.IO
{
	/// <summary>Mouse Input and Keyboard Input, based off of Input.</summary>
	public static class I
	{
		/// <param name="BMouse">The EButton press to listen for.</param>
		/// <param name="bHold">Whether or not to check if this button is held down.</param>
		/// <param name="bUp">Whether or not to check if this button is released.</param>
		/// <returns>If the EButton was clicked or held.</returns>
		public static bool Click(EButton BMouse, bool bHold = false, bool bUp = false)
		{
			switch (BMouse)
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

		/// <param name="KCStroke">The KeyCode that was pressed on the keyboard.</param>
		/// <param name="bHold">Whether or not to check if this button is held down.</param>
		/// <param name="bUp">Whether or not to check if this button is released.</param>
		/// <returns>If Stroke was pressed or Held.</returns>
		public static bool Key(KeyCode KCStroke, bool bHold = false, bool bUp = false)
		{
			if (bUp)
				return Input.GetKeyUp(KCStroke);

			if (bHold)
				return Input.GetKey(KCStroke);
			return Input.GetKeyDown(KCStroke);
		}

		/// <summary>Identical to Input.anyKey.</summary>
		/// <returns>True if a key or a mouse button was pressed.</returns>
		public static bool Any()
		{
			return Input.anyKey;
		}
	}
}