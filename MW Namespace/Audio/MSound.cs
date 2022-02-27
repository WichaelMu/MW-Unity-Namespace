using System;
using UnityEngine;

namespace MW.Audio
{
	/// <summary>The settings for audio.</summary>
	[Serializable]
	public class MSound
	{
		/// <summary>The source of this sound.</summary>
		[Tooltip("The source of this sound.")]
		public AudioClip Source;
		/// <summary>The name of this sound.</summary>
		[Tooltip("The name of the sound.")]
		public string Name;

		/// <summary>Sound settings.</summary>
		[Range(0f, 1f)]
		public float Volume = 1, Pitch = 1;

		/// <summary>Should this sound loop?</summary>
		[Tooltip("Should this audio clip loop?")]
		public bool bLoop;

		/// <summary>Should this audio clip automatically play on awake?</summary>
		[Tooltip("Should this audio clip automatically play on awake?")]
		public bool bPlayOnAwake;

		/// <summary>Should this audio clip mute?</summary>
		[Tooltip("Mute the audio source?")]
		public bool bMute;

		[HideInInspector]
		public AudioSource AudioSource;
	}
}
