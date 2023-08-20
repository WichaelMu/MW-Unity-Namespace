using System.Runtime.CompilerServices;
using MW.Extensions;
using static MW.Utils;

namespace MW.Math.Magic
{
	/// <summary>Delegate for FIntegral.</summary>
	/// <decorations decor="public delegate float"></decorations>
	/// <param name="F">Input.</param>
	/// <returns>Float.</returns>
	public delegate float IntegralFunction(float F);

	/// <summary>Fast implementations of common Mathematical functions using Bit Magic.</summary>
	/// <decorations decor="public static class"></decorations>
	public static class Fast
	{

		////////////////////////////////////////////////////////////////////////////////
		// NUMERICAL FUNCTIONS
		////////////////////////////////////////////////////////////////////////////////

		/// <summary>1 / sqrt(N).</summary>
		/// <remarks>Modified from: <see href="https://github.com/id-Software/Quake-III-Arena/blob/dbe4ddb10315479fc00086f08e25d968b4b43c49/code/game/q_math.c#L552"/></remarks>
		/// <docremarks>Modified from: &lt;a href="https://github.com/id-Software/Quake-III-Arena/blob/dbe4ddb10315479fc00086f08e25d968b4b43c49/code/game/q_math.c#L552"&gt;The 'Quake III Fast Inv. Sqrt Algorithm'&lt;/a&gt;</docremarks>
		/// <decorations decor="public static unsafe float"></decorations>
		/// <param name="N">1 / sqrt(x) where x is N.</param>
		/// <param name="NewtonIterations">The number of Newton Iterations to perform. + = Increased accuracy, decreased speed. - = Decreased accuracy, increased speed.</param>
		/// <returns>An approximation for calculating: 1 / sqrt(N), within +-.00001 of the real inverse square root with 3 Newton Iterations.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe float FInverseSqrt(float N, int NewtonIterations = 3)
		{
			int F = *(int*)&N;
			F = 0x5F3759DF - (F >> 1);
			float R = *(float*)&F;

			ClampMin(ref NewtonIterations, 1);

			for (int i = 0; i < NewtonIterations; ++i)
				R *= 1.5f - .5f * N * R * R;

			return R;
		}

		/// <summary>Faster version of <see cref="UnityEngine.Mathf.Sqrt(float)"/>.</summary>
		/// <docs>Faster version of Mathf.Sqrt().</docs>
		/// <decorations decor="public static float"></decorations>
		/// <param name="F"></param>
		/// <param name="NewtonIterations">The number of Newton Iterations to perform. + = Increased accuracy, decreased speed. - = Decreased accuracy, increased speed.</param>
		/// <returns>An approximation for the Square Root of F, within +-.00001 of the real square root with 3 Newton Iterations.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float FSqrt(float F, int NewtonIterations = 3) => FInverseSqrt(Max(F, MVector.kEpsilon), NewtonIterations) * F;

		/// <summary>Fast reciprocal/inverse function for any float N. 1f / N.</summary>
		/// <decorations decor="public static unsafe float"></decorations>
		/// <param name="N">The number to take the inverse of.</param>
		/// <param name="NewtonRaphsonIterations">The number of Newton Raphson iterations to perform. + = Increased accuracy, decreased speed. - = Decreased accuracy, increased speed.</param>
		/// <returns>An approximation for calculating: 1 / N, within +-.000008 of the real reciprocal with 5 Newton Raphson iterations.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe float FInverse(float N, int NewtonRaphsonIterations = 5)
		{
			int F = *(int*)&N;
			F = 0x7ED311C3 - F;
			float R = *(float*)&F;

			ClampMin(ref NewtonRaphsonIterations, 1);

			for (int i = 0; i < NewtonRaphsonIterations; ++i)
				R *= -R * N + 2f;

			return R;
		}

		/// <summary>Approximates the integral between LowerLimit and UpperLimit by Delta.</summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="LowerLimit">The lower limit.</param>
		/// <param name="UpperLimit">The upper limit.</param>
		/// <param name="Func">The function to pass in.</param>
		/// <param name="Delta">The delta.</param>
		/// <returns>An approximation of LowerLimit ∫ UpperLimit Func() Deltax.</returns>
		public static float FIntegral(float LowerLimit, float UpperLimit, IntegralFunction Func, float Delta = .0001f)
		{
			if (Func == null || Delta == 0f)
				return float.NaN;

			if (Delta < 0f)
				FAbs(ref Delta);

			float Result = 0f;
			long Deltas = (long)((UpperLimit - LowerLimit) * FInverse(Delta));

			for (long i = 0; i < Deltas; ++i)
			{
				float Begin = LowerLimit + i * Delta;
				float End = LowerLimit + (i + 1) * Delta;
				Result += Delta * (Func.Invoke(Begin) + Func.Invoke(End)) * .5f;
			}

			return Result;
		}

		////////////////////////////////////////////////////////////////////////////////
		// TRIGONOMETRY FUNCTIONS
		////////////////////////////////////////////////////////////////////////////////

		/// <summary>Faster version of <see cref="UnityEngine.Mathf.Asin(float)"/>.</summary>
		/// <docs>Faster version of Mathf.Asin().</docs>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Angle">The angle to get the inverse Sine of.</param>
		/// <returns>Inverse Sine of Angle in radians, accurate to +-.001 radians.</returns>
		public static float FArcSine(float Angle)
		{
			bool bIsPositive = Angle >= 0f;
			float FAbs = Fast.FAbs(Angle);

			float OneMinusFAbs = 1f - FAbs;
			ClampMin(ref OneMinusFAbs, 0f);

			float Root = FSqrt(OneMinusFAbs);

			const float kASinHalfPI = 1.5707963050f;

			float Approximation = ((((((-0.0012624911f * FAbs + 0.0066700901f) * FAbs - 0.0170881256f) * FAbs + 0.0308918810f) * FAbs - 0.0501743046f) * FAbs + 0.0889789874f) * FAbs - 0.2145988016f) * FAbs + kASinHalfPI;
			Approximation *= Root;

			return bIsPositive ? kASinHalfPI - Approximation : Approximation - kASinHalfPI;
		}

