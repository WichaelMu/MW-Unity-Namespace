using System;
using System.Runtime.CompilerServices;

namespace MW
{
	public class MDeque<T> : MContainer<T>
	{
		public MDeque() : base() { }
		public MDeque(int InitialSize) : base(InitialSize) { }

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
		public override T[] TArray() => Elements;

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
				throw new IndexOutOfRangeException($"Expected Index >= 0 && Index < {Num}. Index: {I}");
			}
		}
	}
}
