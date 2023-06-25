#if RELEASE
using System;
using UnityEngine;

namespace MW.Audio
{
	/// <summary>The settings for audio.</summary>
	/// <decorations decor="[Serializable] public class"></decorations>
	[Serializable]
	public class MSound
	{
		/// <summary>The source of this sound.</summary>
		/// <decorations decor="public AudioClip"></decorations>
		[Tooltip("The source of this sound.")]
		public AudioClip Clip;
		/// <summary>The name of this sound.</summary>
		/// <decorations decor="public string"></decorations>
		[Tooltip("The name of the sound.")]
		public string Name;

		/// <summary>Sound settings.</summary>
		/// <decorations decor="[Range(0, 1)] public float"></decorations>
		[Range(0f, 1f)]
		public float Volume = 1, Pitch = 1;

		/// <summary>Should this sound loop?</summary>
		/// <decorations decor="public bool"></decorations>
		[Tooltip("Should this audio clip loop?")]
		public bool bLoop;

		/// <summary>Should this audio clip automatically play on awake?</summary>
		/// <decorations decor="public bool"></decorations>
		[Tooltip("Should this audio clip automatically play on awake?")]
		public bool bPlayOnAwake;

		/// <summary>Should this audio clip mute?</summary>
		/// <decorations decor="public bool"></decorations>
		[Tooltip("Mute the audio source?")]
		public bool bMute;

		[HideInInspector]
		public AudioSource AudioSourceComponent;
	}
}
#endif // RELEASE