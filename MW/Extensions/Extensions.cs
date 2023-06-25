#if RELEASE
using System.Runtime.CompilerServices;
using UnityEngine;
using static MW.Math.Magic.Fast;

namespace MW.Extensions
{
	/// <summary>Utility extension methods for Unity Vector and Quaternion Mathematics.</summary>
	/// <decorations decor="public static class"></decorations>
	public static class MathematicsExtensions
	{
		/// <summary>Fast normalisation of a Vector3.</summary>
		/// <decorations decor="|Extension| Vector3"></decorations>
		/// <param name="V">The Vector3 to normalise.</param>
		/// <returns>Normalised Vector3.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 FNormalise(this Vector3 V) => V * FInverseSqrt(V.sqrMagnitude);

		/// <summary>Fast normalisation of a Vector2.</summary>
		/// <decorations decor="|Extension| Vector2"></decorations>
		/// <param name="V">The Vector2 to normalise.</param>
		/// <returns>Normalised Vector2.</returns>
		public static Vector2 FNormalise(this Vector2 V) => V * FInverseSqrt(V.sqrMagnitude);

		/// <summary>Fast magnitude of a Vector3.</summary>
		/// <decorations decor="|Extension| float"></decorations>
		/// <param name="V">The Vector3 to get the magnitude of.</param>
		/// <returns>The magnitude of V.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float FMagnitude(this Vector3 V) => FSqrt(V.sqrMagnitude);

		/// <summary>Fast magnitude of a Vector2.</summary>
		/// <decorations decor="|Extension| float"></decorations>
		/// <param name="V">The Vector2 to get the magnitude of.</param>
		/// <returns>The magnitude of V.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float FMagnitude(this Vector2 V) => FSqrt(V.sqrMagnitude);
		/// <summary>Fast distance between two Vector3s.</summary>
		/// <decorations decor="|Extension| float"></decorations>
		/// <param name="V">The Vector3 to measure the distance from.</param>
		/// <param name="R">The Vector3 to measure the distance to.</param>
		/// <returns>The distance between V and R.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float FDistance(this Vector3 V, Vector3 R) => (V - R).FMagnitude();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 FClampMagnitude(this Vector3 V, float MaxMagnitude)
		{
			Vector3 Normalised = V.FNormalise();
			if (Normalised.sqrMagnitude > Vector3.kEpsilon)
				return Normalised * MaxMagnitude;

			return V;
		}

		/// <summary>Converts a <see cref="Quaternion"/> to an MRotator.</summary>
		/// <docs>Converts a Quaternion to an MRotator.</docs>
		/// <decorations decor="|Extension| MRotator"></decorations>
		/// <param name="Q">The Quaternion to extract Pitch, Yaw and Roll.</param>
		/// <docreturns>An MRotator rotated according to a Quaternion.</docreturns>
		/// <returns>An MRotator rotated according to a <see cref="Quaternion"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MRotator MakeRotator(this Quaternion Q) => MRotator.Rotator(Q);

		/// <summary>Converts an MVector to a Vector3.</summary>
		/// <decorations decors="|Extension| Vector3"></decorations>
		/// <param name="M">The MVector to convert.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 V3(this MVector M) => new Vector3(M.X, M.Y, M.Z);

		/// <summary>Converts a Vector3 to an MVector.</summary>
		/// <decorations decors="|Extension| MVector"></decorations>
		/// <param name="V">The Vector3 to convert.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MVector MV(this Vector3 V) => new MVector(V.x, V.y, V.z);
	}
}
#endif // RELEASE
