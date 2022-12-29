using Microsoft.VisualStudio.TestTools.UnitTesting;
using MW;
using MW.Extensions;
using MW.Math;
using UnityEngine;
using static MTest.Tolerance;
using static MW.Math.Magic.Fast;

namespace MTest
{
	[TestClass]
	public class CoreTests
	{
		[TestMethod]
		public void RoundToDPTest()
		{
			float Number, Result;
			int DP;

			Number = 0.00f;
			DP = 2;
			Result = Utils.RoundToDP(Number, DP);
			Assert.AreEqual(0.000f, Result);

			Number = -0.00001f;
			DP = 2;
			Result = Utils.RoundToDP(Number, DP);
			Assert.AreEqual(0.000f, Result);

			Number = 0.00001f;
			DP = 2;
			Result = Utils.RoundToDP(Number, DP);
			Assert.AreEqual(0.000f, Result);

			Number = 127.47329f;
			DP = 2;
			Result = Utils.RoundToDP(Number, DP);
			Assert.AreEqual(127.47f, Result);

			Number = -94275.4298347f;
			DP = 2;
			Result = Utils.RoundToDP(Number, DP);
			Assert.AreEqual(-94275.43f, Result);

			Number = 5.5f;
			DP = 0;
			Result = Utils.RoundToDP(Number, DP);
			Assert.AreEqual(6f, Result);

			Number = 3.14159f;
			DP = 5;
			Result = Utils.RoundToDP(Number, DP);
			Assert.AreEqual(3.14159f, Result);

			Number = 3.14159f;
			DP = 9;
			Result = Utils.RoundToDP(Number, DP);
			Assert.AreEqual(3.14159f, Result);

			Number = 3.14159f;
			DP = 1;
			Result = Utils.RoundToDP(Number, DP);
			Assert.AreEqual(3.1f, Result);

			Number = 273489f;
			DP = 10;
			Result = Utils.RoundToDP(Number, DP);
			Assert.AreEqual(273489f, Result);

			Number = .5f;
			DP = 1;
			Result = Utils.RoundToDP(Number, DP);
			Assert.AreEqual(.5f, Result);

			Number = .49999999999999f;
			DP = 1;
			Result = Utils.RoundToDP(Number, DP);
			Assert.AreEqual(.5f, Result);

			Number = .50000000000001f;
			DP = 1;
			Result = Utils.RoundToDP(Number, DP);
			Assert.AreEqual(.5f, Result);

			Number = .49999999999999f;
			DP = 5;
			Result = Utils.RoundToDP(Number, DP);
			Assert.AreEqual(.5f, Result);

			Number = .50000000000001f;
			DP = 5;
			Result = Utils.RoundToDP(Number, DP);
			Assert.AreEqual(.5f, Result);

			Number = 4333f;
			DP = -2;
			Result = Utils.RoundToDP(Number, DP);
			Assert.AreEqual(4300f, Result);
		}

		[TestMethod]
		public void SwapTests()
		{

			// Base Tests.
			int L = 1, C = 2, R = 3;

			Utils.Swap(ref L, ref R);
			Utils.Swap(ref R, ref C);

			Assert.AreEqual(L, 3);
			Assert.AreEqual(R, 2);
			Assert.AreEqual(C, 1);

			// Reference Tests.
			TTestClass T1, T2, T3;
			T1 = new TTestClass(-1);
			T2 = new TTestClass(0);
			T3 = new TTestClass(1);

			Utils.Swap(ref T1, ref T3);
			Utils.Swap(ref T3, ref T2);

			Assert.AreEqual(T1.Value, 1);
			Assert.AreEqual(T3.Value, 0);
			Assert.AreEqual(T2.Value, -1);

			// Structure Tests.
			MVector NOne = new MVector(-1);
			MVector Zero = MVector.Zero;
			MVector One = MVector.One;

			Utils.Swap(ref NOne, ref One);
			Utils.Swap(ref One, ref Zero);

			Assert.AreEqual(NOne, MVector.One);
			Assert.AreEqual(One, MVector.Zero);
			Assert.AreEqual(Zero, new MVector(-1));
		}
	}

	[TestClass]
	public class MWTests
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

