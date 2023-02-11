using System.Runtime.CompilerServices;
using UnityEngine;

namespace MW.Extensions
{
	/// <summary>Utility extension methods for Unity Rigidbody and Rigidbody 2D Physics.</summary>
	/// <decorations decor="public static class"></decorations>
	public static class UnityPhysicsExtensions
	{
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
	}
}
