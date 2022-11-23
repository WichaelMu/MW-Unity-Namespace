using System;
using System.Collections.Generic;

namespace MW.Pathfinding
{
	/// <summary>Computes a number of paths over a number of frames.</summary>
	/// <typeparam name="T">Generic type that implements INode and IHeapItem for T that defines a traversable waypoint.</typeparam>
	/// <decorations decor="public static class {T} where T : INode{T}, IHeapItem{T}"></decorations>
	public static class PathRegister<T> where T : MNode, IHeapItem<T>
	{
		static Queue<PathRequest> PathsToCompute = new();
		static bool bIsCurrentlyComputingPath;

		/// <summary>Register a path to compute when possible.</summary>
		/// <remarks>This is on a first-in, first-out basis. A Queue.</remarks>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Origin">T position to begin pathfinding.</param>
		/// <param name="Destination">T position to pathfind to.</param>
		/// <param name="OnPathCalculated">What to do when a path is found? List of T pathway.</param>
		/// <param name="OnPathFailed">What to do when a path cannot be found? List of T pathway attempt.</param>
		public static void RequestPath(T Origin, T Destination, Action<List<T>> OnPathCalculated, Action<List<T>> OnPathFailed = null)
		{
			PathRequest NewPath = new(Origin, Destination, OnPathCalculated, OnPathFailed);
			PathsToCompute.Enqueue(NewPath);
		}

		/// <summary>Computes the next path in FIFO.</summary>
		/// <decorations decor="public static bool"></decorations>
		/// <returns>Whether or not a computation was executed.</returns>
		public static bool ComputeNext()
		{
			bool bHasComputedPath = false;

			if (!bIsCurrentlyComputingPath && PathsToCompute.Count > 0)
			{
				bIsCurrentlyComputingPath = true;

				PathRequest CurrentComputation = PathsToCompute.Dequeue();
				bool bPathFound = Pathfinding<T>.AStar(CurrentComputation.Origin, CurrentComputation.Destination, out MArray<T> Path);

				if (bPathFound)
				{
					CurrentComputation.OnPathCalculated?.Invoke(Path);
				}
				else
				{
					CurrentComputation.OnPathFailed?.Invoke(Path);
				}

				bHasComputedPath = true;
			}

			bIsCurrentlyComputingPath = false;

			return bHasComputedPath;
		}

		/// <summary>Computes BatchSize paths in a single call.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="BatchSize">The number of paths to compute.</param>
		public static void ComputeBatch(uint BatchSize)
		{
			for (uint i = 0; i < BatchSize; ++i)
			{
				if (!ComputeNext())
					break;
			}
		}

		/// <summary>Gets the number of agents waiting to compute paths.</summary>
		/// <decorations decor="public static uint"></decorations>
		/// <returns>Unsigned integer number of T's awaiting a path.</returns>
		public static uint GetPathQueueSize()
		{
			return Convert.ToUInt32(PathsToCompute.Count);
		}

		// Information storing a request for a path.
		// Uses default Pathfinding values.
		struct PathRequest
		{
			public T Origin;
			public T Destination;
			public Action<List<T>> OnPathCalculated;
			public Action<List<T>> OnPathFailed;

			public PathRequest(T Origin, T Destination, Action<List<T>> OnPathCalculated, Action<List<T>> OnPathFailed)
			{
				this.Origin = Origin;
				this.Destination = Destination;
				this.OnPathCalculated = OnPathCalculated;
				this.OnPathFailed = OnPathFailed;
			}
		}
	}
}
