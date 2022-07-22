﻿using static MW.Utils;

namespace MW.Math.Magic
{
	/// <summary>Fast implementations of common Mathematical functions using Bit Magic.</summary>
	/// <decorations decor="public static class"></decorations>
	public static class Fast
	{
		/// <summary>1 / sqrt(N).</summary>
		/// <remarks>Modified from: <see href="https://github.com/id-Software/Quake-III-Arena/blob/dbe4ddb10315479fc00086f08e25d968b4b43c49/code/game/q_math.c#L552"/></remarks>
		/// <docremarks>Modified from: &lt;a href="https://github.com/id-Software/Quake-III-Arena/blob/dbe4ddb10315479fc00086f08e25d968b4b43c49/code/game/q_math.c#L552"&gt;The 'Quake III Fast Inv. Sqrt Algorithm'&lt;/a&gt;</docremarks>
		/// <decorations decor="public static unsafe float"></decorations>
		/// <param name="N">1 / sqrt(x) where x is N.</param>
		/// <param name="AdditionalIterations">The number of additional Newton Iterations to perform.</param>
		/// <returns>An approximation for calculating: 1 / sqrt(N).</returns>
		public static unsafe float InverseSqrt(float N, int AdditionalIterations = 1)
		{
			int F = *(int*)&N;
			F = 0x5F3759DF - (F >> 1);
			float X = *(float*)&F;

			float ISqrt = X * (1.5f - .5f * N * X * X);
			for (int i = 0; i < AdditionalIterations; ++i)
				ISqrt *= (1.5f - .5f * N * ISqrt * ISqrt);
			return ISqrt;
		}

		/// <summary>Faster version of <see cref="UnityEngine.Mathf.Sqrt(float)"/>.</summary>
		/// <docs>Faster version of Mathf.Sqrt().</docs>
		/// <decorations decor="public static float"></decorations>
		/// <param name="F"></param>
		/// <param name="Iterations">The number of Newton Iterations to perform.</param>
		/// <returns>An approximation for the Square Root of F.</returns>
		public static float Sqrt(float F, int Iterations = 2) => InverseSqrt(Max(F, MVector.kEpsilon), Iterations) * F;

		/// <summary>Faster version of <see cref="UnityEngine.Mathf.Asin(float)"/>.</summary>
		/// <docs>Faster version of Mathf.Asin().</docs>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Angle">The angle to get the inverse Sine of.</param>
		/// <returns>Inverse Sine of Angle.</returns>
		public static float ArcSine(float Angle)
		{
			bool bIsPositive = Angle >= 0f;
			float FAbs = Abs(Angle);

			float OneMinusFAbs = 1f - FAbs;
			ClampMin(ref OneMinusFAbs, 0f);

			float Root = Sqrt(OneMinusFAbs);

			const float kASinHalfPI = 1.5707963050f;

			float Approximation = ((((((-0.0012624911f * FAbs + 0.0066700901f) * FAbs - 0.0170881256f) * FAbs + 0.0308918810f) * FAbs - 0.0501743046f) * FAbs + 0.0889789874f) * FAbs - 0.2145988016f) * FAbs + kASinHalfPI;
			Approximation *= Root;

			return bIsPositive ? kASinHalfPI - Approximation : Approximation - kASinHalfPI;
		}

		/// <summary>Absolute Value of I.</summary>
		/// <decorations decor="public static int"></decorations>
		/// <param name="I">The int to get the Absolute Value of.</param>
		/// <returns>The value of I regardless of its sign.</returns>
		public static int Abs(int I)
		{
			return 0x7FFFFFFF & I;
		}

		/// <summary>Absolute Value of F.</summary>
		/// <decorations decor="public static unsafe float"></decorations>
		/// <param name="F">The float to get the Absolute Value of.</param>
		/// <returns>The value of F regardless of its sign.</returns>
		public static unsafe float Abs(float F)
		{
			int T = Abs(*(int*)&F);
			return *(float*)&T;
		}
	}
}
