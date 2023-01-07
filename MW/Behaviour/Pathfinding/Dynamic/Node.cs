﻿using UnityEngine;
using static MW.Math.Magic.Fast;

namespace MW.Pathfinding
{
	/// <summary>A sample Node class for <see cref="Pathfinding{T}.AStar(T, T, out MArray{T}, uint, uint, bool)"/>.</summary>
	/// <docs>A sample Node class for Pathfinding.</docs>
	/// <decorations decor="public class Node : MNode, IHeapItem{Node}"></decorations>
	public class Node : MNode, IHeapItem<Node>
	{
		public Vector3 Position;

		public int CompareTo(Node Other)
		{
			return base.CompareTo(Other);
		}

		/// <summary>Registers N as a neighbour, and this as N's neighbour.</summary>
		/// <param name="N">Neighbour.</param>
		public void Push(Node N)
		{
			if (N.bIsTraversable)
			{
				Neighbours.PushUnique(N);
				N.Neighbours.PushUnique(this);
			}
		}

		public override float DistanceHeuristic(MNode RelativeTo)
		{
			return FSqrt((Position - ((Node)RelativeTo).Position).sqrMagnitude);
		}

		public override bool IsTraversable()
		{
			return bIsTraversable;
		}

		public override int GetHashCode()
		{
			return Position.GetHashCode();
		}
	}
}