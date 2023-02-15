using Microsoft.VisualStudio.TestTools.UnitTesting;
using MW;
using MW.Extensions;

namespace MTest
{
	[TestClass]
	public class CastTests
	{
		[TestMethod]
		public void ObjectCastTests()
		{
			Assert.AreEqual(4f, 4.Cast<float>());
			Assert.AreEqual(4, 4f.Cast<int>());

			Assert.AreEqual(true, 4.Cast<bool>());
			Assert.AreEqual(1, true.Cast<int>());
			Assert.AreEqual(0, false.Cast<int>());

			Assert.AreEqual("4", 4.Cast<string>());

			MArray<string> Array = new MArray<string>();
			Assert.IsTrue(Array.Implements<IEnumerable<string>>());
			Assert.IsTrue(Array.Implements<MArray>());

			object FourPointTwo = 4.2f;
			Assert.AreEqual(FourPointTwo, 4.2f);
			Assert.AreEqual(4.2f, FourPointTwo.Cast<float>());

			ClassB B = new ClassB("MTest", 1 << 42, 1234.4321);

			Assert.IsTrue(B.Implements<ClassA>(out ClassA OutVar1));
			Assert.IsTrue(OutVar1.Char == B.String[0]);
			Assert.IsTrue(OutVar1.Integer == B.Long.Cast<int>());
			Assert.IsTrue(OutVar1.Float == B.Double.Cast<float>());
			Assert.AreEqual(B.String[0], B.Cast<ClassA>().Char);
			Assert.AreEqual(B.Long, B.Cast<ClassA>().Integer);
			Assert.AreEqual(B.Double, B.Cast<ClassA>().Float, .0001);

			object M = new MVector(1, 2, 6);
			Assert.IsTrue(M.Is<MVector>());
			Assert.AreEqual(1, M.Cast<MVector>().X);
			Assert.AreEqual(2, M.Cast<MVector>().Y);
			Assert.AreEqual(6, M.Cast<MVector>().Z);
		}
	}
}
