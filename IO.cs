using UnityEngine;

namespace MW.IO {


    /// <summary>Input/Output.</summary>
    public static class IO {

        /// <param name="bHold">Whether or not to check if this button is held down.</param>
        /// <param name="bUp">Whether or not to check if this button is released.</param>
        /// <returns>If the Left Mouse Button was clicked or Held.</returns>
        public static bool LeftClick(bool bHold = false, bool bUp = false) {
            if (bUp)
                return Input.GetMouseButtonUp(1);

            if (bHold)
                return Input.GetMouseButton(0);
            return Input.GetMouseButtonDown(0);
        }

        /// <param name="bHold">Whether or not to check if this button is held down.</param>
        /// <param name="bUp">Whether or not to check if this button is released.</param>
        /// <returns>If the Right Mouse Button was clicked or Held.</returns>
        public static bool RightClick(bool bHold = false, bool bUp = false) {
            if (bUp)
                return Input.GetMouseButtonUp(1);

            if (bHold)
                return Input.GetMouseButton(1);
            return Input.GetMouseButtonDown(1);
        }

        /// <param name="bHold">Whether or not to check if this button is held down.</param>
        /// /// <param name="bUp">Whether or not to check if this button is released.</param>
        /// <returns>If the Middle Mouse Button was clicked or Held.</returns>
        public static bool MiddleClick(bool bHold = false, bool bUp = false) {
            if (bUp)
                return Input.GetMouseButtonUp(2);

            if (bHold)
                return Input.GetMouseButton(2);
            return Input.GetMouseButtonDown(2);
        }

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
