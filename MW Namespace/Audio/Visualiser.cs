using UnityEngine;

namespace MW.Audio
{
	/// <summary>A Sound Visualiser.</summary>
	public class Visualiser
	{
		/// <summary>The number of samples in the audio clip.</summary>
		public int Samples = 1024;
		internal static float RMS0_N1_Db = 0.1f;
		internal static float Threshold = 0.02f;

		internal float[] Internal_Samples;
		internal float[] Spectrum;
		internal float Sample;

		/// <summary>The source of the sound.</summary>
		public AudioSource Source;
		internal EComponentAxis Internal_PositiveVisualiserAxis;
		/// <summary>How to visualise the sound in Visualiser.</summary>
		public EComponentAxis PositiveVisualiserAxis
		{
			get => Internal_PositiveVisualiserAxis;
			set
			{
				Mask = new MVector();
				Mask.X = (float)(EComponentAxis.X & value);
				Mask.Y = (float)(EComponentAxis.Y & value);
				Mask.Z = (float)(EComponentAxis.Z & value);

				Internal_PositiveVisualiserAxis = value;
			}
		}

		MVector Mask;

		/// <summary>Constructs a new Audio Visualiser.</summary>
		/// <param name="Source">The source of the sound to visualise.</param>
		/// <param name="Samples">The number of samples to visualise.</param>
		/// <param name="PositiveVisualiserAxis">Defines axis/es the visualisation is displayed. This takes affect only when Analysed using an MVector[] or a LineRenderer.</param>
		public Visualiser(AudioSource Source, int Samples, EComponentAxis PositiveVisualiserAxis = EComponentAxis.Y)
		{
			this.Source = Source;
			this.Samples = Samples;
			this.PositiveVisualiserAxis = PositiveVisualiserAxis;

			PrepareAnalyser();
		}

		void PrepareAnalyser()
		{
			Internal_Samples = new float[Samples];
			Spectrum = new float[Samples];
			Sample = AudioSettings.outputSampleRate;
		}

		/// <summary>Visualises sound emitted by an AudioSource.</summary>
		/// <returns>VisualInformation. Notably, decibels and pitch of the analysed sound.</returns>
		public VisualInformation Analyse()
		{
			Internal_Analyser(Source, out float RMS, out float Decibels, out float Pitch);

			return new VisualInformation(RMS, Decibels, Pitch);
		}

		/// <summary>Visualises sound emitted by an AudioSource.</summary>
		/// <param name="Visualiser">The sound visualiser displayed as an array of MVectors. Where left indices are low frequencies and right indices are high.</param>
		/// <returns>VisualInformation. Notably, decibels and pitch of the analysed sound.</returns>
		public VisualInformation Analyse(out MVector[] Visualiser)
		{
			VisualInformation Information = Analyse();

			Visualiser = MakeVisualiser();

			return Information;
		}

		/// <summary>Visualises sound emitted by an AudioSource.</summary>
		/// <returns>VisualInformation. Notably, decibels and pitch of the analysed sound.</returns>
		public VisualInformation Analyse(ref LineRenderer LineRenderer, float Modifier, float Smooth, float MaxHeight, float DeltaTime)
		{
			if (LineRenderer.positionCount <= 2)
			{
				LineRenderer.positionCount = Samples;
			}

			VisualInformation Information = Analyse();

			VisualiseOnLineRenderer(ref LineRenderer, ref Modifier, ref Smooth, ref MaxHeight, ref DeltaTime);

			return Information;
		}

		void Internal_Analyser(AudioSource Source, out float RMS, out float Decibels, out float Pitch)
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

		MVector[] MakeVisualiser()
		{
			MVector[] Ret = new MVector[Samples];

			int VisualIndex = 0;
			int SpectrumIndex = 0;

			while (VisualIndex < Samples)
			{
				float PositionOnSpectrum = Spectrum[SpectrumIndex];
				Ret[VisualIndex] = Mask * PositionOnSpectrum;
				++SpectrumIndex;
				++VisualIndex;
			}

			return Ret;
		}

		void VisualiseOnLineRenderer(ref LineRenderer LineRenderer, ref float Modifier, ref float Smooth, ref float MaxHeight, ref float DeltaTime)
		{
			float t = DeltaTime * Smooth;

			int VisualIndex = 0;
			int SpectrumIndex = 0;
			const int Size = 1;

			while (VisualIndex < Samples)
			{
				int i = 0;
				float sum = 0;
				while (i < Size)
				{
					sum += Spectrum[SpectrumIndex];
					++SpectrumIndex;
					++i;
				}

				float LinePosition = sum * Modifier;
				Vector3 Interp = Vector3.Lerp(LineRenderer.GetPosition(VisualIndex), new Vector3(SpectrumIndex * .5f, Mathf.Min(LinePosition, MaxHeight)), t);
				LineRenderer.SetPosition(VisualIndex, Interp);

				VisualIndex++;
			}
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
