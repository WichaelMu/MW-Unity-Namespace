using UnityEngine;

namespace MW.Conversion
{
	/// <summary>Colour conversions.</summary>
	public static class Colour
	{

		/// <summary>The corresponding colour in RGA using Vector3.</summary>
		/// <param name="vColour">The RGB/XYZ channel values, respectively.</param>
		public static Color Colour255(Vector3 vColour)
		{
			vColour *= Utils.k1To255RGB;

			for (int i = 0; i < 3; i++)
				vColour[i] = Mathf.Clamp(vColour[i], 0, 255);

			return new Color(vColour.x, vColour.y, vColour.z);
		}

		/// <summary>The corresponding colour from 0 - 255 in RGB.</summary>
		/// <param name="r">The red value.</param>
		/// <param name="g">The green value.</param>
		/// <param name="b">The blue value.</param>
		public static Color Colour255(int r, int g, int b)
		{

			float _r = r * Utils.k1To255RGB;
			float _g = g * Utils.k1To255RGB;
			float _b = b * Utils.k1To255RGB;

			return new Color(_r, _g, _b);
		}

		/// <summary>The corresponding colour from 0 - 255 in RGB.</summary>
		/// <param name="r">The red value.</param>
		/// <param name="g">The green value.</param>
		/// <param name="b">The blue value.</param>
		public static Color Colour255(float r, float g, float b)
		{

			r *= Utils.k1To255RGB;
			g *= Utils.k1To255RGB;
			b *= Utils.k1To255RGB;

			return new Color(r, g, b);
		}

		/// <summary>The corresponding colour in RGBA using Vector4.</summary>
		/// <param name="v4Colour">The RGBA/XYZW channel values, respectivaly.</param>
		public static Color Colour255(Vector4 v4Colour)
		{
			v4Colour *= Utils.k1To255RGB;

			for (int i = 0; i < 4; i++)
				v4Colour[i] = Mathf.Clamp(v4Colour[i], 0, 255);

			return new Color(v4Colour.x, v4Colour.y, v4Colour.z, v4Colour.w);
		}

		/// <summary>The corresponding colour from 0 - 255 in RGBA.</summary>
		/// <param name="r">The red value.</param>
		/// <param name="g">The green value.</param>
		/// <param name="b">The blue value.</param>
		/// <param name="a">The alpha value.</param>
		public static Color Colour255(int r, int g, int b, int a)
		{

			float _r = r * Utils.k1To255RGB;
			float _g = g * Utils.k1To255RGB;
			float _b = b * Utils.k1To255RGB;
			float _a = a * Utils.k1To255RGB;

			return new Color(_r, _g, _b, _a);
		}

		/// <summary>The corresponding colour from 0 - 255 in RGBA.</summary>
		/// <param name="r">The red value.</param>
		/// <param name="g">The green value.</param>
		/// <param name="b">The blue value.</param>
		/// <param name="a">The alpha value.</param>
		public static Color Colour255(float r, float g, float b, float a)
		{

			r *= Utils.k1To255RGB;
			g *= Utils.k1To255RGB;
			b *= Utils.k1To255RGB;
			a *= Utils.k1To255RGB;

			return new Color(r, g, b, a);
		}

		///<summary>Converts a hexadecimal to its corresponding colour.</summary>
		/// <param name="sHex">The hexadecimal in the format: "#RRGGBB"; where '#' denotes a hexadecimal, 'RR' denotes the Red colour channel, 'GG' denotes the Green colour channel and 'BB' denotes the Blue colour channel.</param>
		public static Color ColourHex(string sHex)
		{

			if (sHex[0] != '#')
			{
				Debug.LogWarning("Please use '#' to denote Hex.\nReturning White by default.");
				return Color.white;
			}

			if (sHex.Length > 7)
			{
				Debug.LogWarning("Please use the format: '#RRGGBB' for hex to colour conversion.\nReturning White by default");
				return Color.white;
			}

			int nH1 = int.Parse(sHex[1] + "" + sHex[2] + "", System.Globalization.NumberStyles.HexNumber);
			int nH2 = int.Parse(sHex[3] + "" + sHex[4] + "", System.Globalization.NumberStyles.HexNumber);
			int nH3 = int.Parse(sHex[5] + "" + sHex[6] + "", System.Globalization.NumberStyles.HexNumber);

			Vector3 V = new Vector3
			{
				x = nH1,
				y = nH2,
				z = nH3
			};

			return Colour255(V);
		}

