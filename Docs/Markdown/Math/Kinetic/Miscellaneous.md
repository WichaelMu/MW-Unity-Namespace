# MW.Kinetic.Miscellaneous
Currently, Miscellaneous only provides collision avoidance functionality.

Because `Miscellaneous` may be too long to consistently type out, use:
```cs
using static MW.Kinetic.Miscellaneous;
```

## Collision Avoidance
```cs
using UnityEngine;

Transform Self = transform;
float SearchAngle = 90f; // Degrees.
float SearchDistance = 500f; // Units.
LayerMask Obstacles = 1 << 8;
bool bDebug = true;

Vector3 AvoidancePoint = CollisionAvoidance(Self, SearchAngle, SearchDistance, Obstacles, bDebug);

// AvoidancePoint is the position in world-space where Self should head towards to avoid a
// collision with Obstacles.
```