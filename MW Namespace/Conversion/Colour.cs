using UnityEngine;

namespace MW.Conversion
{
	/// <summary>Colour conversions.</summary>
	public static class Colour
	{

		/// <summary>The corresponding colour in RGA using Vector3.</summary>
		/// <param name="Colour">The RGB/XYZ channel values, respectively.</param>
		public static Color Colour255(Vector3 Colour)
		{
			Colour *= Utils.k1To255RGB;

			for (int i = 0; i < 3; i++)
				Colour[i] = Mathf.Clamp(Colour[i], 0, 255);

			return new Color(Colour.x, Colour.y, Colour.z);
		}

		/// <summary>The corresponding colour from 0 - 255 in RGB.</summary>
		/// <param name="R">The red value.</param>
		/// <param name="G">The green value.</param>
		/// <param name="B">The blue value.</param>
		public static Color Colour255(int R, int G, int B)
		{
			float r = R * Utils.k1To255RGB;
			float g = G * Utils.k1To255RGB;
			float b = B * Utils.k1To255RGB;

			return new Color(r, g, b);
		}

		/// <summary>The corresponding colour from 0 - 255 in RGB.</summary>
		/// <param name="R">The red value.</param>
		/// <param name="G">The green value.</param>
		/// <param name="B">The blue value.</param>
		public static Color Colour255(float R, float G, float B)
		{

			R *= Utils.k1To255RGB;
			G *= Utils.k1To255RGB;
			B *= Utils.k1To255RGB;

			return new Color(R, G, B);
		}

		/// <summary>The corresponding colour in RGBA using Vector4.</summary>
		/// <param name="ColourWithAlpha">The RGBA/XYZW channel values, respectively.</param>
		public static Color Colour255(Vector4 ColourWithAlpha)
		{
			ColourWithAlpha *= Utils.k1To255RGB;

			for (int i = 0; i < 4; i++)
				ColourWithAlpha[i] = Mathf.Clamp(ColourWithAlpha[i], 0, 255);

			return new Color(ColourWithAlpha.x, ColourWithAlpha.y, ColourWithAlpha.z, ColourWithAlpha.w);
		}

		/// <summary>The corresponding colour from 0 - 255 in RGBA.</summary>
		/// <param name="R">The red value.</param>
		/// <param name="G">The green value.</param>
		/// <param name="B">The blue value.</param>
		/// <param name="A">The alpha value.</param>
		public static Color Colour255(int R, int G, int B, int A)
		{

			float r = R * Utils.k1To255RGB;
			float g = G * Utils.k1To255RGB;
			float b = B * Utils.k1To255RGB;
			float a = A * Utils.k1To255RGB;

			return new Color(r, g, b, a);
		}

		/// <summary>The corresponding colour from 0 - 255 in RGBA.</summary>
		/// <param name="R">The red value.</param>
		/// <param name="G">The green value.</param>
		/// <param name="B">The blue value.</param>
		/// <param name="A">The alpha value.</param>
		public static Color Colour255(float R, float G, float B, float A)
		{

			R *= Utils.k1To255RGB;
			G *= Utils.k1To255RGB;
			B *= Utils.k1To255RGB;
			A *= Utils.k1To255RGB;

			return new Color(R, G, B, A);
		}

		///<summary>Converts a hexadecimal to its corresponding colour.</summary>
		/// <param name="Hex">The hexadecimal in the format: "#RRGGBB"; where '#' denotes a hexadecimal, 'RR' denotes the Red colour channel, 'GG' denotes the Green colour channel and 'BB' denotes the Blue colour channel.</param>
		public static Color ColourHex(string Hex)
		{

			if (Hex[0] != '#')
			{
				Debug.LogWarning("Please use '#' to denote Hex.\nReturning White by default.");
				return Color.white;
			}

			if (Hex.Length > 7)
			{
				Debug.LogWarning("Please use the format: '#RRGGBB' for hex to colour conversion.\nReturning White by default");
				return Color.white;
			}

			int R = int.Parse(Hex[1] + "" + Hex[2] + "", System.Globalization.NumberStyles.HexNumber);
			int G = int.Parse(Hex[3] + "" + Hex[4] + "", System.Globalization.NumberStyles.HexNumber);
			int B = int.Parse(Hex[5] + "" + Hex[6] + "", System.Globalization.NumberStyles.HexNumber);

			Vector3 V = new Vector3
			{
				x = R,
				y = G,
				z = B
			};

			return Colour255(V);
		}

