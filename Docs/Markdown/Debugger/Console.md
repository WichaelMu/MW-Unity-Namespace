# MW.Debugger.Console
A developer Console debugger and helper for arbitrary code execution during runtime.

The following documents example usages of MConsole in a RELEASE configuration. MConsole
can also be used in both RELEASE and STANDALONE builds.

STANDALONE builds of MConsole have some limited functionality compared to RELEASE builds.
Nonetheless, running the `__HELP__` built-in function will print a build-specific help message.

MConsole is defined in:
```cs
using MW.Console;
```

## Example Usage in Unity Engine
`UE_Console.cs`
```cs
using MW.Console;
using UnityEngine;

public class UE_Console : MConsole
{
	public override Type ExecTypes { get => typeof(UE_Console); set; }

	public override void Awake()
	{
		base.Awake(); // Required.

		// ...
	}

	protected override void Update()
	{
		base.Update(); // Required.

		// ...
		
		// Pressing '`' will show the console.`
		if (Input.GetKeyDown(KeyCode.BackQuote))
			ShowConsole();
	}

	public override void OnGUI()
	{
		base.OnGUI(); // Required.

		// ...
	}
}
```
`PlayerController.cs`
```cs
using MW.Console;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public static PlayerController Instance;

	void Awake() { Instance = this; }

	public float Health = 100f;

	public bool IsDead() => Health <= 0f;
	public void Heal(float HealAmount) { Health += HealAmount; }

	// This Exec is marked for arbitrary execution.
	// Execute with:
	//     GiveHealth 2.5
	// to be invoked as:
	//     GiveHealth( 2.5f );
	[Exec("Gives the player Health")]
	public void GiveHealth(float HealthToAdd)
	{
		// ...
	}
}
```
`UnitTest.cs`
```cs
public static class UnitTest
{
	// This Exec will be executed as:
	//     DamageAndHealTest( 90f, 40f );
	// on UE_Console.Awake().
	// You can still call this Exec arbitrarily with custom parameters.
	[Exec("Test Damage and Heal Functions", bExecOnAwake: true, 90f, 40f)]
	public void DamageAndHealTest(float Damage, float Heal)
	{
		PlayerController PC = PlayerController.Instance;

		// Deal 90 damage.
		PC.TakeDamage(Damage);
		if (PC.IsDead())
			Log.E("Damage Test Failed!");

		// Heal 40 health.
		PC.Heal(Heal);
		if (PC.Health != 50f) // 100 - 90 + 40 = 50.
			Log.E("Heal Test Failed!");
	}
}
```

1. Show the Console (press the `` ` `` (backquote) key), to display the Console GUI.
1. You should see a list of executable functions.
1. Type them out, case sensitive, pass any required parameters, and press `Enter`.
1. Code should have executed.

## Example usage in a STANDALONE Build

STANDALONE builds of MConsole are limited to `public static` functions ONLY.

`ExecRunner.cs`
```cs

public class EntryPoint
{
	public static void Main(string[] Args)
	{
		ExecRunner R = new ExecRunner();

		R.Exec(Args);
	}
}

public class ExecRunner : MConsole
{
	public override Type[] ExecTypes => new Type[] { typeof(EntryPoint) };

	[Exec("Execute Function with string, int, float")]
	public static void Function(string S, int I, float F)
	{
		// ...
	}
}

```

1. Compile ExecRunner with `csc`, `dotnet`, `msbuild`, `mono`, or any other C# compiler into a binary file.
1. Run ExecRunner providing command-line arguments of the name of the Exec function, followed by any parameters.
1. Code should have executed.