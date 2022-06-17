# The MW Unity Namespace

Welcome to the MW Namespace Source Code!
<br>
<br>
A library containing helpful classes, methods, functions, and data structures to help with game development in the Unity Engine.
<br>

## Usage
The MW Unity Namespace was built for ` Unity Engine 2021.1.3f1 ` and previously ` Unity Engine 2020.2.4f1 `.
* Either:
  * Clone, Compile and Build MW as a C# Class Library using .NET Standard 2.0. (MW was developed and built using Visual Studio 2022).
  * Download the latest release of MW.
* Drag MW.dll, and MW.xml if you want documentation, somewhere in your Assets/ folder in your Unity Project.
* Unity will recompile with MW.dll.
* In any C# script in your Project, add:
   <br>
   ` using MW; `,
   <br>
   or any other sub-namespace, such as 
   <br>
   ` using MW.Math; `.
* You should now have access to the MW Namespace.

You can view the documentation explaining each namespace and its classes in [The Documentation](#the-documentation).

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

### MW.Conversion
Currently only provides conversion into [Colours](https://docs.unity3d.com/ScriptReference/Color.html).
* Converts RGB values ranging from 0-255 into Unity's Colour struct.
  * From Vector3, Vector4, and from Hex strings, with alpha channel manipulation.

### MW.Diagnostics
Provides methods and classes used to measure game performance and logging.
* A short-hand version of Debug.Log() with ` Log.P();`.
* Methods to Log Stacktraces to the Unity Editor Console.
* A stopwatch to track the execution time of game functions.

### MW.IO
Shorthand versions of Mouse and Keyboard Input based on [Unity's Input System](https://docs.unity3d.com/ScriptReference/Input.html).
* More accessible methods to register Mouse and Keyboard Down, Hold, or Up Inputs.

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

### MW.HUD
Utility methods and functions for any in-game on-screen elements.
<br>
Any text-related function assumes that [TextMeshPro](https://docs.unity3d.com/Manual/com.unity.textmeshpro.html) is used.
* Text Typewriting with optional sounds.
* Line and Circle draw functions with a Line Renderer.
* Canvas UI-element scaling.

## The Documentation
Documentation can be found [here](https://wichaelmu.github.io/MW-Unity-Namespace/).
<br>
<br>
The actual documentation files of MW can be found as ` .html` files in ` Docs/HTML/ `.
<br>
Open a file in a Web Browser.
* The left of the page lists the Classes or Namespaces of MW. Navigate using these links.

The Content Area is everything excluding the left navigation menu.
<br>
<br>
The documentation is structured such that the Main Text in the middle of the Content Area is the name of the class with a summary underneath.
<br>
<br>
Any non-bolded text (without brackets) are variables (properties or fields) of that class.
<br>
Any bolded text (with brackets) are publicly callable methods or functions of that class.
<br>
<b>Currently, there is no way of determining whether a function is statically called or called through an instance.</b>
<br>
<br>
Underneath the method or function, you will see a summary of what the method or function does, along with any input parameters and return values.
<br>
<br>
` ref ` or ` out ` parameters are denoted with an ` & ` at the end of the parameter type.
<br>
<br>
The documentation is automatically generated. The source code for generation is separate from MW. The Documentation Generator can be found [here](https://github.com/WichaelMu/MW-Unity-Namespace/tree/main/Docs/Generator).

## About the MW Namespace
The MW Unity Namespace was an idea conceived in December 2020, after four months of developing games in the Unity Engine.
<br>
<br>
The mission is to provide as much re-usable code in one place that will help game development in Unity.
<br>
<br>
Other engines, such as CRYENGINE and UnrealEngine, have a significant number of utility classes, methods, and functions that the Unity API lacks.
The MW Unity Namespace aims to bridge the gap by providing implementations for such utilities for the Unity Engine.
<br>
<br>
The MW Unity Namespace is under constant development.

## Additional Notes
The MW Unity Namespace is purpose-built for a private game project for the simulation of Airplanes.
<br>
<br>
While most, if not all, methods and functions have gone through testing, some, especially Aerodynamics, are purpose-built and tested only on the project mentioned above, which may not work on your project.
