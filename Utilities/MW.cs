

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

		public override int GetHashCode() {
			int hash = 17;

			hash = hash * 31 + First.GetHashCode();
			hash = hash * 31 + Second.GetHashCode();

			return hash;
		}
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

		public override int GetHashCode()
		{
			int hash = 17;

			hash = hash * 31 + First.GetHashCode();
			hash = hash * 31 + Second.GetHashCode();
			hash = hash * 31 + Third.GetHashCode();

			return hash;
		}
	}
}