#if !MICROSOFT_TESTING
using MTest.Output;
using MW;

namespace MTest
{
	internal class Assertion
	{
		public static bool Assert(int TestNumber, bool bCondition, ref int Passed, string Operation = "")
		{
			if (!bCondition)
			{
				O.Failed(TestNumber, true.ToString(), false.ToString(), Operation);
			}
			else
			{
				Passed++;
			}

			return bCondition;
		}

		public static void AssertEquals(int TestNumber, float Result, float Expected, ref int Passed, string Operation = "")
		{
			if (Result != Expected)
			{
				O.Failed(TestNumber, Expected, Result, Operation);
			}
			else
			{
				Passed++;
			}
		}

		public static void AssertEquals(int TestNumber, MVector Result, MVector Expected, ref int Passed, string Operation = "")
		{
			if (Result != Expected)
			{
				O.Failed(TestNumber, Expected.ToString(), Result.ToString(), Operation);
			}
			else
			{
				Passed++;
			}
		}
	}
}
#endif