		/// <summary>The corresponding hexadecimal and alpha colour.</summary>
		/// <param name="sHex">The hexadecimal in the format: "#RRGGBB"; where '#' denotes a hexadecimal, 'RR' denotes the Red colour channel, 'GG' denotes the Green colour channel and 'BB' denotes the Blue colour channel.</param>
		/// <param name="fAlpha">The float alpha.</param>
		public static Color ColourHex(string sHex, float fAlpha)
		{

			if (sHex[0] != '#')
			{
				Debug.LogWarning("Please use '#' to denote Hex.\nReturning White by default.");
				return Color.white;
			}

			if (sHex.Length > 7)
			{
				Debug.LogWarning("Please use the format: '#RRGGBB' for hex to colour conversion.\nReturning White by default");
				return Color.white;
			}

			int nH1 = int.Parse(sHex[1] + "" + sHex[2] + "", System.Globalization.NumberStyles.HexNumber);
			int nH2 = int.Parse(sHex[3] + "" + sHex[4] + "", System.Globalization.NumberStyles.HexNumber);
			int nH3 = int.Parse(sHex[5] + "" + sHex[6] + "", System.Globalization.NumberStyles.HexNumber);

			Vector4 V = new Vector4
			{
				x = nH1,
				y = nH2,
				z = nH3,
				w = fAlpha
			};

			return Colour255(V);

		}

		/// <summary>The corresponding hexadecimal colour and hexadecimal alpha.</summary>
		/// <param name="sHex">The hexadecimal in the format: "#RRGGBB"; where '#' denotes a hexadecimal, 'RR' denotes the Red colour channel, 'GG' denotes the Green colour channel and 'BB' denotes the Blue colour channel.</param>
		/// <param name="sAlpha">The hexadecimal in the format: "#AA"; where '#' denotes a hexadecimal and 'AA' denotes the Alpha channel.</param>
		public static Color ColourHex(string sHex, string sAlpha)
		{

			if (sHex[0] != '#' || sAlpha[0] != '#')
			{
				Debug.LogWarning("Please use '#' to denote Hex.\nReturning White by default.");
				return Color.white;
			}

			if (sHex.Length > 7)
			{
				Debug.LogWarning("Please use the format: '#RRGGBB' for hex to colour conversion.\nReturning White by default");
				return Color.white;
			}

			if (sAlpha.Length > 3)
			{
				Debug.LogWarning("Please use the format: '#AA' for hex to alpha conversion.\nReturning White by default");
				return Color.white;
			}

			int nH1 = int.Parse(sHex[1] + "" + sHex[2] + "", System.Globalization.NumberStyles.HexNumber);
			int nH2 = int.Parse(sHex[3] + "" + sHex[4] + "", System.Globalization.NumberStyles.HexNumber);
			int nH3 = int.Parse(sHex[5] + "" + sHex[6] + "", System.Globalization.NumberStyles.HexNumber);
			int nH4 = int.Parse(sAlpha[1] + "" + sAlpha[2] + "", System.Globalization.NumberStyles.HexNumber);

			Vector4 V = new Vector4
			{
				x = nH1,
				y = nH2,
				z = nH3,
				w = nH4
			};

			return Colour255(V);

		}

		public static Vector3 VColour(Color CColour)
		{
			return new Vector3(CColour.r / Utils.k1To255RGB, CColour.g / Utils.k1To255RGB, CColour.b / Utils.k1To255RGB);
		}

		public static Vector4 VColourA(Color CColour)
		{
			return new Vector4(CColour.r / Utils.k1To255RGB, CColour.g / Utils.k1To255RGB, CColour.b / Utils.k1To255RGB, CColour.a / Utils.k1To255RGB);
		}
	}
}
