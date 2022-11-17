# MW.CameraUtils.Tracking

Utilities for basic Camera following and Billboarding.

```cs
using static MW.CameraUtils.Tracking;
```

## Billboarding
Have a UI element, or anything else, constantly face towards the Camera.
```cs
using UnityEngine;

...

Transform Self = transform;
Camera MainCamera = Camera.main;

void LateUpdate()
{
	Billboard(Self, MainCamera);
}
```

## Basic Following (Deprecated & Unsupported)

```diff
- These functions have been deprecated.
- Use SpringArm instead.
```

Have a Camera follow a Target.
```cs
using UnityEngine;

Camera MainCamera = Camera.main;

...

Transform Target = ...;
Vector3 Offset = new Vector3(0, 15f, -10f);

void LateUpdate()
{
	CameraFollow(MainCamera, Target, Offset);
}
```

