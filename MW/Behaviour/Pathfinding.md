# MW.Pathfinding

A* Pathfinding with Heap optimisations.

```cs
using MW.Pathfinding;
```

## Pathfinding
For pathfinding to be allowed, a generic type `T` that implements `INode<T>` and `IHeapItem<T>` is required.

`PathfindingNode.cs`
```cs
public class PathfindingNode : INode<PathfindingNode>, IHeapItem<PathfindingNode>
{
	public MVector Position;
	PathfindingNode[] Neighbours = new PathfindingNode[8];
	public float F { get => f; set => f = value; }
	public float G { get => g; set => g = value; }
	public float H { get => h; set => h = value; }
	public PathfindingNode Parent { get => parent; set => parent = value; }
	public int HeapItemIndex { get => HeapIndex; set => HeapIndex = value; }

	float f, g, h;
	PathfindingNode parent;
	int HeapIndex;

	public int CompareTo(PathfindingNode Other)
	{
		int Compare = F.CompareTo(Other.F);

		if (Compare == 0)
			Compare = H.CompareTo(Other.H);

		return -Compare;
	}

	public float DistanceHeuristic(PathfindingNode RelativeTo) =>
		MVector.Distance(Position, RelativeTo.Position);

	public bool IsTraversable() =>
		true;

	public INode<s> Neighbour(int Direction) =>
		Neighbours[Direction];

	public int NumberOfDirections() =>
		Neighbours.Length;
}
```

```cs
using static MW.Pathfinding;

PathfindingNode Origin = ...;
PathfindingNode Destination = ...;
uint Depth = 1 << 31;                // Default is int.MaxValue.
uint MapSize = 10000;                // Default is 10000.
Action<List<T>> OnPathFound = null;  // Default is null.
Action<List<T>> OnPathFailed = null; // Default is null.
bool bUseDiagnostics = false;        // Default is false.

if (AStar(Origin, Destination, out List<PathfindingNode> Path, Depth, MapSize, OnPathFound, OnPathFailed, bUseDiagnostics))
	// Path is shortest set of PathfindingNodes from Origin to Destination
	// found from Depth and MapSize.
```

### Batch Pathfinding Processing
Compute multiple paths over a number of frames so as to avoid stuttering.

`AI.cs`
```cs
using UnityEngine;

public class AI : Monobehaviour
{
	...

	void Pathfind(PathfindingNode Origin, PathfindingNode Destination, Action<List<PathfindingNode>> OnFoundPath)
	{
		PathRegister<PathfindingNode>.RequestPath(Origin, Destination, OnFoundPath);
	}
}
```
Batch pathfinding processes are handled through `MPathManager<T>`. In this example, we will continue using `PathfindingNode` as `T`.

MPathManager has settings to determine the number of paths that can be executed in a single frame with `MPathManager<PathfindingNode>.ComputationsPerFrame`.
Additionally, MPathManager can also adjust the number of frames between computations with `MPathManager<PathfindingNode>.FramesBetweenComputations`.

Pathfinding with MPathManager is done in the Unity `Update()` loop, and can paused with `Pause()`.