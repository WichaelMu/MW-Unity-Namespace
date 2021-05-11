using UnityEngine;

namespace MW.IO {

    public static class Mouse {

        public enum Button {
            LeftMouse,
            MiddleMouse,
            RightMouse
		}

        /// <param name="BMouse">The mouse press to listen for.</param>
        /// <param name="bHold">Whether or not to check if this button is held down.</param>
        /// <param name="bUp">Whether or not to check if this button is released.</param>
        /// <returns>If the BMouse was clicked or held.</returns>
        public static bool Click(Button BMouse, bool bHold = false, bool bUp = false) {
            switch(BMouse) {
                case Button.LeftMouse:
                    if (bUp)
                        return Input.GetMouseButtonUp(1);

                    if (bHold)
                        return Input.GetMouseButton(0);
                    return Input.GetMouseButtonDown(0);
                case Button.RightMouse:
                    if (bUp)
                        return Input.GetMouseButtonUp(1);

                    if (bHold)
                        return Input.GetMouseButton(1);
                    return Input.GetMouseButtonDown(1);
                case Button.MiddleMouse:
                    if (bUp)
                        return Input.GetMouseButtonUp(2);

                    if (bHold)
                        return Input.GetMouseButton(2);
                    return Input.GetMouseButtonDown(2);
			}

            return false;
        }
    }

    public static class Keyboard {

        /// <param name="KCStroke">The key that was pressed on the keyboard.</param>
        /// <param name="bHold">Whether or not to check if this button is held down.</param>
        /// <param name="bUp">Whether or not to check if this button is released.</param>
        /// <returns>If Stroke was pressed or Held.</returns>
        public static bool Key(KeyCode KCStroke, bool bHold = false, bool bUp = false) {
            if (bUp)
                return Input.GetKeyUp(KCStroke);

            if (bHold)
                return Input.GetKey(KCStroke);
            return Input.GetKeyDown(KCStroke);
        }
    }
}
