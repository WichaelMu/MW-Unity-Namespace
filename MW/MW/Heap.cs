

namespace MW
{
	/// <summary>The implementation of a Minimum or Maximum Heap.</summary>
	/// <remarks>
	/// <see cref="SortFunc{T}"/> returns &lt; 0 for lower priority and &gt; 0 for higher priority.
	/// <br></br>
	/// This is a wrapper class for MHeap. If you want to create a standard Heap, consider using <see cref="MHeap{T}"/>.
	/// </remarks>
	/// <docremarks>
	/// SortFunc&lt;T&gt; returns &lt; 0 for lower priority and &gt; 0 for higher priority.
	/// &lt;br&gt;
	/// This is a wrapper class for MHeap. If you want to create a standard Heap, consider using MHeap&lt;T&gt;.
	/// </docremarks>
	/// <typeparam name="T">The type to store in this heap.</typeparam>
	/// <typeparam name="R">The raw type to use for comparisons.</typeparam>
	/// <decorations decor="public class {T} where T : IHeapItem"></decorations>
	public class THeap<T, R> where T : IHeapItem<R>
	{
		protected T[] Items;
		protected SortFunc<R> SortFunc;

		/// <summary>The number of elements in the heap.</summary>
		/// <decorations decor="public int"></decorations>
		public int Num;

		/// <summary>Generates a new Heap.</summary>
		/// <remarks>Initialised with an initial size of 32.</remarks>
		/// <param name="HeapOrder">The ordering in which to sort T in a heap.</param>
		public THeap(SortFunc<R> HeapOrder)
		{
			SortFunc = HeapOrder;
			Items = new T[32];
		}

		/// <summary>Generates a new Heap, initialised with MaxSize.</summary>
		/// <param name="InitialSize">The size of the heap.</param>
		/// <param name="HeapOrder">The ordering in which to sort T in a heap.</param>
		public THeap(int InitialSize, SortFunc<R> HeapOrder) : this(HeapOrder)
		{
			Items = new T[InitialSize];
		}

		/// <summary>Adds an item to this Heap.</summary>
		/// <decorations decor="public void"></decorations>
		/// <param name="Item">The item to add.</param>
		public void Add(T Item)
		{
			if (Num == Items.Length)
				Resize(Num + 5);

			Item.HeapItemIndex = Num;
			Items[Num] = Item;

			SortUp(Item);
			Num++;
		}

		/// <summary>Remove the Element at the root of this Heap.</summary>
		/// <decorations decor="public R"></decorations>
		/// <returns>The element that was removed.</returns>
		public R RemoveFirst()
		{
			T T_ = Items[0];
			Num--;

			Items[0] = Items[Num];
			Items[0].HeapItemIndex = 0;
			SortDown(Items[0]);

			return T_.Element;
		}

		/// <summary>Updates Item's position in the Heap.</summary>
		/// <decorations decor="public void"></decorations>
		/// <param name="Item">The item to update.</param>
		public void UpdateItem(T Item)
		{
			SortUp(Item);
			SortDown(Item);
		}

		/// <summary>Sorts this Item upwards.</summary>
		/// <decorations decor="public void"></decorations>
		/// <param name="Item">The item to update.</param>
		public void UpdateItemUp(T Item)
		{
			SortUp(Item);
		}

		/// <summary>Sorts this Item downwards.</summary>
		/// <decorations decor="public void"></decorations>
		/// <param name="Item">The item to update.</param>
		public void UpdateItemDown(T Item)
		{
			SortDown(Item);
		}

		/// <summary>Whether or not this Heap contains Item.</summary>
		/// <decorations decor="public bool"></decorations>
		/// <param name="Item">The Item to check.</param>
		/// <returns>True if Item exists in this Heap.</returns>
		public bool Contains(T Item) => Equals(Items[Item.HeapItemIndex], Item);

		/// <summary>Clears this Heap.</summary>
		/// <decorations decor="public void"></decorations>
		public void Flush()
		{
			if (Num > 0)
			{
				System.Array.Clear(Items, 0, Num);
				Num = 0;
			}
		}

		/// <summary>Reduces this Heap's memory usage to smallest possible required to store its elements.</summary>
		/// <decorations decor="public T[]"></decorations>
		/// <returns>The shrunk array.</returns>
		public T[] Shrink()
		{
			Resize(Num);
			return Array();
		}

		void Resize(int NewSize)
		{
			T[] Resized = new T[NewSize];
			System.Array.Copy(Items, Resized, Num);
			Items = Resized;
		}

		void SortDown(T Item)
		{
			while (true)
			{
				int nLeft = Item.HeapItemIndex * 2 + 1;
				int nRight = Item.HeapItemIndex * 2 + 2;
				int nSwap;

				if (nLeft < Num)
				{
					nSwap = nLeft;

					if (nRight < Num)
					{
						if (SortFunc(Items[nLeft].Element, Items[nRight].Element) < 0)
						{
							nSwap = nRight;
						}
					}

					if (SortFunc(Item.Element, Items[nSwap].Element) < 0)
					{
						Swap(Item, Items[nSwap]);
					}
					else
					{
						return;
					}
				}
				else
				{
					return;
				}
			}
		}

		void SortUp(T Item)
		{
			int nParent = (Item.HeapItemIndex - 1) / 2;

			while (true)
			{
				T TParent = Items[nParent];

				if (SortFunc(Item.Element, TParent.Element) > 0)
				{
					Swap(Item, TParent);
				}
				else
				{
					break;
				}

				nParent = (Item.HeapItemIndex - 1) / 2;
			}
		}

