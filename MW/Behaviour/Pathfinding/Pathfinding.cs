using System;
using System.Collections.Generic;
using MW.Diagnostics;

namespace MW.Pathfinding
{
	/// <summary>Provides the A* Pathfinding implementation for T.</summary>
	/// <typeparam name="T">Generic type that implements INode and IHeapItem for T that defines a traversable waypoint.</typeparam>
	/// <decorations decor="public static class {T} where T : MNode, IHeapItem{T}"></decorations>
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

			THeap<T> Open = new(MapSize, (MNL, MNR) => MNL.CompareTo(MNR));
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
}
