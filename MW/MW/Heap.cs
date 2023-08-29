

namespace MW
{
	/// <summary>The implementation of a Minimum or Maximum Heap.</summary>
	/// <remarks><see cref="SortFunc{T}"/> returns &lt; 0 for Max Heap and &gt; 0 for Min Heap.</remarks>
	/// <docremarks>SortFunc&lt;T&gt; returns &lt; 0 for Max Heap and &gt; 0 for Min Heap.</docremarks>
	/// <typeparam name="T">The type to store in this heap.</typeparam>
	/// <decorations decor="public class {T} where T : IHeapItem{T}"></decorations>
	public class THeap<T> where T : IHeapItem<T>
	{
		T[] Items;
		int Count;

		SortFunc<T> SortFunc;

		/// <summary>The number of elements in the heap.</summary>
		/// <decorations decor="public int"></decorations>
		public int Num
		{
			get
			{
				return Count;
			}
		}

		/// <summary>Generates a new Heap.</summary>
		/// <remarks>Initialised with an initial size of 32.</remarks>
		/// <param name="HeapOrder">The ordering in which to sort T in a heap.</param>
		public THeap(SortFunc<T> HeapOrder)
		{
			SortFunc = HeapOrder;
			Items = new T[32];
		}

		/// <summary>Generates a new Heap, initialised with MaxSize.</summary>
		/// <param name="InitialSize">The size of the heap.</param>
		/// <param name="HeapOrder">The ordering in which to sort T in a heap.</param>
		public THeap(uint InitialSize, SortFunc<T> HeapOrder) : this(HeapOrder)
		{
			Items = new T[InitialSize];
		}

		/// <summary>Adds an item to this Heap.</summary>
		/// <decorations decor="public void"></decorations>
		/// <param name="Item">The item to add.</param>
		public void Add(T Item)
		{
			if (Count == Items.Length)
				Resize(Count + 5);

			Item.HeapItemIndex = Count;
			Items[Count] = Item;

			SortUp(Item);
			Count++;
		}

		/// <summary>Remove the element at the root of this Heap.</summary>
		/// <decorations decor="public T"></decorations>
		/// <returns>The element that was removed.</returns>
		public T RemoveFirst()
		{
			T T_ = Items[0];
			Count--;

			Items[0] = Items[Count];
			Items[0].HeapItemIndex = 0;
			SortDown(Items[0]);

			return T_;
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
				Count = 0;
			}
		}

		/// <summary>Reduces this Heap's memory usage to smallest possible required to store its elements.</summary>
		/// <decorations decor="public T[]"></decorations>
		/// <returns>The shrunk array.</returns>
		public T[] Shrink()
		{
			Resize(Count);
			return Array();
		}

		void Resize(int NewSize)
		{
			T[] Resized = new T[NewSize];
			System.Array.Copy(Items, Resized, Count);
			Items = Resized;
		}

		void SortDown(T Item)
		{
			while (true)
			{
				int nLeft = Item.HeapItemIndex * 2 + 1;
				int nRight = Item.HeapItemIndex * 2 + 2;
				int nSwap;

				if (nLeft < Count)
				{
					nSwap = nLeft;

					if (nRight < Count)
					{
						if (SortFunc(Items[nLeft], Items[nRight]) < 0)
						{
							nSwap = nRight;
						}
					}

					if (SortFunc(Item, Items[nSwap]) < 0)
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

				if (SortFunc(Item, TParent) > 0)
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
			T[] Array = new T[Count];
			for (int i = 0; i < Count; ++i)
				Array[i] = Items[i];

			return Array;
		}
	}

	/// <summary>The Interface that T must implement if it is to be used as a Heap.</summary>
	/// <typeparam name="T">The type to make compatible with THeap.</typeparam>
	/// <decorations decor="public interface {T}"></decorations>
	public interface IHeapItem<T>
	{
		/// <summary>The position in a THeap.</summary>
		/// <decorations decor="int"></decorations>
		int HeapItemIndex { get; set; }
	}
}
