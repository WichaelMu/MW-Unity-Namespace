

namespace MW
{
	public class THeap<T> where T : IHeapItem<T>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "TItems needs to be modified when adding or removing from THeap.")]
		T[] TItems;
		int nCount;

		public int Count
		{
			get
			{
				return nCount;
			}
		}

		public THeap(uint uMaxSize)
		{
			TItems = new T[uMaxSize];
		}

		public void Add(T TItem)
		{
			TItem.HeapItemIndex = nCount;
			TItems[nCount] = TItem;

			SortUp(TItem);
			nCount++;
		}

		public T RemoveFirst()
		{
			T T_ = TItems[0];
			nCount--;

			TItems[0] = TItems[nCount];
			TItems[0].HeapItemIndex = 0;
			SortDown(TItems[0]);

			return T_;
		}

		public void UpdateItem(T TItem)
		{
			SortUp(TItem);
			SortDown(TItem);
		}

		public void UpdateItemUp(T TItem)
		{
			SortUp(TItem);
		}

		public void UpdateItemDown(T TItem)
		{
			SortDown(TItem);
		}

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

	public interface IHeapItem<T> : System.IComparable<T>
	{
		int HeapItemIndex { get; set; }
	}
}
