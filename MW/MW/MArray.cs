﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MW
{
	/// <summary>A dynamic generic array combining the functionality of a List and a Dictionary.</summary>
	/// <typeparam name="T">The generic type.</typeparam>
	/// <decorations decor="public sealed class {T} : MArray, IEnumerable{T}"></decorations>
	[Serializable]
	public sealed class MArray<T> : MArray, IEnumerable<T>
	{
#if RELEASE
		[UnityEngine.SerializeField]
#endif
		List<T> Items;

		Dictionary<T, MDeque<int>> HashMap;

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

		public MArray(T[] Array) : this(Array.Length)
		{
			Items = new List<T>(Array);
			Remap();
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

			HashMap[Item].AddEnd(Num);

			Items.Add(Item);
		}

		/// <summary>Adds a number of Items.</summary>
		/// <decorations decor="public void"></decorations>
		/// <param name="Items">The list of elements to add.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Push(params T[] Items)
		{
			foreach (T T in Items)
				Push(T);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushUnique(T Item)
		{
			if (!Contains(Item))
				Push(Item);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushUnique(params T[] Items)
		{
			foreach (T T in Items)
				PushUnique(T);
		}

		/// <summary>Removes the most recent push of Item.</summary>
		/// <decorations decor="public int"></decorations>
		/// <remarks>If you plan to Pull every element from the MArray, see PullAll(T).</remarks>
		/// <param name="Item">The element to remove.</param>
		/// <docreturns>The new size of this MArray, or kInvalid if Item doesn't exist.</docreturns>
		/// <returns>The new size of this MArray, or <see cref="MArray.kInvalid"/> if Item doesn't exist.</returns>
		public int Pull(T Item)
		{
			if (!Contains(Item))
				return kInvalid;

			int NewNum = PullWithoutRemap(Item);
			Remap();

			return NewNum;
		}

		/// <summary>Removes all occurrences of Item.</summary>
		/// <decorations decor="public int"></decorations>
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
				PullWithoutRemap(Item);

			Remap();

			return Num;
		}

		/// <summary>Conducts a single Pull operation on every T item.</summary>
		/// <decorations decor="public int"></decorations>
		/// <param name="Items">The most recent push of each Item to pull.</param>
		/// <docreturns>The new size of this MArray, or kInvalid if Item doesn't exist. Returns Num if Length of Items is 0.</docreturns>
		/// <returns>The new size of this MArray, or <see cref="MArray.kInvalid"/> if Item doesn't exist. Returns <see cref="Num"/> if Length of Items is 0.</returns>
		public int Pull(params T[] Items)
		{
			if (Items.Length == 0)
				return Num;
			if (Items.Length == 1)
				return Pull(Items[0]);

			// Heap = O(x log(x))
			// x = x * log(x)
			// x = 10
			if (Items.Length <= 10)
			{
				foreach (T Item in Items)
					Pull(Item);
			}
			else
			{
				int[] IndicesToRemove = new int[Items.Length];
				int i = 0;

				foreach (T Item in Items)
					if (Contains(Item))
						IndicesToRemove[i++] = HashMap[Item].End();

				PullMultiIndex(IndicesToRemove);
			}

			return Num;
		}

		/// <summary>Pulls an Item from an Index.</summary>
		/// <decorations decor="public T"></decorations>
		/// <param name="Index">The Index to pull from.</param>
		/// <returns>The pulled T from Index.</returns>
		public T PullAtIndex(int Index)
		{
			if (Index >= Num || Index < 0)
				throw new IndexOutOfRangeException($"Index out of range! Expected Index < {Num} && Index >= 0. Index: {Index}");

			T ItemAtIndex = PullWithoutRemap(Index);
			Remap();

			return ItemAtIndex;
		}

		/// <summary>Pull from multiple indices.</summary>
		/// <decorations decor="public MArray&lt;T&gt;"></decorations>
		/// <param name="Indices">The index positions to remove.</param>
		/// <returns>An MArray of the removed elements.</returns>
		/// <exception cref="IndexOutOfRangeException">If one of Indices are out of range.</exception>
		public void PullMultiIndex(params int[] Indices)
		{
			if (Indices.Length == 0)
				return;

			if (Indices.Length == 1)
			{
				PullAtIndex(Indices[0]);
				return;
			}

			// Descending order.
			MHeap<int> MinHeap = MHeap<int>.Heapify(Indices, (L, R) => L == R ? 0 : L > R ? 1 : -1);

			while (MinHeap.Num != 0)
			{
				int Index = MinHeap.RemoveFirst();
				if (Index >= Num || Index < 0)
					throw new IndexOutOfRangeException($"Index out of range! Expected Index >= 0 && Index < {Num}. Index: {Index}");

				PullWithoutRemap(Index);
			}

			Remap();
		}

		int PullWithoutRemap(T Item)
		{
			Items.RemoveAt(HashMap[Item].PopEnd());

			if (HashMap[Item].Num == 0)
			{
				HashMap.Remove(Item);
			}

			return Num;
		}

		T PullWithoutRemap(int Index)
		{
			T ItemAtIndex = Items[Index];
			Items.RemoveAt(Index);

			return ItemAtIndex;
		}

		/// <summary>Copies Num elements from an MArray to another MArray from Start.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Destination">The MArray that will be copied *into*.</param>
		/// <param name="Source">The MArray that will be copied *from*.</param>
		/// <param name="Start">The index from Source to begin copying into Destination.</param>
		/// <param name="Num">The number of elements to copy into Destination.</param>
		public static void Copy(MArray<T> Destination, MArray<T> Source, int Start, int Num)
		{
			if (Start < 0 || Start >= Source.Num)
				throw new IndexOutOfRangeException($"Expected Start ({Start}) to be >= 0 && < Source.Num ({Source.Num}).");

			if (CheckNull(Destination))
				Destination = new MArray<T>();

			FMath.Clamp(ref Num, 0, Source.Num - 1);

			if (Num > 0)
				Destination.Push(Source.Items.GetRange(Start, Num).ToArray());
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
				Occurrences = HashMap[Item].Num,
				Positions = HashMap[Item].TArray()
			};

			return Data;
		}

		int IndexOf(T Item, ESearchDirection SearchDirection = ESearchDirection.LatestPush)
		{
			if (!Contains(Item))
				return kInvalid;

			if (SearchDirection == ESearchDirection.LatestPush)
				return HashMap[Item].End();

			AccessedData Data = Access(Item);
			return Data.Positions[Data.Positions.Length - 1];
		}

		/// <summary>The index of the first occurrence of Item; index of Item's <see cref="ESearchDirection.EarliestPush"/>.</summary>
		/// <docs>The index of the first occurrence of Item; index of Item's Earliest Push.</docs>
		/// <decorations decor="public int"></decorations>
		/// <param name="Item">The Item to search for.</param>
		/// <docreturns>Zero-indexed position, if found. Otherwise, kInvalid.</docreturns>
		/// <returns>Zero-indexed position, if found. Otherwise, <see cref="MArray.kInvalid"/>.</returns>
		public int FirstIndexOf(T Item) => IndexOf(Item, ESearchDirection.EarliestPush);
		/// <summary>The index of the last occurrence of Item; index of Item's <see cref="ESearchDirection.LatestPush"/>.</summary>
		/// <docs>The index of the last occurrence of Item; index of Item's Latest Push.</docs>
		/// <decorations decor="public int"></decorations>
		/// <param name="Item">The Item to search for.</param>
		/// <docreturns>Zero-indexed position, if found. Otherwise, kInvalid.</docreturns>
		/// <returns>Zero-indexed position, if found. Otherwise, <see cref="MArray.kInvalid"/>.</returns>
		public int LastIndexOf(T Item) => IndexOf(Item, ESearchDirection.LatestPush);

		/// <decorations decor="public T"></decorations>
		/// <returns>The item at the front of the queue.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T First()
		{
			return Items[0];
		}

		/// <summary>Pops the item at the front of the queue.</summary>
		/// <decorations decor="public T"></decorations>
		/// <returns>The item that was at the front of the queue.</returns>
		public T FirstPop()
		{
			if (Num <= 0)
				throw new IndexOutOfRangeException($"Cannot {nameof(FirstPop)} when MArray is Empty!");

			T T = First();
			PullAtIndex(0);

			return T;
		}

		/// <decorations decor="public T"></decorations>
		/// <returns>The item at the top of the stack.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
		/// <returns>If this MArray is not empty Index is greater than or equal to zero and less than the number of elements.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool InRange(int Index)
		{
			return !IsEmpty() && Index >= 0 && Index < Num;
		}

		/// <decorations decor="public bool"></decorations>
		/// <param name="Item">Item to check for existence.</param>
		/// <returns>Whether the HashCode of Item exists within the internal HashMap.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(T Item)
		{
			return HashMap.ContainsKey(Item);
		}

		/// <summary>Clears this MArray.</summary>
		/// <decorations decor="public void"></decorations>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Flush()
		{
			Items.Clear();
			HashMap.Clear();
		}

		/// <decorations decor="public bool"></decorations>
		/// <docreturns>If this MArray is considered empty; Num == 0.</docreturns>
		/// <returns><see langword="true"/> <see langword="if"/> (<see cref="Num"/> == 0).</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
					HashMap.Add(Items[i], new());

				HashMap[Items[i]].AddEnd(i);
			}
		}

		/// <summary>Every Item in this MArray represented as a <see cref="string"/> with <see cref="object.ToString"/>.</summary>
		/// <docs>Every Item in this MArray represented as a string with object.ToString().</docs>
		/// <remarks>Assumes that T has a readable ToString() defined.</remarks>
		/// <decorations decor="public string"></decorations>
		/// <param name="bWithIndex">Should the resulting string include the index of every item?</param>
		/// <param name="Separator">The string that will separate each element.</param>
		/// <returns>A string form of all T.</returns>
		public string Print(bool bWithIndex = false, string Separator = "")
		{
			StringBuilder SB = new();

			if (bWithIndex)
			{
				for (int i = 0; i < Num; ++i)
				{
					SB.Append($"{i}: {Items[i]}");
					if (i != Num - 1)
						SB.Append(Separator);
				}
			}
			else
			{
				for (int i = 0; i < Num; ++i)
				{
					SB.Append(Items[i].ToString());
					if (i != Num - 1)
						SB.Append(Separator);
				}
			}

			return SB.ToString();
		}

		/// <summary>This MArray as T[].</summary>
		/// <decorations decor="public T[]"></decorations>
		/// <returns>T[].</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] TArray()
		{
			return Items.ToArray();
		}

		/// <summary>This MArray as List&lt;T&gt;</summary>
		/// <decorations decor="public List&lt;T&gt;"></decorations>
		/// <returns>List&lt;T&gt;</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public List<T> LArray()
		{
			return Items;
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
		public static implicit operator List<T>(MArray<T> Any) => Any.LArray();
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
	/// <decorations decor="public class"></decorations>
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
			/// <remarks>The array order from left to right is Latest Push to Earliest Push.</remarks>
			/// <decorations decor="public int[]"></decorations>
			public int[] Positions;

			internal AccessedData(int Occurrences, int[] Positions)
			{
				this.Occurrences = Occurrences;
				this.Positions = Positions;
			}

			/// <summary>Data containing whether or not an accessed item has data associated with it.</summary>
			/// <decorations decor="public bool"></decorations>
			/// <docreturns>Whether or not an accessed item exists within an MArray{T}.</docreturns>
			/// <returns><see langword="true"/> if the accessed item does not exist in this <see cref="MArray{T}"/>.</returns>
			public bool IsNone() => Occurrences == kInvalid && Positions.Length == 0;
		}
	}

	internal enum ESearchDirection : byte
	{
		EarliestPush,
		LatestPush
	}
}
