using System;
using System.Collections.Generic;
using MW.Diagnostics;
using UnityEngine;

namespace MW.Pathfinding
{
	/// <summary>Provides the A* Pathfinding implementation for T.</summary>
	/// <typeparam name="T">Generic type that implements INode and IHeapItem for T that defines a traversable waypoint.</typeparam>
	/// <decorations decor="public static class {T} where T : INode{T}, IHeapItem{T}"></decorations>
	public static class Pathfinding<T> where T : MNode, IHeapItem<T>
	{
		/// <summary>A* pathfinds from Origin to Destination looking uDepth times within a uMapSize.</summary>
		/// <decorations decor="public static bool"></decorations>
		/// <param name="Origin">T position to begin pathfinding.</param>
		/// <param name="Destination">T position to pathfind to.</param>
		/// <param name="Path">Reference T List of that make up the path from Origin to Destination.</param>
		/// <param name="Depth">The depth to search to.</param>
		/// <param name="MapSize">The total size of the map to be traversed. (The number of INodes).</param>
		/// <param name="bUseDiagnostics">Time the duration of Pathfinding?</param>
		/// <returns>Whether or not a path was found from Origin to Destination within uDepth in uMapSize.</returns>
		public static bool AStar(T Origin, T Destination, out MArray<T> Path, uint Depth = int.MaxValue, uint MapSize = 10000, bool bUseDiagnostics = false)
		{
			Stopwatch sw = new(bUseDiagnostics);

			Path = new();
			bool bFoundPath = false;

			THeap<T> Open = new(MapSize);
			HashSet<T> Closed = new();
			Open.Add(Origin);

			while (Open.Num > 0 && !bFoundPath && Depth-- != 0)
			{
				T Current = Open.RemoveFirst();
				Closed.Add(Current);

				if (Current == Destination)
				{
					bFoundPath = true;
					break;
				}

				for (int i = 0; i < Current.NumberOfDirections(); ++i)
				{
					T Neighbour = Current.Neighbour(i) as T;
					if (!Neighbour.IsTraversable() || Closed.Contains(Neighbour)) { continue; }

					float fUpdatedCost = Current.G + Current.DistanceHeuristic(Neighbour);
					if (fUpdatedCost < Neighbour.G || !Open.Contains(Neighbour))
					{
						Neighbour.G = fUpdatedCost;
						Neighbour.H = Neighbour.DistanceHeuristic(Destination);

						Neighbour.Parent = Current;

						if (!Open.Contains(Neighbour))
						{
							Open.Add(Neighbour);
						}
						else
						{
							Open.UpdateItem(Neighbour);
						}
					}
				}
			}

			if (bFoundPath)
			{
				long Time = sw.Time();

				T Traverse = Destination;
				while (Traverse != Origin)
				{
					Path.Push(Traverse);
					Traverse = Traverse.Parent as T;
				}

				Path.Reverse();

				if (bUseDiagnostics)
				{
					Log.P("Path found in:", Time, "ms. Path reversed in:", sw.Stop());
				}
			}

			return bFoundPath;
		}
	}

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

	/// <summary>The MonoBehavior script that manages pathfinding over frames.</summary>
	/// <typeparam name="T">Generic type that implements INode and IHeapItem for T that defines a traversable waypoint.</typeparam>
	/// <decorations decor="public class {T} : MonoBehaviour where T : INode{T}, IHeapItem{T}"></decorations>
	public class MPathManager<T> : MonoBehaviour where T : MNode, IHeapItem<T>
	{
		/// <summary>The number of paths to compute per frame.</summary>
		/// <decorations decor="[SerializeField] [Min(1)] uint"></decorations>
		[SerializeField]
		[Min(1)]
		[Tooltip("The number of paths to compute per frame.")]
		uint ComputationsPerFrame = 1;

		/// <summary>The number of frames between computing path/s.</summary>
		/// <decorations decor="[SerializeField] [Min(1)] uint"></decorations>
		[SerializeField]
		[Min(1)]
		[Tooltip("The number of frames before path/s are computed.")]
		uint FramesBetweenComputations = 1;

		bool bIsPaused = false;
		uint FrameTracker = 0;

		public virtual void Update()
		{
			#region First Compute Skip Explanation.
			/* Assume ComputationsPerFrame = 1. Assume FramesBeforeComputation = 1.
			 * First frame:
				* Will always skip the first call to compute paths.
				* 
				* Beginning of frame:
				* FrameTracker = 1.
				* No. Paths Computed = 0.
				* 
				* End of frame:
				* FrameTracker = 1.
				* No. Paths Computed = 0. // Skipped first frame computation.
				* 
			 * Second frame:
				* Beginning of frame:
				* FrameTracker = 1.
				* No. Paths Computed = 0.
				* 
				* End of frame:
				* FrameTracker = 0.
				* No.PathsComputed = 1.
				* 
			 * Third frame:
				* Beginning of frame:
				* FrameTracker = 0.
				* No. Paths Computed = 1.
				* 
				* End of frame:
				* FrameTracker = 0.
				* No. Paths Computed = 2.
				* 
			 * 
			 * ...
			 */
			#endregion

			if (!bIsPaused)
			{
				if (FrameTracker >= FramesBetweenComputations)
				{
					for (uint i = 0; i < ComputationsPerFrame; ++i)
						PathRegister<T>.ComputeNext();

					FrameTracker = 0;
				}

				FrameTracker++;
			}
		}

		/// <summary>Temporarily stop the computation of paths.</summary>
		/// <decorations decor="public void"></decorations>
		public void Pause()
		{
			bIsPaused = true;
		}

		/// <summary>Continue the computation of paths.</summary>
		/// <decorations decor="public void"></decorations>
		public void Resume()
		{
			bIsPaused = false;
		}

		/// <summary>Prints the current status of this Path Manager.</summary>
		/// <decorations decor="public EStatus"></decorations>
		/// <returns>If this Path Manager is currently Paused, or Running.</returns>
		public EStatus Status()
		{
			Log.P(nameof(MPathManager<T>), "is", bIsPaused ? "Paused." : "Running.");

			return bIsPaused ? EStatus.Paused : EStatus.Running;
		}

		public enum EStatus { Paused, Running }
	}

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

	public abstract class MNode : INode<MNode>, IHeapItem<MNode>
	{
		public bool bIsTraversable;
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

		public abstract float DistanceHeuristic(MNode RelativeTo);

		public abstract bool IsTraversable();

		public virtual INode<MNode> Neighbour(int Direction)
		{
			if (Direction < Neighbours.Num)
				return Neighbours[Direction];
			return null;
		}

		public virtual int NumberOfDirections() => Neighbours.Num;

		public static implicit operator bool(MNode M) => M != null;
	}
}
