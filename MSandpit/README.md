

# MSandpit Environment

MSandpit is an internal testing environment for the MW Unity Namespace.
It is used to test the functionality of the MW Unity Namespace where unit testing with MTest is not possible.

All scripts should exist in ` Assets/MScripts ` so that they are properly ignored and excluded from the repository.

The MW Namespace compiled binaries - ` MW.dll `, ` MW.xml `, and ` MWEditor.dll ` - should be automatically exported to ` Assets/MW ` upon building and compiling MW.
