using System;

namespace MW.Pathfinding
{
	/// <summary>The Interface that T must implement if it is to be used by <see cref="Pathfinding{T}"/>.</summary>
	/// <docs>The Interface that T must implement if it is to be used by Pathfinding.</docs>
	/// <typeparam name="T">The type to declare a node.</typeparam>
	/// <decorations decor="public interface {T} where T : IComparable{T}"></decorations>
	public interface INode<T> where T : IComparable<T>
	{
		/// <summary>This Node's F score.</summary>
		/// <decorations decor="float"></decorations>
		float F { get; set; }
		/// <summary>This Node's G score.</summary>
		/// <decorations decor="float"></decorations>
		float G { get; set; }
		/// <summary>This Node's H score.</summary>
		/// <decorations decor="float"></decorations>
		float H { get; set; }

		T Parent { get; set; }

		/// <summary>How many directions can this Node point to?</summary>
		/// <decorations decor="uint"></decorations>
		int NumberOfDirections();

		/// <summary>Is this block traversable?</summary>
		/// <decorations decor="bool"></decorations>
		bool IsTraversable();
		/// <summary>Get the Neighbouring Node at Direction.</summary>
		/// <decorations decor="INode{T}"></decorations>
		/// <param name="Direction">The neighbour of this Node in this direction.</param>
		INode<T> Neighbour(int Direction);
		/// <summary>The distance heuristic to calculate pathfinding scores.</summary>
		/// <decorations decor="float"></decorations>
		/// <param name="RelativeTo">Distance to from this T to Relative To.</param>
		/// <returns>An indicative distance from this T, Relative To.</returns>
		float DistanceHeuristic(T RelativeTo);
	}
}
