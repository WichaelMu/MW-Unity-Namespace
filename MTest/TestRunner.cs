#define WITH_PASS_MESSAGES

using MTest.Output;

namespace MTest
{
	internal class TestRunner
	{
		public static void FullSuite()
		{
			Execute.CoreTests.RoundToDPTest(out int DPPassed, out int DPTotalTests);
			Execute.CoreTests.SwapTests(out int SWPassed, out int SWTotalTests);
			Execute.MWTests.MVectorTest(out int MVPassed, out int MVTotalTests);
			Execute.MWTests.MArrayTests(out int MAPassed, out int MATotalTests);
			Execute.MWTests.THeapTests(out int THPassed, out int THTotalTests);

#if WITH_PASS_MESSAGES
			O.Line(nameof(Execute.CoreTests.RoundToDPTest) + $" Completed with: {DPPassed}/{DPTotalTests}");
			O.Line(nameof(Execute.CoreTests.SwapTests) + $" Completed with: {SWPassed}/{SWTotalTests}");
			O.Line(nameof(Execute.MWTests.MVectorTest) + $" Completed with: {MVPassed}/{MVTotalTests}");
			O.Line(nameof(Execute.MWTests.MArrayTests) + $" Completed with: {MAPassed}/{MATotalTests}");
			O.Line(nameof(Execute.MWTests.THeapTests) + $" Completed with: {THPassed}/{THTotalTests}");
#endif
		}
	}
}
