using MTest.Output;

namespace MTest
{
	class Entry
	{
		public static void Main(string[] Args)
		{

			if (Args.Length == 0)
			{
				TestRunner.FullSuite();
			}

			foreach (string Namespace in Args)
			{
				
			}
		}
	}
}