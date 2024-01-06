using System;
using System.Runtime.CompilerServices;

namespace MW.Extensions
{
	public static class StringExtensions
	{
		/// <summary>The index of the last character in String. If String is null or empty, then -1.</summary>
		/// <decorations decor="|Extension| int"></decorations>
		/// <param name="String"></param>
		/// <returns>The last index.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetLastIndex(this string String)
			=> string.IsNullOrEmpty(String) ? -1 : String.Length - 1;

		/// <summary>The last character of this String. If String is null or empty, then \0.</summary>
		/// <decorations decor="|Extension| char"></decorations>
		/// <param name="String"></param>
		/// <returns>The last character.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static char GetLastChar(this string String)
			=> string.IsNullOrEmpty(String) ? char.MinValue : String[String.GetLastIndex()];

		/// <summary>Make String begin at Start. If String is null or empty, then string.Empty.</summary>
		/// <remarks>Assumes Start &lt; the Length of String.</remarks>
		/// <decorations decor="|Extension| string"></decorations>
		/// <param name="String"></param>
		/// <param name="Start">The number of characters to skip from the beginning.</param>
		/// <returns>A substring ignoring the first Start characters.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string MakeStringStart(this string String, int Start)
			=> string.IsNullOrEmpty(String) ? string.Empty : String.Substring(Start, String.GetLastIndex());

		/// <summary>Make String terminate at End. If String is null or empty, then string.Empty.</summary>
		/// <decorations decor="|Extension| string"></decorations>
		/// <param name="String"></param>
		/// <param name="End">The number of characters to skip from GetLastIndex().</param>
		/// <returns>A substring ignoring the last End characters.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string MakeStringEnd(this string String, int End)
			=> String.Substring(0, String.GetLastIndex() - End);
	}
}
