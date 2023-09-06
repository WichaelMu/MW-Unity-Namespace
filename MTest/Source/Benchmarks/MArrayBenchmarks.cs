using Microsoft.VisualStudio.TestTools.UnitTesting;
using MW;
using MW.Diagnostics;

namespace MTest
{
	[TestClass]
	public class MArrayBenchmarks
	{
		[TestMethod]
		public void MArrayVList_Basic()
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

			Assert.IsTrue(MArrayTimer.Time() <= ListTimer.Time(), $"Push / Add Single Element. MArrayTimer: {MArrayTimer.Time()} | ListTimer: {ListTimer.Time()}.");

			ListTimer.Restart();
			List.Add(1);
			List.Add(2);
			List.Add(3);
			List.Add(4);
			ListTimer.Stop();

			MArrayTimer.Restart();
			MArray.Push(1, 2, 3, 4);
			MArrayTimer.Stop();

			Assert.IsTrue(MArrayTimer.Time() <= ListTimer.Time(), $"Push / Add Multiple Elements. MArrayTimer: {MArrayTimer.Time()} | ListTimer: {ListTimer.Time()}.");

			ListTimer.Restart();
			List.Contains(3);
			ListTimer.Stop();

			MArrayTimer.Restart();
			MArray.Contains(3);
			MArrayTimer.Stop();

			Assert.IsTrue(MArrayTimer.Time() <= ListTimer.Time(), $"Contains Single Element. MArrayTimer: {MArrayTimer.Time()} | ListTimer: {ListTimer.Time()}.");

			ListTimer.Restart();
			List.Remove(3);
			ListTimer.Stop();

			MArrayTimer.Restart();
			MArray.Pull(3);
			MArrayTimer.Stop();

			Assert.IsTrue(MArrayTimer.Time() <= ListTimer.Time(), $"Pull / Remove Single Element. MArrayTimer: {MArrayTimer.Time()} | ListTimer: {ListTimer.Time()}.");

			ListTimer.Restart();
			List.Remove(4);
			List.Remove(2);
			List.Remove(1);
			ListTimer.Stop();

			MArrayTimer.Restart();
			MArray.Pull(4, 2, 1);
			MArrayTimer.Stop();
			Assert.IsTrue(MArrayTimer.Time() <= ListTimer.Time(), $"Pull / Remove Multiple Elements. MArrayTimer: {MArrayTimer.Time()} | ListTimer: {ListTimer.Time()}.");
		}

		[TestMethod]
		public void MArrayVList_Limits()
		{
			const int kLimit = 10_000;

			List<int> List = new List<int>(kLimit);
			MArray<int> MArray = new MArray<int>(kLimit);

			Stopwatch ListTimer = new Stopwatch();
			for (int i = 0; i < kLimit; ++i)
				List.Add(i);
			ListTimer.Stop();

			Stopwatch MArrayTimer = new Stopwatch();
			for (int i = 0; i < kLimit; ++i)
				MArray.Push(i);
			MArrayTimer.Stop();

			Assert.IsTrue(MArrayTimer.Time() <= ListTimer.Time(), $"Push Elements. MArrayTimer: {MArrayTimer.Time()} | ListTimer: {ListTimer.Time()}.");

			ListTimer.Restart();
			for (int i = 0; i < kLimit; i += 3)
				List.Add(i);
			ListTimer.Stop();

			MArrayTimer.Restart();
			for (int i = 0; i < kLimit; i += 3)
				MArray.Push(i);
			MArrayTimer.Stop();

			Assert.IsTrue(MArrayTimer.Time() <= ListTimer.Time(), $"Push 3x Elements. MArrayTimer: {MArrayTimer.Time()} | ListTimer: {ListTimer.Time()}.");

			ListTimer.Restart();
			List.IndexOf(1599);
			ListTimer.Stop();

			MArrayTimer.Restart();
			MArray.FirstIndexOf(1599);
			MArrayTimer.Stop();

			Assert.IsTrue(MArrayTimer.Time() <= ListTimer.Time(), $"IndexOf. MArrayTimer: {MArrayTimer.Time()} | ListTimer: {ListTimer.Time()}.");

			ListTimer.Restart();
			List.LastIndexOf(1599);
			ListTimer.Stop();

			MArrayTimer.Restart();
			MArray.LastIndexOf(1599);
			MArrayTimer.Stop();

			Assert.IsTrue(MArrayTimer.Time() <= ListTimer.Time(), $"LastIndexOf. MArrayTimer: {MArrayTimer.Time()} | ListTimer: {ListTimer.Time()}.");
		}
	}
}
