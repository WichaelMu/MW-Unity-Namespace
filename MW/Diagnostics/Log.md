# MW.Diagnostics.Log
Short-hand functions for logging to the Unity Editor console.

## Log
Prints to the Unity Editor console window.

### Normal
Prints to the Unity Editor console in the default verbosity.
```cs
Log.P(1, 2f, true, "three", MVector.One);
```

### Warning
Prints to the Unity Editor console with the Warning verbosity.
```cs
Log.W("Something might be wrong!", 1, 2f, true, "three", MVector.Zero);
```

### Error
Prints to the Unity Editor console with the Error verbosity.
```cs
Log.E("Something has broken!", -1, -2f, false, "eerht", MVector.NaN);
```

### Coloured
Prints to the Unity Editor console with a Colour and EVerbosity.
```cs
string Content = "Now in Colour!";
MVector RGB = new MVector(255, 0, 255); // Purple.
EVerbosity Verbosity = EVerbosity.Log;  // Normal. Default is EVerbosity.Log.

Log.Colourise(Content, RGB, Verbosity);
```