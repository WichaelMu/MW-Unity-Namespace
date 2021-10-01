

namespace MW {

    /// <summary>Orientations for specific faces.</summary>
    public enum EDirection {
        Forward,
        Right,
        Up,
        Back,
        Left,
        Down
    }

    /// <summary>Interpolating equations.</summary>
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

    /// <summary>Units of measurement of speed.</summary>
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

    /// <summary>The mouse buttons on a standard mouse.</summary>
    public enum EButton {
        LeftMouse,
        MiddleMouse,
        RightMouse
    }

    /// <summary>Generates a new pair of two types of values.</summary>
    /// <typeparam name="TFirst">The type of the first variable to store.</typeparam>
    /// <typeparam name="TSecond">The type of the second variable to store.</typeparam>
    public struct TPair<TFirst, TSecond> {
        public TFirst First { get; set; }
        public TSecond Second { get; set; }

        public TPair(TFirst First, TSecond Second) {
            this.First = First;
            this.Second = Second;
        }

		public override int GetHashCode() {
            int hash = 17;
            hash *= 31 + First.GetHashCode();
            hash *= 31 + Second.GetHashCode();

            return hash;
        }
    }

    /// <summary>Generates a new variable of three types of values.</summary>
    /// <typeparam name="TFirst">The type of the first variable to store.</typeparam>
    /// <typeparam name="TSecond">The type of the second variable to store.</typeparam>
    /// <typeparam name="TThird">The type of the third variable to store.</typeparam>
    public struct TTriple<TFirst, TSecond, TThird> {
        public TFirst First { get; set; }
        public TSecond Second { get; set; }
        public TThird Third { get; set; }

        public TTriple(TFirst First, TSecond Second, TThird Third) {
            this.First = First;
            this.Second = Second;
            this.Third = Third;
        }

        public override int GetHashCode() {
            int hash = 17;
            hash *= 31 + First.GetHashCode();
            hash *= 31 + Second.GetHashCode();
            hash *= 31 + Third.GetHashCode();

            return hash;
        }
	}

    public class THeap<T> where T : IHeapItem<T> {
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "TItems needs to be modified when adding or removing from THeap.")]
		T[] TItems;
        int nCount;

        public int Count { get {
                return nCount;
			} 
        }

        public THeap(uint uMaxSize) {
            TItems = new T[uMaxSize];
		}

        public void Add(T TItem) {
            TItem.HeapItemIndex = nCount;
            TItems[nCount] = TItem;

            SortUp(TItem);
            nCount++;
		}

        public T RemoveFirst() {
            T T_ = TItems[0];
            nCount--;

            TItems[0] = TItems[nCount];
            TItems[0].HeapItemIndex = 0;
            SortDown(TItems[0]);

            return T_;
		}

        public void UpdateItem(T TItem) {
            SortUp(TItem);
            SortDown(TItem);
        }

        public void UpdateItemUp(T TItem) {
            SortUp(TItem);
        }

        public void UpdateItemDown(T TItem) {
            SortDown(TItem);
        }

        public bool Contains(T TItem) => Equals(TItems[TItem.HeapItemIndex], TItem);

        void SortDown(T TItem) {
            while (true) {
                int nLeft = TItem.HeapItemIndex * 2 + 1;
                int nRight = TItem.HeapItemIndex * 2 + 2;
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
            int nParent = (TItem.HeapItemIndex - 1) / 2;

            while (true) {
                T TParent = TItems[nParent];

                if (TItem.CompareTo(TParent) > 0) {
                    Swap(TItem, TParent);
				} else {
                    break;
				}

                nParent = (TItem.HeapItemIndex - 1) / 2;
            }
		}

        void Swap(T l, T r) {
            TItems[l.HeapItemIndex] = r;
            TItems[r.HeapItemIndex] = l;

            int _ = l.HeapItemIndex;
            l.HeapItemIndex = r.HeapItemIndex;
            r.HeapItemIndex = _;
		}
	}

    public interface IHeapItem<T> : System.IComparable<T> {
        int HeapItemIndex { get; set; }
	}
}