using System;
using UnityEngine;
using MW.Diagnostics;

namespace MW.Audio
{


	[Serializable]
	public class MSound
	{
		[Tooltip("The source of this sound.")]
		public AudioClip ACSource;
		[Tooltip("The name of the sound.")]
		public string sName;

		[Range(0f, 1f)]
		public float fVolume = 1, fPitch = 1;

		[Tooltip("Should this audio clip loop?")]
		public bool bLoop;

		[HideInInspector]
		public AudioSource ASSound;
	}

	/// <summary>The Audio controller for in-game sounds.</summary>
	public class MAudio : MonoBehaviour
	{
		/// <summary>A unique reference to the only Audio object in the scene.</summary>
		public static MAudio AAudioInstance { get => aAudioInstance; set => aAudioInstance = value; }
		static MAudio aAudioInstance;

		/// <summary>Whether or not to mute every sound by default.</summary>
		[Tooltip("Whether or not to mute every sound by default.")]
		public bool bMuteAllByDefault;
		/// <summary>Every sound that this Audio object will control.</summary>
		[Tooltip("Every sound that this Audio object will control.")]
		public MSound[] SSounds;

		const string kErr1 = "Sound of name: ";
		const string kErr2 = " could not be ";

		/// <summary>Populates the Sounds array to match the settings.</summary>
		public virtual void Initialise(MSound[] SSounds)
		{
			if (AAudioInstance == null)
			{
				AAudioInstance = this;
				gameObject.name = "Audio";
			}
			else
			{
				Log.W("Ensure there is only one Audio object in the scene and that only one is being initialised");
				Destroy(gameObject);
			}

			this.SSounds = SSounds;

			if (!bMuteAllByDefault)
				foreach (MSound s in SSounds)
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
			foreach (MSound s in SSounds)
				s.ASSound.Stop();
		}

		/// <summary>Returns a sound in the Sounds array.</summary>
		/// <param name="n">The name of the requested sound.</param>
		/// <returns>The sound clip of the requested sound.</returns>

		MSound Find(string n) => Array.Find(SSounds, sound => sound.sName == n);

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
