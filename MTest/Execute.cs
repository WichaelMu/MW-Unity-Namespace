using UnityEngine;
using MW;
using MW.Math;
using MW.VectorExtensions;
using MTest.Output;

namespace MTest
{
	internal class Execute : Assertion
	{
		internal class CoreTests
		{
			public static void RoundToDPTest(out int Passed, out int TotalTests)
			{
				Passed = 0;
				int TestsPassed = 0;
				float Number, Result;
				int DP;

				int TestNumber = 1;
				Number = 0.00f;
				DP = 2;
				Result = Utils.RoundToDP(Number, DP);
				DiagnosticCheckFailed(0.000f, nameof(Utils.RoundToDP));

				TestNumber = 2;
				Number = -0.00001f;
				DP = 2;
				Result = Utils.RoundToDP(Number, DP);
				DiagnosticCheckFailed(0.000f, nameof(Utils.RoundToDP));

				TestNumber = 3;
				Number = 0.00001f;
				DP = 2;
				Result = Utils.RoundToDP(Number, DP);
				DiagnosticCheckFailed(0.000f, nameof(Utils.RoundToDP));

				TestNumber = 4;
				Number = 127.47329f;
				DP = 2;
				Result = Utils.RoundToDP(Number, DP);
				DiagnosticCheckFailed(127.47f, nameof(Utils.RoundToDP));

				TestNumber = 5;
				Number = -94275.4298347f;
				DP = 2;
				Result = Utils.RoundToDP(Number, DP);
				DiagnosticCheckFailed(-94275.43f, nameof(Utils.RoundToDP));

				TestNumber = 6;
				Number = 5.5f;
				DP = 0;
				Result = Utils.RoundToDP(Number, DP);
				DiagnosticCheckFailed(6f, nameof(Utils.RoundToDP));

				TestNumber = 7;
				Number = 3.14159f;
				DP = 5;
				Result = Utils.RoundToDP(Number, DP);
				DiagnosticCheckFailed(3.14159f, nameof(Utils.RoundToDP));

				TestNumber = 8;
				Number = 3.14159f;
				DP = 9;
				Result = Utils.RoundToDP(Number, DP);
				DiagnosticCheckFailed(3.14159f, nameof(Utils.RoundToDP));

				TestNumber = 9;
				Number = 3.14159f;
				DP = 1;
				Result = Utils.RoundToDP(Number, DP);
				DiagnosticCheckFailed(3.1f, nameof(Utils.RoundToDP));

				TestNumber = 10;
				Number = 273489f;
				DP = 10;
				Result = Utils.RoundToDP(Number, DP);
				DiagnosticCheckFailed(273489f, nameof(Utils.RoundToDP));

				TestNumber = 11;
				Number = .5f;
				DP = 1;
				Result = Utils.RoundToDP(Number, DP);
				DiagnosticCheckFailed(.5f, nameof(Utils.RoundToDP));

				TestNumber = 12;
				Number = .49999999999999f;
				DP = 1;
				Result = Utils.RoundToDP(Number, DP);
				DiagnosticCheckFailed(.5f, nameof(Utils.RoundToDP));

				TestNumber = 13;
				Number = .50000000000001f;
				DP = 1;
				Result = Utils.RoundToDP(Number, DP);
				DiagnosticCheckFailed(.5f, nameof(Utils.RoundToDP));

				TestNumber = 14;
				Number = .49999999999999f;
				DP = 5;
				Result = Utils.RoundToDP(Number, DP);
				DiagnosticCheckFailed(.5f, nameof(Utils.RoundToDP));

				TestNumber = 15;
				Number = .50000000000001f;
				DP = 5;
				Result = Utils.RoundToDP(Number, DP);
				DiagnosticCheckFailed(.5f, nameof(Utils.RoundToDP));

				TestNumber = 16;
				Number = 4333f;
				DP = -2;
				Result = Utils.RoundToDP(Number, DP);
				DiagnosticCheckFailed(4300f, nameof(Utils.RoundToDP));

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
			}

