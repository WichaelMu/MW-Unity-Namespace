# MW.Behaviour.FABRIK

Forward and Backward-Reaching Inverse Kinematics.

## FABRIK Placement
Dynamically place legs on uneven ground.

Attach the FABRIK component to the object that needs the dynamic placement functionality.

Drag the Joints of each Leg into the `FABRIK.Legs` and modify the parameters as necessary.
```diff
  ! It is important that the joints are linked by their 'toes', meaning that the limb that makes contact with the ground should have its coordinates originating from the lowest point of that Leg.
```

Unity's `Update()` should calculate everything for you.

FABRIK has many overridable function that you can use to alter or extend its behaviour. See the [Documentation](https://wichaelmu.github.io/MW-Unity-Namespace/Docs/HTML/Behaviour.html) for more information.