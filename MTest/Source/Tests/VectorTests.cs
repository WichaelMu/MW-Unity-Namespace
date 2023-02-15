using Microsoft.VisualStudio.TestTools.UnitTesting;
using MW;
using MW.Extensions;
using MW.Math;
using UnityEngine;
using static MTest.Tolerance;

namespace MTest
{
	[TestClass]
	public class VectorTests
	{
		[TestMethod]
		public unsafe void MVectorTest()
		{
			MVector M = new MVector(1, 2, 3);
			Vector3 U = new Vector3(1, 2, 3);
			Assert.AreEqual((MVector)U, M);
			Assert.AreEqual((Vector3)M, U);

			MVector ML = new MVector(234, 5.94f, -148);
			MVector MR = new MVector(-5738, 2847, 19.24f);

			Vector3 UL = new Vector3(234, 5.94f, -148);
			Vector3 UR = new Vector3(-5738, 2847, 19.24f);

			VectorToleranceCheck(Vector3.Cross(UL, UR), ML ^ MR, "Cross");
			FloatToleranceCheck(Vector3.Dot(UL, UR), ML | MR, "Dot");
			VectorToleranceCheck(UL.normalized, ML.Normalised, "Normalise");
			VectorToleranceCheck(UR.normalized, MR.Normalised, "Normalise");
			FloatToleranceCheck(UL.sqrMagnitude, ML.SqrMagnitude, "Square Magnitude");
			FloatToleranceCheck(UR.sqrMagnitude, MR.SqrMagnitude, "Square Magnitude");
			FloatToleranceCheck(Vector3.Distance(UL, UR), MVector.Distance(ML, MR), "Distance");
			FloatToleranceCheck(Vector3.Distance(UL, UR), Mathf.Sqrt(ML.SqrDistance(MR)), "MVector SqrDistance -> Vector3 Distance");
			VectorToleranceCheck(UL - UR, ML - MR, "Subtraction");
			VectorToleranceCheck(UR - UL, MR - ML, "Subtraction");
			VectorToleranceCheck(-UL, -ML, "Negation");
			VectorToleranceCheck(-UR, -MR, "Negation");
			VectorToleranceCheck(UL + UR, ML + MR, "Addition");
			VectorToleranceCheck(UR + ML, ML + UR, "Cross Addition");

			for (float F = -10f; F <= 10f; F += .7f)
			{
				VectorToleranceCheck(F * UL, F * ML, "Multiplication by: " + F);
				VectorToleranceCheck(UL / F, ML / F, "Division by: " + F);
			}

			VectorToleranceCheck((UR - UL).normalized, ML > MR, "Direction");
			VectorToleranceCheck((UL - UR).normalized, ML < MR, "Direction");

			Assert.IsTrue(Mathematics.IsNormalised(ML.Normalised));

			// M = (1, 2, 3).
			Assert.AreEqual(M >> 1, new MVector(3, 1, 2));
			Assert.AreEqual(M << 1, new MVector(2, 3, 1));
			Assert.AreEqual(M, new MVector(1, 2, 3));

			Assert.AreEqual(M >> 2, new MVector(2, 3, 1));
			Assert.AreEqual(M << 2, new MVector(3, 1, 2));

			Assert.AreEqual(M >> 3, M);
			Assert.AreEqual(M << 3, M);

			Assert.AreEqual(M >> 4, M >> 1);
			Assert.AreEqual(M << 4, M << 1);

			Assert.AreEqual(Mathematics.SqrDistance(UL, Vector3.zero), ML.SqrMagnitude);
			VectorToleranceCheck(UL.FNormalise(), UL.normalized, "V3 Extension Normalise");
			Assert.AreEqual(Mathematics.Distance(UL, UR), Vector3.Distance(UL, UR));

			Vector3 V = new Vector3(2f, 4f, 6f);
			FVector Clone = FVector.Clone(ref V);

			Assert.AreEqual(Clone.MV(), V.MV());

			V.x = 8;
			V.y = 10;
			V.z = 12;

			Assert.AreEqual(V.MV(), Clone.MV());
			Assert.AreEqual(V.x, *Clone.pX);
			Assert.AreEqual(V.y, *Clone.pY);
			Assert.AreEqual(V.z, *Clone.pZ);

			*Clone.pX = 4f;
			*Clone.pY = 2f;
			*Clone.pZ = 0f;

			Assert.AreEqual(V.x, *Clone.pX);
			Assert.AreEqual(V.y, *Clone.pY);
			Assert.AreEqual(V.z, *Clone.pZ);

			MVector MVectorRotate = new MVector(4f, 2f, 0f);
			MVector Rotated = MVectorRotate.RotateVector(78.24f, MVector.Up);
			Assert.AreEqual(Rotated, Clone.RotateVector(78.24f, Vector3.up).MV());

			Assert.AreEqual(V.normalized.MV(), Clone.FNormalise().MV());

			Clone.Dispose();

			FloatToleranceCheck(Vector3.Angle(Vector3.up, Vector3.right), MVector.Angle(MVector.Up, MVector.Right), "Fast Angle", .01f);

			FloatToleranceCheck(Vector3.Angle(Vector3.up, Vector3.down), MVector.Angle(MVector.Up, -MVector.Up), "Fast Angle", .01f);

			FloatToleranceCheck(Vector3.Angle(Vector3.up, new Vector3(-.1f, -.9f)), MVector.Angle(MVector.Up, new MVector(-.1f, -.9f)), "Fast Angle", .01f);

			MVector L, R;
			L = RandomVector(new System.Random());
			R = RandomVector(new System.Random());
			FloatToleranceCheck(Vector3.Angle(L, R), MVector.Angle(L, R), "Fast Angle", .01f);
		}

