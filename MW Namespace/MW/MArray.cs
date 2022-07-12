using System;
using System.Collections;
using System.Collections.Generic;

namespace MW
{
	/// <summary>A dynamic generic array combining the functionality of a List and a Dictionary.</summary>
	/// <typeparam name="T">The generic type.</typeparam>
	[Serializable]
	public class MArray<T> : IEnumerable<T>
	{
		[UnityEngine.SerializeField] List<T> Items;

		internal Dictionary<T, Stack<int>> HashMap;

		/// <summary>The number of T in this MArray; the size.</summary>
		public int Num { get => Items.Count; }

		/// <summary>Initialises an MArray with the default settings.</summary>
		public MArray()
		{
			Items = new();
			HashMap = new();
		}

		/// <summary>Initialises an MArray with an initial capacity.</summary>
		/// <param name="InitialSize">The number of elements this MArray will begin with.</param>
		public MArray(int InitialSize)
		{
			Items = new(InitialSize);
			HashMap = new(InitialSize);
		}

		/// <summary>Adds Item.</summary>
		/// <param name="Item">The unique element to add.</param>
		public void Push(T Item)
		{
			if (!HashMap.ContainsKey(Item))
			{
				HashMap.Add(Item, new());
				Items.Add(Item);
			}

			HashMap[Item].Push(Num - 1);
		}

		/// <summary>Adds a number of Items.</summary>
		/// <param name="Items">The list of elements to add.</param>
		public void Push(params T[] Items)
		{
			foreach (T T in Items)
				Push(T);
		}

		/// <summary>Removes Item.</summary>
		/// <param name="Item">The element to remove.</param>
		/// <returns>The new size of this MArray.</returns>
		public int Pull(T Item)
		{
			Remap();

			Items.RemoveAt(HashMap[Item].Pop());

			if (HashMap[Item].Count == 0)
				HashMap.Remove(Item);

			Remap();

			return Num;
		}

		/// <summary>Any element.</summary>
		/// <returns>Any random element.</returns>
		public T Random()
		{
			Random r = new();
			int Random = r.Next(0, Num - 1);

			return Items[Random];
		}

		/// <returns>The item at the front of the queue.</returns>
		public T First()
		{
			return Items[0];
		}

		/// <summary>Pops the item at the front of the queue.</summary>
		/// <returns>The item that was at the front of the queue.</returns>
		public T FirstPop()
		{
			T f = First();
			Items.RemoveAt(0);
			HashMap.Remove(f);

			return f;
		}

		/// <returns>The item at the top of the stack.</returns>
		public T Top()
		{
			return Items[Num - 1];
		}

		/// <summary>Pops the item at the top of the stack.</summary>
		/// <returns>The item at the top of the stack.</returns>
		public T TopPop()
		{
			T t = Top();
			Items.RemoveAt(Num - 1);
			HashMap.Remove(t);

			return t;
		}

		/// <summary>Whether or not Index is within range.</summary>
		/// <param name="Index">The Index to check for range.</param>
		/// <returns>If Index is greater than or equal to zero and less than the number of elements.</returns>
		public bool InRange(int Index)
		{
			bool bInRange = Index >= 0 && Index < Num;

			//if (!bInRange) throw new IndexOutOfRangeException("Index check failed. (Index >= 0 && Index < Num) == false with an index of " + Index);

			return bInRange;
		}

		/// <param name="Item">Item to check for existence.</param>
		/// <returns>Whether the HashCode of Item exists within the internal HashMap.</returns>
		public bool Contains(T Item)
		{
			return HashMap.ContainsKey(Item);
		}

		/// <summary>Clears this MArray.</summary>
		public void Flush()
		{
			Items.Clear();
			HashMap.Clear();
		}

		/// <docreturns>If this MArray is considered empty; Num == 0.</docreturns>
		/// <returns><see langword="True"/> <see langword="if"/> (<see cref="Num"/> == 0).</returns>
		public bool IsEmpty() => Num == 0;

