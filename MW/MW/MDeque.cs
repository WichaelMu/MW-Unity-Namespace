﻿using System;
using System.Runtime.CompilerServices;

namespace MW
{
	/// <summary>The implementation of a double-ended Queue; a 'Deque'.</summary>
	/// <typeparam name="T">The type to store in this Deque.</typeparam>
	/// <decorations decor="public class {T} : MContainer{T}"></decorations>
	public class MDeque<T> : MContainer<T>
	{
		/// <summary>Initialisation of a new MDeque with an Initial Size of T-defaulted Elements.</summary>
		/// <param name="Elements">Initial Elements.</param>
		public MDeque(T[] Elements) : base(Elements) { }
		/// <summary>Initialisation of a new MDeque with an Initial Size of T-defaulted Elements.</summary>
		/// <param name="InitialSize">Initial capacity of Elements.</param>
		public MDeque(int InitialSize) : base(InitialSize) { }
		/// <summary>Initialisation of a new MDeque with 32 T-defaulted Elements.</summary>
		public MDeque() : this(32) { }

		/// <summary>Adds an Element to the front of the Queue.</summary>
		/// <decorations decor="public void"></decorations>
		/// <param name="Element">The Element to add.</param>
		public void AddLead(T Element)
		{
			if (Num == 0)
			{
				Elements[0] = Element;
			}
			else
			{
				T[] Prefixed = new T[Num + 1];
				Prefixed[0] = Element;
				Array.Copy(Elements, 0, Prefixed, 1, Num);
				Elements = Prefixed;
			}

			++Num;
		}

		/// <summary>Adds an Element to the end of the Queue.</summary>
		/// <decorations decor="public void"></decorations>
		/// <param name="Element">The Element to add.</param>
		public void AddEnd(T Element)
		{
			if (Num == Elements.Length)
				Resize(Num + 5);

			Elements[Num] = Element;
			++Num;
		}

		/// <summary>Gets the Element at the front of the Queue.</summary>
		/// <decorations decor="public T"></decorations>
		/// <returns>The Element at the front of the Queue.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Lead() => Elements[0];
		/// <summary>Gets the Element at the end of the Queue.</summary>
		/// <decorations decor="public T"></decorations>
		/// <returns>The Element at the end of the Queue.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T End() => Elements[Num - 1];

		/// <summary>Gets and removes the Element at the front of the Queue.</summary>
		/// <decorations decor="public T"></decorations>
		/// <returns>The Element that was at the front of the Queue.</returns>
		public T PopLead()
		{
			if (Num == 0)
				return default;

			T RetVal = Lead();
			--Num;

			T[] Popped = new T[Num];
			Array.Copy(Elements, 1, Popped, 0, Num);
			Elements = Popped;

			return RetVal;
		}

		/// <summary>Gets and removes the Element at the end of the Queue.</summary>
		/// <decorations decor="public T"></decorations>
		/// <returns>The Element that was at the end of the Queue.</returns>
		public T PopEnd()
		{
			T RetVal = End();
			--Num;

			Resize(Num);

			return RetVal;
		}

		/// <summary>Abstract implementation of <see cref="MContainer{T}.TArray"/>.</summary>
		/// <docs>Abstract implementation from MContainer&lt;T&gt;.</docs>
		/// <decorations decor="public override T[]"></decorations>
		/// <returns>T[].</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override T[] TArray()
		{
			T[] RetVal = new T[Num];
			Array.Copy(Elements, RetVal, Num);
			return RetVal;
		}

		/// <summary>Square bracket accessor.</summary>
		/// <decorations decor="public T"></decorations>
		/// <param name="I">The index to access T Element.</param>
		/// <returns>Element at the specified index.</returns>
		/// <exception cref="IndexOutOfRangeException">Raised when I is beyond the range of Elements. See InRange(int).</exception>
		public T this[int I]
		{
			get
			{
				if (InRange(I))
					return Elements[I];
				throw new IndexOutOfRangeException($"Expected Index >= 0 && Index < {Num}. Index: {I}");
			}
			set
			{
				if (InRange(I))
					Elements[I] = value;
				else
					throw new IndexOutOfRangeException($"Expected Index >= 0 && Index < {Num}. Index: {I}");
			}
		}
	}
}
