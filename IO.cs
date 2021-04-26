using UnityEngine;

namespace MW.IO {


    /// <summary>Input/Output.</summary>
    public static class IO {

        /// <param name="Hold">Whether or not to check if this button is held down.</param>
        /// <param name="Up">Whether or not to check if this button is released.</param>
        /// <returns>If the Left Mouse Button was clicked or Held.</returns>
        public static bool LeftClick(bool Hold = false, bool Up = false) {
            if (Up)
                return Input.GetMouseButtonUp(1);

            if (Hold)
                return Input.GetMouseButton(0);
            return Input.GetMouseButtonDown(0);
        }

        /// <param name="Hold">Whether or not to check if this button is held down.</param>
        /// <param name="Up">Whether or not to check if this button is released.</param>
        /// <returns>If the Right Mouse Button was clicked or Held.</returns>
        public static bool RightClick(bool Hold = false, bool Up = false) {
            if (Up)
                return Input.GetMouseButtonUp(1);

            if (Hold)
                return Input.GetMouseButton(1);
            return Input.GetMouseButtonDown(1);
        }

        /// <param name="Hold">Whether or not to check if this button is held down.</param>
        /// /// <param name="Up">Whether or not to check if this button is released.</param>
        /// <returns>If the Middle Mouse Button was clicked or Held.</returns>
        public static bool MiddleClick(bool Hold = false, bool Up = false) {
            if (Up)
                return Input.GetMouseButtonUp(2);

            if (Hold)
                return Input.GetMouseButton(2);
            return Input.GetMouseButtonDown(2);
        }

        /// <param name="Stroke">The key that was pressed on the keyboard.</param>
        /// <param name="Hold">Whether or not to check if this button is held down.</param>
        /// <param name="Up">Whether or not to check if this button is released.</param>
        /// <returns>If Stroke was pressed or Held.</returns>
        public static bool Key(KeyCode Stroke, bool Hold = false, bool Up = false) {
            if (Up)
                return Input.GetKeyUp(Stroke);

            if (Hold)
                return Input.GetKey(Stroke);
            return Input.GetKeyDown(Stroke);
        }
    }
}
