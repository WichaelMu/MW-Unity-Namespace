# MW.Kinetic.Kinematics
MW.Kinematics is used for movable physics-based objects strictly for use in the Unity Game Engine.

Because `Kinematics` may be too long to consistently type out, use:
```cs
using static MW.Kinetic.Kinematics;
```

## Homing Missile Behaviour
Used for weapons systems that require homing-like behaviour, such as an fighter jet's heat seeking missile.

Consider a Missile, with a `Rigidbody`, and an locked-on Target `Transform`:
```cs
using UnityEngine;

Rigidbody Missile = ...;
Transform Target = ...;

float MissileTerminalSpeed = 1234.8f * 5f; // Hyper-sonic.
float MissileMaxTurnDegreesDelta = 15f;

...

void FixedUpdate()
{
	HomeTowards(Missile, Target, MissileTerminalSpeed, MissileMaxTurnDegreesDelta);

	// Missile will track and intercept Target at MissileTerminalSpeed, and can
	// turn MissileMaxturnDegreesDelta every FixedUpdate.
}
```
`Rigidbody2D` Missiles and `Vector3` Targets also exist as overloaded methods.

## Launch Velocity
Compute the required velocity to launch a projectile towards an intended target, with accuracy.

### Given Projectile Speed
This function is used for calculating velocities for projectiles with a pre-determined speed.
```cs
Rigidbody Projectile = GetComponent<Rigidbody>();
Vector3 Origin = Projectile.position;
Vector3 Target = ...;
float LaunchSpeed = 512f;

// There are two options:
// False = Faster time to Target, lower peak altitude.
// True = Slower time to Target, higher peak altitude.
bool bUseHighArc = false;

bool bUse3DGravity = true; // True for Physics.gravity, otherwise Physics2D.gravity. Default is true.

...

if (LaunchTowards(out MVector Velocity, Origin, Target, LaunchSpeed, bUseHighArc, bUse3DGravity))
{
	Projectile.velocity = Velocity;
}
else
{
	// Unable to launch Projectile to Target from Origin at LaunchSpeed.
}
```

### Given Desired Peak Projectile Altitude.
Compute the required velocity to launch a projectile towards an intended target, while also reaching a set maximum altitude, irrespective of speed.
```cs
Rigidbody Projectile = GetComponent<Rigidbody>();
Vector3 Origin = Projectile.position;
Vector3 Target = ...;
float DesiredHeight = 4f;
bool bUse3DGravity = true; // Default is true.
bool bLaunchRegardless = false; // Default is false. True to ignore Desired Height limitations.

...

MVector Velocity = LaunchTowards(Origin, Target, DesiredHeight, out float TimeToTarget, bUse3DGravity, bLaunchRegardless);
if (Time != float.NaN)
{
	Projectile.velocity = Velocity;

	// out float TimeToTarget is the time the Projectile will remain in the airborne.
}
else
{
	// If either Time is NaN or any component of Velocity is NaN,
	// then the Projectile cannot be launched towards Target whilst also 
	// reaching a Desired Height.
}
```

## Projectile Trajectory and Arc
Given two reference points of a projectile in motion, we can calculate its trajectory.
```cs
MVector ProjectileVelocity = ...;      // The speed and direction any moving object.
MVector ProjectilePosition = ...;      // The position of the Projectile.
float GravityMagnitude = -9.81f;
int Resolution = 30;                   // Default is 30.
bool bDrawDebug = false;               // Default is false.
const float kMaxSimulationTime = 100f; // Default is 100.

// Get the points along the Projectile's Trajectory.
MArray<ProjectileArcTracer> Arc = GetProjectileArc(ProjectileVelocity, ProjectilePosition, GravityMagnitude, Resolution, bDrawDebug, kMaxSimulationTime);

// If bDrawDebug is true, a Debug line will be traced out in the Unity Editor Viewport
// from Green-Fast to Red-Slow sections of the Projectile's trajectory.

ProjectileArcTracer PAT = Arc[0];
PAT.Position; // The world-position of the Projectile's trajectory at the provided Time.
PAT.Velocity; // The velocity of the Projectile at the provided Time.
PAT.Time;     // The associated Time.
```

### Projectile Trajectory with Collision Checks
Same as above, but we can also check if and where the Projectile may collide with something.
```cs
// Same as before...
MVector ProjectileVelocity = ...;      // The speed and direction any moving object.
MVector ProjectilePosition = ...;      // The position of the Projectile.
float GravityMagnitude = -9.81f;
int Resolution = 30;                   // Default is 30.
bool bDrawDebug = false;               // Default is false. Will also show Collisions.
const float kMaxSimulationTime = 100f; // Default is 100.

// Addition information required for Collision checks...
GameObject Projectile = ...;       // GameObject of the Projectile.
float ProjectileRadius = .5f;
LayerMask CollisionLayer = 1 << 8; // The layer to consider collisions.
bool bStopOnCollision = true;      // True to stop simulating and calculating the Projectile's trajectory. Default is true.

bool bWillCollide = GetProjectileArc(Projectile, ProjectileVelocity, ProjectilePosition, GravityMagnitude,
	out MArray<ProjectileArcTracer> Trajectory, Projectile Radius, CollisionLayer,
	out MArray<ProjectileArcCollision> Collisions, bStopOnCollision,
	Resolution, bDrawDebug, kMaxSimulationTime);

if (bWillCollide)
{
	ProjectileArcTracer PAT = Trajectory[0]; // Same as above.

	ProjectileArcCollision PAC = Collisions[0];
	PAC.Point;    // The position of the Collision.
	PAC.Velocity; // The velocity upon collision.
	PAC.Collider; // The impact Collider.

	if (bStopOnCollision)
		float TimeOfCollision = PAT[Num - 1].Time;
}
else
{
	// The Projectile of ProjectileRadius won't collide with anything of CollisionLayer.
}
```

## G-Force
This is purpose-built for Aircraft simulation.
The G-Force experienced by something by measuring the displacement between two points over time, under the pull of gravity.
```cs
using UnityEngine;

...

MVector LastPosition, CurrentPosition;

void FixedUpdate()
{
	CurrentPosition = transform.position;
	Vector3 Gravity = Physics.gravity;

	// The direction and force of acceleration experienced by transform.
	MVector GForce = V_GForce(LastPosition, CurrentPosition, Time.fixedDeltaTime, Gravity);

	LastPosition = CurrentPosition;
}
```

## Jump Velocity
Compute the force required to push an object to a certain height.
```cs
Rigidbody Physics = GetComponent<Rigidbody>();
MVector Direction = MVector.Up;
float TargetHeight = 5f;
bool bUse3DGravity = true; // Default is true.

Physics.velocity = ComputeJumpVelocity(Direction, TargetHeight, bUse3DGravity);
```