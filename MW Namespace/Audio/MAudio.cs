using System;
using System.Collections.Generic;
using UnityEngine;
using MW.Diagnostics;

namespace MW.Audio
{
	/// <summary>The Audio controller for in-game sounds.</summary>
	public class MAudio : MonoBehaviour
	{
		/// <summary>A unique reference to the only Audio object in the scene.</summary>
		public static MAudio AudioInstance { get => _AudioInstance; set => _AudioInstance = value; }
		internal static MAudio _AudioInstance;

		/// <summary>Every sound that this Audio object will control.</summary>
		[Tooltip("Every sound that this Audio object will control.")]
		public MSound[] Sounds;

		internal Dictionary<string, MSound> Internal_MSound;

		internal const string kErr1 = "Sound of name: ";
		internal const string kErr2 = " could not be ";

		/// <summary>Populates the Sounds array to match the settings.</summary>
		/// <remarks>If a duplicate name is detected, a failure will occur. In this case,
		/// check the names of the MSounds array.
		/// </remarks>
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
				s.AudioSource = gameObject.AddComponent<AudioSource>();
				s.AudioSource.clip = s.Source;
				s.AudioSource.volume = s.Volume;
				s.AudioSource.pitch = s.Pitch;
				s.AudioSource.loop = s.bLoop;
				s.AudioSource.playOnAwake = s.bPlayOnAwake;
				s.AudioSource.mute = s.bMute;

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

		/// <summary>Plays sound of name n.</summary>
		/// <param name="Name">The name of the requested sound to play.</param>
		/// <param name="bOverlapSound"></param>
		public void Play(string Name, bool bOverlapSound = false)
		{
			MSound s = Find(Name);
			if (s != null && (bOverlapSound || !IsPlaying(s)))
				s.AudioSource.Play();
			if (s == null)
				Log.W(kErr1 + Name + kErr2 + "played!");
		}

		/// <summary>Stops sound of name n.</summary>
		/// <param name="Name">The name of the requested sound to stop playing.</param>
		public void Stop(string Name)
		{
			MSound s = Find(Name);
			if (s != null && IsPlaying(s))
				s.AudioSource.Stop();
			if (s == null)
				Log.W(kErr1 + Name + kErr2 + "stopped!");
		}

		/// <summary>Stop every sound in the game.</summary>
		public void StopAll()
		{
			foreach (MSound s in Sounds)
				s.AudioSource.Stop();
		}

		/// <summary>Returns a sound in the Sounds array.</summary>
		/// <param name="Name">The name of the requested sound.</param>
		/// <returns>The MSound of the requested sound. Null if Name could not be found.</returns>

		public MSound Find(string Name)
		{
			try
			{
				return Internal_MSound[Name];
			}
			catch (KeyNotFoundException)
			{
				return null;
			}
		}

		/// <summary>Whether or not sound of name n is playing.</summary>
		/// <param name="Name">The name of the sound to query.</param>
		public bool IsPlaying(string Name)
		{
			MSound S = Find(Name);

			if (S != null)
				return S.AudioSource.isPlaying;

			Log.W(kErr1 + Name + kErr2 + "couldn't be found!");
			return false;
		}

		internal static bool IsPlaying(MSound s) => s.AudioSource.isPlaying;
	}
}
