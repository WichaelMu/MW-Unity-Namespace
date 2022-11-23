# MW.THeap
This is a data structure for minimum or maximum heaps of a generic type. The type must implement
```cs
public interface IHeapItem<T> : IComparable<T>
```
for it to be recognised by the Heap. This means that primitives types are not supported - a specific class needs to be created for them. There are other data structures that do this, such as:
```cs
using System.Collections.Generic;

List<int> List = new List<int>();
List.Sort();
```

## Example
```cs
using MW;

// Initialise a new minimum Heap with 3 items.
THeap<MyClass> Heap = new THeap(3);
			
TTestClass Item4 = new TTestClass(4);
TTestClass ItemN1 = new TTestClass(-1);
TTestClass Item8 = new TTestClass(8);
Heap1.Add(Item4);
Heap1.Add(ItemN1);
Heap1.Add(Item8);

int Num = Heap.Num; // 3
int Minimum = Heap.RemoveFirst().Value; // -1

Num = Heap.Num; // 2
Minimum = Heap.RemoveFirst().Value; // 4.

bool bContains = Heap.Contains(Item4); // False.
bContains = Heap.Contains(ItemN1); // False.
bContains = Heap.Contains(Item8); // True.

```

Where MyClass is defined:
```cs
class MyClass : IHeapItem<MyClass>
{
	public int Value;

	public int HeapItemIndex { get => Index; set => Index = value; }
	int Index;

	public MyClass(int Value) { this.Value = Value; }

	// Minimum Heap.
	public int CompareTo(TTestClass? Other)
	{
		if (Other != null)
		{
			if (Value < Other.Value)
				return 1;
			if (Value > Other.Value)
				return -1;
		}

		return 0;
	}
}
```