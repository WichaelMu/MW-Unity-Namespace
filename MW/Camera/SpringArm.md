# MW.CameraUtils.SpringArm
A component which allows tracking, following, and rotation inheritance to the given Target.

While commonly used for Cameras, it can be used for any component that needs to track a specified Target.

## SpringArm
Have a Camera follow a Player in third-person.

Camera Class `CameraSpringArmComponent.cs`. Inheriting is optional if you wish to extend the SpringArm's functionality. See [Documentation](https://wichaelmu.github.io/MW-Unity-Namespace/Docs/HTML/CameraUtils.html) for more details
```cs
using UnityEngine;
using MW.CameraUtils;

public class CameraSpringArmComponent : SpringArm
{
	// ...
}
```

The SpringArm does not have any overridable methods, you will need to create your own. However, the SpringArm has publicly exposed fields that are free to modify.