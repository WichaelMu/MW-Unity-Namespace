# MW.Audio.MAudio

This is an audio controller for in-game sounds.

```cs
using MW.Audio;
```

`AudioController.cs`. Drag AudioClips into the `MSound[] Sounds` array.
```cs
using UnityEngine;

public class AudioController : MAudio
{
	void Awake()
	{
		Initialise();
	}
}
```
Play Sounds by Name references.

`SomethingThatPlaysSounds.cs`
```cs
using UnityEngine;

public class SomethingThatPlaysSounds : MonoBehaviour
{
	void Awake()
	{
		MAudio.AudioInstance.Play("Name Of Sound");
		MAudio.AudioInstance.PlayWithOverlap("Overlapping Sound", gameObject, out AudioSource Source);

		if (!MAudio.AudioInstance.IsPlaying("Only Play This Once"))
			MAudio.AudioInstance.Play("Only Play This Once");
	}
}
```