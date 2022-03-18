

namespace MW.Audio
{
	/// <summary>Notes on a standard piano from Octaves 0 - 8.</summary>
	public static class Notes
	{
		public static class Octave0
		{
			public const float C = 16.35f;
			public const float Cs_Db = 17.32f;
			public const float D = 18.35f;
			public const float Ds_Eb = 19.45f;
			public const float E = 20.6f;
			public const float F = 21.83f;
			public const float Fs_Gb = 23.12f;
			public const float G = 24.5f;
			public const float Gs_Ab = 25.96f;
			static readonly Note _A = new Note(21, "A0", 27.5f, 36.36f);
			static readonly Note _As_Bb = new Note(22, "A#/Bb", 29.14f, 34.32f);
			static readonly Note _B = new Note(23, "B0", 30.87f, 32.4f);

			public static readonly Note A = _A;
			public static readonly Note As_Bb = _As_Bb;
			public static readonly Note B = _B;
		}

		public static class Octave1
		{
			static readonly Note _C = new Note(24, "C1", 32.70f, 30.58f);
			static readonly Note _Cs_Db = new Note(25, "C#1/Db1", 34.65f, 28.86f);
			static readonly Note _D = new Note(26, "D1", 36.71f, 27.24f);
			static readonly Note _Ds_Eb = new Note(27, "D#1/Eb1", 38.89f, 25.71f);
			static readonly Note _E = new Note(28, "E1", 41.20f, 24.27f);
			static readonly Note _F = new Note(29, "F1", 43.65f, 22.91f);
			static readonly Note _Fs_Gb = new Note(30, "F#1/Gb1", 46.25f, 21.62f);
			static readonly Note _G = new Note(31, "G1", 49.00f, 20.41f);
			static readonly Note _Gs_Ab = new Note(32, "G#1/Ab1", 51.91f, 19.26f);
			static readonly Note _A = new Note(33, "A1", 55.00f, 18.18f);
			static readonly Note _As_Bb = new Note(34, "A#1/Bb1", 58.27f, 17.16f);
			static readonly Note _B = new Note(35, "B1", 61.74f, 16.2f);

			public static readonly Note C = _C;
			public static readonly Note Cs_Db = _Cs_Db;
			public static readonly Note D = _D;
			public static readonly Note Ds_Eb = _Ds_Eb;
			public static readonly Note E = _E;
			public static readonly Note F = _F;
			public static readonly Note Fs_Gb = _Fs_Gb;
			public static readonly Note G = _G;
			public static readonly Note _Gs = _Gs_Ab;
			public static readonly Note A = _A;
			public static readonly Note As_Bb = _As_Bb;
			public static readonly Note B = _B;
		}

		public static class Octave2
		{
			static readonly Note _C = new Note(36, "C2", 65.41f, 15.29f);
			static readonly Note _Cs_Db = new Note(37, "C#2/Db2", 69.30f, 14.29f);
			static readonly Note _D = new Note(38, "D2", 73.42f, 13.62f);
			static readonly Note _Ds_Eb = new Note(39, "D#2/Eb2", 77.78f, 12.86f);
			static readonly Note _E = new Note(40, "E2", 82.41f, 12.13f);
			static readonly Note _F = new Note(41, "F2", 87.31f, 11.45f);
			static readonly Note _Fs_Gb = new Note(42, "F#2/Gb2", 92.50f, 10.81f);
			static readonly Note _G = new Note(43, "G2", 98.00f, 10.2f);
			static readonly Note _Gs_Ab = new Note(44, "G#2/Ab2", 103.83f, 9.631f);
			static readonly Note _A = new Note(45, "A2", 110.00f, 9.091f);
			static readonly Note _As_Bb = new Note(46, "A#2/Bb2", 116.54f, 8.581f);
			static readonly Note _B = new Note(47, "B2", 123.47f, 8.099f);

			public static readonly Note C = _C;
			public static readonly Note Cs_Db = _Cs_Db;
			public static readonly Note D = _D;
			public static readonly Note Ds_Eb = _Ds_Eb;
			public static readonly Note E = _E;
			public static readonly Note F = _F;
			public static readonly Note Fs_Gb = _Fs_Gb;
			public static readonly Note G = _G;
			public static readonly Note _Gs = _Gs_Ab;
			public static readonly Note A = _A;
			public static readonly Note As_Bb = _As_Bb;
			public static readonly Note B = _B;
		}

