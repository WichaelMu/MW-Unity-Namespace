using System.Runtime.CompilerServices;

namespace MW.Extensions
{
	/// <summary>Utility extension methods for integers and floats.</summary>
	/// <decorations decor="public static class"></decorations>
	public static class Numerical
	{
		/// <summary>Gets the floating-point bits of a float as an int.</summary>
		/// <decorations decor="|Extension| int"></decorations>
		/// <param name="F">The float to get the bits of.</param>
		/// <returns>The 1:1 bit conversion of F to an int.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe int GetBits(this float F) => *(int*)&F;

		/// <summary>Set the bits of a float to the bits of an int.</summary>
		/// <decorations decor="|Extension| void"></decorations>
		/// <param name="F">The float to set the bits of.</param>
		/// <param name="I">The bits to pass into F.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe void SetBits(this float F, int I) => F = *(float*)&I;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe bool IsIllegalFloat(this float F, EIllegalFlags Flags = EIllegalFlags.NaN | EIllegalFlags.PositiveInfinity | EIllegalFlags.NegativeInfinity)
		{
			int T = F.GetBits();
			return
				((EIllegalFlags.NaN & Flags) == EIllegalFlags.NaN && (T & 0x7FFFFFFF) > 0x7F800000) ||   // NaN.
				((EIllegalFlags.PositiveInfinity & Flags) == EIllegalFlags.PositiveInfinity && (T == 0x7F800000)) ||               // Infinity.
				((EIllegalFlags.NegativeInfinity & Flags) == EIllegalFlags.NegativeInfinity && (T == unchecked((int)0xFF800000))); // -Infinity.
		}
	}

	public enum EIllegalFlags : byte
	{
		NaN = 1,
		PositiveInfinity = 2,
		NegativeInfinity = 4
	}

}
