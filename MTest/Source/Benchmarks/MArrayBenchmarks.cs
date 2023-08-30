using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MW;
using MW.Diagnostics;

namespace MTest
{
	[TestClass]
	public class MArrayBenchmarks
	{
		[TestMethod]
		public void MArrayVList()
		{
			Stopwatch ListTimer = new Stopwatch();
			List<int> List = new List<int>();
			ListTimer.Stop();

			Stopwatch MArrayTimer = new Stopwatch();
			MArray<int> MArray = new MArray<int>();
			MArrayTimer.Stop();

			Assert.AreEqual(MArrayTimer.Time(), ListTimer.Time(), "Constructors");

			ListTimer.Restart();
			List.Add(0);
			ListTimer.Stop();

			MArrayTimer.Restart();
			MArray.Push(0);
			MArrayTimer.Stop();

			Assert.IsTrue(MArrayTimer.Time() <= ListTimer.Time(), "Push / Add Single Element.");

			ListTimer.Restart();
			List.Add(1);
			List.Add(2);
			List.Add(3);
			List.Add(4);
			ListTimer.Stop();

			MArrayTimer.Restart();
			MArray.Push(1, 2, 3, 4);
			MArrayTimer.Stop();

			Assert.IsTrue(MArrayTimer.Time() <= ListTimer.Time(), "Push / Add Multiple Elements.");

			ListTimer.Restart();
			List.Contains(3);
			ListTimer.Stop();

			MArrayTimer.Restart();
			MArray.Contains(3);
			MArrayTimer.Stop();

			Assert.IsTrue(MArrayTimer.Time() <= ListTimer.Time(), "Contains Single Element.");

			ListTimer.Restart();
			List.Remove(3);
			ListTimer.Stop();

			MArrayTimer.Restart();
			MArray.Pull(3);
			MArrayTimer.Stop();

			Assert.IsTrue(MArrayTimer.Time() <= ListTimer.Time(), "Pull / Remove Single Element.");

			ListTimer.Restart();
			List.Remove(4);
			List.Remove(2);
			List.Remove(1);
			ListTimer.Stop();

			MArrayTimer.Restart();
			MArray.Pull(4, 2, 1);
			MArrayTimer.Stop();
			Assert.IsTrue(MArrayTimer.Time() <= ListTimer.Time(), "Pull / Remove Multiple Elements.");
		}
	}
}
