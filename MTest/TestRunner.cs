#define WITH_PASS_MESSAGES

#if WITH_PASS_MESSAGES
using MTest.Output;
#endif

using System.Diagnostics;


namespace MTest
{
	internal class TestRunner
	{
		public static void FullSuite()
		{
			Stopwatch sw = Stopwatch.StartNew();

			Execute.CoreTests.RoundToDPTest(out int DPPassed, out int DPTotalTests);
			StopAndRestart(sw, out long DPTime);

			Execute.CoreTests.SwapTests(out int SWPassed, out int SWTotalTests);
			StopAndRestart(sw, out long SWTime);

			Execute.CoreTests.SqrtTests(out int SQPassed, out int SQTotalTests);
			StopAndRestart(sw, out long SQTime);

			Execute.MWTests.MVectorTest(out int MVPassed, out int MVTotalTests);
			StopAndRestart(sw, out long MVTime);

			Execute.MWTests.MArrayTests(out int MAPassed, out int MATotalTests);
			StopAndRestart(sw, out long MATime);

			Execute.MWTests.MArrayTests2(out int M2Passed, out int M2TotalTests);
			StopAndRestart(sw, out long M2Time);

			Execute.MWTests.THeapTests(out int THPassed, out int THTotalTests);
			StopAndRestart(sw, out long THTime);

			Execute.FastTests.ASinTests(out int ASPassed, out int ASTotalTests);
			StopAndRestart(sw, out long ASTime);

#if WITH_PASS_MESSAGES
			O.Line(nameof(Execute.CoreTests.RoundToDPTest) + $" \t ({DPPassed}/{DPTotalTests}) Passed.\t Completed in: " + DPTime + "ms.");
			O.Line(nameof(Execute.CoreTests.SwapTests) + $" \t\t ({SWPassed}/{SWTotalTests}) Passed. \t\t Completed in: " + SWTime + "ms.");
			O.Line(nameof(Execute.CoreTests.SqrtTests) + $" \t\t ({SQPassed}/{SQTotalTests}) Passed. \t Completed in: " + SQTime + "ms.");
			O.Line(nameof(Execute.MWTests.MVectorTest) + $" \t ({MVPassed}/{MVTotalTests}) Passed. \t Completed in: " + MVTime + "ms.");
			O.Line(nameof(Execute.MWTests.MArrayTests) + $" \t ({MAPassed}/{MATotalTests}) Passed. \t Completed in: " + MATime + "ms.");
			O.Line(nameof(Execute.MWTests.MArrayTests2) + $" \t ({M2Passed}/{M2TotalTests}) Passed. \t Completed in: " + M2Time + "ms.");
			O.Line(nameof(Execute.MWTests.THeapTests) + $" \t\t ({THPassed}/{THTotalTests}) Passed. \t\t Completed in: " + THTime + "ms.");
			O.Line(nameof(Execute.FastTests.ASinTests) + $" \t\t ({ASPassed}/{ASTotalTests}) Passed. \t Completed in: " + ASTime + "ms.");
#endif
		}

		static void StopAndRestart(Stopwatch SW, out long Time)
		{
			SW.Stop();
			Time = SW.ElapsedMilliseconds;
			SW.Restart();
		}
	}
}
