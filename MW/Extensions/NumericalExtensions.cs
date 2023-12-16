using System.Runtime.CompilerServices;
using static MW.Math.Magic.Fast;
using static MW.Utils;

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

		/// <summary>Gets the bit at BitIndex of float F.</summary>
		/// <remarks>This gets the bits as they are in the floating-point format defined by IEEE-754.</remarks>
		/// <decorations decor="|Extension| bool"></decorations>
		/// <param name="F">The float to get the bit of.</param>
		/// <param name="BitIndex">The index of the bit. Range 0 to 32.</param>
		/// <returns>True if the bit of F at BitIndex is 1.</returns>
		public static unsafe bool GetBit(this float F, int BitIndex) => (*(int*)&F).GetBit(BitIndex);

		/// <summary>Gets the bit at BitIndex of int I.</summary>
		/// <decorations decor="|Extension| bool"></decorations>
		/// <param name="I">The int to get the bit of.</param>
		/// <param name="BitIndex">The index of the bit. Range 0 to 32.</param>
		/// <returns>True if the bit of I at BitIndex is 1.</returns>
		public static bool GetBit(this int I, int BitIndex)
		{
			Clamp(ref BitIndex, 0, 32);
			if (BitIndex == 32)
				return I < 0;

			// 0000 0000 0000 0010 0000 0000 0000 0000 
			int RightBitDifference = 32 - BitIndex;
			return (I << RightBitDifference) >> 31 == 1;
		}

		/// <summary>Flips the bit at BitIndex of float F.</summary>
		/// <remarks>This flips the bits as they are in the floating-point format defined by IEEE-754.</remarks>
		/// <decorations decor="|Extension| float"></decorations>
		/// <param name="F">The float to flip the bit of.</param>
		/// <param name="BitIndex">The index of the bit. Range 0 to 32.</param>
		/// <returns>The resulting float.</returns>
		public static unsafe float FlipBit(this ref float F, int BitIndex)
		{
			fixed (float* pF = &F)
			{
				int IntValue = *(int*)&pF;
				IntValue.FlipBit(BitIndex);
				F = *(float*)&IntValue;
			}

			return F;
		}

		/// <summary>Flips the bit at BitIndex of int I.</summary>
		/// <decorations decor="|Extension| int"></decorations>
		/// <param name="I">The int to flip the bit of.</param>
		/// <param name="BitIndex">The index of the bit. Range 0 to 32.</param>
		/// <returns>The resulting int.</returns>
		public static int FlipBit(this ref int I, int BitIndex)
		{
			Clamp(ref BitIndex, 0, 32);
			if (BitIndex == 32)
				I = I > 0 ? I * -1 : FAbs(I);

			I ^= 1 << BitIndex;

			return I;
		}

		/// <summary>Whether or not a float is considered to be zero.</summary>
		/// <decorations decor="public static bool"></decorations>
		/// <param name="F">The float to check.</param>
		/// <param name="Tolerance">The threshold for F to be considered zero.</param>
		/// <returns>True if F is +- Tolerance of zero.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsZero(this float F, float Tolerance = FMath.kEpsilon)
			=> Utils.IsZero(F, Tolerance);

		/// <summary>Checks if a float cannot be used for arithmetic.</summary>
		/// <decorations decor="|Extension| bool"></decorations>
		/// <param name="F">The float to check.</param>
		/// <param name="Flags">The flags to check against and consider a float as inoperable.</param>
		/// <returns>True if F returns a positive result against Flags.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsIllegalFloat(this float F, EIllegalFlags Flags = EIllegalFlags.NaN | EIllegalFlags.PositiveInfinity | EIllegalFlags.NegativeInfinity)
		{
			int T = F.GetBits();
			return
				((EIllegalFlags.NaN & Flags) == EIllegalFlags.NaN && (T & 0x7FFFFFFF) > 0x7F800000) ||   // NaN.
				((EIllegalFlags.PositiveInfinity & Flags) == EIllegalFlags.PositiveInfinity && (T == 0x7F800000)) ||               // Infinity.
				((EIllegalFlags.NegativeInfinity & Flags) == EIllegalFlags.NegativeInfinity && (T == unchecked((int)0xFF800000))); // -Infinity.
		}
	}

	/// <summary>Flags used to check if a float cannot be used for arithmetic.</summary>
	/// <decorations decor="public eunm : byte"></decorations>
	public enum EIllegalFlags : byte
	{
		/// <summary></summary>
		NaN = 1,
		/// <summary></summary>
		PositiveInfinity = 2,
		/// <summary></summary>
		NegativeInfinity = 4
	}

}
