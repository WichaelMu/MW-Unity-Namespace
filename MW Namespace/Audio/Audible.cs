﻿using System;
using UnityEngine;
using MW.Diagnostics;

namespace MW.Audio
{
	/// <summary>The settings for audio.</summary>
	[Serializable]
	public class MSound
	{
		/// <summary>The source of this sound.</summary>
		[Tooltip("The source of this sound.")]
		public AudioClip ACSource;
		/// <summary>The name of this sound.</summary>
		[Tooltip("The name of the sound.")]
		public string Name;

		/// <summary>Sound settings.</summary>
		[Range(0f, 1f)]
		public float fVolume = 1, fPitch = 1;

		/// <summary>Should this sound loop?</summary>
		[Tooltip("Should this audio clip loop?")]
		public bool bLoop;

		[HideInInspector]
		public AudioSource ASSound;
	}

	/// <summary>The Audio controller for in-game sounds.</summary>
	public class MAudio : MonoBehaviour
	{
		/// <summary>A unique reference to the only Audio object in the scene.</summary>
		public static MAudio AudioInstance { get => _AudioInstance; set => _AudioInstance = value; }
		static MAudio _AudioInstance;

		/// <summary>Whether or not to mute every sound by default.</summary>
		[Tooltip("Whether or not to mute every sound by default.")]
		public bool bMuteAllByDefault;
		/// <summary>Every sound that this Audio object will control.</summary>
		[Tooltip("Every sound that this Audio object will control.")]
		public MSound[] Sounds;

		const string kErr1 = "Sound of name: ";
		const string kErr2 = " could not be ";

		/// <summary>Populates the Sounds array to match the settings.</summary>
		/// <param name="Sounds">The sounds to initialise into the game.</param>
		public virtual void Initialise(MSound[] Sounds)
		{
			if (AudioInstance == null)
			{
				AudioInstance = this;
				gameObject.name = "Audio";
			}
			else
			{
				Log.W("Ensure there is only one Audio object in the scene and that only one is being initialised");
				Destroy(gameObject);
			}

			this.Sounds = Sounds;

			if (!bMuteAllByDefault)
				foreach (MSound s in Sounds)
				{
					s.ASSound = gameObject.AddComponent<AudioSource>();
					s.ASSound.clip = s.ACSource;
					s.ASSound.volume = s.fVolume;
					s.ASSound.pitch = s.fPitch;
					s.ASSound.loop = s.bLoop;
				}
		}

		/// <summary>Plays sound of name n.</summary>
		/// <param name="sName">The name of the requested sound to play.</param>
		/// <param name="bOverlapSound"></param>
		public void Play(string sName, bool bOverlapSound = false)
		{
			if (bMuteAllByDefault)
				return;
			MSound s = Find(sName);
			if (s != null && (bOverlapSound || !IsPlaying(s)))
				s.ASSound.Play();
			if (s == null)
				Log.W(kErr1 + sName + kErr2 + "played!");
		}

		/// <summary>Stops sound of name n.</summary>
		/// <param name="sName">The name of the requested sound to stop playing.</param>
		public void Stop(string sName)
		{
			if (bMuteAllByDefault)
				return;
			MSound s = Find(sName);
			if (s != null && IsPlaying(s))
				s.ASSound.Stop();
			if (s == null)
				Log.W(kErr1 + sName + kErr2 + "stopped!");
		}

		/// <summary>Stop every sound in the game.</summary>
		public void StopAll()
		{
			if (bMuteAllByDefault)
				return;
			foreach (MSound s in Sounds)
				s.ASSound.Stop();
		}

		/// <summary>Returns a sound in the Sounds array.</summary>
		/// <param name="n">The name of the requested sound.</param>
		/// <returns>The MSound of the requested sound.</returns>

		public MSound Find(string n) => Array.Find(Sounds, sound => sound.Name == n);

		/// <summary>Whether or not sound of name n is playing.</summary>
		/// <param name="sName">The name of the sound to query.</param>
		public bool IsPlaying(string sName)
		{
			if (bMuteAllByDefault)
				return false;
			return Find(sName).ASSound.isPlaying;
		}

		static bool IsPlaying(MSound s) => s.ASSound.isPlaying;
	}
}
