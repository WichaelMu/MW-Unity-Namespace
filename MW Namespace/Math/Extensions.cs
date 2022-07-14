using UnityEngine;
using static MW.Math.Magic.Fast;

namespace MW.Extensions
{
	/// <summary>Utility extension methods.</summary>
	public static class Methods
	{
		/// <summary>Fast normalisation of a Vector3.</summary>
		/// <param name="V">The Vector3 to normalise.</param>
		/// <returns>Normalised Vector3.</returns>
		public static Vector3 Normalise(this Vector3 V)
		{
			return V * InverseSqrt(V.sqrMagnitude);
		}

		/// <summary>Converts a <see cref="UnityEngine.Quaternion"/> to an MRotator.</summary>
		/// <docs>Converts a Quaternion to an MRotator.</docs>
		/// <param name="Q">The Quaternion to extract Pitch, Yaw and Roll.</param>
		/// <docreturns>An MRotator rotated according to a Quaternion.</docreturns>
		/// <returns>An MRotator rotated according to a <see cref="UnityEngine.Quaternion"/>.</returns>
		public static MRotator MakeRotator(this Quaternion Q)
		{
			return MRotator.Rotator(Q);
		}
	}
}
