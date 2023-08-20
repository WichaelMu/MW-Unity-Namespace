# The MW Unity Namespace

Welcome to the MW Namespace Source Code!

A library containing commonly used and helpful classes, methods, functions, and data structures to help with game development in the Unity Engine.

The MW Namespace isn't exclusively restricted to extending the Unity Engine! Most functionality and utilities provided by the namespace can be
used to extend the normal C# language.

## Usage
The MW Unity Namespace was built for ` Unity Engine 2021.1.3f1 ` and previously ` Unity Engine 2020.2.4f1 `.
* Either:
  * See [Building and Compiling MW](#building-and-compiling-mw).
  * [Download](https://github.com/WichaelMu/MW-Unity-Namespace/releases) the latest release of MW.
* Drag MW.dll, and MW.xml if you want documentation, somewhere in your Assets/ folder in your Unity Project.
* Unity will recompile with MW.dll.
* In any C# script in your Project, add:
   ```cs
   using MW;
   ```
   or any other sub-namespace, such as 
   ```cs
   using MW.Math;
   using MW.Math.Magic;
   ```
* You should now have access to the MW Namespace.

You can view the documentation explaining each namespace and its classes in [The Documentation](#the-documentation).

### Building and Compiling MW
MW was developed and built using Visual Studio 2022 and Windows 10.

` cd ` into ` Scripts/ ` and run ` Initialise.bat Release ` to initialise the MW Unity Namespace in the correct order.

To do it manually:
1. Clone or Download this repository with the <b>Download ZIP</b> option under the <b>Code</b> dropdown.
1. If Downloaded, extract the downloaded ZIP into a directory of your choosing.
1. Open ` MW.sln `. This is the main Visual Studio Solution file that contains the MW Namespace, including MTest, MWEditor, MGenerator, and the MSandbox solution.
1. Before you can build MW, you need to ensure that:
    1. You have ` UnityEngine.dll ` and ` Unity.TextMeshPro.dll ` both in ` Extensions/ `. You will need to import your own references to the Unity Engine to the solution. See [Referencing Unity Engine Dependencies](#referencing-unity-engine-dependencies).
    1. You have built Generator.
        * Open the Generator solution.
        * Ensure that the Solution Configuration is set to ` Release `.
        * Ensure that the Solution Platform is set to ` Any CPU `.
        * Build the solution.
        * <b>NOTE: These are the settings that were used to build Generator.</b>
    1. Ensure that Generator.exe is in ` MGenerator/Output/ `. Otherwise, place it there.
    1. You may also need to build MTest - this automatically tests aspects of the MW Namespace and reports any issues.
        * Open the MTest solution.
        * Ensure that the Solution Configuration is set to ` Release `.
        * Ensure that the Solution Platform is set to ` Any CPU `.
        * Build the solution.
        * <b>NOTE: These are the settings that were used to build MTest.</b>
    1. Ensure that MTest.exe is in ` MTest/Binaries/Output/Release/net6.0/ `. Otherwise, place it there.
1. Open and Build the MW solution.
<br>

You should have MW.dll and MW.xml in ` Output/Binaries/Release/netstandard2.0/ `. Otherwise, re-attempt the steps above.

#### Build Configurations
MW uses tow build configurations when building with Visual Studio 2022. These are:
1. RELEASE
1. STANDALONE

These are used to differentiate between code that is ` STANDALONE ` and ` RELEASE `.
* ` STANDALONE ` builds code that makes no reference to Unity Engine binaries and does not need or operate within the Unity Game Engine.
* ` RELEASE ` builds code that relies on Unity Engine binaries and is meant to be run within the Unity Game Engine either during Runtime or during the Unity Editor.

The usage of these two build configuration can be found in C# Pre-Processor Directives.

```cs
#if RELEASE
// Unity-dependent code...
using UnityEngine;

Debug.Log("...");
#endif
```
```cs
#if STANDALONE
// Non-Unity-dependent code...
using System;

Console.WriteLine("...");
#endif
```

The DEBUG configuration can be used if low-level debugging is desired.

#### Referencing Unity Engine Dependencies
MW is an Extension library for the Unity Game Engine and requires Unity Engine binaries.

Browse through your Unity Engine installation for ` UnityEngine.dll ` and ` Unity.TextMeshPro.dll `. Copy these binaries into
` Extensions/ `. Visual Studio should automatically recognise them. Otherwise, add them yourself:
* Right-click on the MW Project -> Add -> Project Reference -> Find ` Extensions/ ` and add both ` UnityEngine.dll ` and ` Unity.TextMeshPro.dll `.

**NOTE**: If you find that source control detected changes in ` MW.csproj ` related to the references of the Unity Binaries, please ignore
the changes and do not commit or push them the changes.

## Namespaces
Below is a list of all the namespaces in the MW module.

### MW
This namespace contains some basic utility functions, containers, and data structures.
* TPair<T, Y> and TTriple<T, Y, U> containers holding two or three variables, respectively.
* A THeap&lt;T&gt; data structure for Maximum or Minimum Heaps.
* A dynamic generic MArray&lt;T&gt; combining the functionality of a List and a Dictionary.
* A Utils class holding many helpful constants and functions.
* A custom three-dimensional Vector representation of coordinates and points.
* A custom three-axis rotations container representing Pitch, Yaw, and Roll.

### MW.Audio
Helps ease the process of playing Audio in games.
* A MonoBehaviour inherited Audio controller used for in-game sounds.
* A class that contains data about an Audio Clip.
* A sound Visualiser.
* A sound Synthesiser with an accompanying list of musical Notes.

### MW.Behaviour
Provides base classes for 2D and 3D players. Also includes:
* Minimum-Heap optimised A* pathfinding.
* A custom Update loop that processes data based on MArray<>.
* An simple method for processing and passing inter-frame information.

### MW.Camera
Camera Controllers for both 2D and 3D games.
* Orthographic panning and mouse clicking logic.
* Billboarding of Transforms to face specific Cameras.
* Camera tracking and following.
  * Includes a Unity implementation of [Unreal Engine's USpringArmComponent](https://docs.unrealengine.com/5.0/en-US/API/Runtime/Engine/GameFramework/USpringArmComponent/).

### MW.Console
Enables arbitrary code execution during runtime for both games and standalone applications.
* Execute functions on specific GameObjects.
* Call methods and functions with any parameter type during runtime.
* In-game developer console with console outputs during a development build.

### MW.Conversion
Currently only provides conversion into [Colours](https://docs.unity3d.com/ScriptReference/Color.html).
* Converts RGB values ranging from 0-255 into Unity's Colour struct.
  * From Vector3, Vector4, and from Hex strings, with alpha channel manipulation.

### MW.Diagnostics
Provides methods and classes used to measure game performance and logging.
* A short-hand version of Debug.Log() with ` Log.P(); `.
* Methods to Log Stacktraces to the Unity Editor Console.
* A stopwatch to track the execution time of game functions.
* Real-time in-game GameObject diagnostics.

### MW.Extensions
Extension methods for core Unity components and extensions to C# programming.
* FNormalise, FMagnitude, and FDistance for UnityEngine Vector3.
* Numerical operations including bit manipulation and NaN checks.
* Generic Object extensions for object Casting, Implement and Is checks.
* Unity Physics extensions for Rigidbody launching and trajectory calculations.

### MW.IO
Shorthand versions of Mouse and Keyboard Input based on [Unity's Input System](https://docs.unity3d.com/ScriptReference/Input.html).
* More accessible methods to register Mouse and Keyboard Down, Hold, or Up Inputs.
* File creation and file reading implementations.
* Shorthands for standard input. (STANDALONE build only).
* Console colour printing shorthands. (STANDALONE build only).

### MW.Math
Game mathematics functions and utilities.
* Acceleration, Acceleration Rate, and Deceleration calculations.
* Predictive Projectile calculations.
* Common Number functions, including:
  * GCD and LCM.
  * Wrapping.
  * Sine and Cosine.
* Common Vector functions, including:
  * Parallel checks.
  * Normalised checks.
  * Vector angle calculations in 2D and 3D.
* Custom MVector interface for Vector3 with fast operations.
* Custom MRotator interface for Quaternions and Euler angles.

#### MW.Math.Magic
Fast approximation algorithms for common mathematical functions using bit magic.
* Fast Inverse Square Root, accurate to +-.00001.
* Fast Square Root, accurate to +-.00001.
* Fast Inverse/Reciprocal, accurate to +-.000008.
* Integral Approximation.
* Fast Arc Sine, accurate to +-.001 radians.
* Fast Arc Cosine, accurate to +-.001 radians.
* Fast Arc Tangent, accurate to +-.00136 radians.
* Fast Arc Tangent 2, accurate to +-.00136 radians.
* Fast Vector Angle, accurate to +-.1 degrees.

### MW.Kinetic
Calculations for any moving GameObject or Rigidbody.
* Aerodynamic Computations.
  * Air resistance. [Provided by NASA](https://www.grc.nasa.gov/www/k-12/airplane/falling.html)
  * Lift at a speed. [Provided by NASA](https://www.grc.nasa.gov/www/k-12/airplane/lifteq.html#:~:text=The%20lift%20equation%20states%20that,times%20the%20wing%20area%20A.&text=For%20given%20air%20conditions%2C%20shape,Cl%20to%20determine%20the%20lift.)
* Homing / Close-In tracking function.
* Projectile Launching / Motion at a pre-determined:
  * Velocity, or
  * Height.
* G-Forces as a directional Vector, or float Force.
* Calculations of Jumping forces, regardless of 2D or 3D gravity.
* <b>Basic</b> Collision Avoidance.

### MW.Memory
Disk and Memory utilities and operations.
* Object saving and reading to binary files.
* Cryptography with AES256 encryption and decryption.

### MW.UI
Utility methods and functions for any in-game on-screen elements.
<br>
Any text-related function assumes that [TextMeshPro](https://docs.unity3d.com/Manual/com.unity.textmeshpro.html) is used.
* Text Typewriting with optional sounds.
* Line and Circle draw functions with a Line Renderer.
* Canvas UI-element scaling.

## The Documentation
Documentation can be found [here](https://wichaelmu.github.io/MW-Unity-Namespace/).

The actual documentation files of MW can be found as ` .html` files in ` Docs/HTML/ `.

Open a file in a Web Browser.
* The left of the page lists the Classes or Namespaces of MW. Navigate using these links.

The Content Area is everything excluding the left navigation menu.

The documentation is structured such that the Main Text in the middle of the Content Area is the name of the class with a summary underneath.

Any non-bolded text (without brackets) are variables (properties or fields) of that class.

Any bolded text (with brackets) are publicly callable methods or functions of that class.

You can see the declaration signature for variables, methods, and functions, showing their visibility, modifier, return type, and attributes (if any).

Underneath the method or function, you will see a summary of what the method or function does, along with any input parameters and return values.

` ref ` or ` out ` parameters are denoted with an ` & ` at the end of the parameter type.

The documentation also has colour-coded parameter types. Any green-highlighted parameter type is a custom type, any aqua/turquoise colour is a primitive type.

The documentation is automatically generated by [MGenerator](https://github.com/WichaelMu/MW-Unity-Namespace/tree/main/MGenerator).
The source code for generation is separate from MW and provides no runtime functionality to the MW namespace.

## About the MW Namespace
The MW Unity Namespace was an idea conceived in December 2020, after four months of developing games in the Unity Engine.

The mission is to provide as much re-usable code in one place that will help game development in Unity.

Other engines, such as CRYENGINE and UnrealEngine, have a significant number of utility classes, methods, and functions that the Unity API lacks.
The MW Unity Namespace aims to bridge the gap by providing implementations for such utilities for the Unity Engine.

The MW Unity Namespace is under constant development.

## Additional Notes
The MW Unity Namespace is purpose-built for a private game project for the simulation of Airplanes.

While most, if not all, methods and functions have gone through testing, some, especially Aerodynamics, are purpose-built and tested only on the project mentioned above, which may not work on your project.
