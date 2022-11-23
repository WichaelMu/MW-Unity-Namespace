# MW.Utils
This is a utility class to help with game development in the Unity Game Engine.

It houses many mathematical constants and shorthand functions involving numbers and gameplay statics.

## Example

### LineOfSight
Line of Sight is an overloaded function whereby the inclusion of a LayerMask is optional.
```cs
using MW;
// Utils is a static class, you can also use:
// using static MW.Utils;

Vector Self = transform.position;
Vector3 Target = Enemy.position;

// False if Self is blocked by a gameObject in the 1 << 8 LayerMask.
bool bSelfHasLineOfSightToEnemy = Utils.LineOfSight(Self, Target, 1 << 8)
```

### Round To Decimal Place
Rounds a number to the specified number of decimal places.
```cs
Utils.RoundToDP(1.234f); // 1.23f. 2 by default.
Utils.RoundToDP(1.234f, 1); // 1.2f
Utils.RoundToDP(2.5f, 0); // 3f
Utils.RoundToDP(-5.5f, 0); // -6f
Utils.RoundToDP(5678f, -2f); // 5700f
```

### Boolean Flip-Flop
Flips a boolean and then executes the given Callback functions.
```cs
using System;

void TrueFunc() { Console.WriteLine("True"); }
void FalseFunc() { Console.WriteLine("False"); }

...

bool b = true;
Utils.FlipFlop(ref b, TrueFunc, FalseFunc);
Utils.FlipFlop(ref b, TrueFunc, FalseFunc);

// Output:
//	False.
//		b = false;
//	True.
//		b = true;
```

### Mirror Number
Mirrors a number to be between a minimum and maximum value, such that the difference between the number and the result to the minimum or maximum values are equal.
```cs
int Number = 4;
int Minimum = -10;
int Maximum = +10;
int Mirror = Utils.MirrorNumber(Number, Minimum, Maximum); // -4
// Mirror is +6 away from Minimum. Maximum is -6 away from Number.
```

### Clamp Min, Maxm and Abs
Clamps numbers to be within Min and/or Max, or only if the number exceeds Min or Max.
```cs
int I = 5;
Utils.Clamp(ref I, 7, 15); // I = 7

Utils.ClampMin(ref I, 10); // I = 10
Utils.ClampMin(ref I, 5); // I = 10. Remains 10.

Utils.ClampMax(ref I, 20); // I = 10. Remains 10.
Utils.ClampMax(ref I, 3); // I = 3

int N = -4;
Utils.Abs(ref N); // N = 4
```
These functions also accept floats as overloaded functions.

### Lock Cursor
Locks and hides the in-game mouse cursor.
```cs
using MW;
using UnityEngine;

...

bool bGameIsPaused = false;

void Update()
{
	if (Input.GetKeyUp(KeyCode.P)
		bGameIsPaused = !bGameIsPaused;
	
	Utils.LockCursor(bGameIsPaused, bHideCursorOnLock: true);
}
```

### FPS Counter
Returns the FPS in-game using unscaled time.
```cs
using MW;
using UnityEngine;

...

void OnGUI()
{
	// Print the current Frames Per Second in the top-right of the game viewport.
	// For example: 'FPS: 250'.
	GUI.Label(new Rect(10, 25, 250, 250), $"FPS: {Utils.FPS()}");
}
```