# MW.MVector
This folder also contains the definitions of MVector, MRotator, and UVector. These are still a part of, and accessed through, the core MW namespace:
```cs
using MW;
```

## MVector
Most of the MVector API is duplicating the functionality of UnityEngine's Vector3. However, there are a few notable differences:
  * Distance, Magnitude, and Normalisation is done using the Fast Inverse Square Root Algorithm, provided in
  ```cs
  using MW.Math.Magic;
  ```
  * Has built-in support for vector-axis rotations,
  * And provides many short-hands for productivity.

```cs
// Implicit Conversion to and from UnityEngine.
MVector M = new Vector3(1, 3, 9);
Vector3 U = new MVector(1, 2, 4);

// The following are approximations, all tested to be within .1% of the real value.
float Distance = MVector.Distance(M, U);
float Magnitude = M.Magnitude;
MVector Normal = M.Normalised;

// Short-hand.
MVector Direction = M > U; // Direction from M to U.
MVector Cross = M ^ U;
float Dot = M | U.

// Rotations.
Quaternion Q = M.Rotation().Quaternion(); // The associated rotation of this directional MVector.
MVector Rotated = M.RotateVector(45f, MVector.Up); // M rotated 45 degrees clockwise about the global Up axis.

// Shader-like Syntax.
MVector XY = M.XY; // Ignores and zeroes the Z Component.
MVector XZ = M.XZ; // Ignores and zeroes the Y Component.
MVector YZ = M.YZ; // Ignores and zeroes the X Component.

```