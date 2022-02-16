using MW.Diagnostics;

namespace MW.IO
{
	public static class O
	{
		/// <summary>Identical to <see cref="Log.P"/>.</summary>
		/// <param name="debug">The list of <see cref="object"/>s to log separated by a space.</param>
		public static void Out(params object[] debug)
		{
			Log.P(debug);
		}
	}
}
