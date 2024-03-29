﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MW;
using MW.Math;
using static MTest.Tolerance;
using static MW.Math.Magic.Fast;

namespace MTest
{
	[TestClass]
	public class ApproximationTests
	{
		[TestMethod]
		public void SinCosTests()
		{
			for (float F = -365f; F <= 365f; F += .07f)
			{
				Mathematics.SinCos(out float S, out float C, F);
				FloatToleranceCheck(MathF.Sin(F), S, "Sine", .000024f);
				FloatToleranceCheck(MathF.Cos(F), C, "Cosine", .000024f);
			}
		}

		[TestMethod]
		public void SqrtTests()
		{
			// This also checks FInverseSqrt()...

			for (float F = 0f; F <= 1500.442; F += .023f)
			{
				FloatToleranceCheck(MathF.Sqrt(F), FSqrt(F), "Fast Sqrt", .00001f);
			}

			Random R = new Random();
			for (float F = 0f; F <= 1f; F += .00001f)
			{
				MVector V1 = VectorTests.RandomVector(R);
				MVector V2 = VectorTests.RandomVector(R);
				float SquareMagnitude = V1.SqrMagnitude * V2.SqrMagnitude;
				FloatToleranceCheck(MathF.Sqrt(SquareMagnitude), FSqrt(SquareMagnitude), "Lim -> 0 Fast Sqrt", .00001f);
			}
		}

		[TestMethod]
		public void InverseTests()
		{
			for (float F = -1500.442f; F <= 1500.422f; F += .023f)
			{
				FloatToleranceCheck(1f / F, FInverse(F), "Fast Inverse", .000008f);
			}
		}

		[TestMethod]
		public void IntegralTests()
		{
			FloatToleranceCheck(177.94934906f, FIntegral(0, FMath.kPI, F => MathF.Pow(6.33f, F)), "Fast Integral", .04f);
		}

		[TestMethod]
		public void ASinTests()
		{
			for (float F = -1f; F <= 1f; F += .023f)
			{
				FloatToleranceCheck(MathF.Asin(F), FArcSine(F), $"Fast ASin {F}");
			}
		}

		[TestMethod]
		public void ACosTests()
		{
			for (float F = -1f; F <= 1f; F += .001f)
			{
				FloatToleranceCheck(MathF.Acos(F), FArcCosine(F), $"Fast ACosine {F}");
			}
		}

		[TestMethod]
		public void ATanTests()
		{
			for (float F = -1500.422f; F < 1500.422f; F += .023f)
			{
				FloatToleranceCheck(MathF.Atan(F), FArcTangent(F), $"Fast ATan {F}", .00136f);
			}
		}

		[TestMethod]
		public void ATan2Tests()
		{
			for (float Y = -50.442f; Y <= 50.422f; Y += .23f)
			{
				for (float X = 25.422f; X >= -25.422f; X -= .23f)
				{
					FloatToleranceCheck(MathF.Atan2(Y, X), FArcTangent2(Y, X), $"Fast ATan2 {Y}, {X}", .00136f);
				}
			}
		}
	}
}
