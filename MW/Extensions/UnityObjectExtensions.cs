#if RELEASE
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MW.Extensions
{
	public static class UnityObjectExtensions
	{
		/// <summary>Gets the position and Rotation</summary>
		/// <decorations decor="|Extension| void"></decorations>
		/// <param name="T"></param>
		/// <param name="Position"></param>
		/// <param name="Rotation"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void GetPositionAndRotation(this Transform T, out Vector3 Position, out Quaternion Rotation)
		{
			Position = T.position;
			Rotation = T.rotation;
		}

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

		/// <summary>Does G have an attached T component?</summary>
		/// <typeparam name="T">The type of component to check if G has.</typeparam>
		/// <decorations decor="|Extension| bool"></decorations>
		/// <param name="G">The GameObject to check if it has an attached T component.</param>
		/// <returns>True if G has an attached T component.</returns>
		public static bool Is<T>(this GameObject G) where T : Component
			=> G.Is<T>(out _);

		/// <summary>Does G have an attached T component?</summary>
		/// <typeparam name="T">The type to check if G has or is.</typeparam>
		/// <decorations decor="|Extension| bool"></decorations>
		/// <param name="G">The GameObject to check if it has or is T.</param>
		/// <param name="Component">Out the attached Component, otherwise null.</param>
		/// <returns>True if G has an attached T component.</returns>
		public static bool Is<T>(this GameObject G, out T Component) where T : Component
		{
			G.TryGetComponent(out Component);
			return Component != null;
		}
	}
}
#endif // RELEASE