		public static class Octave3
		{
			static readonly Note _C = new Note(48, "C3", 130.81f, 7.645f);
			static readonly Note _Cs_Db = new Note(49, "C#3/Db3", 138.59f, 7.216f);
			static readonly Note _D = new Note(50, "D3", 146.83f, 6.811f);
			static readonly Note _Ds_Eb = new Note(51, "D#3/Eb3", 155.56f, 6.428f);
			static readonly Note _E = new Note(52, "E3", 164.81f, 6.068f);
			static readonly Note _F = new Note(53, "F3", 174.61f, 5.727f);
			static readonly Note _Fs_Gb = new Note(54, "F#3/Gb3", 185.00f, 5.405f);
			static readonly Note _G = new Note(55, "G3", 196.00f, 5.102f);
			static readonly Note _Gs_Ab = new Note(56, "G#3/Ab3", 207.65f, 4.816f);
			static readonly Note _A = new Note(57, "A3", 220.00f, 4.545f);
			static readonly Note _As_Bb = new Note(58, "A#3/Bb3", 233.08f, 4.29f);
			static readonly Note _B = new Note(59, "B3", 246.94f, 4.05f);

			public static readonly Note C = _C;
			public static readonly Note Cs_Db = _Cs_Db;
			public static readonly Note D = _D;
			public static readonly Note Ds_Eb = _Ds_Eb;
			public static readonly Note E = _E;
			public static readonly Note F = _F;
			public static readonly Note Fs_Gb = _Fs_Gb;
			public static readonly Note G = _G;
			public static readonly Note _Gs = _Gs_Ab;
			public static readonly Note A = _A;
			public static readonly Note As_Bb = _As_Bb;
			public static readonly Note B = _B;
		}

		public static class Octave4
		{
			static readonly Note _C = new Note(60, "C4", 261.63f, 3.822f);
			static readonly Note _Cs_Db = new Note(61, "C#4/Db4", 277.18f, 3.608f);
			static readonly Note _D = new Note(62, "D4", 293.66f, 3.405f);
			static readonly Note _Ds_Eb = new Note(63, "D#4/Eb4", 311.13f, 3.214f);
			static readonly Note _E = new Note(64, "E4", 329.63f, 3.034f);
			static readonly Note _F = new Note(65, "F4", 349.23f, 2.863f);
			static readonly Note _Fs_Gb = new Note(66, "F#4/Gb4", 369.99f, 2.703f);
			static readonly Note _G = new Note(67, "G4", 392.00f, 2.551f);
			static readonly Note _Gs_Ab = new Note(68, "G#4/Ab4", 415.30f, 2.408f);
			static readonly Note _A = new Note(69, "A4", 440.00f, 2.273f);
			static readonly Note _As_Bb = new Note(70, "A#4/Bb4", 466.16f, 2.145f);
			static readonly Note _B = new Note(71, "B4", 493.88f, 2.025f);

			public static readonly Note C = _C;
			public static readonly Note Cs_Db = _Cs_Db;
			public static readonly Note D = _D;
			public static readonly Note Ds_Eb = _Ds_Eb;
			public static readonly Note E = _E;
			public static readonly Note F = _F;
			public static readonly Note Fs_Gb = _Fs_Gb;
			public static readonly Note G = _G;
			public static readonly Note _Gs = _Gs_Ab;
			public static readonly Note A = _A;
			public static readonly Note As_Bb = _As_Bb;
			public static readonly Note B = _B;
		}

		public static class Octave5
		{
			static readonly Note _C = new Note(72, "C5", 523.25f, 1.91f);
			static readonly Note _Cs_Db = new Note(73, "C#5/Db5", 554.37f, 1.804f);
			static readonly Note _D = new Note(74, "D5", 587.33f, 1.703f);
			static readonly Note _Ds_Eb = new Note(75, "D#5/Eb5", 622.25f, 1.607f);
			static readonly Note _E = new Note(76, "E5", 659.25f, 1.517f);
			static readonly Note _F = new Note(77, "F5", 698.46f, 1.432f);
			static readonly Note _Fs_Gb = new Note(78, "F#5/Gb5", 739.99f, 1.351f);
			static readonly Note _G = new Note(79, "G5", 783.99f, 1.276f);
			static readonly Note _Gs_Ab = new Note(80, "G#5/Ab5", 830.61f, 1.204f);
			static readonly Note _A = new Note(81, "A5", 880.00f, 1.136f);
			static readonly Note _As_Bb = new Note(82, "A#5/Bb5", 932.33f, 1.073f);
			static readonly Note _B = new Note(83, "B5", 987.77f, 1.012f);

			public static readonly Note C = _C;
			public static readonly Note Cs_Db = _Cs_Db;
			public static readonly Note D = _D;
			public static readonly Note Ds_Eb = _Ds_Eb;
			public static readonly Note E = _E;
			public static readonly Note F = _F;
			public static readonly Note Fs_Gb = _Fs_Gb;
			public static readonly Note G = _G;
			public static readonly Note _Gs = _Gs_Ab;
			public static readonly Note A = _A;
			public static readonly Note As_Bb = _As_Bb;
			public static readonly Note B = _B;
		}

