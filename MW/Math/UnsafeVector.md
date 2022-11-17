# MW.UVector
It is memory-inefficient to constantly need to convert to and from MVector and Vector3. Each implicit conversion requires the creation and allocation of 3 new floats, which may not be performant.

This UVector aims to mitigate this memory issue by using pointers to existing Vector3 components. This means that the modification of a Vector3 will automatically modify the linked UVector.

```cs
using MW;

Vector3 V = new Vector3(2f, 4f, 6f);
UVector Clone = UVector.Clone(ref V); // *pX = V.x. *pY = V.y. *pZ = V.z.

// Modifying Vector3.
V.Normalize();
// *Clone.pX = .267...f
// *Clone.pY = .534...f
// *Clone.pZ = .801...f

// Modifying UVector.
*Clone.pX = 4f;      // V.x = 4f
*Clone.pY = 2f;      // V.y = 2f
*Clone.pZ = 0f;      // V.z = 0f

Clone.RotateVector(31.1415f, Vector3.right); // Pitches V and Clone downwards 31.1415 degrees.
Clone.Normalise(); // Fast normalisation of V.
Clone.Rotation(); // Pitch and Yaw in the direction of V. Roll = 0f.
```

UVector should only be used to avoid converting multiple Vector3s to MVectors (and vice versa) to increase performance while using the fast MVector utilities. Because of this, UVector only provides access to the MVector API if specific functions use fast approximations. E.g., `Normalise()` and `Distance()`.

It can also be used to use the MVector's built-in functions, such as `RotateVector()`, `Rotation()`, `XY`, `XZ`, `YZ`;