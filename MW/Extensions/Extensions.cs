using System.Runtime.CompilerServices;
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

		/// <summary>Fast magnitude of a Rigidbody's velocity.</summary>
		/// <decorations decor="|Extension| float"></decorations>
		/// <param name="R">The Rigidbody to get the magnitude of.</param>
		/// <returns>The magnitude of R's velocity.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float FMagnitude(this Rigidbody R) => R.velocity.FMagnitude();

		/// <summary>Fast magnitude of a Rigidbody2D's velocity.</summary>
		/// <decorations decor="|Extension| float"></decorations>
		/// <param name="R">The Rigidbody2D to get the magnitude of.</param>
		/// <returns>The magnitude of R's velocity.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float FMagnitude(this Rigidbody2D R) => R.velocity.FMagnitude();

		/// <summary>Fast direction of a Rigidbody's velocity.</summary>
		/// <decorations decor="|Extension| float"></decorations>
		/// <param name="R">The Rigidbody to get the direction of.</param>
		/// <returns>The direction of R's velocity.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 FDirection(this Rigidbody R) => R.velocity.FNormalise();

		/// <summary>Fast direction of a Rigidbody2D's velocity.</summary>
		/// <decorations decor="|Extension| float"></decorations>
		/// <param name="R">The Rigidbody2D to get the direction of.</param>
		/// <returns>The direction of R's velocity.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 FDirection(this Rigidbody2D R) => R.velocity.FNormalise();

		/// <summary>Fast distance between two Vector3s.</summary>
		/// <decorations decor="|Extension| float"></decorations>
		/// <param name="V">The Vector3 to measure the distance from.</param>
		/// <param name="R">The Vector3 to measure the distance to.</param>
		/// <returns>The distance between V and R.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float FDistance(this Vector3 V, Vector3 R) => (V - R).FMagnitude();

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

		/// <summary>Gets or Adds T Component to a GameObject.</summary>
		/// <typeparam name="T">The type to Get or Add to G.</typeparam>
		/// <decorations decor="|Extension| T"></decorations>
		/// <param name="G">The GameObject to Get or Add T.</param>
		/// <returns>The T Component attached to G.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T GetOrAddComponent<T>(this GameObject G) where T : Component
		{
			if (G.TryGetComponent(out T Component))
				return Component;
			return G.AddComponent<T>();
		}

		/// <summary>Gets or Adds T Component to a Transform.</summary>
		/// <typeparam name="T">The type to Get or Add to R.</typeparam>
		/// <decorations decor="|Extension| T"></decorations>
		/// <param name="R">The Transform to Get or Add T.</param>
		/// <returns>The T Component attached to R.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T GetOrAddComponent<T>(this Transform R) where T : Component => R.gameObject.GetOrAddComponent<T>();

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

		public static unsafe bool IsIllegalFloat(this float F)
		{
			int T = F.GetBits();
			return
				(T & 0x7FFFFFFF) > 0x7F800000 || // NaN.
				(T == 0x7F800000) || // Infinity.
				(T == unchecked((int)0xFF800000)); // -Infinity.
		}

		/// <summary>Is a MonoBehaviour derived from T?</summary>
		/// <typeparam name="T">The type to check against B's parent.</typeparam>
		/// <decorations decor="|Extension| bool"></decorations>
		/// <param name="B">The MonoBehaviour to check if it derives from T.</param>
		/// <docreturns>True if B inherits T.</docreturns>
		/// <returns><see langword="True"/> if B inherits T.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Is<T>(this MonoBehaviour B) where T : MonoBehaviour => B is T;
		/// <summary>Is a MonoBehaviour derived from T? If so, cast it to T.</summary>
		/// <typeparam name="T">The type to check against B's parent.</typeparam>
		/// <decorations decor="|Extension| bool"></decorations>
		/// <param name="B">The MonoBehaviour to check if it derives from T.</param>
		/// <param name="Behaviour">Out B as T, if B derives from T.</param>
		/// <docreturns>True if B inherits T.</docreturns>
		/// <returns><see langword="True"/> if B inherits T.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Is<T>(this MonoBehaviour B, out T Behaviour) where T : MonoBehaviour
		{
			Behaviour = null;
			if (B.Is<T>())
				Behaviour = B as T;

			return Behaviour != null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Is<T>(this object O) => O is T || O.GetType() == typeof(T);

		/// <summary>Cast an object to a Type.</summary>
		/// <typeparam name="T">The class to convert O to.</typeparam>
		/// <param name="O">The object to cast.</param>
		/// <returns>T class type of O if O is T, otherwise null.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Cast<T>(this object O) => O is T R ? R : default;
	}
}
