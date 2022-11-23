# MW.MRotator
This folder also contains the definitions of MVector, MRotator, and UVector. These are still a part of, and accessed through, the core MW namespace:
```cs
using MW;
```

## MRotator
The MRotator API was built to avoid the complexities of Quaternion, complex, imaginary, and matrix mathematics. It also saves Unity's call to C++ when converting Quaternions to Eulers and vice versa.
```cs
MRotator PYR = new MRotator(42.55f, 742.34f, -49.1f);

// Conversion of equivalent rotations.
Quaternion Q = PYR; // Implicit.
MRotator M = Quaternion.identity; // MRotator.Neutral.

MRotator T = transform.rotation.Rotatator();
transform.rotation = PYR.Quaternion();

MVector Face = PYR.MV().Normalised;

// Interpolation of Rotations. Optional Animation Curve.
MRotator.Interpolate(PYR, M, .2f, AnimationCurve);
```