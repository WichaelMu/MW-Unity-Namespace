#if RELEASE
using System;
using System.Collections;
using UnityEngine;

namespace MW.Audio
{
	/// <summary>An audio synthesiser.</summary>
	/// <decorations decor="[RequireComponent{AudioSource}] public class : MonoBehaviour"></decorations>
	[RequireComponent(typeof(AudioSource))]
	public class Synthesiser : MonoBehaviour
	{
		/// <summary>The volume of this Synthesiser</summary>
		/// <decorations decor="[SerializeField] float"></decorations>
		[SerializeField, Range(0f, 1f), Tooltip("The volume of this Synthesiser.")] float Gain;

		/// <summary>The Sample Frequency in System Sample Rate. (Default is 48,000)</summary>
		/// <decorations decor="[SerializeField] ESampleRate"></decorations>
		[SerializeField, Tooltip("The Sample Frequency in Edit -> Project Settings -> Audio -> System Sample Rate. (Default is 48,000)")] ESampleRate SampleRate = ESampleRate.SR_48K;
		[SerializeField, Tooltip("The frequency of the notes, and their duration, to play.")] Key[] Notes;

		internal float CurrentFrequency = 0;
		internal uint CurrentNote = 0;
		internal bool bIsRunning = false;

		internal float PlaybackRate;
		internal float Phase;

		internal const float kSampleFrequency24K = 1.3089969389957471826927680763665e-4f; // Mathf.PI * (1 / 24,000f)
		internal const float kSampleFrequency48K = 6.5449846949787359134638403818323e-5f; // Mathf.PI * (1 / 48,000f)
		internal const float kSampleFrequency96K = 3.2724923474893679567319201909161e-5f; // Mathf.PI * (1 / 96,000f)

		/// <summary>Play Notes from Starting Note.</summary>
		/// <decorations decor="public IEnumerator"></decorations>
		/// <param name="StartingNote">The Note to begin with in Notes. Default is zero.</param>
		/// <returns>The IEnumerator that plays the Notes.</returns>
		public IEnumerator Play(uint StartingNote = 0)
		{
			Debug.Assert(StartingNote < Notes.Length, "StartingNote exceeds the length of Notes.");

			CurrentNote = StartingNote;
			IEnumerator Speaker = Internal_PlayNotes();
			StartCoroutine(Speaker);

			return Speaker;
		}

		internal IEnumerator Internal_PlayNotes()
		{
			while (CurrentNote < Notes.Length)
			{
				bIsRunning = true;
				CurrentFrequency = Notes[CurrentNote].Frequency;

				yield return new WaitForSeconds(Notes[CurrentNote].Duration);

				++CurrentNote;
			}

			bIsRunning = false;
		}

		void OnAudioFilterRead(float[] Data, int Channels)
		{
			if (!bIsRunning)
				return;

			if (SampleRate == ESampleRate.SR_24K)
				PlaybackRate = CurrentFrequency * 2f * kSampleFrequency24K;
			else if (SampleRate == ESampleRate.SR_48K)
				PlaybackRate = CurrentFrequency * 2f * kSampleFrequency48K;
			else if (SampleRate == ESampleRate.SR_96K)
				PlaybackRate = CurrentFrequency * 2f * kSampleFrequency96K;
			else // SampleFrequency != Any ESampleFrequency
				PlaybackRate = CurrentFrequency * 2f * kSampleFrequency48K;

			bool bStereo = Channels == 2;

			for (int i = 0; i < Data.Length; i += Channels)
			{
				Phase += PlaybackRate;
				Data[i] = Gain * Mathf.Sin((float)Phase);

				if (bStereo)
				{
					Data[i + 1] = Data[i];
				}

				if (Phase > 6.2831853f)
				{
					Phase = 0f;
				}
			}
		}
	}

	/// <summary>The frequency and duration of a synthesised sound.</summary>
	/// <decorations decor="[Serializable] public struct"></decorations>
	[Serializable]
	public struct Key
	{
		/// <summary>The frequency in Hertz of this key.</summary>
		/// <decorations decor="public float"></decorations>
		public float Frequency;
		/// <summary>The time this key will play in seconds.</summary>
		/// <decorations decor="public float"></decorations>
		public float Duration;

		/// <summary>Makes a Key out of a Note.</summary>
		/// <param name="Note">The Note on a piano.</param>
		/// <param name="Duration">The time this key will play.</param>
		public Key(Note Note, float Duration)
		{
			Frequency = Note.FrequencyHz;
			this.Duration = Duration;
		}

		/// <summary>Makes a Key out of a frequency in Hertz.</summary>
		/// <param name="Frequency">The frequency in Hertz of this Key.</param>
		/// <param name="Duration">The time this frequency will play.</param>
		public Key(float Frequency, float Duration)
		{
			this.Frequency = Frequency;
			this.Duration = Duration;
		}

		public override string ToString()
		{
			return "Frequency: " + Frequency + " Duration: " + Duration;
		}
	}

	/// <summary></summary>
	/// <decorations decor="public enum : int"></decorations>
	public enum ESampleRate : int
	{
		/// <summary>24KHz Sample Rate.</summary>
		[InspectorName("24,000")]
		SR_24K,
		/// <summary>48KHz Sample Rate.</summary>
		[InspectorName("48,000")]
		SR_48K,
		/// <summary>96KHz Sample Rate.</summary>
		[InspectorName("96,000")]
		SR_96K
	}
}
#endif // RELEASE