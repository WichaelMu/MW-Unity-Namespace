using MW.Debugger;
using UnityEngine;

namespace MW.Diagnostics
{
	[ExecuteInEditMode]
	public class RotationVisualiser : MonoBehaviour
	{
		void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Arrow.GizmoArrow(transform.position, transform.right, .1f, 45f);

			Gizmos.color = Color.green;
			Arrow.GizmoArrow(transform.position, transform.up, .1f, 45f);

			Gizmos.color = Color.blue;
			Arrow.GizmoArrow(transform.position, transform.forward, .1f, 45f);
		}
	}
}
