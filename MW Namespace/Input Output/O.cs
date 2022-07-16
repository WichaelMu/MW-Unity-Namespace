using MW.Diagnostics;

namespace MW.IO
{
	/// <summary></summary>
	/// <decorations decor="public static class"></decorations>
	public static class O
	{
		/// <summary>Identical to <see cref="Log.P(object[])"/>.</summary>
		/// <docs>Identical to Log.P(object[]).</docs>
		/// <decorations decor="public static void"></decorations>
		/// <param name="args">The list of objects to log separated by a space.</param>
		public static void Out(params object[] args)
		{
			Log.P(args);
		}
	}
}
