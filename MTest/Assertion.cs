using MTest.Output;
using MW;
using UnityEngine;

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

		public static void AssertEquals<T>(int TestNumber, T Result, T Expected, ref int Passed, string Operation = "")
		{
			if (Assert(TestNumber, Result != null, ref Passed, Operation + " L is null."))
				return;
			if (Assert(TestNumber, Expected != null, ref Passed, Operation + " R is null."))
				return;

			if (Result != null && Expected != null)
			{
				if (!Result.Equals(Expected))
				{
					O.Failed(TestNumber, Result.ToString() + " == " + Expected.ToString(), false.ToString(), Operation, Result, Expected);
				}
				else
				{
					Passed++;
				}
			}
		}
	}
}