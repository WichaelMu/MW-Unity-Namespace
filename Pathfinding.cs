using System;
using System.Collections.Generic;
using MW.Behaviour;
using MW.Diagnostics;

namespace MW.Pathfinding
{
	/// <summary>Provides the A* Pathfinding implementation for T.</summary>
	/// <typeparam name="T">Generic type that derives from MW.Behaviour.MBehaviour and implements INode and IHeapItem for T.</typeparam>
	public static class Pathfinding<T> where T : MBehaviour, INode<T>, IHeapItem<T>
	{
		/// <summary>A* pathfinds from Origin to Destination looking uDepth times within a uMapSize.</summary>
		/// <param name="Origin">T position to begin pathfinding.</param>
		/// <param name="Destination">T position to pathfind to.</param>
		/// <param name="Path">Reference T List of that make up the path from Origin to Destination.</param>
		/// <param name="uDepth">The depth to search to.</param>
		/// <param name="uMapSize">The total size of the map to be traversed. (The number of INodes).</param>
		/// <param name="OnPathFound">What to do when a path is found? Passes the reference successful Path as a parameter.</param>
		/// <param name="OnPathFailed">What to do when a path cannot be found? Passes the current state of the Path as a parameter.</param>
		/// <param name="bUseDiagnostics">Time the duration of Pathfinding?</param>
		/// <returns>Whether or not a path was found from Origin to Destination within uDepth in uMapSize.</returns>
		public static bool AStar(T Origin, T Destination, ref List<T> Path, uint uDepth, uint uMapSize, Action<List<T>> OnPathFound = null, Action<List<T>> OnPathFailed = null, bool bUseDiagnostics = false)
		{
			Stopwatch sw = new Stopwatch(bUseDiagnostics);

			bool bFoundPath = false;

			THeap<T> Open = new THeap<T>(uMapSize);
			HashSet<T> Closed = new HashSet<T>();
			Open.Add(Origin);

			while (Open.Count > 0 && !bFoundPath && uDepth-- != 0)
			{
				T Current = Open.RemoveFirst();
				Closed.Add(Current);

				if (Current == Destination)
				{
					bFoundPath = true;
					break;
				}

				for (uint i = 0; i < Current.Directions; ++i)
				{
					T Neighbour = (T)Current.Neighbour(i);
					if (!Neighbour.IsTraversable() || Closed.Contains(Neighbour)) { continue; }

					float fUpdatedCost = Current.G + Current.Position.SqrDistance(Neighbour.Position);
					if (fUpdatedCost < Neighbour.G || !Open.Contains(Neighbour))
					{
						Neighbour.G = fUpdatedCost;
						Neighbour.H = Neighbour.Position.SqrDistance(Destination.Position);

						Neighbour.Parent = Current;

						if (!Open.Contains(Neighbour))
						{
							Open.Add(Neighbour);
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
					Path.Add(Traverse);
					Traverse = Traverse.Parent;
				}

				Path.Reverse();

				if (bUseDiagnostics)
				{
					Log.Print("Path found in:", Time, "Path reversed in:", sw.Stop());
				}

				OnPathFound?.Invoke(Path);
			}
			else
			{
				OnPathFailed?.Invoke(Path);
			}

			return bFoundPath;
		}
	}

	public interface INode<T>
	{
		/// <summary>This Node's F score.</summary>
		float F { get; set; }
		/// <summary>This Node's G score.</summary>
		float G { get; set; }
		/// <summary>This Node's H score.</summary>
		float H { get; set; }

		T Parent { get; set; }

		/// <summary>How many directions can this Node point to?</summary>
		uint Directions { get; set; }
		/// <summary>Is this block traversable?</summary>
		bool IsTraversable();
		/// <summary>Get the Neighbouring Node at uDirection.</summary>
		/// <param name="uDirection">The neighbour of this Node in this direction.</param>
		INode<T> Neighbour(uint uDirection);
	}
}
