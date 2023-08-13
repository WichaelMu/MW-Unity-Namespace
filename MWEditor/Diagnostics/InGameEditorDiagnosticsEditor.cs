using UnityEngine;
using UnityEditor;
using MW.Diagnostics;
using MW;
using MW.Extensions;
using System.Net;
using System.Text;

[CustomEditor(typeof(InGameObjectDiagnostics))]
internal class InGameEditorDiagnosticsEditor : Editor
{
	const int kFontSize = 24;

	void OnSceneGUI()
	{
		GUIStyle Style = new GUIStyle();
		Style.fontSize = kFontSize;

		InGameObjectDiagnostics Diagnostics = (InGameObjectDiagnostics)target;
		Diagnostics.transform.GetPositionAndRotation(out Vector3 Position, out Quaternion Quat);
		Vector3 Rotation = Quat.eulerAngles;
		Vector3 Scale = Diagnostics.transform.lossyScale;

		if (Diagnostics.bDrawPosition)
		{
			StringBuilder PositionLabel = new StringBuilder();
			PositionLabel.
				Append($"X: {Position.x}\n").
				Append($"Y: {Position.y}\n").
				Append($"Z: {Position.z}");

			Vector3 DrawLocation = GetDrawLocation(Position, Diagnostics.DrawLocation);
			Handles.Label(DrawLocation, PositionLabel.ToString(), Style);
		}

		if (Diagnostics.bDrawOrientation)
		{
			for (int i = 0; i < 3; ++i)
				if (Rotation[i] > 180f)
					Rotation[i] -= 360f;
			Handles.Label(Position + (Quat * Vector3.right * Scale.x), $"Roll: {Rotation.z:F2}", Style);
			Handles.Label(Position + (Quat * Vector3.up * Scale.y), $"Pitch: {Rotation.x:F2}", Style);
			Handles.Label(Position + (Quat * Vector3.forward * Scale.z), $"Yaw: {Rotation.y:F2}", Style);
		}
	}

	Vector3 GetDrawLocation(Vector3 TransformPosition, DrawLocation Location)
	{
		Vector3 RetVal = Vector3.zero;

		switch (Location)
		{
			case DrawLocation.Above:
				RetVal = TransformPosition + Vector3.up;
				break;
			case DrawLocation.Below:
				RetVal = TransformPosition + Vector3.down;
				break;
			case DrawLocation.Left:
				RetVal = TransformPosition + Vector3.left;
				break;
			case DrawLocation.Right:
				RetVal = TransformPosition + Vector3.right;
				break;
			case DrawLocation.AboveLeft:
				RetVal = TransformPosition + new Vector3(-1, 1);
				break;
			case DrawLocation.AboveRight:
				RetVal = TransformPosition + MVector.One.XY;
				break;
			case DrawLocation.BelowLeft:
				RetVal = TransformPosition + -MVector.One.XY;
				break;
			case DrawLocation.BelowRight:
				RetVal = TransformPosition + new Vector3(1, -1);
				break;
		}

		return RetVal;
	}
}
