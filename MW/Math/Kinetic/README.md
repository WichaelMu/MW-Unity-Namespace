# MW.Kinetic
This namespace provides utility functions for game development in the Unity Game Engine. It is used for anything in a Scene that can move without any input from the player.

```cs
using MW.Kinetic;
```

This namespace contains computations for Aerodynamics, Homing Velocity and Rotations, computing Jump Forces and Launch Velocities, Projectile Motion and Trajectories, G-Force, and Collision Avoidance.

A total of three classes split these computations:
```cs
public static class Aerodynamics { }
public static class Kinematics { }
public static class Miscellaneous { }
```

<br>

```diff
NOTE:
	These functions have only been tested in a purpose-built game.
	The MW Unity Namespace was purpose-built for an aircraft simulation game. Most functions have been directly ported over to the MW Unity Namespace.
	These functions are purpose-built and may not work in your implementation.
```