# MW.Console

This namespace provides the API used to help debug and develop games within the Unity Game Engine.

It also provides a powerful Developer Console to enable arbitrary code execution during play.

## Steps to use the Debug Console.

Firstly, you need to be using MW.Console.:

```cs
using MW.Console;
```

This will give you access to MConsole, which you can inherit and use in your game!

Example class:
```cs
using MW.Console;
using UnityEngine;

public class InGameDebugConsole : MConsole
{
	public override Type[] ExecTypes { get => new Type[] { typeof(InGameDebugConsole) }; }

	void Update()
	{
		// Pressing the '`' key shows the Console.
		if (Input.GetKeyDown(KeyCode.BackQuote))
			ShowConsole();
	}

	[Exec("Executes Method and Does Something...")]
	public void Method()
		=> Debug.Log("Do Something...");

	[Exec("Support for Primitive Types")]
	public void MethodWithParameters(float F, int I, string S)
		=> Debug.Log($"{F} {I} {S}");

	[Exec("Support for Vectors, MVectors, and MRotators")]
	public void PredefinedStructs(Vector3 V, MVector V, MRotator R)
		=> Debug.Log($"{V} {V} {R}");

	[Exec("Support for GameObject and Transform Parameters by Name")]
	public void GameObjects(GameObject O, Transform T, MVector M)
	{
		Destroy(O);
		T.position += M;
	}

	[Exec("Supports custom MonoBehaviour classes by GameObject Name")]
	public void CustomMonoBehaviours(MyCustomClass SomeClass, Rigidbody R)
	{
		SomeClass.DoSomething();

		R.useGravity = false;
		R.AddForce(MVector.Up);
	}
}

public class MyCustomClass : MonoBehaviour
{
	public void DoSomething() { ... }
}
```

Attach ` InGameDebugConsole ` to a GameObject in your game's hierarchy.

Note that any method that can be executed and called by MConsole must be marked public and be marked with the ` Exec ` attribute. MConsole can print the return values of functions, but cannot directly pass/pipe them into other functions.

<b>
<span style="color:red">
MConsole uses System.Reflection methods. All Exec methods are cached during Awake and cannot be modified during runtime. For performance reasons, do not allow any MConsole-derived class from being built into a production release.
</span>
</b>