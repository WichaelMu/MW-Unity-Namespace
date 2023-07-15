using __CS_MATH__ = System.Math;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MW.Extensions;
using MW.Math;
using static MW.Math.Magic.Fast;
using MW.Console;

namespace MW
{
	/// <summary>A Mathematics Function interface.</summary>
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct FMath
	{
		/// <summary>The golden ratio.</summary>
		/// <decorations decor="public const float"></decorations>
		public const float kPhi = 1.6180339887498948482045868343656381F;

		/// <summary>Euler's number. (e)</summary>
		/// <decorations decor="public const float"></decorations>
		public const float kE = 2.71828182845904523536F;

		/// <summary>Shorthand for writing FSqrt(2).</summary>
		/// <decorations decor="public const float"></decorations>
		public const float kSqrt2 = 1.4142135623730950488016887242097F;

		/// <summary>Shorthand for writing FSqrt(3).</summary>
		/// <decorations decor="public const float"></decorations>
		public const float kSqrt3 = 1.7320508075688772935274463415059F;

		/// <summary>PI</summary>
		/// <decorations decor="public const float"></decorations>
		public const float kPI = 3.1415926535897932384626433832795F;

		/// <summary>Shorthand for writing 1 / kPI.</summary>
		/// <decorations decor="public const float"></decorations>
		public const float kInversePI = .31830988618379067153776752674503F;

		/// <summary>Shorthand for writing kPI * kHalf.</summary>
		/// <decorations decor="public const float"></decorations>
		public const float kHalfPI = 1.5707963267948966192313216916398F;

		/// <summary>Shorthand for writing kPI * 2f.</summary>
		/// <decorations decor="public const float"></decorations>
		public const float k2PI = 6.283185307179586476925286766559F;

		/// <summary>The conversion from 0-1 to 0-255.</summary>
		/// <decorations decor="public const float"></decorations>
		public const float k1To255RGB = 0.0039215686274509803921568627451F;
		public const float kInfinity = float.PositiveInfinity;
		public const float kNegativeInfinity = float.NegativeInfinity;
		public const float D2R = 0.01745329251994329576923690768489F;
		public const float R2D = 57.29578F;
		public const float kEpsilon = 1E-05F;
		public const float NaN = 0F / 0F;

		/// <summary>Shorthand for writing / 3.</summary>
		/// <decorations decor="public const float"></decorations>
		public const float kOneThird = .3333333333333333333333333333333333F;
		/// <summary>Shorthand for writing / 1.5.</summary>
		/// <decorations decor="public const float"></decorations>
		public const float kTwoThirds = .6666666666666666666666666666666666F;

		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsPowerOfTwo(float F) => Mathematics.IsPowerOfTwo((int)F);

		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Sine(float F) => F.IsIllegalFloat() ? float.NaN : (float)__CS_MATH__.Sin(F);
		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Cos(float F) => F.IsIllegalFloat() ? float.NaN : (float)__CS_MATH__.Cos(F);
		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Tan(float F) => F.IsIllegalFloat() ? float.NaN : (float)__CS_MATH__.Tan(F);

		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float ArcSine(float F) => FArcSine(F);
		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float ArcCos(float F) => FArcCosine(F);
		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float ArcTan(float F) => FArcTangent(F);
		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float ArcTan2(float Y, float X) => FArcTangent2(Y, X);

		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Sqrt(float F, bool bWithAccuracy = false, int NewtonIterations = 3)
		{
			if (bWithAccuracy)
				return (float)__CS_MATH__.Sqrt(F);
			return FSqrt(F, NewtonIterations);
		}

		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float InvSqrt(float F, bool bWithAccuracy = false, int NewtonIterations = 3)
		{
			if (bWithAccuracy)
				return 1f / Sqrt(F, bWithAccuracy);
			return FInverseSqrt(F, NewtonIterations);
		}

		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static sbyte Abs(sbyte B) => (sbyte)(B < 0 ? -B : B);
		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short Abs(short S) => (short)(S < 0 ? -S : S);
		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Abs(int I) => I < 0 ? -I : I;
		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Abs(float F) => F < 0f ? -F : F;
		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double Abs(double D) => D < .0 ? -D : D;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static sbyte Min(sbyte A, sbyte B) => A < B ? A : B;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short Min(short A, short B) => A < B ? A : B;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Min(int A, int B) => A < B ? A : B;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Min(float A, float B) => A < B ? A : B;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double Min(double A, double B) => A < B ? A : B;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static sbyte Max(sbyte A, sbyte B) => A > B ? A : B;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short Max(short A, short B) => A > B ? A : B;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Max(int A, int B) => A > B ? A : B;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Max(float A, float B) => A > B ? A : B;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double Max(double A, double B) => A > B ? A : B;

		public static sbyte Min(params sbyte[] Params)
		{
			if (Params.Length == 0)
				return 0;
			sbyte RetVal = Params[0];
			foreach (sbyte s in Params)
				RetVal = Min(RetVal, s);

			return RetVal;
		}

		public static short Min(params short[] Params)
		{
			if (Params.Length == 0)
				return 0;
			short RetVal = Params[0];
			foreach (short s in Params)
				RetVal = Min(RetVal, s);

			return RetVal;
		}

		public static int Min(params int[] Params)
		{
			if (Params.Length == 0)
				return 0;
			int RetVal = Params[0];
			foreach (int s in Params)
				RetVal = Min(RetVal, s);

			return RetVal;
		}

		public static float Min(params float[] Params)
		{
			if (Params.Length == 0)
				return 0;
			float RetVal = Params[0];
			foreach (float s in Params)
				RetVal = Min(RetVal, s);

			return RetVal;
		}

		[StandaloneExec]
		public static double Min(params double[] Params)
		{
			if (Params.Length == 0)
				return 0;
			double RetVal = Params[0];
			foreach (double s in Params)
				RetVal = Min(RetVal, s);

			return RetVal;
		}

		public static sbyte Max(params sbyte[] Params)
		{
			if (Params.Length == 0)
				return 0;
			sbyte RetVal = Params[0];
			foreach (sbyte s in Params)
				RetVal = Max(RetVal, s);

			return RetVal;
		}

		public static short Max(params short[] Params)
		{
			if (Params.Length == 0)
				return 0;
			short RetVal = Params[0];
			foreach (short s in Params)
				RetVal = Max(RetVal, s);

			return RetVal;
		}

		public static int Max(params int[] Params)
		{
			if (Params.Length == 0)
				return 0;
			int RetVal = Params[0];
			foreach (int s in Params)
				RetVal = Max(RetVal, s);

			return RetVal;
		}

		public static float Max(params float[] Params)
		{
			if (Params.Length == 0)
				return 0;
			float RetVal = Params[0];
			foreach (float s in Params)
				RetVal = Max(RetVal, s);

			return RetVal;
		}

		[StandaloneExec]
		public static double Max(params double[] Params)
		{
			if (Params.Length == 0)
				return 0;
			double RetVal = Params[0];
			foreach (double s in Params)
				RetVal = Max(RetVal, s);

			return RetVal;
		}

		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Power(float F, float P) => (float)__CS_MATH__.Pow(F, P);
		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Exp(float F) => (float)__CS_MATH__.Exp(F);

		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Log(float F) => (float)__CS_MATH__.Log(F);
		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Log10(float F) => (float)__CS_MATH__.Log10(F);
		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Log(float F, float Base) => (float)__CS_MATH__.Log(F, Base);

		[StandaloneExec]
		public static float Ceiling(float F) => (float)__CS_MATH__.Ceiling(F);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int CeilingInt(float F) => (int)Ceiling(F);
		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Floor(float F) => (float)__CS_MATH__.Floor(F);
		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int FloorInt(float F) => (int)Floor(F);
		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Round(float F) => (float)__CS_MATH__.Round(F);
		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int RoundInt(float F) => (int)__CS_MATH__.Round(F);

		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Sign(sbyte B) => B >= 0 ? 1 : -1;
		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Sign(short S) => S >= 0 ? 1 : -1;
		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Sign(int I) => I >= 0 ? 1 : -1;
		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Sign(float F) => F >= 0f ? 1 : -1;
		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Sign(double D) => D >= 0 ? 1 : -1;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Clamp(ref sbyte B, sbyte Min, sbyte Max)
		{
			if (Min > Max)
				Utils.Swap(ref Min, ref Max);
			B = Clamp(B, Min, Max);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Clamp(ref short S, short Min, short Max)
		{
			if (Min > Max)
				Utils.Swap(ref Min, ref Max);
			S = Clamp(S, Min, Max);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Clamp(ref int I, int Min, int Max)
		{
			if (Min > Max)
				Utils.Swap(ref Min, ref Max);
			I = Clamp(I, Min, Max);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Clamp(ref float F, float Min, float Max)
		{
			if (Min > Max)
				Utils.Swap(ref Min, ref Max);
			F = Clamp(F, Min, Max);
		}

		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Clamp(ref double D, double Min, double Max)
		{
			if (Min > Max)
				Utils.Swap(ref Min, ref Max);
			D = Clamp(D, Min, Max);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static sbyte Clamp(sbyte I, sbyte Min, sbyte Max)
		{
			if (I < Min)
			{
				I = Min;
			}
			else if (I > Max)
			{
				I = Max;
			}

			return I;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short Clamp(short I, short Min, short Max)
		{
			if (I < Min)
			{
				I = Min;
			}
			else if (I > Max)
			{
				I = Max;
			}

			return I;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Clamp(int I, int Min, int Max)
		{
			if (I < Min)
			{
				I = Min;
			}
			else if (I > Max)
			{
				I = Max;
			}

			return I;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Clamp(float I, float Min, float Max)
		{
			if (I < Min)
			{
				I = Min;
			}
			else if (I > Max)
			{
				I = Max;
			}

			return I;
		}

		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double Clamp(double I, double Min, double Max)
		{
			if (I < Min)
			{
				I = Min;
			}
			else if (I > Max)
			{
				I = Max;
			}

			return I;
		}

		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DeltaAngle(float Now, float Target)
		{
			float RetVal = Mathematics.Wrap(Target - Now, 0f, 360f);
			return RetVal > 180f ? RetVal - 360f : RetVal;
		}

		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Lerp(float A, float B, float Alpha) => A + (B - A) * Alpha;
		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float LerpAngle(float A, float B, float Alpha)
		{
			return A + DeltaAngle(A, B) * Clamp(Alpha, 0f, 1f);
		}

		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float InvLerp(float A, float B, float F)
		{
			if (!IsApproxEqual(A, B))
				return Clamp((F - A) * FInverse(B - A), 0f, 1f);
			return 0f;
		}


		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Approach(float Now, float Target, float MaximumDelta)
		{
			if (Abs(Target - Now) <= MaximumDelta)
				return Target;
			return Now + Sign(Target - Now) * MaximumDelta;
		}

		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float ApproachAngle(float Now, float Target, float MaximumDelta)
		{
			float Delta = DeltaAngle(Now, Target);
			if (0f - MaximumDelta < Delta && Delta < MaximumDelta)
				return Target;

			Target = Now + Delta;
			return Approach(Now, Target, MaximumDelta);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Smooth(float From, float To, float Alpha)
		{
			Clamp(ref Alpha, 0f, 1f);
			Alpha = -2f * Alpha * Alpha * Alpha + 3f * Alpha * Alpha;
			return To * Alpha + From * (1f - Alpha);
		}

		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsApproxEqual(float A, float B, float Tolerance = kEpsilon)
		{
			return Abs(A - B) <= Tolerance;
		}

		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsApproxEqual(MVector A, MVector B, float Tolerance = kEpsilon)
		{
			return IsApproxEqual(A.SqrMagnitude, B.SqrMagnitude, Tolerance);
		}

		public static float SmoothDamp(float Now, float Target, ref float RefVelocity, float SmoothTime, float DeltaTime, float Terminal = kInfinity)
		{
			SmoothTime = Max(0.0001f, SmoothTime);

			float TOST = 2f * FInverse(SmoothTime);
			float TSDT = TOST * DeltaTime;

			float num3 = FInverse(1f + TSDT + 0.48f * TSDT * TSDT + 0.235f * TSDT * TSDT * TSDT);
			float Difference = Now - Target;
			float T = Target;
			float FIS1 = Terminal * SmoothTime;

			Difference = Clamp(Difference, 0f - FIS1, FIS1);
			Target = Now - Difference;

			float FIS2 = (RefVelocity + TOST * Difference) * DeltaTime;

			RefVelocity = (RefVelocity - TOST * FIS2) * num3;
			float RetVal = Target + (Difference + FIS2) * num3;

			if (T - Now > 0f == RetVal > T)
			{
				RetVal = T;
				RefVelocity = (RetVal - T) * FInverse(DeltaTime);
			}

			return RetVal;
		}

		public static float SmoothDampAngle(float Now, float Target, ref float RefVelocity, float SmoothTime, float DeltaTime, float Terminal = kInfinity)
		{
			Target = Now + DeltaAngle(Now, Target);
			return SmoothDamp(Now, Target, ref RefVelocity, SmoothTime, DeltaTime, Terminal);
		}

		[StandaloneExec]
		public static float Oscillate(float F, float Magnitude)
		{
			F = Mathematics.Wrap(F, 0f, Magnitude * 2f);
			return Magnitude - Abs(F - Magnitude);
		}

		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float PreviousMultiple(float F, float Multiple) => F - (F % Multiple);

		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float NextMultiple(float F, float Multiple)
			=> F + (Multiple - (F % Multiple));

		[StandaloneExec]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float ClosestMultiple(float F, float Multiple)
		{
			float Mod = F % Multiple;
			float P = F - Mod;
			float N = F + (Multiple - Mod);
			return F - P < N - F ? P : N;
		}
	}
}
