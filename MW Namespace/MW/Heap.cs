namespace MW
{
	/// <summary>The implementation of a Minimum or Maximum Heap.</summary>
	/// <typeparam name="T">The type to store in this heap.</typeparam>
	public class THeap<T> where T : IHeapItem<T>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "TItems needs to be modified when adding or removing from THeap.")]
		T[] TItems;
		int nCount;

		/// <summary>The number of elements in the heap.</summary>
		public int Count
		{
			get
			{
				return nCount;
			}
		}

		/// <summary>Generates a new Heap, initialised with uMaxSize.</summary>
		/// <param name="uMaxSize"></param>
		public THeap(uint uMaxSize)
		{
			TItems = new T[uMaxSize];
		}

		/// <summary>Adds an item to this Heap.</summary>
		/// <param name="TItem">The item to add.</param>
		public void Add(T TItem)
		{
			TItem.HeapItemIndex = nCount;
			TItems[nCount] = TItem;

			SortUp(TItem);
			nCount++;
		}

		/// <summary>Remove the element at the root of this Heap.</summary>
		/// <returns>The element that was removed.</returns>
		public T RemoveFirst()
		{
			T T_ = TItems[0];
			nCount--;

			TItems[0] = TItems[nCount];
			TItems[0].HeapItemIndex = 0;
			SortDown(TItems[0]);

			return T_;
		}

		/// <summary>Updates Item's position in the Heap.</summary>
		/// <param name="TItem">The item to update.</param>
		public void UpdateItem(T TItem)
		{
			SortUp(TItem);
			SortDown(TItem);
		}

		/// <summary>Sorts this Item upawrds.</summary>
		/// <param name="TItem">The item to update.</param>
		public void UpdateItemUp(T TItem)
		{
			SortUp(TItem);
		}

		/// <summary>Sorts this Item downwards.</summary>
		/// <param name="TItem">The item to update.</param>
		public void UpdateItemDown(T TItem)
		{
			SortDown(TItem);
		}

		/// <summary>Whether or not this Heap contains Item.</summary>
		/// <param name="TItem">The Item to check.</param>
		/// <returns>True if Item exists in this Heap.</returns>
		public bool Contains(T TItem) => Equals(TItems[TItem.HeapItemIndex], TItem);

		void SortDown(T TItem)
		{
			while (true)
			{
				int nLeft = TItem.HeapItemIndex * 2 + 1;
				int nRight = TItem.HeapItemIndex * 2 + 2;
				int nSwap;

				if (nLeft < nCount)
				{
					nSwap = nLeft;

					if (nRight < nCount)
					{
						if (TItems[nLeft].CompareTo(TItems[nRight]) < 0)
						{
							nSwap = nRight;
						}
					}

					if (TItem.CompareTo(TItems[nSwap]) < 0)
					{
						Swap(TItem, TItems[nSwap]);
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

		void SortUp(T TItem)
		{
			int nParent = (TItem.HeapItemIndex - 1) / 2;

			while (true)
			{
				T TParent = TItems[nParent];

				if (TItem.CompareTo(TParent) > 0)
				{
					Swap(TItem, TParent);
				}
				else
				{
					break;
				}

				nParent = (TItem.HeapItemIndex - 1) / 2;
			}
		}

		void Swap(T l, T r)
		{
			TItems[l.HeapItemIndex] = r;
			TItems[r.HeapItemIndex] = l;

			int _ = l.HeapItemIndex;
			l.HeapItemIndex = r.HeapItemIndex;
			r.HeapItemIndex = _;
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
