using System;
using System.Runtime.CompilerServices;

namespace MW
{
	/// <summary>The base class for generic, template data Containers.</summary>
	/// <typeparam name="T">The type to store.</typeparam>
	/// <decorations decor="public abstract class {T}"></decorations>
	public abstract class MContainer<T>
	{
		/// <summary>T Elements.</summary>
		/// <decorations decor="protected T[]"></decorations>
		protected T[] Elements;

		/// <summary>The number of Elements in this Container.</summary>
		/// <decorations decor="public int"></decorations>
		public int Num;

		/// <summary>Initialisation of a new Container with an Initial Size of T-defaulted Elements.</summary>
		/// <param name="Elements">Initial Elements.</param>
		public MContainer(T[] Elements)
		{
			this.Elements = Elements;
			Num = Elements.Length;
		}

		/// <summary>Initialisation of a new Container with an Initial Size of T-defaulted Elements.</summary>
		/// <param name="InitialSize">Initial capacity of Elements.</param>
		public MContainer(int InitialSize)
		{
			Elements = new T[FMath.Max(32, InitialSize)];
			Num = 0;
		}

		/// <summary>Initialisation of a new Container with 32 T-defaulted Elements.</summary>
		public MContainer() : this(32) { }

		/// <summary>Reduces this Container's memory usage to smallest possible required to store its Elements.</summary>
		/// <decorations decor="public T[]"></decorations>
		/// <returns>The shrunk array.</returns>
		public T[] Shrink()
		{
			Resize(Num);
			return TArray();
		}

		/// <summary>Checks if this Container is Empty.</summary>
		/// <decorations decor="public bool"></decorations>
		/// <returns>True if Num &lt;= 0. Otherwise, false.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsEmpty() => Num <= 0;

		/// <summary>Resizes this Container to a NewSize.</summary>
		/// <decorations decor="public void"></decorations>
		/// <param name="NewSize">The new capacity of this Container.</param>
		public void Resize(int NewSize)
		{
			if (NewSize == Num)
				return;
			T[] Resized = new T[NewSize];
			Array.Copy(Elements, Resized, Num);
			Elements = Resized;
		}

		/// <summary>Sets this MContainer of T Elements.</summary>
		/// <param name="Elements">The Elements to set.</param>
		public void SetFromElements(T[] Elements) { this.Elements = Elements; }

		/// <summary>Converts this Container to a T[].</summary>
		/// <decorations decor="public abstract T[]"></decorations>
		/// <returns>T[].</returns>
		public abstract T[] TArray();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected internal bool InRange(int Index) => Index >= 0 && Index < Num;
	}
}
