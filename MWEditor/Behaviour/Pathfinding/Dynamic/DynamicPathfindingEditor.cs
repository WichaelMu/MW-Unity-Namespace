using MW.Pathfinding;
using UnityEngine;
using UnityEditor;

namespace MEditor.Pathfinding
{
	[CustomEditor(typeof(DynamicPathfinding))]
	public class DynamicPathfindingEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DynamicPathfinding DP = (DynamicPathfinding)target;

			if (GUILayout.Button("Precompute Nodes"))
			{
				DP.PlotPoints();
				DP.ForceReconnectAll();
			}

			if (GUILayout.Button("Flush"))
				DP.Nodes.Flush();

			DrawDefaultInspector();
		}
	}
}
