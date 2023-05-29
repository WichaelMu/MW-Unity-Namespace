using Microsoft.VisualStudio.TestTools.UnitTesting;
using MW.Diagnostics;

namespace MTest
{
	[TestClass]
	public class StacktraceTests
	{
		[TestMethod]
		public void StacktraceInfoTests()
		{
			StacktraceInfo StacktraceInfo = Stacktrace.Here();

			Assert.AreEqual(nameof(StacktraceTests), StacktraceInfo.Class.Name);
			Assert.AreEqual(nameof(StacktraceInfoTests), StacktraceInfo.Function.Name);
		}
	}
}