		/// <returns>The mirror position of index over minimum zero, maximum Num.</returns>
		public T Mirror(int Index)
		{
			InRange(Index);
			return Mirror(0, Index);
		}

		/// <returns>The mirror position of index over Minimum to maximum Num.</returns>
		public T Mirror(int Minimum, int Index)
		{
			InRange(Index);
			return Items[Minimum + Num - 1 - Index];
		}

		/// <summary>The incoming and reflected Item of this mirror from zero to maximum Num.</summary>
		/// <docreturns>Source and Reflected information.</docreturns>
		/// <returns><see cref="Reflected"/> - Source and Reflected information.</returns>
		public Reflected Reflect(int Index)
		{
			InRange(Index);
			return Reflect(0, Index);
		}

		/// <summary>The incoming and reflected Item of this mirror from Minimum to maximum Num.</summary>
		/// <docreturns>Source and Reflected information.</docreturns>
		/// <returns><see cref="Reflected"/> - Source and Reflected information.</returns>
		public Reflected Reflect(int Minimum, int Index)
		{
			InRange(Index);
			return new Reflected(Items[Index], Mirror(Minimum, Index));
		}

		/// <summary>Reflects over Minimum, Maximum with Index.</summary>
		/// <returns>Outs the incoming and reflected Item of this mirror of Minimum, maximum Num.</returns>
		public void Reflect(int Minimum, int Index, out T Source, out T Reflected)
		{
			InRange(Index);

			Source = Items[Index];
			Reflected = Mirror(Minimum, Index);
		}

		/// <summary>Sorts Items.</summary>
		public void Sort()
		{
			Items.Sort();

			Remap();
		}

		/// <summary>Sorts Items from Index for Count using Comparer.</summary>
		/// <param name="Index"></param>
		/// <param name="Count"></param>
		/// <param name="Comparer"></param>
		public void Sort(int Index, int Count, IComparer<T> Comparer)
		{
			Items.Sort(Index, Count, Comparer);

			Remap();
		}

		/// <summary>Sorts Items with a Comparison&lt;T&gt;.</summary>
		/// <param name="Comparison"></param>
		public void Sort(Comparison<T> Comparison)
		{
			Sort(Comparison);

			Remap();
		}

		/// <summary>Sorts Items with an IComparer&lt;T&gt;.</summary>
		/// <param name="Comparer"></param>
		public void Sort(IComparer<T> Comparer)
		{
			Items.Sort(Comparer);

			Remap();
		}

		/// <summary>Reverses Items.</summary>
		public void Reverse()
		{
			Items.Reverse();

			Remap();
		}

		/// <summary>Reverses Items from Index for Count.</summary>
		/// <param name="Index"></param>
		/// <param name="Count"></param>
		public void Reverse(int Index, int Count)
		{
			Items.Reverse(Index, Count);

			Remap();
		}

		void Remap()
		{
			HashMap.Clear();
			for (int i = 0; i < Num; ++i)
			{
				if (!HashMap.ContainsKey(Items[i]))
				{
					HashMap.Add(Items[i], new());
				}

				HashMap[Items[i]].Push(i);
			}
		}

		public string Print()
		{
			string S = "";

			foreach (T T in Items)
				S += T.ToString() + " ";

			return S;
		}

		/// <summary>Converts this MArray into T[].</summary>
		/// <returns>T[].</returns>
		public T[] TArray()
		{
			return Items.ToArray();
		}

		/// <summary>Square bracket accessor.</summary>
		/// <param name="i">The index to access T item.</param>
		/// <returns>The Item at the specified index.</returns>
		public T this[int i] => Items[i];

		/// <param name="Check">The MArray to check for initialisation.</param>
		/// <returns>True if Check is null.</returns>
		public static bool CheckNull(MArray<T> Check) => Check == null;