			public static void SwapTests(out int Passed, out int TotalTests)
			{
				Passed = 0;

				// Base Tests.
				int L = 1, C = 2, R = 3;

				Utils.Swap(ref L, ref R);
				Utils.Swap(ref R, ref C);

				Assert(1, L == 3, ref Passed, "Swap Int");
				Assert(2, R == 2, ref Passed, "Swap Int");
				Assert(3, C == 1, ref Passed, "Swap Int");

				// Reference Tests.
				TTestClass T1, T2, T3;
				T1 = new TTestClass(-1);
				T2 = new TTestClass(0);
				T3 = new TTestClass(1);

				Utils.Swap(ref T1, ref T3);
				Utils.Swap(ref T3, ref T2);

				Assert(4, T1.Value == 1, ref Passed, "Swap Reference Type");
				Assert(5, T3.Value == 0, ref Passed, "Swap Reference Type");
				Assert(6, T2.Value == -1, ref Passed, "Swap Reference Type");

				// Structure Tests.
				MVector NOne = new MVector(-1);
				MVector Zero = MVector.Zero;
				MVector One = MVector.One;

				Utils.Swap(ref NOne, ref One);
				Utils.Swap(ref One, ref Zero);

				Assert(7, NOne == MVector.One, ref Passed, "Swap Structure");
				Assert(8, One == MVector.Zero, ref Passed, "Swap Structure");
				Assert(9, Zero == new MVector(-1), ref Passed, "Swap Structure");

				TotalTests = 9;
			}

			public static void SqrtTests(out int Passed, out int TotalTests)
			{
				Passed = 0;
				TotalTests = 50;
				for (float f = 0; f < TotalTests; f += .23f)
				{
					Tolerance.FloatToleranceCheck(1, Mathf.Sqrt(f), MW.Math.Magic.Fast.Sqrt(f), "Fast Sqrt", ref Passed);
				}

				TotalTests = (int)(TotalTests / .23f) + 1;
			}
		}