		/// <summary>Faster version of <see cref="UnityEngine.Mathf.Acos(float)"/>.</summary>
		/// <docs>Faster version of Mathf.Acos().</docs>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Angle">The angle to get the inverse Cosine of.</param>
		/// <returns>Inverse Cosine of Angle in radians, accurate to +-.001 radians.</returns>
		public static float FArcCosine(float Angle)
		{
			float A = FAbs(Angle);
			float ACos = ((-.0206452f * A + .0764532f) * A + -.21271f) * A + FMath.kHalfPI;
			ACos *= FSqrt(1f - A);

			return Angle > 0f ? ACos : FMath.kPI - ACos;
		}

		/// <summary>Faster version of <see cref="UnityEngine.Mathf.Atan(float)"/>.</summary>
		/// <docs>Faster version of Mathf.Atan().</docs>
		/// <decorations decor="public static float"></decorations>
		/// <param name="N">The ratio O/A.</param>
		/// <returns>Inverse Tangent of N in radians, accurate to +-.00136 radians.</returns>
		public static float FArcTangent(float N)
		{
			float F = FAbs(N);
			float T = (F < 1f) ? F : FInverse(F);
			float TT = T * T;
			float P = .0872929f;
			P = -.301895f + P * TT;
			P = 1f + P * TT;
			P *= T;

			float R = F < 1f ? P : FMath.kHalfPI - P;
			return N < 0f ? -R : R;
		}

		/// <summary>Faster version of <see cref="UnityEngine.Mathf.Atan2(float, float)"/>.</summary>
		/// <docs>Faster version of Mathf.Atan2().</docs>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Y">The Y component of a point.</param>
		/// <param name="X">The X component of a point.</param>
		/// <returns>Approximation of Atan2, accurate to +-.00136 radians.</returns>
		public static float FArcTangent2(float Y, float X)
		{
			if (Y == 0f && X == 0f)
				return 0f;
			if (Y == 0f && X > 0f)
				return 0f;
			if (Y == 0f && X < 0f)
				return FMath.kPI;
			if (Y > 0f && X == 0f)
				return FMath.kHalfPI;
			if (Y < 0f && X == 0f)
				return -FMath.kHalfPI;
			if (X.IsIllegalFloat() || Y.IsIllegalFloat())
				return FMath.NaN;

			float YX = Y * FInverse(X);
			float XY = X * FInverse(Y);

			float TYX = FArcTangent(YX);
			float TPXY = FArcTangent(XY);

			return X >= 0f
				? Y >= 0f
					? Y < X
						? TYX
						: FMath.kHalfPI - TPXY
					: -Y < X
						? TYX
						: -FMath.kHalfPI - TPXY
				: Y >= 0f
					? Y < -X
						? TYX + FMath.kPI
						: FMath.kHalfPI - TPXY
					: -Y < -X
						? TYX - FMath.kPI
						: -FMath.kHalfPI - TPXY;
		}

		/// <summary>Faster version of <see cref="UnityEngine.Vector3.Angle(UnityEngine.Vector3, UnityEngine.Vector3)"/>.</summary>
		/// <docs>Faster version of Vector3.Angle().</docs>
		/// <decorations decor="public static float"></decorations>
		/// <param name="L">The Vector in which the angular difference is measured.</param>
		/// <param name="R">The Vector in which the angular difference is measured.</param>
		/// <returns>The Angle between L and R in degrees, accurate to +-.1 degrees.</returns>
		public static float FAngle(MVector L, MVector R)
		{
			float ZeroOrEpsilon = FSqrt(L.SqrMagnitude * R.SqrMagnitude);
			if (ZeroOrEpsilon < 1E-15f)
				return 0f;

			float Radians = (L | R) * FInverse(ZeroOrEpsilon);
			Clamp(ref Radians, -1f, 1f);
			return FArcCosine(Radians) * UnityEngine.Mathf.Rad2Deg;
		}

		////////////////////////////////////////////////////////////////////////////////
		// ABSOLUTE VALUES
		////////////////////////////////////////////////////////////////////////////////

		/// <summary>Absolute Value of I.</summary>
		/// <decorations decor="public static int"></decorations>
		/// <param name="I">The int to get the Absolute Value of.</param>
		/// <returns>The value of I regardless of its sign.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int FAbs(int I)
		{
			return 0x7FFFFFFF & I;
		}

		/// <summary>Absolute Value of F.</summary>
		/// <decorations decor="public static unsafe float"></decorations>
		/// <param name="F">The float to get the Absolute Value of.</param>
		/// <returns>The value of F regardless of its sign.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe float FAbs(float F)
		{
			int T = FAbs(*(int*)&F);
			return *(float*)&T;
		}

		/// <summary>Absolute Value of F.</summary>
		/// <decorations decor="public static unsafe float"></decorations>
		/// <param name="F">The float to set the Absolute Value of.</param>
		/// <returns>The value of F regardless of its sign.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe float FAbs(ref float F)
		{
			fixed (float* pF = &F)
			{
				int T = FAbs(*(int*)pF);
				return *(float*)&T;
			}
		}
	}
}
