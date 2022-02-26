using MW.Diagnostics;

namespace MW.IO
{
	/// <summary></summary>
	public static class O
	{
		/// <summary>Identical to Log.P(object[]).</summary>
		/// <param name="args">The list of objects to log separated by a space.</param>
		public static void Out(params object[] args)
		{
			Log.P(args);
		}
	}
}
