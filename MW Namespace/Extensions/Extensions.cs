using UnityEngine;
using static MW.Math.Magic.Fast;

namespace MW.Extensions
{
	/// <summary>Utility extension methods.</summary>
	/// <decorations decor="public static class"></decorations>
	public static class Methods
	{
		/// <summary>Fast normalisation of a Vector3.</summary>
		/// <decorations decor="|Extension| Vector3"></decorations>
		/// <param name="V">The Vector3 to normalise.</param>
		/// <returns>Normalised Vector3.</returns>
		public static Vector3 Normalise(this Vector3 V)
		{
			return V * InverseSqrt(V.sqrMagnitude);
		}

		/// <summary>Converts a <see cref="Quaternion"/> to an MRotator.</summary>
		/// <docs>Converts a Quaternion to an MRotator.</docs>
		/// <decorations decor="|Extension| MRotator"></decorations>
		/// <param name="Q">The Quaternion to extract Pitch, Yaw and Roll.</param>
		/// <docreturns>An MRotator rotated according to a Quaternion.</docreturns>
		/// <returns>An MRotator rotated according to a <see cref="UnityEngine.Quaternion"/>.</returns>
		public static MRotator MakeRotator(this Quaternion Q)
		{
			return MRotator.Rotator(Q);
		}

		/// <summary>Converts an MVector to a Vector3.</summary>
		/// <decorations decors="|Extension| Vector3"></decorations>
		/// <param name="M">The MVector to convert.</param>
		public static Vector3 V3(this MVector M) => new Vector3(M.X, M.Y, M.Z);

		/// <summary>Converts a Vector3 to an MVector.</summary>
		/// <decorations decors="|Extension| MVector"></decorations>
		/// <param name="V">The Vector3 to convert.</param>
		public static MVector MV(this Vector3 V) => new MVector(V.x, V.y, V.z);
	}
}
