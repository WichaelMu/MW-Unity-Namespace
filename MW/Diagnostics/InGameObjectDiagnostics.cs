using MW.Diagnostics;
using UnityEngine;

namespace MW.Diagnostics
{
	[ExecuteInEditMode]
	public class InGameObjectDiagnostics : MonoBehaviour
	{
		public bool bDrawPosition;
		public DrawLocation DrawLocation = DrawLocation.BelowRight;
		public bool bDrawOrientation;

		void OnDrawGizmos()
		{
			Vector3 Scale = transform.lossyScale;

			if (bDrawOrientation)
			{
				Gizmos.color = Color.red;
				Arrow.GizmoArrow(transform.position, transform.right * Scale.x, .1f, 45f);

				Gizmos.color = Color.green;
				Arrow.GizmoArrow(transform.position, transform.up * Scale.y, .1f, 45f);

				Gizmos.color = Color.blue;
				Arrow.GizmoArrow(transform.position, transform.forward * Scale.z, .1f, 45f);
			}
		}
	}

	public enum DrawLocation : byte
	{
		Above, Below,
		Left, Right,
		AboveLeft, AboveRight,
		BelowLeft, BelowRight
	}
}
