

namespace MW
{

	/// <summary>Generates a new pair of two types of values.</summary>
	/// <typeparam name="TFirst">The type of the first variable to store.</typeparam>
	/// <typeparam name="TSecond">The type of the second variable to store.</typeparam>
	public struct TPair<TFirst, TSecond>
	{
		public TFirst First { get; set; }
		public TSecond Second { get; set; }

		public TPair(TFirst First, TSecond Second)
		{
			this.First = First;
			this.Second = Second;
		}

		public override int GetHashCode() => System.HashCode.Combine<TFirst, TSecond>(First, Second);
	}

	/// <summary>Generates a new variable of three types of values.</summary>
	/// <typeparam name="TFirst">The type of the first variable to store.</typeparam>
	/// <typeparam name="TSecond">The type of the second variable to store.</typeparam>
	/// <typeparam name="TThird">The type of the third variable to store.</typeparam>
	public struct TTriple<TFirst, TSecond, TThird>
	{
		public TFirst First { get; set; }
		public TSecond Second { get; set; }
		public TThird Third { get; set; }

		public TTriple(TFirst First, TSecond Second, TThird Third)
		{
			this.First = First;
			this.Second = Second;
			this.Third = Third;
		}

		public override int GetHashCode() => System.HashCode.Combine<TFirst, TSecond, TThird>(First, Second, Third);
	}
}