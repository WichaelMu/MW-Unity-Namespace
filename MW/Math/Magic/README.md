# MW.Math.Magic

This namespace provides fast implementations of common mathematical functions using bit magic. These implementations are approximations only, and have been tested to be accurate to .1% of the real answer.

```cs
using static MW.Math.Magic.Fast;
```

## Fast Square Root and Inverse Square Root
Approximately 1 / √π. Accurate to .1% at the default of 1 additional iteration.
```cs
float RSqrt = FInverseSqrt(3.14159265f, 1);
```
The number of additional iterations is 1 by default and the algorithm automatically applies one iteration.

Knowing how to get 1 / √f, we can get the normal Square Root with (1 / √f) * f.
```cs
float Sqrt = FSqrt(3.14159265f, 1).
```
Where 1 is the number of additional iterations to `FInverseSqrt(..., AdditionalIterations)`.

## Fast Arc Sine
Arc Sine is used to compute the angle of a triangle's opposite and hypotenuse.
```cs
float FASine = FArcSine(1); // 90 Degrees.
```

Commonly used to measure the angle between two vectors.
```cs
using MW;

MVector M1 = new MVector(44f, 33f, 22f);
MVector M2 = new MVector(11f, 22f, 33f);
float Dot = M1 | M2;

Utils.Clamp(ref Dot, -1f, 1f);
float Angle = FArcSine(Dot); // Angle in Radians.
```

Note that Unity uses a different coordinate system in which Sine and Cosine are switched. This is accounted for in:
```cs
Angle = FAngle(M1, M2);
```
Which does this conversion for you. (It's just subtracting 90 from the result in degrees).

## Sine Cosine
This was lightly touched upon in `MW.Math`. While `SinCos()` doesn't calculate with bit magic, it is an approximation accurate to .1% of the real value.
```cs
using System;

float Angle = 90;

float CS_Sine = Math.Sin(Angle);
float CS_Cosine = Math.Cos(Angle);

SinCos(out float MW_Sine, out float MW_Cosine, Angle);

// CS_Sine and MW_Sine are within equivalent within .1%.
// CS_Cosine and MW_Cosine are within equivalent within .1%.
```

## Absolute Value Short-Hand
It can be repetitive and cluttering to calculate and assign the absolute values of numbres in one line.
```cs
using System;

// Typical assignment of Absolute Values.
float F = -42;
F = Math.Abs(F);

// MW.Math.Magic.
float M = -42;
FAbs(ref M);
```
While not much of a short-hand, it is much cleaner.