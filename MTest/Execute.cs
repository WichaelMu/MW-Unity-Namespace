#if MICROSOFT_TESTING
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using MW;
using MW.Extensions;
using MW.Math;
using UnityEngine;
#if !MICROSOFT_TESTING
using MTest.Output;
using static MTest.Assertion;
#endif
using static MTest.Tolerance;
using static MW.Math.Magic.Fast;

namespace MTest
{
#if MICROSOFT_TESTING
	[TestClass]
#endif
	public class CoreTests
	{
#if MICROSOFT_TESTING
		[TestMethod]
#endif
		public void RoundToDPTest(
#if MICROSOFT_TESTING
#else
			out int Passed, out int TotalTests
#endif
			)
		{
#if MICROSOFT_TESTING
#else
			Passed = 0;
			int TestsPassed = 0;
			int TestNumber = 1;
#endif
			float Number, Result;
			int DP;

			Number = 0.00f;
			DP = 2;
			Result = Utils.RoundToDP(Number, DP);
#if MICROSOFT_TESTING
			Assert.AreEqual(0.000f, Result);
#else
			DiagnosticCheckFailed(0.000f, nameof(Utils.RoundToDP));
			TestNumber = 2;
#endif

			Number = -0.00001f;
			DP = 2;
			Result = Utils.RoundToDP(Number, DP);
#if MICROSOFT_TESTING
			Assert.AreEqual(0.000f, Result);
#else
			DiagnosticCheckFailed(0.000f, nameof(Utils.RoundToDP));
			TestNumber = 3;
#endif

			Number = 0.00001f;
			DP = 2;
			Result = Utils.RoundToDP(Number, DP);
#if MICROSOFT_TESTING
			Assert.AreEqual(0.000f, Result);
#else
			DiagnosticCheckFailed(0.000f, nameof(Utils.RoundToDP));
			TestNumber = 4;
#endif

			Number = 127.47329f;
			DP = 2;
			Result = Utils.RoundToDP(Number, DP);
#if MICROSOFT_TESTING
			Assert.AreEqual(127.47f, Result);
#else
			DiagnosticCheckFailed(127.47f, nameof(Utils.RoundToDP));
			TestNumber = 5;
#endif

			Number = -94275.4298347f;
			DP = 2;
			Result = Utils.RoundToDP(Number, DP);
#if MICROSOFT_TESTING
			Assert.AreEqual(-94275.43f, Result);
#else
			DiagnosticCheckFailed(-94275.43f, nameof(Utils.RoundToDP));
			TestNumber = 6;
#endif

			Number = 5.5f;
			DP = 0;
			Result = Utils.RoundToDP(Number, DP);
#if MICROSOFT_TESTING
			Assert.AreEqual(6f, Result);
#else
			DiagnosticCheckFailed(6f, nameof(Utils.RoundToDP));
			TestNumber = 7;
#endif

			Number = 3.14159f;
			DP = 5;
			Result = Utils.RoundToDP(Number, DP);
#if MICROSOFT_TESTING
			Assert.AreEqual(3.14159f, Result);
#else
			DiagnosticCheckFailed(3.14159f, nameof(Utils.RoundToDP));
			TestNumber = 8;
#endif

			Number = 3.14159f;
			DP = 9;
			Result = Utils.RoundToDP(Number, DP);
#if MICROSOFT_TESTING
			Assert.AreEqual(3.14159f, Result);
#else
			DiagnosticCheckFailed(3.14159f, nameof(Utils.RoundToDP));
			TestNumber = 9;
#endif

			Number = 3.14159f;
			DP = 1;
			Result = Utils.RoundToDP(Number, DP);
#if MICROSOFT_TESTING
			Assert.AreEqual(3.1f, Result);
#else
			DiagnosticCheckFailed(3.1f, nameof(Utils.RoundToDP));
			TestNumber = 10;
#endif

			Number = 273489f;
			DP = 10;
			Result = Utils.RoundToDP(Number, DP);
#if MICROSOFT_TESTING
			Assert.AreEqual(273489f, Result);
#else
			DiagnosticCheckFailed(273489f, nameof(Utils.RoundToDP));
			TestNumber = 11;
#endif

			Number = .5f;
			DP = 1;
			Result = Utils.RoundToDP(Number, DP);
#if MICROSOFT_TESTING
			Assert.AreEqual(.5f, Result);
#else
			DiagnosticCheckFailed(.5f, nameof(Utils.RoundToDP));
			TestNumber = 12;
#endif

			Number = .49999999999999f;
			DP = 1;
			Result = Utils.RoundToDP(Number, DP);
#if MICROSOFT_TESTING
			Assert.AreEqual(.5f, Result);
#else
			DiagnosticCheckFailed(.5f, nameof(Utils.RoundToDP));
			TestNumber = 13;
#endif

			Number = .50000000000001f;
			DP = 1;
			Result = Utils.RoundToDP(Number, DP);
#if MICROSOFT_TESTING
			Assert.AreEqual(.5f, Result);
#else
			DiagnosticCheckFailed(.5f, nameof(Utils.RoundToDP));
			TestNumber = 14;
#endif

			Number = .49999999999999f;
			DP = 5;
			Result = Utils.RoundToDP(Number, DP);
#if MICROSOFT_TESTING
			Assert.AreEqual(.5f, Result);
#else
			DiagnosticCheckFailed(.5f, nameof(Utils.RoundToDP));
			TestNumber = 15;
#endif

			Number = .50000000000001f;
			DP = 5;
			Result = Utils.RoundToDP(Number, DP);
#if MICROSOFT_TESTING
			Assert.AreEqual(.5f, Result);
#else
			DiagnosticCheckFailed(.5f, nameof(Utils.RoundToDP));
			TestNumber = 16;
#endif

			Number = 4333f;
			DP = -2;
			Result = Utils.RoundToDP(Number, DP);
#if MICROSOFT_TESTING
			Assert.AreEqual(4300f, Result);
#else
			DiagnosticCheckFailed(4300f, nameof(Utils.RoundToDP));
#endif

#if !MICROSOFT_TESTING
			void DiagnosticCheckFailed(float Expected, string Function)
			{
				if (Expected != Result)
				{
					O.Failed(TestNumber, Expected, Result, Function, Number, DP);
				}
				else
				{
					TestsPassed++;
				}
			}
			Passed = TestsPassed;
			TotalTests = TestNumber;
#endif
		}

#if MICROSOFT_TESTING
		[TestMethod]
#endif
		public void SwapTests(
#if MICROSOFT_TESTING
#else
			out int Passed, out int TotalTests
#endif
			)
		{
#if !MICROSOFT_TESTING
			Passed = 0;
#endif

			// Base Tests.
			int L = 1, C = 2, R = 3;

			Utils.Swap(ref L, ref R);
			Utils.Swap(ref R, ref C);

#if MICROSOFT_TESTING
			Assert.AreEqual(L, 3);
			Assert.AreEqual(R, 2);
			Assert.AreEqual(C, 1);
#else
			Assert(1, L == 3, ref Passed, "Swap Int");
			Assert(2, R == 2, ref Passed, "Swap Int");
			Assert(3, C == 1, ref Passed, "Swap Int");
#endif

			// Reference Tests.
			TTestClass T1, T2, T3;
			T1 = new TTestClass(-1);
			T2 = new TTestClass(0);
			T3 = new TTestClass(1);

			Utils.Swap(ref T1, ref T3);
			Utils.Swap(ref T3, ref T2);

#if MICROSOFT_TESTING
			Assert.AreEqual(T1.Value, 1);
			Assert.AreEqual(T3.Value, 0);
			Assert.AreEqual(T2.Value, -1);
#else
			Assert(4, T1.Value == 1, ref Passed, "Swap Reference Type");
			Assert(5, T3.Value == 0, ref Passed, "Swap Reference Type");
			Assert(6, T2.Value == -1, ref Passed, "Swap Reference Type");
#endif

			// Structure Tests.
			MVector NOne = new MVector(-1);
			MVector Zero = MVector.Zero;
			MVector One = MVector.One;

			Utils.Swap(ref NOne, ref One);
			Utils.Swap(ref One, ref Zero);

#if MICROSOFT_TESTING
			Assert.AreEqual(NOne, MVector.One);
			Assert.AreEqual(One, MVector.Zero);
			Assert.AreEqual(Zero, new MVector(-1));
#else
			Assert(7, NOne == MVector.One, ref Passed, "Swap Structure");
			Assert(8, One == MVector.Zero, ref Passed, "Swap Structure");
			Assert(9, Zero == new MVector(-1), ref Passed, "Swap Structure");

			TotalTests = 9;
#endif
		}

#if MICROSOFT_TESTING
		[TestMethod]
#endif
		public void SqrtTests(
#if !MICROSOFT_TESTING
			out int Passed, out int TotalTests
#endif
			)
		{
#if !MICROSOFT_TESTING
			Passed = 0;
			TotalTests = 50;
#endif
			for (float f = 0; f < 50; f += .23f)
			{
				FloatToleranceCheck(1, Mathf.Sqrt(f), FSqrt(f), "Fast Sqrt"
#if !MICROSOFT_TESTING
					, ref Passed
#endif
					);
			}
#if !MICROSOFT_TESTING
			TotalTests = (int)(TotalTests / .23f) + 1;
#endif
		}
	}

#if MICROSOFT_TESTING
	[TestClass]
#endif
	public class MWTests
	{
#if MICROSOFT_TESTING
		[TestMethod]
#endif
		public unsafe void MVectorTest(
#if MICROSOFT_TESTING
#else
			out int Passed, out int TotalTests
#endif
			)
		{
			MVector M = new MVector(1, 2, 3);
			Vector3 U = new Vector3(1, 2, 3);
#if MICROSOFT_TESTING
			Assert.AreEqual((MVector)U, M);
			Assert.AreEqual((Vector3)M, U);
#else
			if ((MVector)U != M)
			{
				O.Failed(1, M.ToString(), ((MVector)U).ToString(), "MVector -> Vector3 Implicit Conversion", M, U);
				Environment.Exit(-1);
			}

			if ((Vector3)M != U)
			{
				O.Failed(1, U.ToString(), ((Vector3)M).ToString(), "Vector3 -> MVector Implicit Conversion", U, M);
				Environment.Exit(-1);
			}
#endif

			MVector ML = new MVector(234, 5.94f, -148);
			MVector MR = new MVector(-5738, 2847, 19.24f);

			Vector3 UL = new Vector3(234, 5.94f, -148);
			Vector3 UR = new Vector3(-5738, 2847, 19.24f);

#if !MICROSOFT_TESTING
			Passed = 0;
#endif
			VectorToleranceCheck(1, Vector3.Cross(UL, UR), ML ^ MR, "Cross"
#if !MICROSOFT_TESTING
			, ref Passed
#endif
);
			FloatToleranceCheck(2, Vector3.Dot(UL, UR), ML | MR, "Dot"
#if !MICROSOFT_TESTING
			, ref Passed
#endif
);
			VectorToleranceCheck(3, UL.normalized, ML.Normalised, "Normalise"
#if !MICROSOFT_TESTING
			, ref Passed
#endif
);
			VectorToleranceCheck(4, UR.normalized, MR.Normalised, "Normalise"
#if !MICROSOFT_TESTING
			, ref Passed
#endif
);
			FloatToleranceCheck(5, UL.sqrMagnitude, ML.SqrMagnitude, "Square Magnitude"
#if !MICROSOFT_TESTING
			, ref Passed
#endif
);
			FloatToleranceCheck(6, UR.sqrMagnitude, MR.SqrMagnitude, "Square Magnitude"
#if !MICROSOFT_TESTING
			, ref Passed
#endif
);
			FloatToleranceCheck(7, Vector3.Distance(UL, UR), MVector.Distance(ML, MR), "Distance"
#if !MICROSOFT_TESTING
			, ref Passed
#endif
);
			FloatToleranceCheck(8, Vector3.Distance(UL, UR), Mathf.Sqrt(ML.SqrDistance(MR)), "MVector SqrDistance -> Vector3 Distance"
#if !MICROSOFT_TESTING
			, ref Passed
#endif
);
			VectorToleranceCheck(9, UL - UR, ML - MR, "Subtraction"
#if !MICROSOFT_TESTING
			, ref Passed
#endif
);
			VectorToleranceCheck(10, UR - UL, MR - ML, "Subtraction"
#if !MICROSOFT_TESTING
			, ref Passed
#endif
);
			VectorToleranceCheck(11, -UL, -ML, "Negation"
#if !MICROSOFT_TESTING
			, ref Passed
#endif
);
			VectorToleranceCheck(12, -UR, -MR, "Negation"
#if !MICROSOFT_TESTING
			, ref Passed
#endif
);
			VectorToleranceCheck(13, UL + UR, ML + MR, "Addition"
#if !MICROSOFT_TESTING
			, ref Passed
#endif
);
			VectorToleranceCheck(14, UR + ML, ML + UR, "Cross Addition"
#if !MICROSOFT_TESTING
			, ref Passed
#endif
);

			for (float F = -10f; F <= 10f; F += .7f)
			{
				VectorToleranceCheck(15, F * UL, F * ML, "Multiplication by: " + F
#if !MICROSOFT_TESTING
					, ref Passed
#endif
					);
				VectorToleranceCheck(16, UL / F, ML / F, "Division by: " + F
#if !MICROSOFT_TESTING
					, ref Passed
#endif
					);
			}

			VectorToleranceCheck(17, (UR - UL).normalized, ML > MR, "Direction"
#if !MICROSOFT_TESTING
					, ref Passed
#endif
					);
			VectorToleranceCheck(18, (UL - UR).normalized, ML < MR, "Direction"
#if !MICROSOFT_TESTING
					, ref Passed
#endif
					);

#if MICROSOFT_TESTING
			Assert.IsTrue(Mathematics.IsNormalised(ML.Normalised));
#else
			Assert(19, Mathematics.IsNormalised(ML.Normalised), ref Passed, nameof(Mathematics.IsNormalised));
#endif

			// M = (1, 2, 3).
#if MICROSOFT_TESTING
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
			VectorToleranceCheck(30, UL.Normalise(), UL.normalized, "V3 Extension Normalise");
			Assert.AreEqual(Mathematics.Distance(UL, UR), Vector3.Distance(UL, UR));
#else
			AssertEquals(20, M >> 1, new MVector(3, 1, 2), ref Passed, "Right Shift 1");
			AssertEquals(21, M << 1, new MVector(2, 3, 1), ref Passed, "Left Shift 1");
			AssertEquals(22, M, new MVector(1, 2, 3), ref Passed, "Not Modified After Shifting");

			AssertEquals(23, M >> 2, new MVector(2, 3, 1), ref Passed, "Right Shift 2");
			AssertEquals(24, M << 2, new MVector(3, 1, 2), ref Passed, "Left Shift 2");

			AssertEquals(25, M >> 3, M, ref Passed, "Right Shift 3");
			AssertEquals(26, M << 3, M, ref Passed, "Left Shift 3");

			AssertEquals(27, M >> 4, M >> 1, ref Passed, "Right Shift 4 = 1");
			AssertEquals(28, M << 4, M << 1, ref Passed, "Left Shift 4 = 1");

			AssertEquals(29, Mathematics.SqrDistance(UL, Vector3.zero), ML.SqrMagnitude, ref Passed, "V3 Extension SqrDist");
			AssertEquals(30, UL.Normalise(), UL.normalized, ref Passed, "V3 Extension Normalise");
			AssertEquals(31, Mathematics.Distance(UL, UR), Vector3.Distance(UL, UR), ref Passed, "V3 Extension Dist");
#endif

			Vector3 V = new Vector3(2f, 4f, 6f);
			UVector Clone = UVector.Clone(ref V);
#if MICROSOFT_TESTING
			Assert.AreEqual(Clone.Construct(), V.MV());
#else
			AssertEquals(32, Clone.Construct(), V, ref Passed, "Clone Equality");
#endif
			V.x = 8;
			V.y = 10;
			V.z = 12;
#if MICROSOFT_TESTING
			Assert.AreEqual(Clone.Construct(), V.MV());
#else
			AssertEquals(33, Clone.Construct(), V, ref Passed, "Clone Equality");
#endif
			Clone.Dispose();

#if !MICROSOFT_TESTING
			// Total number of tests (31) + the difference between -10 and 10 divided by .7 (28) * 2 for both Multiplication and Division.
			TotalTests = 33 + 28 * 2;
#endif
		}

#if MICROSOFT_TESTING
		[TestMethod]
#endif
		public void MArrayTests(
#if MICROSOFT_TESTING
#else
			out int Passed, out int TotalTests
#endif
			)
		{
#if !MICROSOFT_TESTING
			Passed = 0;
#endif
			MArray<int> M = new MArray<int>(17);
			M.Push(17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1);

#if MICROSOFT_TESTING
			Assert.IsTrue(M.Contains(1));
			Assert.IsTrue(M.Contains(2));
			Assert.IsTrue(M.Contains(3));
			Assert.IsTrue(M.Contains(4));

			Assert.IsFalse(M.Contains(18));
			Assert.IsFalse(M.Contains(-1));
			Assert.IsFalse(M.Contains(54));
			Assert.IsFalse(M.Contains(00));
#else
			Assert(1, M.Contains(1), ref Passed, "Contains");
			Assert(2, M.Contains(2), ref Passed, "Contains");
			Assert(3, M.Contains(3), ref Passed, "Contains");
			Assert(4, M.Contains(4), ref Passed, "Contains");

			Assert(5, !M.Contains(18), ref Passed, "Contains");
			Assert(6, !M.Contains(-1), ref Passed, "Contains");
			Assert(7, !M.Contains(54), ref Passed, "Contains");
			Assert(8, !M.Contains(00), ref Passed, "Contains");
#endif

#if MICROSOFT_TESTING
			Assert.IsTrue(M.Num == 17);
#else
			Assert(9, M.Num == 17, ref Passed, "Num");
#endif
			M.Push(18);
#if MICROSOFT_TESTING
			Assert.IsTrue(M.Num == 18);
#else
			Assert(10, M.Num == 18, ref Passed, "Num");
#endif

			// M = (17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 18)
#if MICROSOFT_TESTING
			Assert.IsTrue(M.Contains(18));

			Assert.IsTrue(M.FirstPop() == 17);
			Assert.IsTrue(M.FirstPop() == 16);
#else
			Assert(11, M.Contains(18), ref Passed, "Push");

			Assert(12, M.FirstPop() == 17, ref Passed, "First Pop");
			Assert(13, M.FirstPop() == 16, ref Passed, "First Pop");
#endif

			// M = (15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 18)
#if MICROSOFT_TESTING
			Assert.IsTrue(M.TopPop() == 18);
			Assert.IsTrue(M.TopPop() == 1);
#else
			Assert(14, M.TopPop() == 18, ref Passed, "Top Pop");
			Assert(15, M.TopPop() == 1, ref Passed, "Top Pop");
#endif

			// M = (15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2)
#if MICROSOFT_TESTING
			Assert.IsTrue(M[0] == 15);

			Assert.IsTrue(M.Mirror(0) == 2);
			Assert.IsTrue(M.Mirror(M.Num - 1) == 15);

			Assert.IsTrue(M.Contains(9));
			Assert.IsTrue(M[6] == 9);
#else
			Assert(16, M[0] == 15, ref Passed, "Square Bracket Accessor");

			Assert(17, M.Mirror(0) == 2, ref Passed, "Mirror");
			Assert(18, M.Mirror(M.Num - 1) == 15, ref Passed, "Mirror");

			Assert(19, M.Contains(9), ref Passed, "Contains");
			Assert(20, M[6] == 9, ref Passed, "Square Bracket Accessor");
#endif

			M.Pull(9);
#if MICROSOFT_TESTING
			Assert.IsFalse(M.Contains(9));
#else
			Assert(21, !M.Contains(9), ref Passed, "Pull -> Contains");
#endif

			// M = (15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2)

			MArray<int> M2 = new MArray<int>(10);
			M2.Push(-2, 0, 2, 4, 6, 8, 10, 12, 14, 16);

			MArray<int> And = M2 & M;
#if MICROSOFT_TESTING
			Assert.IsTrue(And.Contains(2));
			Assert.IsTrue(And.Contains(10));
			Assert.IsFalse(And.Contains(16));
			Assert.IsFalse(And.Contains(18));
#else
			Assert(22, And.Contains(2), ref Passed, "And -> Contains");
			Assert(23, And.Contains(10), ref Passed, "And -> Contains");
			Assert(24, !And.Contains(16), ref Passed, "And -> Contains");
			Assert(25, !And.Contains(18), ref Passed, "And -> Contains");
#endif
			// M   = (15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2)
			// M2  = (-2, 0, 2, 4, 6, 8, 10, 12, 14, 16)
			// And = (14, 12, 10, 8, 6, 4, 2)

			MArray<int> XOR = M2 ^ M;
#if MICROSOFT_TESTING
			Assert.IsTrue(XOR.Contains(-2));
			Assert.IsTrue(XOR.Contains(0));
			Assert.IsTrue(XOR.Contains(16));
			Assert.IsFalse(XOR.Contains(2));
			Assert.IsTrue(XOR.Num == 3);
#else
			Assert(26, XOR.Contains(-2), ref Passed, "XOR -> Contains");
			Assert(27, XOR.Contains(0), ref Passed, "XOR -> Contains");
			Assert(28, XOR.Contains(16), ref Passed, "XOR -> Contains");
			Assert(29, !XOR.Contains(2), ref Passed, "XOR -> Contains");
			Assert(30, XOR.Num == 3, ref Passed, "XOR -> Num");
#endif

			// M   = (15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2)
			// M2  = (-2, 0, 2, 4, 6, 8, 10, 12, 14, 16)
			// XOR = (-2, 0, 16)

			// No longer used for testing from here.
			And.Flush();
			XOR.Flush();

			MArray<float> L, R;
			L = new MArray<float>();
			R = new MArray<float>();
			L.Push(1.5f, 2.75f);
			R.Push(4f, 5.25f);

			MArray<float> LR = L + R;
#if MICROSOFT_TESTING
			Assert.IsTrue(LR.Num == 4);
			Assert.IsTrue(LR[0] == L[0]);
			Assert.IsTrue(LR[1] == L[1]);
			Assert.IsTrue(LR[2] == R[0]);
			Assert.IsTrue(LR[3] == R[1]);
#else
			Assert(31, LR.Num == 4, ref Passed, "+ -> Num");
			Assert(32, LR[0] == L[0], ref Passed, "+ -> []");
			Assert(33, LR[1] == L[1], ref Passed, "+ -> []");
			Assert(34, LR[2] == R[0], ref Passed, "+ -> []");
			Assert(35, LR[3] == R[1], ref Passed, "+ -> []");
#endif

			// LR = (1.5, 2.75, 4, 5.25)

			LR.Push(4f, 2.75f, 6.5f);

			// LR = (1.5, 2.75, 4, 5.25, 4, 2.75, 6.5)
#if MICROSOFT_TESTING
			Assert.IsTrue(LR.Contains(4f));
#else
			Assert(36, LR.Contains(4f), ref Passed, "+ -> Contains");
#endif
			LR.Pull(4f);
#if MICROSOFT_TESTING
			Assert.IsTrue(LR.Contains(4f));
#else
			Assert(37, LR.Contains(4f), ref Passed, "+ -> Pull -> Contains");
#endif

			// LR = (1.5, 2.75, 4, 5.25, 2.75, 6.5)

			LR.Pull(4f);
#if MICROSOFT_TESTING
			Assert.IsFalse(LR.Contains(4f));
#else
			Assert(38, !LR.Contains(4f), ref Passed, "+ -> Pull -> Contains");
#endif

			// LR = (1.5, 2.75, 5.25, 2.75, 6.5)

			M.Sort();
			for (byte i = 0; i < M.Num - 1; ++i)
#if MICROSOFT_TESTING
				Assert.IsTrue(M[i] < M[i + 1]);
#else
				Assert(39, M[i] < M[i + 1], ref Passed, "Sort");
#endif

			MArray<byte> Forward = new();
			MArray<byte> Backward = new();
			Forward.Push(1, 2, 3, 4, 5, 6, 7);
			Backward.Push(7, 6, 5, 4, 3, 2, 1);

			for (byte i = 0; i < Forward.Num; ++i)
#if MICROSOFT_TESTING
				Assert.IsTrue(Forward.Mirror(i) == Backward[i]);
#else
				Assert(40, Forward.Mirror(i) == Backward[i], ref Passed, "Mirror");
#endif

			Backward.Reverse();
			for (byte i = 0; i < Forward.Num; ++i)
#if MICROSOFT_TESTING
				Assert.IsTrue(Forward[i] == Backward[i]);
#else
				Assert(41, Forward[i] == Backward[i], ref Passed, "Reverse");
			// Total number of tests (41) + M's size after Sort() - 1 (12) + Forward.Num * 2 (14) - one for each loop (3).
			TotalTests = 41 + 12 + 14 - 3;
#endif
		}

#if MICROSOFT_TESTING
		[TestMethod]
#endif
		public void MArrayTests2(
#if MICROSOFT_TESTING
#else
			out int Passed, out int TotalTests
#endif
			)
		{
#if !MICROSOFT_TESTING
			Passed = 0;
#endif
			MArray<TTestClass> M = new(17);
#if MICROSOFT_TESTING
			Assert.IsTrue(M.IsEmpty());
			Assert.IsTrue(M.Num == 0);
#else
			Assert(1, M.IsEmpty(), ref Passed, "Initial Size");
			Assert(2, M.Num == 0, ref Passed, "Initial Size");
#endif

			TTestClass T1 = new TTestClass(-1);
			TTestClass T2 = new TTestClass(-2);
			TTestClass T3 = new TTestClass(-3);
			TTestClass T4 = new TTestClass(-4);
			TTestClass T5 = new TTestClass(-5);
			TTestClass T6 = new TTestClass(-6);
			TTestClass T7 = new TTestClass(-7);
			TTestClass T8 = new TTestClass(-8);

			M.Push(T1, T2, T3, T4, T5, T6, T7, T8, T1);
			M.Push(T1, T2, T3, T4, T5, T6, T7, T8, T1);

			MArray.AccessedData AD = M.Access(T1);
#if MICROSOFT_TESTING
			Assert.IsTrue(AD.Occurrences == 4 && AD.Positions.Length == AD.Occurrences);
#else
			Assert(3, AD.Occurrences == 4 && AD.Positions.Length == AD.Occurrences, ref Passed, "Occurrences && Positions.Length");
#endif

#if MICROSOFT_TESTING
			Assert.IsTrue(M.Num == 9 * 2);
#else
			Assert(4, M.Num == 9 * 2, ref Passed, "Push Num");
#endif
			M.Pull(T1);
#if MICROSOFT_TESTING
			Assert.IsTrue(M.Num == 9 * 2 - 1);
#else
			Assert(5, M.Num == 9 * 2 - 1, ref Passed, "Push Num");
#endif

			AD = M.Access(T1);
#if MICROSOFT_TESTING
			Assert.IsTrue(AD.Occurrences == 3 && AD.Positions.Length == AD.Occurrences);
			Assert.IsTrue(AD.Positions[0] == 9 && AD.Positions[1] == 8 && AD.Positions[2] == 0);
			Assert.IsTrue(M.Contains(T1));
#else
			Assert(6, AD.Occurrences == 3 && AD.Positions.Length == AD.Occurrences, ref Passed, "Pull -> Occurrences && Positions.Length");

			Assert(7, AD.Positions[0] == 9 && AD.Positions[1] == 8 && AD.Positions[2] == 0, ref Passed, "Positions");

			Assert(8, M.Contains(T1), ref Passed, "Still Contains");
#endif
			M.Pull(T1);
#if MICROSOFT_TESTING
			Assert.IsTrue(M.Contains(T1));
#else
			Assert(9, M.Contains(T1), ref Passed, "Still Contains");
#endif
			M.Pull(T1);
#if MICROSOFT_TESTING
			Assert.IsTrue(M.Contains(T1));

			Assert.IsTrue(M.Access(T1).Occurrences == 1);
#else
			Assert(10, M.Contains(T1), ref Passed, "Still Contains");

			Assert(11, M.Access(T1).Occurrences == 1, ref Passed, "Only One Left");
#endif
			M.Pull(T1);
#if MICROSOFT_TESTING
			Assert.IsTrue(M.Access(T1).Occurrences == -1);
			Assert.IsFalse(M.Contains(T1));
#else
			Assert(12, M.Access(T1).Occurrences == -1, ref Passed, "Only One Left");
			Assert(13, !M.Contains(T1), ref Passed, "Does not Contain");
#endif
			M.Push(T1);
#if MICROSOFT_TESTING
			Assert.IsTrue(M.Top().Value == T1.Value);

			Assert.IsTrue(M.Num == 9 * 2 - 3);
#else
			Assert(14, M.Top().Value == T1.Value, ref Passed, "Push");

			Assert(15, M.Num == 9 * 2 - 3, ref Passed, "Num");
#endif

			M += M;
#if MICROSOFT_TESTING
			Assert.IsTrue(M.Num == 2 * (9 * 2 - 3));
#else
			Assert(16, M.Num == 2 * (9 * 2 - 3), ref Passed, "+=");
#endif

			AD = M.Access(T1);
#if MICROSOFT_TESTING
			Assert.IsTrue(AD.Occurrences == 2 && AD.Positions.Length == AD.Occurrences);

			Assert.IsTrue(AD.Positions[0] == M.Num - 1);
			Assert.IsTrue(AD.Positions[1] == (M.Num - 1) / 2);
#else
			Assert(17, AD.Occurrences == 2 && AD.Positions.Length == AD.Occurrences, ref Passed, "+= -> Occurrences && Positions");

			Assert(18, AD.Positions[0] == M.Num - 1, ref Passed, "+= -> Positions");
			Assert(19, AD.Positions[1] == (M.Num - 1) / 2, ref Passed, "+= -> Positions");
#endif

			M.Flush();
#if MICROSOFT_TESTING
			Assert.IsTrue(M.Num == 0);
#else
			Assert(20, M.Num == 0, ref Passed, "Flush");
#endif

			MArray<int> M2 = new();
			M2.Push(8, 8, 8, 8, 8, 8);
#if MICROSOFT_TESTING
			Assert.IsTrue(M2.Access(8).Occurrences == M2.Num);
			Assert.IsTrue(M2.Pull(8) == 5);
			Assert.IsTrue(M2.Contains(8));
#else
			Assert(21, M2.Access(8).Occurrences == M2.Num, ref Passed, "Duplicate Occurrences");
			Assert(22, M2.Pull(8) == 5, ref Passed, "Pull -> Num");
			Assert(23, M2.Contains(8), ref Passed, "Duplicate -> Contains");
#endif

			int[] Positions = M2.Access(8).Positions;
			bool bIsContinuous = true;
			for (int i = 0; i < Positions.Length - 1 && bIsContinuous; ++i)
				bIsContinuous = Positions[i] == Positions[i + 1] + 1;
#if MICROSOFT_TESTING
			Assert.IsTrue(bIsContinuous);
#else
			Assert(24, bIsContinuous, ref Passed, "Access Positions");
#endif

			M2.Pull(9); // No effect.
#if MICROSOFT_TESTING
			Assert.IsTrue(M2.Num == 5);
#else
			Assert(25, M2.Num == 5, ref Passed, "Pull Non-Existent");
#endif

			M2.PullAll(8);
#if MICROSOFT_TESTING
			Assert.IsTrue(M2.Num == 0);
#else
			Assert(26, M2.Num == 0, ref Passed, "Force Remove");
#endif

			MArray<float> M3 = new();
			MArray.AccessedData Access;

			M3.Push(2.4f, 2.4f, 4.667f, 1.6f, 1.6f, -.5f);
			Access = M3.Access(2.4f);
#if MICROSOFT_TESTING
			Assert.IsTrue(Access.Occurrences == 2);
			Assert.IsTrue(Access.Positions[0] == 1 && Access.Positions[1] == 0);

			Assert.IsTrue(M3.FirstPop() == 2.4f);
#else
			Assert(27, Access.Occurrences == 2, ref Passed, "Access Occurrences");
			Assert(28, Access.Positions[0] == 1 && Access.Positions[1] == 0, ref Passed, "Access Positions");

			Assert(29, M3.FirstPop() == 2.4f, ref Passed, "FirstPop");
#endif
			Access = M3.Access(2.4f);
#if MICROSOFT_TESTING
			Assert.IsTrue(M3.Contains(2.4f));
			Assert.IsTrue(M3.Num == 5);
			Assert.IsTrue(Access.Occurrences == 1);
			Assert.IsTrue(Access.Positions[0] == 0);

			Assert.IsTrue(M3.FirstPop() == 2.4f);
#else
			Assert(30, M3.Contains(2.4f), ref Passed, "FirstPop -> Contains");
			Assert(31, M3.Num == 5, ref Passed, "FirstPop -> Num");
			Assert(32, Access.Occurrences == 1, ref Passed, "FirstPop -> Access Occurrences");
			Assert(33, Access.Positions[0] == 0, ref Passed, "FirstPop -> Access Positions");

			Assert(34, M3.FirstPop() == 2.4f, ref Passed, "FirstPop");
#endif
			Access = M3.Access(2.4f);
#if MICROSOFT_TESTING
			Assert.IsFalse(M3.Contains(2.4f));
			Assert.IsTrue(M3.Num == 4);
			Assert.IsTrue(Access.Occurrences == -1);
			Assert.IsTrue(Access.Positions.Length == 0);
			Assert.IsTrue(M3.First() == 4.667f);
#else
			Assert(35, !M3.Contains(2.4f), ref Passed, "FirstPop -> !Contains");
			Assert(36, M3.Num == 4, ref Passed, "FirstPop -> Num");
			Assert(37, Access.Occurrences == -1, ref Passed, "FirstPop -> Access Occurrences");
			Assert(38, Access.Positions.Length == 0, ref Passed, "FirstPop -> Access Positions");
			Assert(39, M3.First() == 4.667f, ref Passed, "FirstPop -> First");
#endif

			Access = M3.Access(1.6f);
#if MICROSOFT_TESTING
			Assert.IsTrue(Access.Occurrences == 2);
			Assert.IsTrue(Access.Positions[0] == 2 && Access.Positions[1] == 1);
			Assert.IsTrue(M3.TopPop() == -.5f);
			Assert.IsTrue(M3.TopPop() == 1.6f);
			Assert.IsTrue(M3.Contains(1.6f));
#else
			Assert(40, Access.Occurrences == 2, ref Passed, "Access Occurrences");
			Assert(41, Access.Positions[0] == 2 && Access.Positions[1] == 1, ref Passed, "Access Positions");
			Assert(42, M3.TopPop() == -.5f, ref Passed, "TopPop");
			Assert(43, M3.TopPop() == 1.6f, ref Passed, "TopPop");
			Assert(44, M3.Contains(1.6f), ref Passed, "TopPop -> Contains");
#endif
			Access = M3.Access(1.6f);
#if MICROSOFT_TESTING
			Assert.IsTrue(Access.Occurrences == 1);
			Assert.IsTrue(Access.Positions[0] == 1);
			Assert.IsTrue(M3.Num == 2);
			Assert.IsTrue(M3.Contains(1.6f));
			Assert.IsTrue(M3.TopPop() == 1.6f);
#else
			Assert(45, Access.Occurrences == 1, ref Passed, "TopPop -> Access Occurrences");
			Assert(46, Access.Positions[0] == 1, ref Passed, "TopPop -> Access Positions");
			Assert(47, M3.Num == 2, ref Passed, "TopPop -> Num");
			Assert(48, M3.Contains(1.6f), ref Passed, "TopPop -> Contains");
			Assert(49, M3.TopPop() == 1.6f, ref Passed, "TopPop");
#endif
			Access = M3.Access(1.6f);
#if MICROSOFT_TESTING
			Assert.IsTrue(Access.Occurrences == -1 && Access.Positions.Length == 0);
			Assert.IsTrue(!M3.Contains(1.6f));
			Assert.IsTrue(M3.Num == 1);
			Assert.IsTrue(M3[0] == 4.667f);
			Assert.IsTrue(M3.First() == M3.Top());
#else
			Assert(50, Access.Occurrences == -1 && Access.Positions.Length == 0, ref Passed, "TopPop -> Access Occurrences & Positions");
			Assert(51, !M3.Contains(1.6f), ref Passed, "TopPop -> !Contains");
			Assert(52, M3.Num == 1, ref Passed, "TopPop -> Num");
			Assert(53, M3[0] == 4.667f, ref Passed, "TopPop -> []");
			Assert(54, M3.First() == M3.Top(), ref Passed, "First == Top");
#endif

			M3.Flush();


			M3.Push(1.4f, 1.4f, 1.4f, 6f, 1.4f);

			Access = M3.Access(1.4f);
			int OccurrencesOf1p4 = Access.Occurrences;

			for (int i = 0; i < OccurrencesOf1p4; ++i)
			{
				Access = M3.Access(6f);
				int PositionOf6 = Access.Positions[0];
#if MICROSOFT_TESTING
				Assert.IsTrue(PositionOf6 == 3 - i);
#else
				Assert(55, PositionOf6 == 3 - i, ref Passed, "FirstPop Loop");
#endif
				M3.FirstPop();
			}
#if MICROSOFT_TESTING
			Assert.IsTrue(M3.Top() == M3.First());
#else
			Assert(56, M3.Top() == M3.First(), ref Passed, "Top == First");
#endif

			M3.Flush();

			M3.Push(1.4f, 1.4f, 1.4f, 6f, 1.4f, 1.5f, 1.5f, 1.5f);
			Access = M3.Access(1.5f);
			int OccurrencesOf1p5 = Access.Occurrences;

			for (int i = 0; i < OccurrencesOf1p5; ++i)
			{
				Access = M3.Access(1.5f);
#if MICROSOFT_TESTING
				Assert.IsTrue(Access.Occurrences == OccurrencesOf1p5 - i);
#else
				Assert(57, Access.Occurrences == OccurrencesOf1p5 - i, ref Passed, "TopPop Loop");
#endif

				M3.TopPop();
			}
#if MICROSOFT_TESTING
			Assert.IsFalse(M3.Contains(1.5f));
#else
			Assert(58, !M3.Contains(1.5f), ref Passed, "TopPop -> !Contains");
#endif
			M3.FirstPop();
			Access = M3.Access(6f);
#if MICROSOFT_TESTING
			Assert.IsTrue(Access.Positions[0] == 2);
#else
			Assert(59, Access.Positions[0] == 2, ref Passed, "TopPop -> FirstPop -> Access Positions");
#endif

			M3.Flush();

			M3.Push(1.4f, 1.4f, 6f, 6f, 1.4f, 1.4f);
			M3.Pull(6f);
			Access = M3.Access(1.4f);
#if MICROSOFT_TESTING
			Assert.IsTrue(Access.Positions[0] == 4);
#else
			Assert(60, Access.Positions[0] == 4, ref Passed, "Multi-Mid Pull -> Access Positions");
			TotalTests = 65;
#endif
		}

#if MICROSOFT_TESTING
		[TestMethod]
#endif
		public void THeapTests(
#if MICROSOFT_TESTING
#else
			out int Passed, out int TotalTests
#endif
			)
		{
#if !MICROSOFT_TESTING
			Passed = 0;
#endif

			THeap<TTestClass> Heap = new(11);
			Heap.Add(new TTestClass(46));
			Heap.Add(new TTestClass(42));
			Heap.Add(new TTestClass(52));
			Heap.Add(new TTestClass(44));
			Heap.Add(new TTestClass(45));
			Heap.Add(new TTestClass(50));
			Heap.Add(new TTestClass(51));
			Heap.Add(new TTestClass(48));
			Heap.Add(new TTestClass(43));
			Heap.Add(new TTestClass(47));
			Heap.Add(new TTestClass(49));

			for (byte i = 0; i < Heap.Num; ++i)
#if MICROSOFT_TESTING
				Assert.IsTrue(Heap.RemoveFirst().Value < Heap.RemoveFirst().Value);
#else
				Assert(1, Heap.RemoveFirst().Value < Heap.RemoveFirst().Value, ref Passed, "Remove First is Minimum");
			TotalTests = 4;
#endif

		}
	}

#if MICROSOFT_TESTING
	[TestClass]
#endif
	public class FastTests
	{
#if MICROSOFT_TESTING
		[TestMethod]
#endif
		public void ASinTests(
#if MICROSOFT_TESTING
#else
			out int Passed, out int TotalTests
#endif
			)
		{
#if !MICROSOFT_TESTING
			Passed = 0;
#endif
			for (float F = -50f; F <= 50f; F += .23f)
			{
				FloatToleranceCheck(1, Mathf.Asin(F), FArcSine(F), "Fast ASin"
#if !MICROSOFT_TESTING
					, ref Passed
#endif
					);
			}
#if !MICROSOFT_TESTING
			// 100 / .23;
			TotalTests = (int)(434.78260869565217391304347826087f) + 1;
#endif
		}
	}

	class TTestClass : IHeapItem<TTestClass>
	{
		public int Value;

		int Index;

		public TTestClass(int Value)
		{
			this.Value = Value;
		}

		public int HeapItemIndex { get => Index; set => Index = value; }

		public int CompareTo(TTestClass? Other)
		{
			if (Other != null)
			{
				if (Value < Other.Value)
					return 1;
				if (Value > Other.Value)
					return -1;
			}

			return 0;
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public static implicit operator int(TTestClass TTC) => TTC.Value;
	}
}
