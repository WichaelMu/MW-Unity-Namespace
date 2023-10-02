namespace MW.Pathfinding
{
	/// <summary>Base class for Nodes to be use in A* Pathfinding.</summary>
	/// <decorations decor="public abstract class : INode{MNode}"></decorations>
	public abstract class MNode : INode<MNode>
	{
		/// <summary>Is this Node an obstacle for Pathfinding?</summary>
		public bool bIsTraversable;
		/// <summary>The connected Nodes to this Node.</summary>
		public MArray<MNode> Neighbours;

		#region Pathfinding Interface

		public float F { get => f; set => f = value; }
		public float G { get => g; set => g = value; }
		public float H { get => h; set => h = value; }
		public MNode Parent { get => parent; set => parent = value; }
		public int HeapItemIndex { get => HeapIndex; set => HeapIndex = value; }

		float f, g, h;
		MNode parent;
		int HeapIndex;

		#endregion;

		public MNode()
		{
			f = g = h = 0f;
			HeapIndex = 0;
			parent = null;

			Neighbours = new MArray<MNode>(6);
			bIsTraversable = true;
		}

		public virtual int CompareTo(MNode Other)
		{
			int Compare = F.CompareTo(Other.F);

			if (Compare == 0)
				Compare = H.CompareTo(Other.H);

			return -Compare;
		}

		/// <summary>The distance heuristic to calculate pathfinding scores.</summary>
		/// <decorations decor="public abstract float"></decorations>
		/// <param name="RelativeTo">Distance to from this T to Relative To.</param>
		/// <returns>An indicative distance from this T, Relative To.</returns>
		public abstract float DistanceHeuristic(MNode RelativeTo);

		/// <summary>Is this block traversable?</summary>
		/// <decorations decor="public abstract bool"></decorations>
		public abstract bool IsTraversable();

		/// <summary>Get the Neighbouring Node at Direction.</summary>
		/// <decorations decor="INode{MNode}"></decorations>
		/// <param name="Direction">The neighbour of this Node in this direction.</param>
		public virtual INode<MNode> Neighbour(int Direction)
		{
			if (Direction < Neighbours.Num)
				return Neighbours[Direction];
			return null;
		}

		/// <summary>How many directions can this Node point to?</summary>
		/// <decorations decor="public virtual int"></decorations>
		public virtual int NumberOfDirections() => Neighbours.Num;

		/// <summary>Is MNode M null?</summary>
		/// <decorations decor="public static implicit operator bool"></decorations>
		/// <param name="M"></param>
		public static implicit operator bool(MNode M) => M != null;
	}
}