		public IEnumerator<T> GetEnumerator()
		{
			return ((IEnumerable<T>)Items).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)Items).GetEnumerator();
		}

		/// <summary>If this MArray is null.</summary>
		/// <docs>If this MArray is null.</docs>
		/// <param name="CheckIfNull">The MArray to check whether it is uninitialised.</param>
		/// <returns>True if CheckIfNullOrEmpty is null.</returns>
		public static bool operator !(MArray<T> CheckIfNull) => CheckNull(CheckIfNull);

		/// <summary>Adds Right to the end of Left.</summary>
		/// <param name="Left">The MArray to append to.</param>
		/// <param name="Right">The MArray to append to Left.</param>
		/// <returns>A contiguous MArray from Left to Right.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static MArray<T> operator +(MArray<T> Left, MArray<T> Right)
		{
			if (CheckNull(Left))
				throw new ArgumentNullException(nameof(Left), "MArray is null!");
			if (CheckNull(Right))
				throw new ArgumentNullException(nameof(Right), "MArray is null.");

			if (Left.IsEmpty())
				return Right;
			if (Right.IsEmpty())
				return Left;

			MArray<T> NewMArray = new(Left.Num + Right.Num);
			NewMArray.Items.AddRange(Left.Items);
			NewMArray.Items.AddRange(Right.Items);

			return NewMArray;
		}

		/// <summary>Left elements that exist in Right.</summary>
		/// <param name="Left">The MArray to check AND.</param>
		/// <param name="Right">The MArray to compare to.</param>
		/// <returns>An MArray of Left elements that also exist in Right.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static MArray<T> operator &(MArray<T> Left, MArray<T> Right)
		{
			if (CheckNull(Left))
				throw new ArgumentNullException(nameof(Left), "MArray is null!");
			if (CheckNull(Right))
				throw new ArgumentNullException(nameof(Right), "MArray is null!");

			MArray<T> AndMArray = new();

			for (int i = 0; i < Left.Num; ++i)
			{
				T ItemAnd = Left[i];
				if (Right.Contains(ItemAnd))
					AndMArray.Push(ItemAnd);
			}

			return AndMArray;
		}

		/// <summary>Left elements that do not exist in Right.</summary>
		/// <param name="Left">The MArray to check OR.</param>
		/// <param name="Right">The MArray to compare to.</param>
		/// <returns>An MArray of Left's elements that do not exist in Right.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static MArray<T> operator ^(MArray<T> Left, MArray<T> Right)
		{
			if (CheckNull(Left))
				throw new ArgumentNullException(nameof(Left), "MArray is null!");
			if (CheckNull(Right))
				throw new ArgumentNullException(nameof(Right), "MArray is null!");

			if (Right.IsEmpty())
				return Left;

			MArray<T> OrMArray = new();
			for (int i = 0; i < Left.Num; ++i)
			{
				T ItemOr = Left[i];
				if (!Right.Contains(ItemOr))
					OrMArray.Push(ItemOr);
			}

			return OrMArray;
		}

		public static implicit operator int(MArray<T> Any) => Any.Num;
		public static implicit operator List<T>(MArray<T> Any) => Any.Items;
		public static implicit operator T[](MArray<T> Any) => Any.TArray();

		/// <summary>The incoming and reflected Item of this mirror over the provided Minimum and Maximum Num.</summary>
		public struct Reflected
		{
			/// <summary>In reflection.</summary>
			public T Source;
			/// <summary>Out reflection.</summary>
			public T Reflection;

			public Reflected(T Source, T Reflection)
			{
				this.Source = Source;
				this.Reflection = Reflection;
			}
		}

		/// <summary>Information which T failed to be pushed into an MArray&lt;T&gt;.</summary>
		public struct PushRangeFailed
		{
			/// <summary>The T that couldn't be added into the MArray.</summary>
			public T AttemptedItemToAdd;
			/// <summary>The index of the T in range that couldn't be added.</summary>
			public int IndexOfAttempt;

			public PushRangeFailed(T AttemptedItemToAdd, int IndexOfAttempt)
			{
				this.AttemptedItemToAdd = AttemptedItemToAdd;
				this.IndexOfAttempt = IndexOfAttempt;
			}
		}
	}
}
