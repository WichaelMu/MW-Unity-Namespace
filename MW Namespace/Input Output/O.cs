using MW.Diagnostics;

namespace MW.IO
{
	public static class O
	{
		/// <summary>Identical to Log.P(object[]).</summary>
		/// <param name="debug">The list of objects to log separated by a space.</param>
		public static void Out(params object[] debug)
		{
			Log.P(debug);
		}
	}
}