		[TestMethod]
		public void VectorRotationTest()
		{
			MVector ToRotate = MVector.Up;
			MVector Result;

			// Clockwise.
			Result = ToRotate.RotateVector(45f, MVector.Forward);
			Assert.AreEqual(MVector.One.XY.Normalised, Result, "45 Clockwise");

			Result = ToRotate.RotateVector(90f, MVector.Forward);
			Assert.AreEqual(MVector.Right, Result, "90 Clockwise");

			Result = ToRotate.RotateVector(135f, MVector.Forward);
			Assert.AreEqual(new MVector(1f, -1f).Normalised, Result, "135 Clockwise");

			Result = ToRotate.RotateVector(180f, MVector.Forward);
			Assert.AreEqual(-MVector.Up, Result, "180 Clockwise");

			Result = ToRotate.RotateVector(225f, MVector.Forward);
			Assert.AreEqual(-MVector.One.XY.Normalised, Result, "225 Clockwise");

			Result = ToRotate.RotateVector(270f, MVector.Forward);
			Assert.AreEqual(-MVector.Right, Result, "270 Clockwise");

			Result = ToRotate.RotateVector(315f, MVector.Forward);
			Assert.AreEqual(new MVector(-1f, 1f).Normalised, Result, "315 Clockwise");

			Result = ToRotate.RotateVector(360f, MVector.Forward);
			Assert.AreEqual(ToRotate, Result, "360 Clockwise");

			// Clockwise Wrapped.
			Result = ToRotate.RotateVector(360f + 45f, MVector.Forward);
			Assert.AreEqual(MVector.One.XY.Normalised, Result, "45 Clockwise Wrapped");

			Result = ToRotate.RotateVector(360f + 90f, MVector.Forward);
			Assert.AreEqual(MVector.Right, Result, "90 Clockwise Wrapped");

			Result = ToRotate.RotateVector(360f + 135f, MVector.Forward);
			Assert.AreEqual(new MVector(1f, -1f).Normalised, Result, "135 Clockwise Wrapped");

			Result = ToRotate.RotateVector(360f + 180f, MVector.Forward);
			Assert.AreEqual(-MVector.Up, Result, "180 Clockwise Wrapped");

			Result = ToRotate.RotateVector(360f + 225f, MVector.Forward);
			Assert.AreEqual(-MVector.One.XY.Normalised, Result, "225 Clockwise Wrapped");

			Result = ToRotate.RotateVector(360f + 270f, MVector.Forward);
			Assert.AreEqual(-MVector.Right, Result, "270 Clockwise Wrapped");

			Result = ToRotate.RotateVector(360f + 315f, MVector.Forward);
			Assert.AreEqual(new MVector(-1f, 1f).Normalised, Result, "315 Clockwise Wrapped");

			Result = ToRotate.RotateVector(360f + 360f, MVector.Forward);
			Assert.AreEqual(ToRotate, Result, "360 Clockwise Wrapped");

			// Anti-Clockwise.
			Result = ToRotate.RotateVector(-45f, MVector.Forward);
			Assert.AreEqual(new MVector(-1f, 1f).Normalised, Result, "45 Anti-Clockwise");

			Result = ToRotate.RotateVector(-90f, MVector.Forward);
			Assert.AreEqual(-MVector.Right, Result, "90 Anti-Clockwise");

			Result = ToRotate.RotateVector(-135f, MVector.Forward);
			Assert.AreEqual(-MVector.One.XY.Normalised, Result, "135 Anti-Clockwise");

			Result = ToRotate.RotateVector(-180f, MVector.Forward);
			Assert.AreEqual(-MVector.Up, Result, "180 Anti-Clockwise");

			Result = ToRotate.RotateVector(-225f, MVector.Forward);
			Assert.AreEqual(new MVector(1f, -1f).Normalised, Result, "225 Anti-Clockwise");

			Result = ToRotate.RotateVector(-270f, MVector.Forward);
			Assert.AreEqual(MVector.Right, Result, "270 Anti-Clockwise");

			Result = ToRotate.RotateVector(-315f, MVector.Forward);
			Assert.AreEqual(MVector.One.XY.Normalised, Result, "315 Anti-Clockwise");

			Result = ToRotate.RotateVector(-360f, MVector.Forward);
			Assert.AreEqual(ToRotate, Result, "360 Anti-Clockwise");

			// Anti-Clockwise Wrapped.
			Result = ToRotate.RotateVector(-360f - 45f, MVector.Forward);
			Assert.AreEqual(new MVector(-1f, 1f).Normalised, Result, "45 Anti-Clockwise Wrapped");

			Result = ToRotate.RotateVector(-360f - 90f, MVector.Forward);
			Assert.AreEqual(-MVector.Right, Result, "90 Anti-Clockwise Wrapped");

			Result = ToRotate.RotateVector(-360f - 135f, MVector.Forward);
			Assert.AreEqual(-MVector.One.XY.Normalised, Result, "135 Anti-Clockwise Wrapped");

			Result = ToRotate.RotateVector(-360f - 180f, MVector.Forward);
			Assert.AreEqual(-MVector.Up, Result, "180 Anti-Clockwise Wrapped");

			Result = ToRotate.RotateVector(-360f - 225f, MVector.Forward);
			Assert.AreEqual(new MVector(1f, -1f).Normalised, Result, "225 Anti-Clockwise Wrapped");

			Result = ToRotate.RotateVector(-360f - 270f, MVector.Forward);
			Assert.AreEqual(MVector.Right, Result, "270 Anti-Clockwise Wrapped");

			Result = ToRotate.RotateVector(-360f - 315f, MVector.Forward);
			Assert.AreEqual(MVector.One.XY.Normalised, Result, "315 Anti-Clockwise Wrapped");

			Result = ToRotate.RotateVector(-360f - 360f, MVector.Forward);
			Assert.AreEqual(ToRotate, Result, "360 Anti-Clockwise Wrapped");

			// Arbitrary Angle-Axis Rotations.
			System.Random R = new System.Random();
			const float kRotateBoundary = 1440f;

			for (int ArbitraryAngleIteration = 0; ArbitraryAngleIteration < 1000; ++ArbitraryAngleIteration)
			{
				float RandomAngle = R.NextSingle();
				RandomAngle = (RandomAngle * 2f - 1f) * kRotateBoundary;
				MVector RandAxis = RandomVector(R);
				ToRotate = MVector.Cross(RandomVector(R), RandAxis).Normalised;

				MVector Rotated = ToRotate.RotateVector(RandomAngle, RandAxis);
				FloatToleranceCheck(Vector3.Angle(ToRotate, Rotated), MVector.Angle(ToRotate, Rotated), $"Arbitrary Rotation {RandomAngle} Degrees\nAngle Iteration: {ArbitraryAngleIteration}\nToRotate: {ToRotate}\nRotated: {Rotated}", .1f);
			}
		}

		public static MVector RandomVector(System.Random R)
		{
			float RandX = (R.NextSingle() * 2f) - 1f;
			float RandY = (R.NextSingle() * 2f) - 1f;
			float RandZ = (R.NextSingle() * 2f) - 1f;
			MVector RandAxis = new MVector(RandX, RandY, RandZ);

			return RandAxis.Normalised;
		}
	}
}