		internal class MWTests
		{
			public static void MVectorTest(out int Passed, out int TotalTests)
			{
				MVector M = new MVector(1, 2, 3);
				Vector3 U = new Vector3(1, 2, 3);

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

				MVector ML = new MVector(234, 5.94f, -148);
				MVector MR = new MVector(-5738, 2847, 19.24f);

				Vector3 UL = new Vector3(234, 5.94f, -148);
				Vector3 UR = new Vector3(-5738, 2847, 19.24f);

				Passed = 0;
				Tolerance.VectorToleranceCheck(1, Vector3.Cross(UL, UR), ML ^ MR, "Cross", ref Passed);
				Tolerance.FloatToleranceCheck(2, Vector3.Dot(UL, UR), ML | MR, "Dot", ref Passed);
				Tolerance.VectorToleranceCheck(3, UL.normalized, ML.Normalised, "Normalise", ref Passed);
				Tolerance.VectorToleranceCheck(4, UR.normalized, MR.Normalised, "Normalise", ref Passed);
				Tolerance.FloatToleranceCheck(5, UL.sqrMagnitude, ML.SqrMagnitude, "Square Magnitude", ref Passed);
				Tolerance.FloatToleranceCheck(6, UR.sqrMagnitude, MR.SqrMagnitude, "Square Magnitude", ref Passed);
				Tolerance.FloatToleranceCheck(7, Vector3.Distance(UL, UR), MVector.Distance(ML, MR), "Distance", ref Passed);
				Tolerance.FloatToleranceCheck(8, Vector3.Distance(UL, UR), Mathf.Sqrt(ML.SqrDistance(MR)), "MVector SqrDistance -> Vector3 Distance", ref Passed);
				Tolerance.VectorToleranceCheck(9, UL - UR, ML - MR, "Subtraction", ref Passed);
				Tolerance.VectorToleranceCheck(10, UR - UL, MR - ML, "Subtraction", ref Passed);
				Tolerance.VectorToleranceCheck(11, -UL, -ML, "Negation", ref Passed);
				Tolerance.VectorToleranceCheck(12, -UR , -MR, "Negation", ref Passed);
				Tolerance.VectorToleranceCheck(13, UL + UR, ML + MR, "Addition", ref Passed);
				Tolerance.VectorToleranceCheck(14, UR + ML, ML + UR, "Cross Addition", ref Passed);

				for (float F = -10f; F <= 10f; F += .7f)
				{
					Tolerance.VectorToleranceCheck(15, F * UL, F * ML, "Multiplication by: " + F, ref Passed);
					Tolerance.VectorToleranceCheck(16, UL / F, ML / F, "Division by: " + F, ref Passed);
				}

				Tolerance.VectorToleranceCheck(17, (UR - UL).normalized, ML > MR, "Direction", ref Passed);
				Tolerance.VectorToleranceCheck(18, (UL - UR).normalized, ML < MR, "Direction", ref Passed);

				Assert(19, Mathematics.IsNormalised(ML.Normalised), ref Passed, nameof(Mathematics.IsNormalised));

				// M = (1, 2, 3).
				AssertEquals(20, M >> 1, new MVector(3, 1, 2), ref Passed, "Right Shift 1");
				AssertEquals(21, M << 1, new MVector(2, 3, 1), ref Passed, "Left Shift 1");
				AssertEquals(22, M, new MVector(1, 2, 3), ref Passed, "Not Modified After Shifting");

				AssertEquals(23, M >> 2, new MVector(2, 3, 1), ref Passed, "Right Shift 2");
				AssertEquals(24, M << 2, new MVector(3, 1, 2), ref Passed, "Left Shift 2");

				AssertEquals(25, M >> 3, M, ref Passed, "Right Shift 3");
				AssertEquals(26, M << 3, M, ref Passed, "Left Shift 3");

				AssertEquals(27, M >> 4, M >> 1, ref Passed, "Right Shift 4 = 1");
				AssertEquals(28, M << 4, M << 1, ref Passed, "Left Shift 4 = 1");

				AssertEquals(29, Fast.SqrDistance(UL, Vector3.zero), ML.SqrMagnitude, ref Passed, "V3 Extension SqrDist");
				AssertEquals(30, UL.Normalise(), UL.normalized, ref Passed, "V3 Extension Normalise");
				AssertEquals(31, Fast.Distance(UL, UR), Vector3.Distance(UL, UR), ref Passed, "V3 Extension Dist");

				// Total number of tests (31) + the difference between -10 and 10 divided by .7 (28) * 2 for both Multiplication and Division.
				TotalTests = 31 + 28 * 2;
			}

