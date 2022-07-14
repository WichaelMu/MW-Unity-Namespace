using MTest.Output;
using MW;
using UnityEngine;

namespace MTest
{
	internal static class Tolerance
	{
		const float kFloatingPointTolerancePercentage = .001f; // .001 = .1% error.
		const float kVectorTolerancePercentage = .001f; // .001 = .1% error.

		public static void FloatToleranceCheck(int TestNumber, float L, float R, string Operation, ref int Passed)
		{
			if (Mathf.Abs(L - R) > kFloatingPointTolerancePercentage)
				O.Failed(TestNumber, L, R, Operation);
			else
				Passed++;
		}

		public static void VectorToleranceCheck(int TestNumber, MVector M, Vector3 U, string Operation, ref int Passed)
		{
			float X = M.X - U.x;
			float Y = M.Y - U.y;
			float Z = M.Z - U.z;

			X = Mathf.Abs(X);
			Y = Mathf.Abs(Y);
			Z = Mathf.Abs(Z);

			bool bFailed = X > kVectorTolerancePercentage || Y > kVectorTolerancePercentage || Z > kVectorTolerancePercentage;

			if (bFailed)
				O.Failed(TestNumber, U, M, Operation);
			else
				Passed++;
		}
	}
}