# How to Contribute to the Development of the MW Unity Namespace

Any and all willing contributors are welcome to add to the development of the MW Unity Namespace through pull requests.

If you think there is a bug, issue, problem, unexpected behaviour, or any type of error, you are free to submit a pull request with a fix. Alternatively, you can always raise an issue.

On the other hand, if you want to add a feature to the namespace, you can do so as well. Please be clear in what your new feature solves and/or how it assists in the development of games or adds to the C# language.

## Naming Convention

__The MW Unity Namespace follows a strict naming convention.__

The source code was written with an indentation rule of __tabs__ set to 8 spaces. This is so that developers do not write large blocks of functions and methods that require horizontal scrolling.

### Standard Naming Rules

The MW Unity Namespace uses PascalCase for everything.

Variables.
``` cs
float fHealth = 100f;  // This was the previous convetion and has since bee disallowed - This is Hungarian Notation and
                       // begins with a lowercase letter.
float Health  = 100f;  // Good.
```
Class.
``` cs
public class customClass {  }  // Not allowed - This is camelCasing the class name.
public class CustomClass {  }  // Good.
public class CustomBehaviour : MonoBehaviour {  }  // Not allowed - Any class deriving from Unity's MonoBehaviour must be
                                                   // prefixed wih an 'M'.
public class McustomBehaviour : MonoBehaviour {  } // Not allowed - The 'M' prefix is not a substitute for PascalCase.
public class MCustomBehaviour : MonoBehaviour {  } // Good.
```
Structs.
``` cs
public struct Vector {  }  // Not allowed - Any structs adding to the functionality to the MW Unity Library must be prefixed
                           // with an 'M', or another appropriate prefix.
public struct MVector {  } // Good.
public struct FVector {  } // Also allowed - In certain cases when a struct is purpose-built to serve a specific function
                           // that will only be used in a specific use-case, you can prefix it with another letter.
			   // See later for more.
```
Interfaces.
``` cs
public interface DraggableElement {  }  // Not allowed - Interfaces must be prefixed with a 'I'.
public interface IDraggableElement {  } // Good.
```

#### Prefixes

Prefixes are used primarily to distinguish between the MW Unity Namespace and other libraries, including Unity Engine's API.

Prefixes should only consist of one letter or be separated by an `_` if using full word. The prefixes currently in use are:
* `M` for classes and structs.
* `I` for interfaces.
* `E` for enums.
* `F` for any approximation algorithm. (Primarily used in ` MW.Math.Magic; `.
* `T` for template/generic classes/structs. This takes the lowest precendence of all other prefixes. E.g., This does not apply to interfaces, or classes, etc.
* `b` for Booleans. (See below for more)
* `k` for constants. (See below for more)
* `Internal_` for internal methods for exclusive use within the MW Unity Namespace.

Feel free to add new prefixes where you think it is appropriate.

### Booleans
Booleans must be prefixed with a `b`, must ask a question whereby `true` will answer that question with a "Yes", and must not include any contradicting characteristics that may be otherwise confusing.

For example,
``` cs 
bool IsPlayerAboveGround = false;
```
is disallowed because it breaks the `b` prefix, even though it satisfies the 2nd and 3rd Boolean conventions.

``` cs
bool bIsPlayerNotNotDead = true;
```
is also not allowed because it violates the 3rd Boolean convention - it includes a "Not" in the question. In this case, this is equivalent and should be renamed to:
``` cs
bool bIsPlayerDead = false;
```
This makes it clear that the player is not dead because it is set to ` false `. The previous example may be confusing to some when having to frequently flip ` true ` and ` false ` around between the meaning of the name and its value.

### Constants
Constants must be prefixed with a `k`.

For example,
```cs
float MaxHealth = 100f  // Not allowed - constants must be prefixed with a 'k' for clarity.
float kMaxHealth = 100f // Good.
```

## Coding Standards
The MW Unity Namespace also has strict coding standards.

## Brackets, Parentheses, and Square Brackets
Braces, or curly brackets, `{` and `}`, must be placed on their own line, regardless of the type of scope they are covering.

The following example:
``` cs
public class MBehaviour<T> {
	
	public void Function() {
		if (true) {
		}
	}
}
```
is an illegal bracket formation. Instead, it should look like the following:
``` cs
public class MBehaviour<T>
{	
	public void Function()
	{
		if (true)
		{
		}
	}
}
```
If an ` if ` statement executes exactly one line, it is fine to do:
``` cs
if (true)
	Function();
```
However, if that ` if ` statement also contains an ` else ` or an ` else if ` block, you must insert braces on all conditions.

For example, the following is not allowed.
``` cs
if (true)
	Function();
else if (false)
	throw new Exception();
else
	return;
```

There mustn't be a space between any brackets of any sort.
The following are all disallowed because of the spaceing between the method or parameters and the name of the function.
``` cs
var R = Array [0];
var G = Function ();
var P = Function( 4f, this );
var M = GetComponent <MBehaviour>();
```

## Documentation
Any and all public methods of the MW Unity Namespace must be accompanied with XML documentation with a summary and a return value (if applicable).

The langauge we use for documentation is __British English__. If there are any discrepancies between British and Australian English, you shuold always fall towards Australian English. When referencing other libraries or code, such as the UnityEngine API, it is fine to use their American counterparts.

This means that referencing ` UnityEngine.Color ` in XML documentation is fine *only* if you are referencing the UnityEngine struct, and not the word "Colour".

When writing documentation for function parameters, you must never use the parameter's name in its own description, unless it is completely clear what the parameter does and can be clearly inferred from the name.

## Important Note
These naming conventions only apply to the C# side of the MW Unity Namespace. This means that the source code for: ` MW `, ` MTest `, and ` MWEditor ` are all subject to these naming conventions. ` MGenerator ` is not as it is written in C++.






