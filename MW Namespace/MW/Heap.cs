

namespace MW
{
	/// <summary>The implementation of a Minimum or Maximum Heap.</summary>
	/// <remarks><see cref="System.IComparable{T}"/> returns &lt; 0 for Max Heap and &gt; 0 for Min Heap.</remarks>
	/// <docremarks>IComparable&lt;T&gt; returns &lt; 0 for Max Heap and &gt; 0 for Min Heap.</docremarks>
	/// <typeparam name="T">The type to store in this heap.</typeparam>
	/// <decorations decor="public class {T} where T : IHeapItem{T}"></decorations>
	public class THeap<T> where T : IHeapItem<T>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add read-only modifier", Justification = "TItems needs to be modified when adding or removing from THeap.")]
		T[] Items;
		int Count;

		/// <summary>The number of elements in the heap.</summary>
		/// <decorations decor="public int"></decorations>
		public int Num
		{
			get
			{
				return Count;
			}
		}

		/// <summary>Generates a new Heap, initialised with MaxSize.</summary>
		/// <param name="MaxSize"></param>
		public THeap(uint MaxSize)
		{
			Items = new T[MaxSize];
		}

		/// <summary>Adds an item to this Heap.</summary>
		/// <decorations decor="public void"></decorations>
		/// <param name="Item">The item to add.</param>
		public void Add(T Item)
		{
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
						if (Items[nLeft].CompareTo(Items[nRight]) < 0)
						{
							nSwap = nRight;
						}
					}

					if (Item.CompareTo(Items[nSwap]) < 0)
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

				if (Item.CompareTo(TParent) > 0)
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
	/// <decorations decor="public interface {T} : IComparable{T}"></decorations>
	public interface IHeapItem<T> : System.IComparable<T>
	{
		/// <summary>The position in a THeap.</summary>
		int HeapItemIndex { get; set; }
	}
}
