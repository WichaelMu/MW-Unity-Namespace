# MW.Easing
This namespace provides interpolation equations for use with lerping or numerical smoothing.

```cs
using MW.Easing;
```

## Interpolate
```cs
using UnityEngine;

...

float Alpha = 0f;

void Update()
{
	EEquation Equation = EEquation.EaseOutSine;
	float Start = 0f;
	float End = 1f;

	Alpha += Time.deltaTime;

	float Interp = Interpolate.Ease(Equation, Start, End, Alpha);

	// Alpha->0 = Start.
	// Alpha->1 = End.
}
```