			public static void MArrayTests(out int Passed, out int TotalTests)
			{
				Passed = 0;
				MArray<int> M = new MArray<int>(17);
				M.Push(17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1);

				Assert(1, M.Contains(1), ref Passed, "Contains");
				Assert(2, M.Contains(2), ref Passed, "Contains");
				Assert(3, M.Contains(3), ref Passed, "Contains");
				Assert(4, M.Contains(4), ref Passed, "Contains");

				Assert(5, !M.Contains(18), ref Passed, "Contains");
				Assert(6, !M.Contains(-1), ref Passed, "Contains");
				Assert(7, !M.Contains(54), ref Passed, "Contains");
				Assert(8, !M.Contains(00), ref Passed, "Contains");

				Assert(9, M.Num == 17, ref Passed, "Num");
				M.Push(18);
				Assert(10, M.Num == 18, ref Passed, "Num");

				// M = (17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 18)

				Assert(11, M.Contains(18), ref Passed, "Push");

				Assert(12, M.FirstPop() == 17, ref Passed, "First Pop");
				Assert(13, M.FirstPop() == 16, ref Passed, "First Pop");

				// M = (15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 18)

				Assert(14, M.TopPop() == 18, ref Passed, "Top Pop");
				Assert(15, M.TopPop() == 1, ref Passed, "Top Pop");

				// M = (15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2)

				Assert(16, M[0] == 15, ref Passed, "Square Bracket Accessor");

				Assert(17, M.Mirror(0) == 2, ref Passed, "Mirror");
				Assert(18, M.Mirror(M.Num - 1) == 15, ref Passed, "Mirror");

				Assert(19, M.Contains(9), ref Passed, "Contains");
				Assert(20, M[6] == 9, ref Passed, "Square Bracket Accessor");

				M.Pull(9);
				Assert(21, !M.Contains(9), ref Passed, "Pull -> Contains");

				// M = (15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2)

				MArray<int> M2 = new MArray<int>(10);
				M2.Push(-2, 0, 2, 4, 6, 8, 10, 12, 14, 16);

				MArray<int> And = M2 & M;
				Assert(22, And.Contains(2), ref Passed, "And -> Contains");
				Assert(23, And.Contains(10), ref Passed, "And -> Contains");
				Assert(24, !And.Contains(16), ref Passed, "And -> Contains");
				Assert(25, !And.Contains(18), ref Passed, "And -> Contains");

				// M   = (15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2)
				// M2  = (-2, 0, 2, 4, 6, 8, 10, 12, 14, 16)
				// And = (14, 12, 10, 8, 6, 4, 2)

				MArray<int> XOR = M2 ^ M;
				Assert(26, XOR.Contains(-2), ref Passed, "XOR -> Contains");
				Assert(27, XOR.Contains(0), ref Passed, "XOR -> Contains");
				Assert(28, XOR.Contains(16), ref Passed, "XOR -> Contains");
				Assert(29, !XOR.Contains(2), ref Passed, "XOR -> Contains");
				Assert(30, XOR.Num == 3, ref Passed, "XOR -> Num");

				// M   = (15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2)
				// M2  = (-2, 0, 2, 4, 6, 8, 10, 12, 14, 16)
				// XOR = (-2, 0, 16)

				MArray<float> L, R;
				L = new MArray<float>();
				R = new MArray<float>();
				L.Push(1.5f, 2.75f);
				R.Push(4f, 5.25f);

				MArray<float> LR = L + R;

				Assert(31, LR.Num == 4, ref Passed, "+ -> Num");
				Assert(32, LR[0] == L[0], ref Passed, "+ -> []");
				Assert(33, LR[1] == L[1], ref Passed, "+ -> []");
				Assert(34, LR[2] == R[0], ref Passed, "+ -> []");
				Assert(35, LR[3] == R[1], ref Passed, "+ -> []");

				// LR = (1.5, 2.75, 4, 5.25)

				LR.Push(4f, 2.75f, 6.5f);

				// LR = (1.5, 2.75, 4, 5.25, 4, 2.75, 6.5)

				Assert(36, LR.Contains(4f), ref Passed, "+ -> Contains");
				LR.Pull(4f);
				Assert(37, LR.Contains(4f), ref Passed, "+ -> Pull -> Contains");

				// LR = (1.5, 2.75, 4, 5.25, 2.75, 6.5)

				LR.Pull(4f);
				Assert(38, !LR.Contains(4f), ref Passed, "+ -> Pull -> Contains");

				// LR = (1.5, 2.75, 5.25, 4, 2.75, 6.5)

				M.Sort();
				for (byte i = 0; i < M.Num - 1; ++i)
					Assert(39, M[i] < M[i + 1], ref Passed, "Sort");

				MArray<byte> Forward = new();
				MArray<byte> Backward = new();
				Forward.Push(1, 2, 3, 4, 5, 6, 7);
				Backward.Push(7, 6, 5, 4, 3, 2, 1);

				for (byte i = 0; i < Forward.Num; ++i)
					Assert(40, Forward.Mirror(i) == Backward[i], ref Passed, "Mirror");

				Backward.Reverse();
				for (byte i = 0; i < Forward.Num; ++i)
					Assert(41, Forward[i] == Backward[i], ref Passed, "Reverse");

				// Total number of tests (41) + M's size after Sort() - 1 (12) + Forward.Num * 2 (14) - one for each loop (3).
				TotalTests = 41 + 12 + 14 - 3;
			}

			public static void THeapTests(out int Passed, out int TotalTests)
			{
				Passed = 0;

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
					Assert(1, Heap.RemoveFirst().Value < Heap.RemoveFirst().Value, ref Passed, "Remove First is Minimum");

				TotalTests = 4;
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
		}
	}
}