		void Swap(T T1, T T2)
		{
			Items[T1.HeapItemIndex] = T2;
			Items[T2.HeapItemIndex] = T1;

			int _ = T1.HeapItemIndex;
			T1.HeapItemIndex = T2.HeapItemIndex;
			T2.HeapItemIndex = _;
		}

		/// <summary>The Heap as a T[].</summary>
		/// <decorations decor="public T[]"></decorations>
		/// <returns>T[] in the order of this THeap.</returns>
		public T[] Array()
		{
			T[] Array = new T[Num];
			for (int i = 0; i < Num; ++i)
				Array[i] = Items[i];

			return Array;
		}

		protected internal bool InRange(int Index) => Index >= 0 && Index < Num;
	}

	/// <summary>The Interface that T must implement if it is to be used as a Heap.</summary>
	/// <typeparam name="T">The type to make compatible with THeap.</typeparam>
	/// <decorations decor="public interface {T}"></decorations>
	public interface IHeapItem<T>
	{
		/// <summary>The T Element in this Heap.</summary>
		/// <decorations decor="public T"></decorations>
		T Element { get; set; }

		/// <summary>The position in a THeap.</summary>
		/// <decorations decor="int"></decorations>
		int HeapItemIndex { get; set; }
	}

	/// <summary>The implementation of a THeap without type constraints.</summary>
	/// <remarks><see cref="SortFunc{T}"/> returns &lt; 0 for lower priority and &gt; 0 for higher priority.</remarks>
	/// <docremarks>SortFunc&lt;T&gt; returns &lt; 0 for lower priority and &gt; 0 for higher priority.</docremarks>
	/// <typeparam name="T">The type to store in this heap.</typeparam>
	/// <decorations decor="public class {T} : THeap{THeapInterface{T, T}, T}"></decorations>
	public class MHeap<T> : THeap<THeapInterface<T, T>, T>
	{
		/// <summary>Generates a new Heap.</summary>
		/// <remarks>Initialised with an initial size of 32.</remarks>
		/// <param name="HeapOrder">The ordering in which to sort T in a heap.</param>
		public MHeap(SortFunc<T> HeapOrder) : base(HeapOrder)
		{
		}

		/// <summary>Generates a new Heap, initialised with MaxSize.</summary>
		/// <param name="InitialSize">The size of the heap.</param>
		/// <param name="HeapOrder">The ordering in which to sort T in a heap.</param>
		public MHeap(int InitialSize, SortFunc<T> HeapOrder) : base(InitialSize, HeapOrder)
		{
		}

		/// <summary></summary>
		/// <param name="Element"></param>
		public void Push(T Element)
		{
			THeapInterface<T, T> NewElement = new THeapInterface<T, T>(Element);
			Add(NewElement);
		}

		/// <summary></summary>
		/// <param name="Element"></param>
		public void UpdateElement(T Element)
		{
			if (Find(Element, out THeapInterface<T, T> HeapInterface))
				UpdateItem(HeapInterface);
		}

		/// <summary></summary>
		/// <param name="Element"></param>
		/// <returns></returns>
		public bool Contains(T Element) => Find(Element, out _);

		bool Find(T Element, out THeapInterface<T, T> HeapInterface)
		{
			THeapInterface<T, T> Root = Items[0];
			int DepthLimit = Num + 1;
			return RecursiveFind(ref Root, ref Element, out HeapInterface, ref DepthLimit);
		}

		bool RecursiveFind(ref THeapInterface<T, T> Origin, ref T Element, out THeapInterface<T, T> HeapInterface, ref int DepthLimit)
		{
			HeapInterface = default;
			System.Console.WriteLine($"Finding: {Element} {Origin.Element}");

			if (EqualityCheck(Origin.Element, Element))
			{
				System.Console.WriteLine($"Equality: {Element} {Origin.Element}");
				HeapInterface = Origin;
				return true;
			}

			if (DepthLimit-- == 0)
				return false;

			if (SortFunc(Origin.Element, Element) < 0)
				return false;

			int Left = Origin.HeapItemIndex * 2 + 1;
			int Right = Origin.HeapItemIndex * 2 + 2;

			bool bResult = false;

			if (InRange(Left) && SortFunc(Items[Left].Element, Element) > 0)
				bResult |= RecursiveFind(ref Items[Left], ref Element, out HeapInterface, ref DepthLimit);

			if (InRange(Right) && (!bResult || SortFunc(Items[Right].Element, Element) > 0))
				bResult |= RecursiveFind(ref Items[Right], ref Element, out HeapInterface, ref DepthLimit);

			HeapInterface ??= default;
			return bResult;
		}

		bool EqualityCheck(object A, object B) => (A.Equals(B) || A.GetHashCode() == B.GetHashCode()) && A.GetType() == B.GetType();
	}

	/// <summary>A Heap interface for constructing minimum and maximum Heaps.</summary>
	/// <typeparam name="T">The type to make compatible with THeap.</typeparam>
	/// <typeparam name="R">The type to make compatible with THeap.</typeparam>
	/// <decorations decor="public class THeapInterface{T, R} : IHeapItem{R}"></decorations>
	public class THeapInterface<T, R> : IHeapItem<R>
	{
		/// <summary>The element in this Heap.</summary>
		public R Element { get => Item; set => Item = value; }
		R Item;

		/// <summary>The index in this Heap.</summary>
		/// <remarks>This is provided by IHeapItem.</remarks>
		/// <decorations decor="public int"></decorations>
		public int HeapItemIndex { get => HeapIndex; set => HeapIndex = value; }

		int HeapIndex;

		/// <summary></summary>
		/// <param name="Element"></param>
		public THeapInterface(R Element)
		{
			this.Element = Element;
		}
	}
}
