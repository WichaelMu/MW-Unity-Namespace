# MW.Conversion
Converts RGB values into Unity Engine's `Color` system.

```cs
using MW.Conversion;
```

## Usage
```cs
using static MW.Conversion.Colour;
Color Purple = Colour255(new Vector3(255f, 0f, 255f));
Color Hex = ColourHex("FEFE00"); // A bright yellow.

Color HexA = ColourHex("C4C4C4", "33"); // Transparent grey.
Color Num = Colour255(64f, 64f, 64f, 128f); // Half transparent dark grey.
```