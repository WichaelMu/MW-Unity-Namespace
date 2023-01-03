using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MW;
using MW.Math.Magic;
using UnityEngine;

namespace MTest
{
	internal static class Tolerance
	{
		const float kFloatingPointTolerancePercentage = .001f; // .001 = .1% error.
		const float kVectorTolerancePercentage = .001f; // .001 = .1% error.

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void FloatToleranceCheck(float Expected, float Actual, string Operation, float ToleranceOverride = kFloatingPointTolerancePercentage)
		{
			Assert.AreEqual(Expected, Actual, ToleranceOverride, $"{Operation}\nDifference: {Expected - Actual:F6}");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void VectorToleranceCheck(MVector M, Vector3 U, string Operation, float ToleranceOverride = kVectorTolerancePercentage)
		{
			float X = M.X - U.x;
			float Y = M.Y - U.y;
			float Z = M.Z - U.z;

			Fast.FAbs(ref X);
			Fast.FAbs(ref Y);
			Fast.FAbs(ref Z);

			Assert.IsFalse(X > ToleranceOverride, Operation);
			Assert.IsFalse(Y > ToleranceOverride, Operation);
			Assert.IsFalse(Z > ToleranceOverride, Operation);
		}
	}
}