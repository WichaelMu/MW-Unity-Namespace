﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MW;

namespace MTest
{
	[TestClass]
	public class ContainersTests
	{
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
}