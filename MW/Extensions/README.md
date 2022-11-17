# MW.Extensions
This namespace provides utility extension methods for the Unity Game Engine.

## Fast Vector3 Normalise
Normalise a Vector3 using the Fast Inverse Square Root in `MW.Math.Magic`.
```cs
Transform Target = ...;
Vector3 TargetPosition = Target.position;

// The direction from transform to TargetPosition.
Vector3 Normalised = (TargetPosition - transform.position).FNormalise();
```

## Quaternion-MRotator
Convert a Quaternion to its corresponding Pitch, Yaw, and Roll representation as an MRotator.
```cs
Quaternion Q = transform.rotation;
MRotator R = Q.MakeRotator();

// R is the Pitch, Yaw, and Roll of transform.
```

## Explicit MVector-Vector3 Conversions
Explicitly make an MVector or Vector3 out of the other's components.
```cs

MVector N = MVector.One;
Vector3 U = Vector3.one;

// MVector to Vector3
Vector3 V = N.V3();

// Vector3 to MVector
MVector M = U.MV();
```
Vector3 to MVector conversions can also be done with `UVector.Clone(Vector3)`.