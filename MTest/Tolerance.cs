#if MICROSOFT_TESTING
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using MTest.Output;
#endif

using MW;
using UnityEngine;

namespace MTest
{
	internal static class Tolerance
	{
		const float kFloatingPointTolerancePercentage = .001f; // .001 = .1% error.
		const float kVectorTolerancePercentage = .001f; // .001 = .1% error.

		public static void FloatToleranceCheck(int TestNumber, float L, float R, string Operation
#if !MICROSOFT_TESTING
			, ref int Passed
#endif
			)
		{
#if MICROSOFT_TESTING
			Assert.AreEqual(L, R, kFloatingPointTolerancePercentage, Operation);
#else
			if (Mathf.Abs(L - R) > kFloatingPointTolerancePercentage)
				O.Failed(TestNumber, L, R, Operation);
			else
				Passed++;
#endif
		}

		public static void VectorToleranceCheck(int TestNumber, MVector M, Vector3 U, string Operation
#if !MICROSOFT_TESTING
			, ref int Passed
#endif
			)
		{
#if MICROSOFT_TESTING
			float X = M.X - U.x;
			float Y = M.Y - U.y;
			float Z = M.Z - U.z;

			X = Mathf.Abs(X);
			Y = Mathf.Abs(Y);
			Z = Mathf.Abs(Z);

			Assert.IsFalse(X > kVectorTolerancePercentage, Operation);
			Assert.IsFalse(Y > kVectorTolerancePercentage, Operation);
			Assert.IsFalse(Z > kVectorTolerancePercentage, Operation);
#else

			bool bFailed = X > kVectorTolerancePercentage || Y > kVectorTolerancePercentage || Z > kVectorTolerancePercentage;

			if (bFailed)
				O.Failed(TestNumber, U, M, Operation);
			else
				Passed++;
#endif
		}
	}
}