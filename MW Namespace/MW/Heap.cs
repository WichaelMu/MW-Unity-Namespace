

namespace MW
{
	/// <summary>The implementation of a Minimum or Maximum Heap.</summary>
	/// <typeparam name="T">The type to store in this heap.</typeparam>
	public class THeap<T> where T : IHeapItem<T>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add read-only modifier", Justification = "TItems needs to be modified when adding or removing from THeap.")]
		T[] Internal_Items;
		int Internal_Count;

		/// <summary>The number of elements in the heap.</summary>
		public int Count
		{
			get
			{
				return Internal_Count;
			}
		}

		/// <summary>Generates a new Heap, initialised with MaxSize.</summary>
		/// <param name="MaxSize"></param>
		public THeap(uint MaxSize)
		{
			Internal_Items = new T[MaxSize];
		}

		/// <summary>Adds an item to this Heap.</summary>
		/// <param name="Item">The item to add.</param>
		public void Add(T Item)
		{
			Item.HeapItemIndex = Internal_Count;
			Internal_Items[Internal_Count] = Item;

			SortUp(Item);
			Internal_Count++;
		}

		/// <summary>Remove the element at the root of this Heap.</summary>
		/// <returns>The element that was removed.</returns>
		public T RemoveFirst()
		{
			T T_ = Internal_Items[0];
			Internal_Count--;

			Internal_Items[0] = Internal_Items[Internal_Count];
			Internal_Items[0].HeapItemIndex = 0;
			SortDown(Internal_Items[0]);

			return T_;
		}

		/// <summary>Updates Item's position in the Heap.</summary>
		/// <param name="Item">The item to update.</param>
		public void UpdateItem(T Item)
		{
			SortUp(Item);
			SortDown(Item);
		}

		/// <summary>Sorts this Item upwards.</summary>
		/// <param name="Item">The item to update.</param>
		public void UpdateItemUp(T Item)
		{
			SortUp(Item);
		}

		/// <summary>Sorts this Item downwards.</summary>
		/// <param name="Item">The item to update.</param>
		public void UpdateItemDown(T Item)
		{
			SortDown(Item);
		}

		/// <summary>Whether or not this Heap contains Item.</summary>
		/// <param name="Item">The Item to check.</param>
		/// <returns>True if Item exists in this Heap.</returns>
		public bool Contains(T Item) => Equals(Internal_Items[Item.HeapItemIndex], Item);

		void SortDown(T Item)
		{
			while (true)
			{
				int nLeft = Item.HeapItemIndex * 2 + 1;
				int nRight = Item.HeapItemIndex * 2 + 2;
				int nSwap;

				if (nLeft < Internal_Count)
				{
					nSwap = nLeft;

					if (nRight < Internal_Count)
					{
						if (Internal_Items[nLeft].CompareTo(Internal_Items[nRight]) < 0)
						{
							nSwap = nRight;
						}
					}

					if (Item.CompareTo(Internal_Items[nSwap]) < 0)
					{
						Swap(Item, Internal_Items[nSwap]);
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
				T TParent = Internal_Items[nParent];

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
			Internal_Items[T1.HeapItemIndex] = T2;
			Internal_Items[T2.HeapItemIndex] = T1;

			int _ = T1.HeapItemIndex;
			T1.HeapItemIndex = T2.HeapItemIndex;
			T2.HeapItemIndex = _;
		}

		/// <summary>The Heap as a T[].</summary>
		/// <returns>T[] in the order of this THeap.</returns>
		public T[] Array()
		{
			T[] Array = new T[Internal_Count];
			for (int i = 0; i < Internal_Count; ++i)
				Array[i] = Internal_Items[i];

			return Array;
		}
	}

	/// <summary>The Interface that T must implement if it is to be used as a Heap.</summary>
	/// <typeparam name="T">The type to make compatible with THeap.</typeparam>
	public interface IHeapItem<T> : System.IComparable<T>
	{
		/// <summary>The position in a THeap.</summary>
		int HeapItemIndex { get; set; }
	}
}
