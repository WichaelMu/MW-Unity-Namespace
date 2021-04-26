using UnityEngine;
using MW.General;

namespace MW.Conversion {

    public static class Conversion {

        /// <summary>The corresponding colour in RGA using Vector3.</summary>
        /// <param name="colour">The RGB/XYZ channel values, respectively.</param>
        public static Color Colour255(Vector3 colour) {
            colour *= Generic.k1To255RGB;

            for (int i = 0; i < 3; i++)
                colour[i] = Mathf.Clamp(colour[i], 0, 255);

            return new Color(colour.x, colour.y, colour.z);
        }

        /// <summary>The corresponding colour from 0 - 255 in RGB.</summary>
        /// <param name="r">The red value.</param>
        /// <param name="g">The green value.</param>
        /// <param name="b">The blue value.</param>
        public static Color Colour255(int r, int g, int b) {

            float _r = r * Generic.k1To255RGB;
            float _g = g * Generic.k1To255RGB;
            float _b = b * Generic.k1To255RGB;

            return new Color(_r, _g, _b);
        }

        /// <summary>The corresponding colour from 0 - 255 in RGB.</summary>
        /// <param name="r">The red value.</param>
        /// <param name="g">The green value.</param>
        /// <param name="b">The blue value.</param>
        public static Color Colour255(float r, float g, float b) {

            r *= Generic.k1To255RGB;
            g *= Generic.k1To255RGB;
            b *= Generic.k1To255RGB;

            return new Color(r, g, b);
        }

        /// <summary>The corresponding colour in RGBA using Vector4.</summary>
        /// <param name="colour">The RGBA/XYZW channel values, respectivaly.</param>
        public static Color Colour255(Vector4 colour) {
            colour *= General.Generic.k1To255RGB;

            for (int i = 0; i < 4; i++)
                colour[i] = Mathf.Clamp(colour[i], 0, 255);

            return new Color(colour.x, colour.y, colour.z, colour.w);
        }

        /// <summary>The corresponding colour from 0 - 255 in RGBA.</summary>
        /// <param name="r">The red value.</param>
        /// <param name="g">The green value.</param>
        /// <param name="b">The blue value.</param>
        /// <param name="a">The alpha value.</param>
        public static Color Colour255(int r, int g, int b, int a) {

            float _r = r * Generic.k1To255RGB;
            float _g = g * Generic.k1To255RGB;
            float _b = b * Generic.k1To255RGB;
            float _a = a * Generic.k1To255RGB;

            return new Color(_r, _g, _b, _a);
        }

        /// <summary>The corresponding colour from 0 - 255 in RGBA.</summary>
        /// <param name="r">The red value.</param>
        /// <param name="g">The green value.</param>
        /// <param name="b">The blue value.</param>
        /// <param name="a">The alpha value.</param>
        public static Color Colour255(float r, float g, float b, float a) {

            r *= Generic.k1To255RGB;
            g *= Generic.k1To255RGB;
            b *= Generic.k1To255RGB;
            a *= Generic.k1To255RGB;

            return new Color(r, g, b, a);
        }

        ///<summary>Converts a hexadecimal to its corresponding colour.</summary>
        /// <param name="hex">The hexadecimal in the format: "#RRGGBB"; where '#' denotes a hexadecimal, 'RR' denotes the Red colour channel, 'GG' denotes the Green colour channel and 'BB' denotes the Blue colour channel.</param>
        public static Color ColourHex(string hex) {

            if (hex[0] != '#') {
                Debug.LogWarning("Please use '#' to denote Hex.\nReturning White by default.");
                return Color.white;
            }

            if (hex.Length > 7) {
                Debug.LogWarning("Please use the format: '#RRGGBB' for hex to colour conversion.\nReturning White by default");
                return Color.white;
            }

            int nH1 = int.Parse(hex[1] + "" + hex[2] + "", System.Globalization.NumberStyles.HexNumber);
            int nH2 = int.Parse(hex[3] + "" + hex[4] + "", System.Globalization.NumberStyles.HexNumber);
            int nH3 = int.Parse(hex[5] + "" + hex[6] + "", System.Globalization.NumberStyles.HexNumber);

            Vector3 V = new Vector3 {
                x = nH1,
                y = nH2,
                z = nH3
            };

            return Colour255(V);
        }

