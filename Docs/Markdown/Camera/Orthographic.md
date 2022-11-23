# MW.CameraUtils.Orthographic

Utilities for orthographic cameras or games in 2D.

```cs
using static MW.CameraUtils.Orthographic;
```

## Raycast Under Mouse
Fires a ray under the mouse.
```cs
using UnityEngine;

Camera MainCamera = Camera.main;

...

OrthographicRaycast OR = Raycast(MainCamera);
if (OR.bHit) // If the Raycast hit something.
	OR.Raycast; // The Raycast2D information.
```

## 2D Panning
Pan the camera around using the Mouse.
```cs
using UnityEngine;

Camera MainCamera = Camera.main;

...

EButton ButtonToPan = EButton.MiddleMouse;
float PanSpeed = .25f;
Pan(MainCamera, ButtonToPan, PanSpeed);
```