		/// <summary>The corresponding hexadecimal and alpha colour.</summary>
		/// <param name="Hex">The hexadecimal in the format: "#RRGGBB"; where '#' denotes a hexadecimal, 'RR' denotes the Red colour channel, 'GG' denotes the Green colour channel and 'BB' denotes the Blue colour channel.</param>
		/// <param name="Alpha">The float alpha.</param>
		public static Color ColourHex(string Hex, float Alpha)
		{

			if (Hex[0] != '#')
			{
				Debug.LogWarning("Please use '#' to denote Hex.\nReturning White by default.");
				return Color.white;
			}

			if (Hex.Length > 7)
			{
				Debug.LogWarning("Please use the format: '#RRGGBB' for hex to colour conversion.\nReturning White by default");
				return Color.white;
			}

			int R = int.Parse(Hex[1] + "" + Hex[2] + "", System.Globalization.NumberStyles.HexNumber);
			int G = int.Parse(Hex[3] + "" + Hex[4] + "", System.Globalization.NumberStyles.HexNumber);
			int B = int.Parse(Hex[5] + "" + Hex[6] + "", System.Globalization.NumberStyles.HexNumber);

			Vector4 V = new Vector4
			{
				x = R,
				y = G,
				z = B,
				w = Alpha
			};

			return Colour255(V);

		}

		/// <summary>The corresponding hexadecimal colour and hexadecimal alpha.</summary>
		/// <param name="Hex">The hexadecimal in the format: "#RRGGBB"; where '#' denotes a hexadecimal, 'RR' denotes the Red colour channel, 'GG' denotes the Green colour channel and 'BB' denotes the Blue colour channel.</param>
		/// <param name="Alpha">The hexadecimal in the format: "#AA"; where '#' denotes a hexadecimal and 'AA' denotes the Alpha channel.</param>
		public static Color ColourHex(string Hex, string Alpha)
		{

			if (Hex[0] != '#' || Alpha[0] != '#')
			{
				Debug.LogWarning("Please use '#' to denote Hex.\nReturning White by default.");
				return Color.white;
			}

			if (Hex.Length > 7)
			{
				Debug.LogWarning("Please use the format: '#RRGGBB' for hex to colour conversion.\nReturning White by default");
				return Color.white;
			}

			if (Alpha.Length > 3)
			{
				Debug.LogWarning("Please use the format: '#AA' for hex to alpha conversion.\nReturning White by default");
				return Color.white;
			}

			int R = int.Parse(Hex[1] + "" + Hex[2] + "", System.Globalization.NumberStyles.HexNumber);
			int G = int.Parse(Hex[3] + "" + Hex[4] + "", System.Globalization.NumberStyles.HexNumber);
			int B = int.Parse(Hex[5] + "" + Hex[6] + "", System.Globalization.NumberStyles.HexNumber);
			int A = int.Parse(Alpha[1] + "" + Alpha[2] + "", System.Globalization.NumberStyles.HexNumber);

			Vector4 V = new Vector4
			{
				x = R,
				y = G,
				z = B,
				w = A
			};

			return Colour255(V);

		}

		/// <summary>Converts a Colour to its RGB values as an MVector.</summary>
		/// <param name="Colour">The colour to split.</param>
		/// <returns>An MVector where X = R, Y = G, and Z = B in 255 RGB format.</returns>
		public static MVector Get255RGB(Color Colour)
		{
			return new MVector(Colour.r / Utils.k1To255RGB, Colour.g / Utils.k1To255RGB, Colour.b / Utils.k1To255RGB);
		}

		/// <summary>Converts a Colour to its RGBA values as a Vector4.</summary>
		/// <param name="Colour">The colour to split.</param>
		/// <returns>A Vector4 where x = R, y = G, z = B, and w = A in 255 RGBA format.</returns>
		public static Vector4 Get255RGBA(Color Colour)
		{
			return new Vector4(Colour.r / Utils.k1To255RGB, Colour.g / Utils.k1To255RGB, Colour.b / Utils.k1To255RGB, Colour.a / Utils.k1To255RGB);
		}
	}
}
