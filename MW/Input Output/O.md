# MW.IO.O
Helper class for Unity Engine Console Output.

## Out
Prints to the Unity Engine Console.
```cs
Out((int) 1, (uint) 2, (float)3.14159265f, (bool) true, (string) "String", MVector.One, this).

// Console:
//	"1 2 3.14159265 True String X: 1 Y: 1 Z: 1 System.Type"
```
Assumes that any object passed into Out has overrided or has a human-readable `ToString()` implementation.