			for (
				float X = -370f,    Y =  180f,     Z =  370f;
				      X <  370f  && Y <  370f  &&  Z > -370f;
				      X += .23f,    Y += .23f,     Z -= .23f
				)
			{
				Vector3 FAV1 = new Vector3(X, Y, Z);
				Vector3 FAV2 = new Vector3(Y, Z, X);
				MVector FAM1 = new MVector(X, Y, Z);
				MVector FAM2 = FAM1 << 1;
				FloatToleranceCheck(Vector3.Angle(FAV1, FAV2), FAngle(FAM1, FAM2), "Fast Angle", .5f);
			}

			Clone.Dispose();
		}

		[TestMethod]
		public void MArrayTests()
		{
			MArray<int> M = new MArray<int>(17);
			M.Push(17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1);

			Assert.IsTrue(M.Contains(1));
			Assert.IsTrue(M.Contains(2));
			Assert.IsTrue(M.Contains(3));
			Assert.IsTrue(M.Contains(4));

			Assert.IsFalse(M.Contains(18));
			Assert.IsFalse(M.Contains(-1));
			Assert.IsFalse(M.Contains(54));
			Assert.IsFalse(M.Contains(00));

			Assert.IsTrue(M.Num == 17);

			M.Push(18);
			Assert.IsTrue(M.Num == 18);

			// M = (17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 18)
			Assert.IsTrue(M.Contains(18));

			Assert.IsTrue(M.FirstPop() == 17);
			Assert.IsTrue(M.FirstPop() == 16);

			// M = (15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 18)
			Assert.IsTrue(M.TopPop() == 18);
			Assert.IsTrue(M.TopPop() == 1);

			// M = (15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2)
			Assert.IsTrue(M[0] == 15);

			Assert.IsTrue(M.Mirror(0) == 2);
			Assert.IsTrue(M.Mirror(M.Num - 1) == 15);

			Assert.IsTrue(M.Contains(9));
			Assert.IsTrue(M[6] == 9);

			M.Pull(9);
			Assert.IsFalse(M.Contains(9));

			// M = (15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2)

			MArray<int> M2 = new MArray<int>(10);
			M2.Push(-2, 0, 2, 4, 6, 8, 10, 12, 14, 16);

			MArray<int> And = M2 & M;
			Assert.IsTrue(And.Contains(2));
			Assert.IsTrue(And.Contains(10));
			Assert.IsFalse(And.Contains(16));
			Assert.IsFalse(And.Contains(18));

			// M   = (15, 14, 13, 12, 11, 10, 8, 7, 6, 5, 4, 3, 2)
			// M2  = (-2, 0, 2, 4, 6, 8, 10, 12, 14, 16)
			// And = (14, 12, 10, 8, 6, 4, 2)

			MArray<int> XOR = M2 ^ M;
			Assert.IsTrue(XOR.Contains(-2));
			Assert.IsTrue(XOR.Contains(0));
			Assert.IsTrue(XOR.Contains(16));
			Assert.IsFalse(XOR.Contains(2));
			Assert.IsTrue(XOR.Num == 3);

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
			Assert.IsTrue(LR.Num == 4);
			Assert.IsTrue(LR[0] == L[0]);
			Assert.IsTrue(LR[1] == L[1]);
			Assert.IsTrue(LR[2] == R[0]);
			Assert.IsTrue(LR[3] == R[1]);

			// LR = (1.5, 2.75, 4, 5.25)

			LR.Push(4f, 2.75f, 6.5f);

			// LR = (1.5, 2.75, 4, 5.25, 4, 2.75, 6.5)
			Assert.IsTrue(LR.Contains(4f));

			LR.Pull(4f);

			Assert.IsTrue(LR.Contains(4f));

			// LR = (1.5, 2.75, 4, 5.25, 2.75, 6.5)

			LR.Pull(4f);
			Assert.IsFalse(LR.Contains(4f));

			// LR = (1.5, 2.75, 5.25, 2.75, 6.5)

			M.Sort();
			for (byte i = 0; i < M.Num - 1; ++i)
				Assert.IsTrue(M[i] < M[i + 1]);

			MArray<byte> Forward = new();
			MArray<byte> Backward = new();
			Forward.Push(1, 2, 3, 4, 5, 6, 7);
			Backward.Push(7, 6, 5, 4, 3, 2, 1);

			for (byte i = 0; i < Forward.Num; ++i)
				Assert.IsTrue(Forward.Mirror(i) == Backward[i]);

			Backward.Reverse();
			for (byte i = 0; i < Forward.Num; ++i)
				Assert.IsTrue(Forward[i] == Backward[i]);
		}

		[TestMethod]
		public void MArrayTests2()
		{
			MArray<TTestClass> M = new(17);
			Assert.IsTrue(M.IsEmpty());
			Assert.IsTrue(M.Num == 0);

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
			Assert.IsTrue(AD.Occurrences == 4 && AD.Positions.Length == AD.Occurrences);

			Assert.IsTrue(M.Num == 9 * 2);
			M.Pull(T1);
			Assert.IsTrue(M.Num == 9 * 2 - 1);

			AD = M.Access(T1);
			Assert.IsTrue(AD.Occurrences == 3 && AD.Positions.Length == AD.Occurrences);
			Assert.IsTrue(AD.Positions[0] == 9 && AD.Positions[1] == 8 && AD.Positions[2] == 0);
			Assert.IsTrue(M.Contains(T1));
			M.Pull(T1);
			Assert.IsTrue(M.Contains(T1));

			M.Pull(T1);
			Assert.IsTrue(M.Contains(T1));

			Assert.IsTrue(M.Access(T1).Occurrences == 1);

			M.Pull(T1);
			Assert.IsTrue(M.Access(T1).Occurrences == -1);
			Assert.IsFalse(M.Contains(T1));

			M.Push(T1);

			Assert.IsTrue(M.Top().Value == T1.Value);

			Assert.IsTrue(M.Num == 9 * 2 - 3);

			M += M;
			Assert.IsTrue(M.Num == 2 * (9 * 2 - 3));

			AD = M.Access(T1);

			Assert.IsTrue(AD.Occurrences == 2 && AD.Positions.Length == AD.Occurrences);

			Assert.IsTrue(AD.Positions[0] == M.Num - 1);
			Assert.IsTrue(AD.Positions[1] == (M.Num - 1) / 2);

			M.Flush();

			Assert.IsTrue(M.Num == 0);

			MArray<int> M2 = new();
			M2.Push(8, 8, 8, 8, 8, 8);

			Assert.IsTrue(M2.Access(8).Occurrences == M2.Num);
			Assert.IsTrue(M2.Pull(8) == 5);
			Assert.IsTrue(M2.Contains(8));

			int[] Positions = M2.Access(8).Positions;
			bool bIsContinuous = true;
			for (int i = 0; i < Positions.Length - 1 && bIsContinuous; ++i)
				bIsContinuous = Positions[i] == Positions[i + 1] + 1;
			Assert.IsTrue(bIsContinuous);

			M2.Pull(9); // No effect.
			Assert.IsTrue(M2.Num == 5);

			M2.PullAll(8);
			Assert.IsTrue(M2.Num == 0);

			MArray<float> M3 = new();
			MArray.AccessedData Access;

			M3.Push(2.4f, 2.4f, 4.667f, 1.6f, 1.6f, -.5f);
			Access = M3.Access(2.4f);
			Assert.IsTrue(Access.Occurrences == 2);
			Assert.IsTrue(Access.Positions[0] == 1 && Access.Positions[1] == 0);

			Assert.IsTrue(M3.FirstPop() == 2.4f);
			Access = M3.Access(2.4f);

			Assert.IsTrue(M3.Contains(2.4f));
			Assert.IsTrue(M3.Num == 5);
			Assert.IsTrue(Access.Occurrences == 1);
			Assert.IsTrue(Access.Positions[0] == 0);

			Assert.IsTrue(M3.FirstPop() == 2.4f);

			Access = M3.Access(2.4f);
			Assert.IsFalse(M3.Contains(2.4f));
			Assert.IsTrue(M3.Num == 4);
			Assert.IsTrue(Access.Occurrences == -1);
			Assert.IsTrue(Access.Positions.Length == 0);
			Assert.IsTrue(M3.First() == 4.667f);

			Access = M3.Access(1.6f);

			Assert.IsTrue(Access.Occurrences == 2);
			Assert.IsTrue(Access.Positions[0] == 2 && Access.Positions[1] == 1);
			Assert.IsTrue(M3.TopPop() == -.5f);
			Assert.IsTrue(M3.TopPop() == 1.6f);
			Assert.IsTrue(M3.Contains(1.6f));
			Access = M3.Access(1.6f);

			Assert.IsTrue(Access.Occurrences == 1);
			Assert.IsTrue(Access.Positions[0] == 1);
			Assert.IsTrue(M3.Num == 2);
			Assert.IsTrue(M3.Contains(1.6f));
			Assert.IsTrue(M3.TopPop() == 1.6f);

			Access = M3.Access(1.6f);
			Assert.IsTrue(Access.Occurrences == -1 && Access.Positions.Length == 0);
			Assert.IsTrue(!M3.Contains(1.6f));
			Assert.IsTrue(M3.Num == 1);
			Assert.IsTrue(M3[0] == 4.667f);
			Assert.IsTrue(M3.First() == M3.Top());

			M3.Flush();


			M3.Push(1.4f, 1.4f, 1.4f, 6f, 1.4f);

			Access = M3.Access(1.4f);
			int OccurrencesOf1p4 = Access.Occurrences;

			for (int i = 0; i < OccurrencesOf1p4; ++i)
			{
				Access = M3.Access(6f);
				int PositionOf6 = Access.Positions[0];
				Assert.IsTrue(PositionOf6 == 3 - i);

				M3.FirstPop();
			}
			Assert.IsTrue(M3.Top() == M3.First());

			M3.Flush();

			M3.Push(1.4f, 1.4f, 1.4f, 6f, 1.4f, 1.5f, 1.5f, 1.5f);
			Access = M3.Access(1.5f);
			int OccurrencesOf1p5 = Access.Occurrences;

			for (int i = 0; i < OccurrencesOf1p5; ++i)
			{
				Access = M3.Access(1.5f);
				Assert.IsTrue(Access.Occurrences == OccurrencesOf1p5 - i);

				M3.TopPop();
			}
			Assert.IsFalse(M3.Contains(1.5f));
			M3.FirstPop();
			Access = M3.Access(6f);
			Assert.IsTrue(Access.Positions[0] == 2);

			M3.Flush();

			M3.Push(1.4f, 1.4f, 6f, 6f, 1.4f, 1.4f);
			M3.Pull(6f);
			Access = M3.Access(1.4f);
			Assert.IsTrue(Access.Positions[0] == 4);
		}

		[TestMethod]
		public void THeapTests()
		{
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
				Assert.IsTrue(Heap.RemoveFirst().Value < Heap.RemoveFirst().Value, "Minimum Heap");

			THeap<TTestClass> Heap1 = new THeap<TTestClass>(3);

			TTestClass Item1 = new TTestClass(4);
			TTestClass Item2 = new TTestClass(-1);
			TTestClass Item3 = new TTestClass(8);
			Heap1.Add(Item1);
			Heap1.Add(Item2);
			Heap1.Add(Item3);

			int Num = Heap1.Num; // 3
			Assert.IsTrue(Heap1.RemoveFirst().Value == -1, "Correct Value -1");

			Num = Heap1.Num; // 2
			Assert.IsTrue(Heap1.RemoveFirst().Value == 4, "Correct Value 4");

			Assert.IsFalse(Heap1.Contains(Item1), "Contains 4");
			Assert.IsFalse(Heap1.Contains(Item2), "Contains 0");
			Assert.IsTrue(Heap1.Contains(Item3), "Contains 8");


		}
	}

	[TestClass]
	public class FastTests
	{
		[TestMethod]
		public void SqrtTests()
		{
			// This also checks FInverseSqrt()...

			for (float F = 0; F < 50; F += .23f)
			{
				FloatToleranceCheck(Mathf.Sqrt(F), FSqrt(F), "Fast Sqrt");
			}
		}

		[TestMethod]
		public void InverseTests()
		{
			for (float F = -1500.442f; F <= 1500.422f; F += .23f)
			{
				FloatToleranceCheck(1f / F, FInverse(F, 3), "Fast Inverse");
			}
		}

		[TestMethod]
		public void ASinTests()
		{
			for (float F = -1f; F <= 1f; F += .023f)
			{
				FloatToleranceCheck(Mathf.Asin(F), FArcSine(F), $"Fast ASin {F}");
			}
		}

		[TestMethod]
		public void ACosTests()
		{
			for (float F = -1f; F <= 1f; F += .023f)
			{
				FloatToleranceCheck(Mathf.Acos(F), FArcCosine(F), $"Fast ACosine {F}");
			}
		}
	}

	[TestClass]
	public class ApproximationTests
	{
		[TestMethod]
		public void SinCosTests()
		{
			for (float F = -365f; F <= 365f; F += .23f)
			{
				Mathematics.SinCos(out float S, out float C, F);
				FloatToleranceCheck(Mathf.Sin(F), S, "Sine");
				FloatToleranceCheck(Mathf.Cos(F), C, "Cosine");
			}
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
