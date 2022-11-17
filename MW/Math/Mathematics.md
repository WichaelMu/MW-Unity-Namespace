# MW.Math
This namespace provides the many mathematical functions used in game development or simple calculations.

To access the MW.Math namespace, we can write the following:
```cs
using MW.Math;
```
Alternatively, because the class `Mathematics` might be too long to type out and may bloat source code, it is possible to use:
```cs
using static MW.Math;
```
which is recommended.

## Acceleration and Deceleration
Calculate the acceleration required to reach a Terminal speed, given the Current Speed, desired Rate of Acceleration, and Acceleration Equation/Curve.
```cs
...

float CurrentSpeed = 541.42f;
float RateOfAcceleration = .5f;
float Terminal = 750f;
float Result = Acceleration(EEquation.Linear, CurrentSpeed, RateOfAcceleration, Terminal);

GetComponent<Rigidbody>.velocity += Result * MVector.Forward;
```
We can also calculate the 'acceleration' required to reach a target speed instantly.
```cs
float CurrentSpeed = 1234.8f;
float TargetSpeed = 0f;
float Result = Deceleration(CurrentSpeed, TargetSpeed);

Rigidbody Physics = GetComponent<Rigidbody>;
Vector3 DirectionOfTravel = Physics.velocity.normalised;
Physics.velocity -= DirectionOfTravel * Result; // Instantly reduce speed to TargetSpeed.
```
The rate of acceleration, given two vectors, can also be computed.
```cs
using UnityEngine;

...

MVector PreviousPosition, CurrentPosition;

void Update()
{
	MVector CurrentPosition = transform.position;

	float Rate = AccelerationRate(PreviousPosition, CurrentPosition, Time.deltaTime);

	PreviousPosition = CurrentPosition;
}
```

## Trajectory Prediction
We can estimate the trajectory of a moving object. Note that these functions assumes a constant speed and as such, is not completely accurate, especially with objects with varying speeds. However, accuracy improves as the object maintains speed.

This function can be used for unguided projectiles.
```cs
Rigidbody Projectile = ...; // E.g., A bullet. (A FLAK round)
Rigidbody Target = ...; // E.g., An aircraft. (An enemy fighter jet)

MVector LaunchPosition = transform.position;
float ProjectileMovementSpeed = 450f;

// Make Prediction.
MVector Prediction = PredictiveProjectile(LaunchPosition, ProjectileMovementSpeed, Target.velocity, Target.position);

// Angle and Launch the Projectile.
Projectile.transform.LookAt(Prediction);
Projectile.velocity = ProjectileMovementSpeed * Prediction.Normalised;
```
For guided projectiles, we can continuously predict a Target's trajectory whilst travelling eventually intercepting the Target.
```cs
using UnityEngine;

Rigidbody Projectile = ...; // E.g., A guided missile. (A surface-to-air missile)
Rigidbody Target = ...; // An aircraft. (An enemy fighter jet)

Projectile.AddForce(Projectile.transform.forward); // Launch the Projectile if it hasn't already.

...

void FixedUpdate()
{
	// Make Prediction and alter course.
	MVector Prediction = PredictiveProjectile(Projectile, Target);
	Projectile.transform.forward = Prediction.Normalised;

	Projectile.AddForce(Projectile.transform.forward);

	// -- The use of this method inside a Fixed Update is optional and 
	// -- is only used to provide an example for use with a missile.
}
```

## Generic Mathematics
```cs
int GCD = GreatestCommonDivisor(4, 8); // 4
int LCM = LowestCommonMultiple(12, 100); // 300

float Wrapped = Wrap(555, 24, 78); // 69
float Angled = Wrap(764, 0, 360); // 44 degrees.

bool bIsPowerOfTwo = IsPowerOfTwo(3); // False.

float AngleInDegrees = 73f;
SinCos(out float Sine, out float Cosine, AngleInDegrees);
// Sine = 0.956304756...f
// Cosine = 0.292371704...f

Vector3 LHS = new Vector3(4, 2, 1);
Vector3 RHS = new Vector3(16, 8, 4);
float SquareDistance = SqrDistance(LHS, RHS); // 241
```

## Vector Mathematics
```cs
MVector M1 = MVector.Forward.RotateVector(.1f, MVector.Up); // Rotate a vector slightly clockwise.
MVector M2 = MVector.Forward;
bool bIsParallel = Parallel(M1, M2); // True

MVector M3 = MVector.One;
bool bIsNormalised = IsNormalised(M3); // False.

MVector From = MVector.Zero;
MVector To = MVector.One.XZ;

// Vector-Quaternion.
Quaternion Q = DirectionToQuat(From, To);
MRotator RQ = Q.MakeRotator(); // Pitch = 0. Yaw = 45f. Roll = 0.
Quaternion QD = V2Q(To);
MRotator RQD = QD.MakeRotator(); // Pitch = 0. Yaw = 45f. Roll = 0.

// Vector-Euler
Vector3 Euler = V2PYR(To); // x = 0. y = 45. z = 0.
```