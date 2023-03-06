using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MW.Extensions
{
	/// <summary>Utility extension methods for C# objects.</summary>
	/// <decorations decor="public static class"></decorations>
	public static class ObjectExtensions
	{
		/// <summary>Is an object derived from T?</summary>
		/// <typeparam name="T">The type to check against O's parent.</typeparam>
		/// <decorations decor="|Extension| bool"></decorations>
		/// <param name="O">The object to check if it derives from T.</param>
		/// <docreturns>True if O inherits I.</docreturns>
		/// <returns><see langword="True"/> if O inherits T.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Is<T>(this object O) => O is T || O.GetType() == typeof(T);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Is<T>(this object O, out T Casted)
		{
			Casted = O.Cast<T>();
			return Casted != null;
		}

		/// <summary>Cast an object to a Type.</summary>
		/// <typeparam name="T">The class to convert O to.</typeparam>
		/// <decorations decor="|Extension| T"></decorations>
		/// <param name="O">The object to cast.</param>
		/// <returns>O of type T if O is T, otherwise null.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Cast<T>(this object O)
		{
			return O is T R
				? R
				: (T)Convert.ChangeType(O, typeof(T));
		}

		/// <summary>Cast an object to T if it is attached, implemented, or derived.</summary>
		/// <typeparam name="T">The Unity Component, Interface, or Type.</typeparam>
		/// <decorations decor="public static T"></decorations>
		/// <param name="O">The object to check if it is T is attached, implemented, or derived.</param>
		/// <returns>O as T or default.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T CastAll<T>(this object O)
		{
			if (O.Is(out GameObject G))
				return G.GetComponent<T>();

			if (O.Implements(out T Interface))
				return Interface;

			return O.Cast<T>();
		}

		/// <summary>Check if O implements interface I.</summary>
		/// <typeparam name="I">The interface to cast O to.</typeparam>
		/// <decorations decor="|Extension| bool"></decorations>
		/// <param name="O">The object to convert.</param>
		/// <docreturns>True if O implements I.</docreturns>
		/// <returns><see langword="True"/> if O implements I.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Implements<I>(this object O)
			=> O is I;

		/// <summary>Check if O implements interface I.</summary>
		/// <typeparam name="I">The interface to cast O to.</typeparam>
		/// <decorations decor="|Extension| bool"></decorations>
		/// <param name="O">The object to convert.</param>
		/// <param name="Interface">O's implementation of I.</param>
		/// <docreturns>True if O implements I.</docreturns>
		/// <returns><see langword="True"/> if O implements I.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Implements<I>(this object O, out I Interface)
		{
			Interface = O.Cast<I>();
			return Interface != null;
		}
	}
}