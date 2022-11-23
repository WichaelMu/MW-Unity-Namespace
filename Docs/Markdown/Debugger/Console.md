# MW.Debugger.Console
A developer Console debugger and helper for arbitrary code execution during runtime.

```cs
using MW.Debugger;
```

## Example Usage in Unity Engine
`UE_Console.cs`
```cs
using UnityEngine;

public class UE_Console : Console
{
	public Type TypeAssembly { get => typeof(UE_Console); set; }

	public override void Awake()
	{
		base.Awake();

		// ...
	}

	public override void ShowConsole()
	{
		base.ShowConsole();

		// ...
	}

	public override void OnGUI()
	{
		base.OnGUI(); // Optional. Ignore, if you want to modify how the Console is displayed.

		// ...
	}
}
```
`PlayerController.cs`
```cs
using UnityEngine;
using MW.Debugger;

public class PlayerController : MonoBehaviour
{
	.
	.
	.

	public float Health = 100f;

	public bool IsDead() => Health <= 0f;
	public void Heal(float HealAmount) { Health += HealAmount; }

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
	[Exec(bIsTestExec: true)]
	public void Test()
	{
		// ...
	}

	[Exec("Test Damage and Heal Functions", bOnStart: true, 90f, 100f)]
	public void DamageAndHealTest(float Damage, float Heal)
	{
		PlayerController PC = PlayerController.Instance;

		PC.TakeDamage(Damage);
		if (PC.IsDead())
			Log.E("Damage Test Failed!");

		PC.Heal(Heal);
		if (PC.Health != 100f)
			Log.E("Heal Test Failed!");
	}
}
```

1. Show the Console (press the `` ` `` (backquote) key, by default), to display the Console GUI.
1. You should see a list of executable functions.
1. Type them out, case sensitive, pass any required parameters, and press `Enter`.
1. Code should have executed.