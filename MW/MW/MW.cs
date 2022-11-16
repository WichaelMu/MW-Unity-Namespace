namespace MW
{

	/// <summary>Generates a new pair of two types of values.</summary>
	/// <typeparam name="TFirst">The type of the first variable to store.</typeparam>
	/// <typeparam name="TSecond">The type of the second variable to store.</typeparam>
	/// <decorations decor="public struct"></decorations>
	public struct TPair<TFirst, TSecond>
	{
		/// <summary>The first element in this pair.</summary>
		/// <decorations decor="public TFirst"></decorations>
		public TFirst First { get; set; }
		/// <summary>The second element in this pair.</summary>
		/// <decorations decor="public TSecond"></decorations>
		public TSecond Second { get; set; }

		/// <summary>Constructs a Pair with two generics.</summary>
		/// <param name="First"></param>
		/// <param name="Second"></param>
		public TPair(TFirst First, TSecond Second)
		{
			this.First = First;
			this.Second = Second;
		}

		/// <summary>A combined Hash Code with First and Second.</summary>
		/// <decorations decor="public override int"></decorations>
		/// <returns>HashCode.</returns>
		public override int GetHashCode()
		{
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
	/// <decorations decor="public struct"></decorations>
	public struct TTriple<TFirst, TSecond, TThird>
	{
		/// <summary>The first element in this pair.</summary>
		/// <decorations decor="public TFirst"></decorations>
		public TFirst First { get; set; }
		/// <summary>The second element in this pair.</summary>
		/// <decorations decor="public TSecond"></decorations>
		public TSecond Second { get; set; }
		/// <summary>The third element in this pair.</summary>
		/// <decorations decor="public TThird"></decorations>
		public TThird Third { get; set; }

		/// <summary>Constructs a Triple with three generics.</summary>
		/// <param name="First"></param>
		/// <param name="Second"></param>
		/// <param name="Third"></param>
		public TTriple(TFirst First, TSecond Second, TThird Third)
		{
			this.First = First;
			this.Second = Second;
			this.Third = Third;
		}

		/// <summary>A combined Hash Code with First, Second and Third.</summary>
		/// <decorations decor="public override int"></decorations>
		/// <returns>HashCode.</returns>
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