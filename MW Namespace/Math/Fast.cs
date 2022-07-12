using static MW.Utils;

namespace MW.Math.Magic
{
	/// <summary>Fast implementations of common Mathematical functions using Bit Magic.</summary>
	public static class Fast
	{
		/// <summary>1 / sqrt(N).</summary>
		/// <remarks>Modified from: <see href="https://github.com/id-Software/Quake-III-Arena/blob/dbe4ddb10315479fc00086f08e25d968b4b43c49/code/game/q_math.c#L552"/></remarks>
		/// <docremarks>Modified from: &lt;a href="https://github.com/id-Software/Quake-III-Arena/blob/dbe4ddb10315479fc00086f08e25d968b4b43c49/code/game/q_math.c#L552"&gt;The 'Quake III Fast Inv. Sqrt Algorithm'&lt;/a&gt;</docremarks>
		/// <param name="N">1 / sqrt(x) where x is N.</param>
		/// <param name="Iterations">The number of Newton Iterations to perform.</param>
		/// <returns>An approximation for calculating: 1 / sqrt(N).</returns>
		public static unsafe float InverseSqrt(float N, int Iterations = 1)
		{
			int F = *(int*)&N;
			F = 0x5F3759DF - (F >> 1);
			float X = *(float*)&F;

			float ISqrt = X * (1.5f - .5f * N * X * X);
			for (int i = 0; i < Iterations; ++i)
				ISqrt *= (1.5f - .5f * N * ISqrt * ISqrt);
			return ISqrt;
		}

		/// <summary>Faster version of <see cref="UnityEngine.Mathf.Sqrt(float)"/>.</summary>
		/// <docs>Faster version of Mathf.Sqrt().</docs>
		/// <param name="F"></param>
		/// <param name="Iterations">The number of Newton Iterations to perform.</param>
		/// <returns>An approximation for the Square Root of F.</returns>
		public static float Sqrt(float F, int Iterations = 2) => InverseSqrt(Max(F, MVector.kEpsilon), Iterations) * F;

		/// <summary>Absolute Value of I.</summary>
		/// <param name="I">The int to get the Absolute Value of.</param>
		/// <returns>The value of I regardless of its sign.</returns>
		public static int Abs(int I)
		{
			return 0x7FFFFFFF & I;
		}

		/// <summary>Modifies I to be its Absolute Value.</summary>
		/// <remarks><see langword="ref"/> version of <see cref="Abs(int)"/>.</remarks>
		/// <docremarks>Ref&amp; version of FastAbs().</docremarks>
		/// <param name="I">A reference to the int to modify.</param>
		public static void Abs(ref int I) => I = Abs(I);

		/// <summary>Absolute Value of F.</summary>
		/// <param name="F">The float to get the Absolute Value of.</param>
		/// <returns>The value of F regardless of its sign.</returns>
		public static unsafe float Abs(float F)
		{
			int T = Abs(*(int*)&F);
			return *(float*)&T;
		}

		/// <summary>Modifies F to be its Absolute Value.</summary>
		/// <remarks><see langword="ref"/> version of <see cref="Abs(float)"/></remarks>
		/// <docremarks>Ref&amp; version of FastAbs().</docremarks>
		/// <param name="F">A ference to the float to modify.</param>
		public static void Abs(ref float F) => F = Abs(F);
	}
}
