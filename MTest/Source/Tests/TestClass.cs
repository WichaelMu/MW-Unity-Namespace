using MW;

namespace MTest
{
	class TTestClass : IHeapItem<TTestClass>
	{
		public int Value;

		int Index;

		public TTestClass(int Value)
		{
			this.Value = Value;
			Instance = this;
		}

		public int HeapItemIndex { get => Index; set => Index = value; }
		public TTestClass Element { get => Instance; set => Instance = value; }
		public TTestClass Instance;

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

		public override int GetHashCode()
		{
			return Value;
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public static implicit operator int(TTestClass TTC) => TTC.Value;
	}

	class ClassA
	{
		public char Char;
		public int Integer;
		public float Float;

		public ClassA(char Char, int Integer, float Float)
		{
			this.Char = Char;
			this.Integer = Integer;
			this.Float = Float;
		}
	}

	class ClassB : ClassA
	{
		public string String;
		public long Long;
		public double Double;

		public ClassB(char Char, int Integer, float Float) : base(Char, Integer, Float) { String = string.Empty; }

		public ClassB(string String, long Long, double Double) : base(String[0], (int)Long, (float)Double)
		{
			this.String = String;
			this.Long = Long;
			this.Double = Double;
		}
	}
}
