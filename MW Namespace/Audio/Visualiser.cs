using UnityEngine;

namespace MW.Audio
{
	/// <summary>A Sound Visualiser.</summary>
	public class Visualiser
	{
		/// <summary>The number of samples in the audio clip.</summary>
		public static int Samples = 1024;
		internal static float RMS0_N1_Db = 0.1f;
		internal static float Threshold = 0.02f;

		internal static float[] Internal_Samples;
		internal static float[] Spectrum;
		internal static float Sample;

		/// <summary>Visualises sound emitted by an AudioSource.</summary>
		/// <param name="Source">The source of the sound.</param>
		/// <param name="Visualiser">The sound visualiser displayed as an array of MVectors.</param>
		/// <param name="PositiveVisualiserAxis">How to visualise the sound in Visualiser.</param>
		/// <returns>VisualInformation. Notably, decibels and pitch of the analysed sound.</returns>
		public static VisualInformation Analyse(AudioSource Source, out MVector[] Visualiser, EComponentAxis PositiveVisualiserAxis = EComponentAxis.Y)
		{
			PrepareAnalyser();

			Internal_Analyser(Source, out float RMS, out float Decibels, out float Pitch);

			Visualiser = MakeVisualiser(PositiveVisualiserAxis);

			return new VisualInformation(RMS, Decibels, Pitch);
		}

		static void PrepareAnalyser()
		{
			Internal_Samples = new float[Samples];
			Spectrum = new float[Samples];
			Sample = AudioSettings.outputSampleRate;
		}

		static void Internal_Analyser(AudioSource Source, out float RMS, out float Decibels, out float Pitch)
		{
			Source.GetOutputData(Internal_Samples, 0);
			float sum = 0;

			for (int i = 0; i < Samples; i++)
			{
				sum += Internal_Samples[i] * Internal_Samples[i];
			}

			RMS = Mathf.Sqrt(sum / Samples);
			Decibels = 20 * Mathf.Log10(RMS / RMS0_N1_Db);

			if (Decibels < -160)
			{
				Decibels = -160;
			}

			Source.GetSpectrumData(Spectrum, 0, FFTWindow.BlackmanHarris);
			float MaxV = 0;
			int MaxN = 0;

			for (int i = 0; i < Samples; i++)
			{
				if (Spectrum[i] > MaxV && Spectrum[i] > Threshold)
				{
					MaxV = Spectrum[i];
					MaxN = i;
				}
			}

			float Frequency = MaxN;
			if (MaxN > 0 && MaxN < Samples - 1)
			{
				float DL = Spectrum[MaxN - 1] / Spectrum[MaxN];
				float DR = Spectrum[MaxN + 1] / Spectrum[MaxN];
				Frequency += 0.5f * (DR * DR - DL * DL);
			}

			Pitch = Frequency * (Sample * .5f) / Samples;
		}

		static MVector[] MakeVisualiser(EComponentAxis PositiveVisualiserAxis)
		{
			MVector[] Ret = new MVector[Samples];

			int VisualIndex = 0;
			int SpectrumIndex = 0;

			MVector Mask = new MVector();
			Mask.X = (float)(EComponentAxis.X & PositiveVisualiserAxis);
			Mask.Y = (float)(EComponentAxis.Y & PositiveVisualiserAxis);
			Mask.Z = (float)(EComponentAxis.Z & PositiveVisualiserAxis);

			while (VisualIndex < Samples)
			{
				float PositionOnSpectrum = Spectrum[SpectrumIndex];
				Ret[VisualIndex] = Mask * PositionOnSpectrum;
				++SpectrumIndex;
			}

			return Ret;
		}
	}

	/// <summary>Visualiser information for a analysed sound.</summary>
	public struct VisualInformation
	{
		/// <summary></summary>
		public float RootMeanSquare;
		/// <summary></summary>
		public float Decibels;
		/// <summary></summary>
		public float Pitch;

		internal VisualInformation(float RMS, float dB, float Pitch)
		{
			RootMeanSquare = RMS;
			Decibels = dB;
			this.Pitch = Pitch;
		}
	}
}
