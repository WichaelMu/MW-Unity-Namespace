# MW.Debugger.DrawArrow
A Debugger utility for drawing Gizmos and Debug Lines in the form of arrows to show the direction of rays, rather than having a static line which may be misleading.

```cs
using static MW.Debugger.Arrow;
```

## Gizmos
Draw Arrows as Gizmos.
```cs
using UnityEngine;

...

void OnDrawGizmos()
{
	Vector3 Origin = transform.position;
	Vector3 Direction = ...;

	GizmoArrow(Origin, Direction, Color.green);
	// Draws an arrow from transform in Direction in green as a Gizmo.
}
```

## Debug
Draws Arrows using `Debug.DrawLine()`.
```cs
using UnityEngine;

Vector3 Origin = transform.position;
Vector3 Direction = ...;

DebugArrow(Origin, Direction, Color.red);
// Draws an arrow from transform in Direction in red in the Editor only.
```