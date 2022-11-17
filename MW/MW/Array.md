# MW.MArray
This is a dynamic array which combines the functionality of a List and a Dictionary.

## Example
```cs
using MW;

// Initialise a new MArray.
MArray<float> Array = new MArray<float>();

Array.Push(1.2f, 1.4f, 1.6f, 1.8f, 2.0f, 1.8f);
int Num = Array.Num; // 6.
float Second = Array[1]; // 1.4f.
bool bContains = Array.Contains(1.8f); // True.

Array.Pull(1.8f); // Removes latest addition of 1.8f.
string Print = Array.Print(bWithIndex: false);
// Print = "1.2, 1.4, 1.6, 1.8, 2.0";
```

MArray also provides shorthand operations:
```cs
// Continuing off the above sample...

MArray<float> Array2 = new MArray<float>();
Array2.Push(1.2f, -1f, -2f, -4f, -8f);

Array += Array2;
Print = Array.Print();
// Print = "1.2, 1.4, 1.6, 1.8, 2.0, 1.2, -1, -2, -4, -8";
int Num2 = Array2.Num; // 4.

MArray XORArray = Array ^ Array2;
Print = XORArray.Print(); // Array's elements that do not exist in Array2.
// Print = "1.4, 1.6, 1.8, 2". 1.2f doesn't exist in XORArray.

// Foreach support.
foreach (var M in Array)
	// ...

// List API.
Array.Sort();
Array.Reverse();

// Dictionary API.
bContains = Array.Contains(4.2f) // False.

// Deque API.
float First = Array.FirstPop(); // 1.2f
First = Array.First(); // 1.4f. No popping.

float Top = Array.TopPop() // -8f
Top = Array.Top(); // -4. No popping.

Array.PushUnique(-4f, -8f, -16f); // Only pushes -16f.
Print = Array.Print();
// Print = "1.2, 1.4, 1.6, 1.8, 2.0, 1.2, -1, -2, -4, -8, -16";
```
