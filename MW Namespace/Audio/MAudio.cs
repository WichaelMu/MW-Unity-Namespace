using MW.Diagnostics;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MW.Audio
{
	/// <summary>The Audio controller for in-game sounds.</summary>
	/// <decorations decor="public class : MonoBehaviour"></decorations>
	public class MAudio : MonoBehaviour
	{
		/// <summary>A unique reference to the only Audio object in the scene.</summary>
		/// <decorations decor="public static MAudio"></decorations>
		public static MAudio AudioInstance { get => _AudioInstance; set => _AudioInstance = value; }
		internal static MAudio _AudioInstance;

		/// <summary>Every sound that this Audio object will control.</summary>
		/// <decorations decor="public MSound[]"></decorations>
		[Tooltip("Every sound that this Audio object will control.")]
		public MSound[] Sounds;

		internal Dictionary<string, MSound> Internal_MSound;

		internal const string kErr1 = "Sound of name: ";
		internal const string kErr2 = " could not be found!";

		/// <summary>Populates the Sounds array to match the settings.</summary>
		/// <remarks>If a duplicate name is detected, a failure will occur. In this case,
		/// check the names of the MSounds array.
		/// </remarks>
		/// <decorations decor="public virtual void"></decorations>
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

			if (Internal_MSound == null)
			{
				Internal_MSound = new Dictionary<string, MSound>();
			}

			this.Sounds = Sounds;

			foreach (MSound s in Sounds)
			{
				s.AudioSourceComponent = gameObject.AddComponent<AudioSource>();
				s.AudioSourceComponent.clip = s.Clip;
				s.AudioSourceComponent.volume = s.Volume;
				s.AudioSourceComponent.pitch = s.Pitch;
				s.AudioSourceComponent.loop = s.bLoop;
				s.AudioSourceComponent.playOnAwake = s.bPlayOnAwake;
				s.AudioSourceComponent.mute = s.bMute;

				try
				{
					Internal_MSound.Add(s.Name, s);
				}
				catch (ArgumentException)
				{
					Log.W("An MSound with Name:", s.Name, "already exists!", "\nChoose another name.");
				}
				finally
				{
					// When a duplicate Name is detected, add a _FAILURE_ with a count at the
					// end of the name to show that there was a failure.
					// Sometimes, there may be multiple duplicates, so keep going until there
					// is a valid 'spot' for failure.

					uint FailureCount = 1;
					while (true)
					{
						try
						{
							Internal_MSound.Add(s.Name + "_FAILURE_" + FailureCount, s);
							break;
						}
						catch (ArgumentException)
						{
							FailureCount++;
						}
					}
				}
			}
		}

		/// <summary>Plays the sound named Name.</summary>
		/// <decorations decor="public void"></decorations>
		/// <param name="Name">The name of the requested sound to play.</param>
		public void Play(string Name)
		{
			if (Find(Name, out MSound S) && !IsPlaying(S))
				S.AudioSourceComponent.Play();
		}

		/// <summary>Plays the sound named Name from Emitter.</summary>
		/// <remarks>An AudioSource component will be added to Emitter.
		/// The clip will inherit settings from the MSound named Name.</remarks>
		/// <decorations decor="public void"></decorations>
		/// <param name="Name">The name of the requested sound to play from Emitter.</param>
		/// <param name="Emitter">The GameObject playing the sound.</param>
		public void PlayWithOverlap(string Name, GameObject Emitter)
		{
			if (Find(Name, out MSound S))
			{
				AudioSource Overlap = Emitter.AddComponent<AudioSource>();

				Overlap.volume = S.Volume;
				Overlap.pitch = S.Pitch;
				Overlap.loop = S.bLoop;

				Overlap.clip = S.Clip;
				Overlap.Play();
			}
		}


		/// <summary>Plays the sound named Name from Emitter.</summary>
		/// <remarks>An AudioSource component will be added to Emitter.
		/// The clip will inherit settings from the MSound named Name.</remarks>
		/// <decorations decor="public bool"></decorations>
		/// <param name="Name">The name of the requested sound to play from Emitter.</param>
		/// <param name="Emitter">The GameObject playing the sound.</param>
		/// <param name="EmitterSource">The AudioSource that was added to Emitter.</param>
		/// <returns>If the sound could be played.</returns>
		public bool PlayWithOverlap(string Name, GameObject Emitter, out AudioSource EmitterSource)
		{
			if (Find(Name, out MSound S))
			{
				AudioSource Overlap = Emitter.AddComponent<AudioSource>();

				Overlap.volume = S.Volume;
				Overlap.pitch = S.Pitch;
				Overlap.loop = S.bLoop;

				Overlap.clip = S.Clip;
				Overlap.Play();

				EmitterSource = Overlap;

				return true;
			}

			EmitterSource = null;
			return false;
		}

		/// <summary>Plays the sound named Name from Emitter.</summary>
		/// <remarks>An AudioSource component will be added to Emitter.</remarks>
		/// <decorations decor="public void"></decorations>
		/// <param name="Name">The name of the requested sound to play from Emitter.</param>
		/// <param name="Emitter">The GameObject playing the sound.</param>
		/// <param name="OverrideVolume">The volume to use, instead of the volume setting defined in AudioInstance.</param>
		/// <param name="OverridePitch">The pitch to use, instead of the pitch setting defined in AudioInstance.</param>
		/// <param name="OverrideLoop">Should this sound loop?</param>
		public void PlayWithOverlap(string Name, GameObject Emitter, float OverrideVolume, float OverridePitch, bool OverrideLoop)
		{
			if (Find(Name, out MSound S))
			{
				AudioSource Overlap = Emitter.AddComponent<AudioSource>();

				Overlap.volume = OverrideVolume;
				Overlap.pitch = OverridePitch;
				Overlap.loop = OverrideLoop;

				Overlap.clip = S.Clip;
				Overlap.Play();
			}
		}

		/// <summary>Pauses the sound named Name.</summary>
		/// <decorations decor="public void"></decorations>
		/// <param name="Name">The name of the requested sound to pause.</param>
		public void Pause(string Name)
		{
			if (Find(Name, out MSound S))
				S.AudioSourceComponent.Pause();
		}

		/// <summary>Pauses every sound that is playing in the game.</summary>
		/// <decorations decor="public MArray{MSound}"></decorations>
		/// <returns>An MArray of all sounds that were previously playing and affected by the pause.</returns>
		public MArray<MSound> PauseAll()
		{
			MArray<MSound> AllAffectedByPause = new MArray<MSound>();

			foreach (MSound S in Sounds)
				if (IsPlaying(S))
				{
					S.AudioSourceComponent.Pause();
					AllAffectedByPause.Push(S);
				}

			return AllAffectedByPause;
		}

		/// <summary>Stops the sound named Name.</summary>
		/// <decorations decor="public void"></decorations>
		/// <param name="Name">The name of the requested sound to stop playing.</param>
		public void Stop(string Name)
		{
			if (Find(Name, out MSound S) && IsPlaying(S))
				S.AudioSourceComponent.Stop();
		}

		/// <summary>Stop every sound in the game.</summary>
		/// <decorations decor="public void"></decorations>
		public void StopAll()
		{
			foreach (MSound S in Sounds)
				S.AudioSourceComponent.Stop();
		}

		/// <summary>Returns a sound in the Sounds array.</summary>
		/// <decorations decor="public bool"></decorations>
		/// <param name="Name">The name of the requested sound.</param>
		/// <param name="MSound">The out MSound parameter for the sound named Name, if found, null otherwise.</param>
		/// <returns>The MSound of the requested sound. Null if Name could not be found.</returns>

		public bool Find(string Name, out MSound MSound)
		{
			try
			{
				MSound = Internal_MSound[Name];
				return true;
			}
			catch (KeyNotFoundException)
			{
				MSound = null;
				Log.W(kErr1 + Name + kErr2);

				return false;
			}
		}

		/// <summary>Whether or not sound of name n is playing.</summary>
		/// <decorations decor="public bool"></decorations>
		/// <param name="Name">The name of the sound to query.</param>
		public bool IsPlaying(string Name)
		{
			if (Find(Name, out MSound S))
				return S.AudioSourceComponent.isPlaying;

			return false;
		}

		internal static bool IsPlaying(MSound S) => S.AudioSourceComponent.isPlaying;
	}
}
