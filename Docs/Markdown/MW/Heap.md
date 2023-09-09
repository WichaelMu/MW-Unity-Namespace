# Heaps in MW

There are two types of Heaps in the MW Namespace:
1. THeap&lt;T, R&gt;
1. MHeap&lt;T&gt;

Both these Heaps are dynamically sized and have user-configured behaviour for minimum or maximum heaps.

The inheritance hierarchy is:
```
MContainer<T>
	-> THeap<T, R> where T : IHeapItem<R>
		-> MHeap<T>
```

THeap provides the raw implementation of the MW Heap data structure, while MHeap provides the user-
friendly API. To use MW Heaps, instantiate MHeap, instead of THeap.

## MW.MHeap&lt;T&gt;

This class provides Heap functionality to any generic type `T`. Unlike `THeap<T, R>`, this means
that primitives can be used in the Heap.

There are no generic type constraints when using `MHeap<T>`. All you need to provide is a
function compatible with the `SortFunc<T>` delegate which dictates the Heap order.
* `SortFunc`s compare two values and returns 1 if the left `T` should take a higher precendence
than the right `T`. Return -1 for the opposite effect. Returning 0 indicates equality.

### Example
```cs
using MW;

// Malloc a new minimum Heap with a size of 3.
MHeap<int> Heap = new MHeap<int>(3,
	(L, R) => L == R ? 0 : L < R ? 1 : -1
);

// Add elements.
Heap.Push(4);
Heap.Push(2);
Heap.Push(3);

int Num = Heap.Num; // 3 elements in Heap.
int Minimum = Heap.RemoveFirst(); // 2 is the smallest value (defined in SortFunc<int>).

Num = Heap.Num; // 2 elements in Heap after RemoveFirst().
Minimum = Heap.RemoveFirst(); // 3 is the smallest value after RemoveFirst().

bool bContains = Heap.Contains(4); // True.
bContains = Heap.Contains(2); // False.
bContains = Heap.Contains(3); // False.

```

## MW.THeap&lt;T, R&gt;

This class provides the core logic for minimum or maximum heaps of a generic type. The type must implement:
```cs
public interface IHeapItem<T>
```
for it to be compatible with the Heap. This means that primitives types are not natively supported
by THeap - a specific class needs to be created for them; this is `MHeap<T>`.

Using `THeap<T, R>` is not recommended. This class is only meant to be a wrapper class and may
be too complex and/or induce cluttered source code. Instead, use `MHeap<T>`; it inherits from
`THeap<T, R>`; at its core.

### Example
```cs
using MW;

// Malloc a new minimum Heap with a size of 3.
THeap<MyClass, MyClass> Heap = new THeap<MyClass, MyClass>(3,
	(L, R) => L == R ? 0 : L < R ? 1 : -1
);

// Instantiate and add elements.
TTestClass Item4 = new TTestClass(4);
TTestClass ItemN1 = new TTestClass(-1);
TTestClass Item8 = new TTestClass(8);
Heap1.Add(Item4);
Heap1.Add(ItemN1);
Heap1.Add(Item8);

int Num = Heap.Num; // 3 elements in Heap.
int Minimum = Heap.RemoveFirst().Value; // -1 is the smallest value (defined by SortFunc<int> or MyClass.CompareTo()).

Num = Heap.Num; // 2 elements in Heap after RemoveFirst().
Minimum = Heap.RemoveFirst().Value; // 4 is the smallest value after RemoveFirst().

bool bContains = Heap.Contains(Item4); // False.
bContains = Heap.Contains(ItemN1); // False.
bContains = Heap.Contains(Item8); // True.

```

Where MyClass is defined:
```cs
class MyClass : IHeapItem<MyClass>
{
	public int Value;

	// Implement HeapItemIndex from IHeapItem<MyClass>.
	public int HeapItemIndex { get => Index; set => Index = value; }
	int Index;

	// Implement Element from IHeapItem<MyClass>.
	public TTestClass Element { get => Instance; set => Instance = value; }
	public TTestClass Instance;`

	public MyClass(int Value) { this.Value = Value; }

	// Minimum Heap.
	public int CompareTo(TTestClass? Other)
	{
		if (Other != null)
		{
			// Min Heap means smaller values take higher precendence.
			if (Value < Other.Value)
				return 1;
			if (Value > Other.Value)
				return -1;
		}

		// Return 0 if null or equal.
		return 0;
	}
}
```

This example simply Heapified an integer to be used with `THeap<T, R>` and required the definition
of a new class. Using `THeap<T, R>` directly is unwise. Use `MHeap<T>` instead.