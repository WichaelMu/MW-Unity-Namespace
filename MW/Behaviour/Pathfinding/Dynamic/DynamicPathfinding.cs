using System.Collections;
using UnityEngine;
using MW.Extensions;

namespace MW.Pathfinding
{
	/// <summary>Dynamic Pathfinding that adapts to the world environment.</summary>
	/// <decorations decor="public class : MonoBehaviour"></decorations>
	public class DynamicPathfinding : MonoBehaviour
	{
		/// <summary>Draw the Pathfinding bounding box.</summary>
		/// <decorations decor="public bool"></decorations>
		[Header("Debug")]
		public bool bDrawGizmos;
		/// <summary>Draw the positions of the individual nodes.</summary>
		/// <decorations decor="public bool"></decorations>
		public EDynamicPathfindingGizmosOptions GizmoOptions;
		public bool bShowBounds;

		/// <summary>The local centre of the bounding box.</summary>
		/// <decorations decor="[SerializeField] Vector3"></decorations>
		[Header("3D Pathfinding")]
		[SerializeField] Vector3 LocalCentre;
		/// <summary>The Width, Height, and Depth of the Pathfinding box.</summary>
		/// <decorations decor="[SerializeField] Vector3Int"></decorations>
		[SerializeField] Vector3Int Bounds;
		/// <summary>The layer/s that can be walked on.</summary>
		/// <decorations decor="[SerializeField] LayerMask"></decorations>
		[SerializeField] LayerMask TraversableLayer;
		/// <summary>The number of Nodes per Unity-world unit.</summary>
		/// <decorations decor="[SerializeField] [Min(1)] float"></decorations>
		[SerializeField, Min(1f)] float PointsPerUnit = 1;

		/// <summary>True if this should adapt to the world environment.</summary>
		/// <decorations decor="[SerializeField] bool"></decorations>
		[Header("Dynamic Pathfinding")]
		[SerializeField] bool bIsDynamic;
		/// <summary>The number of frames between each dynamic update to the Pathfinding Nodes.</summary>
		/// <decorations decor="[SerializeField] [Min(1)] int"></decorations>
		[SerializeField, Min(1)] int FramesBetweenUpdate = 10;
		/// <summary>The number of Nodes to update per frame.</summary>
		/// <decorations decor="[SerializeField] [Min(1)] int"></decorations>
		[SerializeField, Min(1)] int NodesPerUpdate = 200;
		IEnumerator AsyncOperation;
		int FrameCount = 0;

		/// <summary>The Nodes that are considered in the Dynamic Pathfinder.</summary>
		/// <remarks>The order is Z, Y, X.</remarks>
		/// <decorations decor="public MArray{Node}"></decorations>
		public MArray<Node> Nodes = new MArray<Node>();

		Vector3 Centre => transform.position + LocalCentre;

		static readonly Vector3 Size = new Vector3(.05f, .05f, .05f);
		Vector2Int Limits;

		void Start()
		{
			PlotPoints();
			ForceReconnectAll();
		}

		void Update()
		{
			if (!bIsDynamic)
				return;

			++FrameCount;
			if (FrameCount % FramesBetweenUpdate != 0)
				return;

			// ...

			if (AsyncOperation == null)
			{
				AsyncOperation = AsyncDynamicNodeReconnect();
				StartCoroutine(AsyncOperation);

				FrameCount = 0;
			}
		}

		/// <summary>Plot the Nodes, adhering to <see cref="Bounds"/>.</summary>
		/// <docs>Plot the Nodes, adhering to Bounds.</docs>
		/// <decorations decor="public void"></decorations>
		public void PlotPoints()
		{
			Nodes = new MArray<Node>();

			float PPU = 1f / PointsPerUnit;
			int i = 0;
			Limits = -Vector2Int.one;

			// Handle 3 offset.
			int IsThree = PointsPerUnit == 3 ? 1 : 0;
			float XLimit = Bounds.x + PPU * IsThree;
			float YLimit = Bounds.y + PPU * IsThree;
			float ZLimit = Bounds.z + PPU * IsThree;

			for (float x = -Bounds.x; x <= XLimit; x += PPU)
			{
				for (float y = -Bounds.y; y <= YLimit; y += PPU)
				{
					for (float z = -Bounds.z; z <= ZLimit; z += PPU)
					{
						Vector3 Point = Centre + new Vector3(x, y, z);
						Node N = new Node();

						N.Position = Point;
						Nodes.Push(N);

						++i;
					}

					if (Limits.y == -1)
						Limits.y = i;
				}

				if (Limits.x == -1)
					Limits.x = i;
			}
		}

		/// <summary>Forces all Nodes to reconnect with each other.</summary>
		/// <decorations decor="public void"></decorations>
		public void ForceReconnectAll()
		{
			// Connect.
			for (int N = 0; N < Nodes.Num; ++N)
			{
				UpdateNode(N);
			}
		}

