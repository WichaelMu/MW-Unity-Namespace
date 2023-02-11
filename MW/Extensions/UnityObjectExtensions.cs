using System.Runtime.CompilerServices;
using UnityEngine;

namespace MW.Extensions
{
	public static class UnityObjectExtensions
	{
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
	}
}
