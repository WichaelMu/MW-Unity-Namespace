using Microsoft.VisualStudio.TestTools.UnitTesting;
using MW;

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

		[TestMethod]
		public void StrCompTests()
		{
			Assert.IsTrue(Utils.Compare("Left String", "Right String") > .75f);
			Assert.IsTrue(Utils.Compare("Completely DIFFERENT", "This should be something that is not in any way similar to the Base String") < .35f);
		}
	}
}
