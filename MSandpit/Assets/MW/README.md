

## Test the MW Namespace with MSandpit

After making any changes, make sure that the correct assemblies are compiled and built into their respective binaries.

**NOTE: There are a number of projects within this MW Solution. Be sure to build the correct one.**
* Assembly-CSharp is MSandpit.
* MGenerator is the automatic documentation generator.
* MTest is the automated unit testing.
* MW is the MW Unity Namespace and the project that contains the source of MW.
* MWEditor is the custom Editor that draws custom inspector GUIs.

Typically, you would build and compile ` MW `. This means modifying the MW Namespace's functionality, documentation, structure, or anything that requires rebuilding and recompiling.
* This should copy the MW binaries over to MSandpit.

Build and compile ` MWEditor ` if you are modifying it. By itself, this doesn't copy ` MWEditor.dll ` binary to MSandpit.
* After building and compiling, build ` MW `.

If you are modifying ` MGenerator `, building it should run MGenerator and generate documentation files, assuming ` MW.xml ` exists.
* You would receive a warning if ` MW.xml ` does not exist in ` MW/Output/Binaries/Release/netstandard2.0/ `. This warning is fine and can be ignored if ` MW ` has never been built, or if ` MW.xml ` is not expected to exist yet.
* After building ` MGenerator `, regardless of the warning, building ` MW ` should automatically generate the documentation.

` MTest ` cannot be tested with MSandpit.