		/// <summary>Find the shortest path from Origin to Destination.</summary>
		/// <decorations decor="public bool"></decorations>
		/// <param name="Origin">The Node to begin searching on.</param>
		/// <param name="Destination">The Node to reach.</param>
		/// <param name="Path">The shortest path from Origin to Destination.</param>
		/// <returns>True if a path from Origin to Destination was found.</returns>
		public bool Pathfind(Node Origin, Node Destination, out MArray<Node> Path)
		{
			if (Pathfinding<Node>.AStar(Origin, Destination, out Path))
			{
				Debug.DrawLine(Origin.Position, Path[0].Position);

				for (int i = 0; i < Path.Num - 1; ++i)
				{
					Debug.DrawLine(Path[i].Position, Path[i + 1].Position, Color.Lerp(Color.green, Color.red, i / (float)Path.Num));
				}

				return true;
			}

			return false;
		}

		public int WorldToIndex(Vector3 InWorld)
		{
			Vector3 LocalWorld = (InWorld * PointsPerUnit) - Centre;

			if (Mathf.Abs(LocalWorld.x) > Bounds.x || Mathf.Abs(LocalWorld.y) > Bounds.y || Mathf.Abs(LocalWorld.z) > Bounds.z)
				return -1;

			int MidPoint = Nodes.Num / 2;

			float X = Limits.x * Mathf.Round(LocalWorld.x);
			float Y = Limits.y * Mathf.Round(LocalWorld.y);
			float Z = Mathf.Round(LocalWorld.z);

			return MidPoint + (int)(X + Y + Z);
		}

		public Node WorldToNode(Vector3 InWorld)
		{
			return Nodes[WorldToIndex(InWorld)];
		}

		public int NodeToIndex(Node Node)
		{
			return WorldToIndex(Node.Position);
		}

		IEnumerator AsyncDynamicNodeReconnect()
		{
			int N = 0;
			while (N < Nodes.Num)
			{
				for (int k = 0; k < NodesPerUpdate && N < Nodes.Num; ++k, ++N)
				{
					UpdateNode(N);
				}

				yield return null;
			}

			AsyncOperation = null;
		}

		void UpdateNode(int NodeIndex)
		{
			Node N = Nodes[NodeIndex];

			float PPU = 1f / PointsPerUnit;
			N.bIsTraversable = !(Physics.OverlapBox(N.Position, Vector3.one * PPU * .5f, Quaternion.identity, ~TraversableLayer).Length > 0);

			// In (+Z).
			if (IsAllowed(NodeIndex, 1))
			{
				Nodes[NodeIndex].Push(Nodes[NodeIndex + 1]);
				//Debug.DrawLine(Nodes[N].Position, Nodes[N + 1].Position, Color.blue);
			}

			// Up (+Y).
			if (IsAllowed(NodeIndex, Limits.x))
			{
				//Debug.Log("X: " + Limits.x);
				Nodes[NodeIndex].Push(Nodes[NodeIndex + Limits.x]);
				//Debug.DrawLine(Nodes[N].Position, Nodes[N + Limits.x].Position, Color.red);
			}

			// Right (+X).
			if (IsAllowed(NodeIndex, Limits.y))
			{
				//Debug.Log("Y: " + Limits.y);
				Nodes[NodeIndex].Push(Nodes[NodeIndex + Limits.y]);
				//Debug.DrawLine(Nodes[N].Position, Nodes[N + Limits.y].Position, Color.green);
			}
		}

		bool IsAllowed(int From, int Direction)
		{
			if (!Nodes.InRange(From) || !Nodes.InRange(From + Direction))
				return false;

			// In (+Z).
			if (Direction == 1)
				return (From % Limits.y) != Limits.y - 1;

			// Up (+Y).
			if (Direction == Limits.y)
				return Vector3.Dot((Nodes[From].Position - Nodes[From + Direction].Position).FNormalise(), Vector3.up) < 0;

			// Right (+X).
			if (Direction == Limits.x)
				return Bounds.x * 2 * Limits.x + (Direction % Limits.x) != Direction;

			return false;
		}

		void OnDrawGizmos()
		{
			if (!bDrawGizmos)
				return;

			if (bShowBounds)
				Gizmos.DrawWireCube(transform.position + LocalCentre, Bounds * 2);

			if (GizmoOptions == 0)
				return;


			for (int i = 0; i < Nodes.Num; ++i)
			{
				Node N = Nodes[i];
				if ((GizmoOptions & EDynamicPathfindingGizmosOptions.CollisionOnly) == EDynamicPathfindingGizmosOptions.CollisionOnly && !N.bIsTraversable)
				{
					Gizmos.color = Color.red;
					Gizmos.DrawCube(N.Position, Size);
				}
				else if ((GizmoOptions & EDynamicPathfindingGizmosOptions.NoCollision) == EDynamicPathfindingGizmosOptions.NoCollision && N.bIsTraversable)
				{
					Gizmos.color = Color.cyan;
					Gizmos.DrawCube(N.Position, Size);
				}
			}
		}
	}

	public enum EDynamicPathfindingGizmosOptions : byte
	{
		[InspectorName("Show None")]
		None = 0,
		[InspectorName("Show Collisions Only")]
		CollisionOnly = 1,
		[InspectorName("Show Traversable Only")]
		NoCollision = 2,
		[InspectorName("Show All")]
		AllNodes = CollisionOnly | NoCollision
	}
}