# MW.Kinetic.Aerodynamics
These aerodynamics functions are not strictly limited to air travel.

Because `Aerodynamics` may be too long to consistently type out, use:
```cs
using static MW.Kinetic.Aerodynamics;
````

## Air Resistance
Calculating air resistance uses Unity Engine's `Rigidbody`. `Rigidbody2D` is not supported.
```cs
Rigidbody Physics = GetComponent<Rigidbody>();

...

// Air Resistance strength as a Vector.
Vector3 Resistance = AirResistance(Physics);
```
This function takes a `Rigidbody`'s speed, direction, and drag into account.

## Lift
The calculation of Lift is provided by [NASA](https://www.grc.nasa.gov/www/k-12/airplane/falling.html).
```cs
const float kLiftCoefficient = .03f; // In testing, values between .001f and .05f yield respectable results, but alter at will.
float AirDensity = 1f;
float Speed = 1234.8f;
float WingArea = 3.6f;

float ScaleOfLift = Lift(kLiftCoefficient, AirDensity, Speed, WingArea);
```
To apply lift, we can do the following:
```cs
using UnityEngine;

void FixedUpdate()
{
	Physics.AddForce(transform.up * ScaleOfLift);
}
```