        /// <summary>The corresponding hexadecimal and alpha colour.</summary>
        /// <param name="hex">The hexadecimal in the format: "#RRGGBB"; where '#' denotes a hexadecimal, 'RR' denotes the Red colour channel, 'GG' denotes the Green colour channel and 'BB' denotes the Blue colour channel.</param>
        /// <param name="alpha">The float alpha.</param>
        public static Color ColourHex(string hex, float alpha) {

            if (hex[0] != '#') {
                Debug.LogWarning("Please use '#' to denote Hex.\nReturning White by default.");
                return Color.white;
            }

            if (hex.Length > 7) {
                Debug.LogWarning("Please use the format: '#RRGGBB' for hex to colour conversion.\nReturning White by default");
                return Color.white;
            }

            int nH1 = int.Parse(hex[1] + "" + hex[2] + "", System.Globalization.NumberStyles.HexNumber);
            int nH2 = int.Parse(hex[3] + "" + hex[4] + "", System.Globalization.NumberStyles.HexNumber);
            int nH3 = int.Parse(hex[5] + "" + hex[6] + "", System.Globalization.NumberStyles.HexNumber);

            Vector4 V = new Vector4 {
                x = nH1,
                y = nH2,
                z = nH3,
                w = alpha
            };

            return Colour255(V);

        }

        /// <summary>The corresponding hexadecimal colour and hexadecimal alpha.</summary>
        /// <param name="hex">The hexadecimal in the format: "#RRGGBB"; where '#' denotes a hexadecimal, 'RR' denotes the Red colour channel, 'GG' denotes the Green colour channel and 'BB' denotes the Blue colour channel.</param>
        /// <param name="alpha">The hexadecimal in the format: "#AA"; where '#' denotes a hexadecimal and 'AA' denotes the Alpha channel.</param>
        public static Color ColourHex(string hex, string alpha) {

            if (hex[0] != '#' || alpha[0] != '#') {
                Debug.LogWarning("Please use '#' to denote Hex.\nReturning White by default.");
                return Color.white;
            }

            if (hex.Length > 7) {
                Debug.LogWarning("Please use the format: '#RRGGBB' for hex to colour conversion.\nReturning White by default");
                return Color.white;
            }

            if (alpha.Length > 3) {
                Debug.LogWarning("Please use the format: '#AA' for hex to alpha conversion.\nReturning White by default");
                return Color.white;
            }

            int nH1 = int.Parse(hex[1] + "" + hex[2] + "", System.Globalization.NumberStyles.HexNumber);
            int nH2 = int.Parse(hex[3] + "" + hex[4] + "", System.Globalization.NumberStyles.HexNumber);
            int nH3 = int.Parse(hex[5] + "" + hex[6] + "", System.Globalization.NumberStyles.HexNumber);
            int nH4 = int.Parse(alpha[1] + "" + alpha[2] + "", System.Globalization.NumberStyles.HexNumber);

            Vector4 V = new Vector4 {
                x = nH1,
                y = nH2,
                z = nH3,
                w = nH4
            };

            return Colour255(V);

        }

        /// <summary>The normalised direction to to, relative to from.</summary>
        /// <param name="from">The Vector3 seeking a direction to to.</param>
        /// <param name="to">The direction to look at.</param>
        public static Vector3 Direction(Vector3 from, Vector3 to) {
            return (to - from).normalized;
        }

        /// <summary>The normalised direction to to, relative to from.</summary>
        /// <param name="from">The Vector3 seeking a direction to to.</param>
        /// <param name="to">The direction to look at.</param>
        public static Vector2 Direction(Vector2 from, Vector2 to) {
            return (to - from).normalized;
        }
    }

}