		public static class Octave6
		{
			static readonly Note _C = new Note(84, "C6", 1046.50f, .9556f);
			static readonly Note _Cs_Db = new Note(85, "C#6/Db6", 1108.73f, .902f);
			static readonly Note _D = new Note(86, "D6", 1174.66f, .8513f);
			static readonly Note _Ds_Eb = new Note(87, "D#6/Eb6", 1244.51f, .8034f);
			static readonly Note _E = new Note(88, "E6", 1318.51f, .7584f);
			static readonly Note _F = new Note(89, "F6", 1396.91f, .7159f);
			static readonly Note _Fs_Gb = new Note(90, "F#6/Gb6", 1479.98f, .6757f);
			static readonly Note _G = new Note(91, "G6", 1567.98f, .6378f);
			static readonly Note _Gs_Ab = new Note(92, "G#6/Ab6", 1661.22f, .602f);
			static readonly Note _A = new Note(93, "A6", 1760.00f, .5682f);
			static readonly Note _As_Bb = new Note(94, "A#6/Bb6", 1864.66f, .5363f);
			static readonly Note _B = new Note(95, "B6", 1975.53f, .5062f);

			public static readonly Note C = _C;
			public static readonly Note Cs_Db = _Cs_Db;
			public static readonly Note D = _D;
			public static readonly Note Ds_Eb = _Ds_Eb;
			public static readonly Note E = _E;
			public static readonly Note F = _F;
			public static readonly Note Fs_Gb = _Fs_Gb;
			public static readonly Note G = _G;
			public static readonly Note _Gs = _Gs_Ab;
			public static readonly Note A = _A;
			public static readonly Note As_Bb = _As_Bb;
			public static readonly Note B = _B;
		}

		public static class Octave7
		{
			static readonly Note _C = new Note(96, "C7", 2093.00f, .4778f);
			static readonly Note _Cs_Db = new Note(97, "C#7/Db7", 2217.46f, .451f);
			static readonly Note _D = new Note(98, "D7", 2349.32f, .4257f);
			static readonly Note _Ds_Eb = new Note(99, "D#7, Eb7", 2489.02f, .4018f);
			static readonly Note _E = new Note(100, "E7", 2637.02f, .3792f);
			static readonly Note _F = new Note(101, "F7", 2793.83f, .358f);
			static readonly Note _Fs_Gb = new Note(102, "F#7/Gb7", 2959.96f, .3378f);
			static readonly Note _G = new Note(103, "G7", 3135.96f, .3189f);
			static readonly Note _Gs_Ab = new Note(104, "G#7/Ab7", 3322.44f, .301f);
			static readonly Note _A = new Note(105, "A7", 3520.00f, .2841f);
			static readonly Note _As_Bb = new Note(106, "A#7/Bb7", 3729.31f, .2681f);
			static readonly Note _B = new Note(107, "B7", 3951.07f, .2531f);

			public static readonly Note C = _C;
			public static readonly Note Cs_Db = _Cs_Db;
			public static readonly Note D = _D;
			public static readonly Note Ds_Eb = _Ds_Eb;
			public static readonly Note E = _E;
			public static readonly Note F = _F;
			public static readonly Note Fs_Gb = _Fs_Gb;
			public static readonly Note G = _G;
			public static readonly Note Gs_Ab = _Gs_Ab;
			public static readonly Note A = _A;
			public static readonly Note As_Bb = _As_Bb;
			public static readonly Note B = _B;
		}

		public static class Octave8
		{
			static readonly Note _C = new Note(108, "C8", 4186.01f, .2389f);
			public const float Cs_Db = 4434.92f;
			public const float D = 4698.63f;
			public const float Ds_Eb = 4978.03f;
			public const float E = 5274.04f;
			public const float F = 5587.65f;
			public const float Fs_Gb = 5919.91f;
			public const float G = 6271.93f;
			public const float Gs_Ab = 6644.88f;
			public const float A = 7040.00f;
			public const float As_Bb = 7458.62f;
			public const float B = 7902.13f;

			public static readonly Note C = _C;
		}
	}

	/// <summary>A Note on a standard piano.</summary>
	public struct Note
	{
		/// <summary>MIDI Number.</summary>
		public byte MIDI;
		/// <summary>The name of this Note.</summary>
		public string NoteName;
		/// <summary>The frequency of this Note in Hertz.</summary>
		public float FrequencyHz;
		/// <summary>The period of this Note in milliseconds.</summary>
		public float PeriodMS;

		internal Note(byte MIDI, string NoteName, float FrequencyHz, float PeriodMS)
		{
			this.MIDI = MIDI;
			this.NoteName = NoteName;
			this.FrequencyHz = FrequencyHz;
			this.PeriodMS = PeriodMS;
		}
	}
}
