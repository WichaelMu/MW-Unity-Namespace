

namespace MW {

    public enum EDirection {
        forward,
        right,
        back,
        left,
        up,
        down
    }

    public enum EEquation {
        Linear,
        EaseInSine,
        EaseOutSine,
        EaseInOutSine,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,
        EaseInQuart,
        EaseOutQuart,
        EaseInOutQuart,
        EaseInQuint,
        EaseOutQuint,
        EaseInOutQuint,
        EaseInExpo,
        EaseOutExpo,
        EaseInOutExpo,
        EaseInCirc,
        EaseOutCirc,
        EaseInOutCirc,
        Spring,
        EaseInBounce,
        EaseOutBounce,
        EaseInOutBounce,
        EaseInBack,
        EaseOutBack,
        EaseInOutBack,
        EaseInElastic,
        EaseOutElastic,
        EaseInOutElastic,
    };

    public enum EUnits {
        MetresPerSecond,
        KilometresPerSecond,
        MetresPerHour,
        KilometrsePerHour,
        FeetPerSecond,
        MilesPerSecond,
        FeetPerHour,
        MilesPerHour
    }

    public enum EButton {
        LeftMouse,
        MiddleMouse,
        RightMouse
    }

    /// <summary>Generates a new pair of two types of values.</summary>
    /// <typeparam name="T_First">The type of the first variable to store.</typeparam>
    /// <typeparam name="T_Second">The type of the second variable to store.</typeparam>
    public struct TPair<T_First, T_Second> {
        public T_First first { get; set; }
        public T_Second second { get; set; }

        public TPair(T_First first, T_Second second) {
            this.first = first;
            this.second = second;
        }
    }

    /// <summary>Generates a new variable of three types of values.</summary>
    /// <typeparam name="T_First">The type of the first variable to store.</typeparam>
    /// <typeparam name="T_Second">The type of the second variable to store.</typeparam>
    /// <typeparam name="T_Third">The type of the third variable to store.</typeparam>
    public struct TTriple<T_First, T_Second, T_Third> {
        public T_First first { get; set; }
        public T_Second second { get; set; }
        public T_Third third { get; set; }

        public TTriple(T_First first, T_Second second, T_Third third) {
            this.first = first;
            this.second = second;
            this.third = third;
        }
    }

    public class THeap<T> where T : IHeapItem<T> {
        T[] TItems;
        int nCount;

        public int Count { get {
                return nCount;
			} 
        }

        public THeap(int nMaxSize) {
            TItems = new T[nMaxSize];
		}

        public void Add(T TItem) {
            TItem.nIndex = nCount;
            TItems[nCount] = TItem;

            SortUp(TItem);
            nCount++;
		}

        public T RemoveFirst() {
            T T_ = TItems[0];
            nCount--;

            TItems[0] = TItems[nCount];
            TItems[0].nIndex = 0;
            SortDown(TItems[0]);

            return T_;
		}

        public void UpdateItem(T TItem) {
            SortUp(TItem);
            SortDown(TItem);
		}

        public bool Contains(T TItem) {
            return Equals(TItems[TItem.nIndex], TItem);
		}

        void SortDown(T TItem) {
            while (true) {
                int nLeft = TItem.nIndex * 2 + 1;
                int nRight = TItem.nIndex * 2 + 2;
                int nSwap;

                if (nLeft < nCount) {
                    nSwap = nLeft;

                    if (nRight < nCount) {
                        if (TItems[nLeft].CompareTo(TItems[nRight]) < 0) {
                            nSwap = nRight;
						}
					}

                    if (TItem.CompareTo(TItems[nSwap]) < 0) {
                        Swap(TItem, TItems[nSwap]);
					} else {
                        return;
					}
				} else {
                    return;
				}
            }
		}

        void SortUp(T TItem) {
            int nParent = (TItem.nIndex - 1) / 2;

            while (true) {
                T TParent = TItems[nParent];

                if (TItem.CompareTo(TParent) > 0) {
                    Swap(TItem, TParent);
				} else {
                    break;
				}

                nParent = (TItem.nIndex - 1) / 2;
            }
		}

        void Swap(T l, T r) {
            TItems[l.nIndex] = r;
            TItems[r.nIndex] = l;

            int _ = l.nIndex;
            l.nIndex = r.nIndex;
            r.nIndex = _;
		}
	}

    public interface IHeapItem<T> : System.IComparable<T> {
        int nIndex { get; set; }
	}
}