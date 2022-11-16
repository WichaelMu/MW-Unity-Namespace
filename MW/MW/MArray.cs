using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MW
{
	/// <summary>A dynamic generic array combining the functionality of a List and a Dictionary.</summary>
	/// <typeparam name="T">The generic type.</typeparam>
	/// <decorations decor="[Serializable] public class {T} : MArray, IEnumerable{T}"></decorations>
	[Serializable]
	public class MArray<T> : MArray, IEnumerable<T>
	{
		[UnityEngine.SerializeField] List<T> Items;

		internal Dictionary<T, Stack<int>> HashMap;

		/// <summary>The number of T in this MArray; the size.</summary>
		/// <decorations decor="public int"></decorations>
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
		/// <decorations decor="public void"></decorations>
		/// <param name="Item">The unique element to add.</param>
		public void Push(T Item)
		{
			if (!HashMap.ContainsKey(Item))
			{
				HashMap.Add(Item, new());
			}

			HashMap[Item].Push(Num);

			Items.Add(Item);
		}

		/// <summary>Adds a number of Items.</summary>
		/// <decorations decor="public void"></decorations>
		/// <param name="Items">The list of elements to add.</param>
		public void Push(params T[] Items)
		{
			foreach (T T in Items)
				Push(T);
		}

		public void PushUnique(T Item)
		{
			if (!Contains(Item))
				Push(Item);
		}

		public void PushUnique(params T[] Items)
		{
			foreach (T T in Items)
				if (!Contains(T))
					Push(T);
		}

		/// <summary>Removes the most recent push of Item.</summary>
		/// <decorations decor="public int"></decorations>
		/// <param name="Item">The element to remove.</param>
		/// <docreturns>The new size of this MArray, or kInvalid if Item doesn't exist.</docreturns>
		/// <returns>The new size of this MArray, or <see cref="MArray.kInvalid"/> if Item doesn't exist.</returns>
		public int Pull(T Item)
		{
			if (!Contains(Item))
				return kInvalid;

			Items.RemoveAt(HashMap[Item].Pop());

			if (HashMap[Item].Count == 0)
			{
				HashMap.Remove(Item);
			}

			Remap();

			return Num;
		}

		/// <summary>Removes all occurrences of Item.</summary>
		/// <param name="Item">The item to remove.</param>
		/// <docreturns>The new size of this MArray, or kInvalid if Item doesn't exist.</docreturns>
		/// <returns>The new size of this MArray, or <see cref="MArray.kInvalid"/> if Item doesn't exist.</returns>
		public int PullAll(T Item)
		{
			if (!Contains(Item))
				return kInvalid;

			AccessedData Data = Access(Item);
			int Occurences = Data.Occurrences;
			for (int i = 0; i < Occurences; ++i)
				Pull(Item);

			return Num;
		}

		/// <summary>Any element.</summary>
		/// <decorations decor="public T"></decorations>
		/// <returns>Any random element.</returns>
		public T Random()
		{
			Random r = new();
			int Random = r.Next(0, Num - 1);

			return Items[Random];
		}

		/// <summary>Accesses data specific to this Item in the MArray.</summary>
		/// <decorations decor="public AccessedData"></decorations>
		/// <param name="Item">The Item to access its data.</param>
		/// <docreturns>Accessed Data Occurrences and Positions, or AccessedData.None if this MArray does not have Item.</docreturns>
		/// <returns>Accessed Data Occurrences and Positions, or <see cref="MArray.AccessedData.None"/> if this MArray does not have Item.</returns>
		public AccessedData Access(T Item)
		{
			if (!Contains(Item))
				return AccessedData.None;

			AccessedData Data = new()
			{
				Occurrences = HashMap[Item].Count,
				Positions = new int[HashMap[Item].Count]
			};

			int i = 0;
			foreach (int Position in HashMap[Item])
				Data.Positions[i++] = Position;

			return Data;
		}

		/// <decorations decor="public T"></decorations>
		/// <returns>The item at the front of the queue.</returns>
		public T First()
		{
			return Items[0];
		}

		/// <summary>Pops the item at the front of the queue.</summary>
		/// <decorations decor="public T"></decorations>
		/// <returns>The item that was at the front of the queue.</returns>
		public T FirstPop()
		{
			T T = First();

			int[] Positions = Access(T).Positions;

			Items.RemoveAt(Positions[Positions.Length - 1]);

			if (HashMap[T].Count == 0)
			{
				HashMap.Remove(T);
			}

			Remap();

			return T;
		}

		/// <decorations decor="public T"></decorations>
		/// <returns>The item at the top of the stack.</returns>
		public T Top()
		{
			return Items[Num - 1];
		}

		/// <summary>Pops the item at the top of the stack.</summary>
		/// <decorations decor="public T"></decorations>
		/// <returns>The item at the top of the stack.</returns>
		public T TopPop()
		{
			T T = Top();
			Pull(T);

			return T;
		}

		/// <summary>Whether or not Index is within range.</summary>
		/// <decorations decor="public bool"></decorations>
		/// <param name="Index">The Index to check for range.</param>
		/// <returns>If Index is greater than or equal to zero and less than the number of elements.</returns>
		public bool InRange(int Index)
		{
			bool bInRange = Index >= 0 && Index < Num;

			//if (!bInRange) throw new IndexOutOfRangeException("Index check failed. (Index >= 0 && Index < Num) == false with an index of " + Index);

			return bInRange;
		}

		/// <decorations decor="public bool"></decorations>
		/// <param name="Item">Item to check for existence.</param>
		/// <returns>Whether the HashCode of Item exists within the internal HashMap.</returns>
		public bool Contains(T Item)
		{
			return HashMap.ContainsKey(Item);
		}

		/// <summary>Clears this MArray.</summary>
		/// <decorations decor="public void"></decorations>
		public void Flush()
		{
			Items.Clear();
			HashMap.Clear();
		}

		/// <decorations decor="public bool"></decorations>
		/// <docreturns>If this MArray is considered empty; Num == 0.</docreturns>
		/// <returns><see langword="true"/> <see langword="if"/> (<see cref="Num"/> == 0).</returns>
		public bool IsEmpty() => Num == 0;

		/// <decorations decor="public T"></decorations>
		/// <returns>The mirror position of index over minimum zero, maximum Num.</returns>
		public T Mirror(int Index)
		{
			InRange(Index);
			return Mirror(0, Index);
		}

		/// <decorations decor="public T"></decorations>
		/// <returns>The mirror position of index over Minimum to maximum Num.</returns>
		public T Mirror(int Minimum, int Index)
		{
			InRange(Index);
			return Items[Minimum + Num - 1 - Index];
		}

		/// <summary>The incoming and reflected Item of this mirror from zero to maximum Num.</summary>
		/// <decorations decor="public Reflected"></decorations>
		/// <docreturns>Source and Reflected information.</docreturns>
		/// <returns><see cref="Reflected"/> - Source and Reflected information.</returns>
		public Reflected Reflect(int Index)
		{
			InRange(Index);
			return Reflect(0, Index);
		}

		/// <summary>The incoming and reflected Item of this mirror from Minimum to maximum Num.</summary>
		/// <decorations decor="public Reflected"></decorations>
		/// <docreturns>Source and Reflected information.</docreturns>
		/// <returns><see cref="Reflected"/> - Source and Reflected information.</returns>
		public Reflected Reflect(int Minimum, int Index)
		{
			InRange(Index);
			return new Reflected(Items[Index], Mirror(Minimum, Index));
		}

		/// <summary>Reflects over Minimum, Maximum with Index.</summary>
		/// <decorations decor="public void"></decorations>
		/// <returns>Outs the incoming and reflected Item of this mirror of Minimum, maximum Num.</returns>
		public void Reflect(int Minimum, int Index, out T Source, out T Reflected)
		{
			InRange(Index);

			Source = Items[Index];
			Reflected = Mirror(Minimum, Index);
		}

		/// <summary>Sorts Items.</summary>
		/// <decorations decor="public void"></decorations>
		public void Sort()
		{
			Items.Sort();

			Remap();
		}

		/// <summary>Sorts Items from Index for Count using Comparer.</summary>
		/// <decorations decor="public void"></decorations>
		/// <param name="Index"></param>
		/// <param name="Count"></param>
		/// <param name="Comparer"></param>
		public void Sort(int Index, int Count, IComparer<T> Comparer)
		{
			Items.Sort(Index, Count, Comparer);

			Remap();
		}

		/// <summary>Sorts Items with a Comparison&lt;T&gt;.</summary>
		/// <decorations decor="public void"></decorations>
		/// <param name="Comparison"></param>
		public void Sort(Comparison<T> Comparison)
		{
			Sort(Comparison);

			Remap();
		}

		/// <summary>Sorts Items with an IComparer&lt;T&gt;.</summary>
		/// <decorations decor="public void"></decorations>
		/// <param name="Comparer"></param>
		public void Sort(IComparer<T> Comparer)
		{
			Items.Sort(Comparer);

			Remap();
		}

		/// <summary>Reverses Items.</summary>
		/// <decorations decor="public void"></decorations>
		public void Reverse()
		{
			Items.Reverse();

			Remap();
		}

		/// <summary>Reverses Items from Index for Count.</summary>
		/// <decorations decor="public void"></decorations>
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

		/// <summary>Every Item in this MArray represented as a <see cref="string"/> with <see cref="object.ToString"/>.</summary>
		/// <docs>Every Item in this MArray represented as a string with object.ToString().</docs>
		/// <remarks>Assumes that T has a readable ToString() defined.</remarks>
		/// <decorations decor="public string"></decorations>
		/// <param name="bWithIndex">Should the resulting string include the index of every item?</param>
		/// <returns>A string form of all T.</returns>
		public string Print(bool bWithIndex = false)
		{
			StringBuilder SB = new();

			if (bWithIndex)
			{
				for (int i = 0; i < Num; ++i)
				{
					SB.Append(i + ": " + Items[i].ToString());
					if (i != Num - 1)
						SB.Append(", ");
				}
			}
			else
			{
				for (int i = 0; i < Num; ++i)
				{
					SB.Append(Items[i].ToString());
					if (i != Num - 1)
						SB.Append(", ");
				}
			}

			return SB.ToString();
		}

		/// <summary>Converts this MArray into T[].</summary>
		/// <decorations decor="public T[]"></decorations>
		/// <returns>T[].</returns>
		public T[] TArray()
		{
			return Items.ToArray();
		}

		/// <summary>Square bracket accessor.</summary>
		/// <decorations decor="public T"></decorations>
		/// <param name="i">The index to access T item.</param>
		/// <returns>The Item at the specified index.</returns>
		public T this[int i] => Items[i];

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
		/// <decorations decor="public static bool operator!"></decorations>
		/// <param name="CheckIfNull">The MArray to check whether it is uninitialised.</param>
		/// <returns>True if CheckIfNull is null.</returns>
		public static bool operator !(MArray<T> CheckIfNull) => CheckNull(CheckIfNull);

		/// <summary>Adds Right to the end of Left.</summary>
		/// <decorations decor="public static MArray{T} operator+"></decorations>
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

			foreach (T T in Left)
				NewMArray.Push(T);
			foreach (T T in Right)
				NewMArray.Push(T);

			NewMArray.Remap();

			return NewMArray;
		}

		/// <summary>Left elements that exist in Right.</summary>
		/// <decorations decor="public static MArray{T} operator&amp;"></decorations>
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

			AndMArray.Remap();

			return AndMArray;
		}

		/// <summary>Left elements that do not exist in Right.</summary>
		/// <decorations decor="public static MArray{T} operator^"></decorations>
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

			OrMArray.Remap();

			return OrMArray;
		}

		public static implicit operator int(MArray<T> Any) => Any.Num;
		public static implicit operator List<T>(MArray<T> Any) => Any.Items;
		public static implicit operator T[](MArray<T> Any) => Any.TArray();

		/// <summary>The incoming and reflected Item of this mirror over the provided Minimum and Maximum Num.</summary>
		/// <decorations decor="public struct"></decorations>
		public struct Reflected
		{
			/// <summary>In reflection.</summary>
			/// <decorations decor="public T"></decorations>
			public T Source;
			/// <summary>Out reflection.</summary>
			/// <decorations decor="public T"></decorations>
			public T Reflection;

			internal Reflected(T Source, T Reflection)
			{
				this.Source = Source;
				this.Reflection = Reflection;
			}
		}
	}

	/// <summary>The base class for an MArray.</summary>
	/// <decorations decor="[Serializable] public class"></decorations>
	[Serializable]
	public class MArray
	{
		/// <summary>Definition of an invalid position or illegal number associated with an MArray.</summary>
		public const int kInvalid = -1;

		/// <decorations decor="public static bool"></decorations>
		/// <param name="Check">The MArray to check for initialisation.</param>
		/// <returns>True if Check is null.</returns>
		public static bool CheckNull(MArray Check) => Check == null;

		/// <summary>Data of an Item in an MArray.</summary>
		/// <decorations decor="public struct"></decorations>
		public struct AccessedData
		{
			static readonly AccessedData none = new AccessedData(kInvalid, Array.Empty<int>());
			/// <summary>No data available.</summary>
			/// <decorations decor="public static readonly AccessedData"></decorations>
			public static readonly AccessedData None = none;

			/// <summary>The number of times an Item appears in an MArray.</summary>
			/// <decorations decor="public int"></decorations>
			public int Occurrences;
			/// <summary>The index locations of this Item in an MArray.</summary>
			/// <decorations decor="public int[]"></decorations>
			public int[] Positions;

			internal AccessedData(int Occurrences, int[] Positions)
			{
				this.Occurrences = Occurrences;
				this.Positions = Positions;
			}

			/// <summary>Data containing whether or not an accessed item has data associated with it.</summary>
			/// <docreturns>Whether or not an accessed item exists within an MArray{T}.</docreturns>
			/// <returns><see langword="true"/> if the accessed item does not exist in this <see cref="MArray{T}"/>.</returns>
			public bool IsNone() => Occurrences == kInvalid && Positions.Length == 0;
		}
	}
}
