using UnityEngine;

namespace MW.Debugger
{
	/// <summary>Debugger utility for drawing Gizmo and Debug Arrows.</summary>
	public static class Arrow
	{
		/// <summary>Draws an Arrow as a Gizmo.</summary>
		/// <param name="Position">The origin of the Arrow.</param>
		/// <param name="Direction">The direction of the Arrow.</param>
		/// <param name="ArrowHeadLength">The length of the Arrow head.</param>
		/// <param name="ArrowHeadAngle">The angle of the Arrow head.</param>
		public static void GizmoArrow(Vector3 Position, Vector3 Direction, float ArrowHeadLength = .25f, float ArrowHeadAngle = 20f)
		{
			Gizmos.DrawRay(Position, Direction);

			Vector3 R = Quaternion.LookRotation(Direction) * Quaternion.Euler(0, 180 + ArrowHeadAngle, 0) * Vector3.forward;
			Vector3 L = Quaternion.LookRotation(Direction) * Quaternion.Euler(0, 180 - ArrowHeadAngle, 0) * Vector3.forward;
			Gizmos.DrawRay(Position + Direction, R * ArrowHeadLength);
			Gizmos.DrawRay(Position + Direction, L * ArrowHeadLength);
		}

		/// <summary>Draws an Arrow as a Gizmo.</summary>
		/// <param name="Position">The origin of the Arrow.</param>
		/// <param name="Direction">The direction of the Arrow.</param>
		/// <param name="Colour">The colour of the Arrow.</param>
		/// <param name="ArrowHeadLength">The length of the Arrow head.</param>
		/// <param name="ArrowHeadAngle">The angle of the Arrow head.</param>
		public static void GizmoArrow(Vector3 Position, Vector3 Direction, Color Colour, float ArrowHeadLength = .25f, float ArrowHeadAngle = 20f)
		{
			Gizmos.color = Colour;
			Gizmos.DrawRay(Position, Direction);

			Vector3 R = Quaternion.LookRotation(Direction) * Quaternion.Euler(0, 180 + ArrowHeadAngle, 0) * Vector3.forward;
			Vector3 L = Quaternion.LookRotation(Direction) * Quaternion.Euler(0, 180 - ArrowHeadAngle, 0) * Vector3.forward;
			Gizmos.DrawRay(Position + Direction, R * ArrowHeadLength);
			Gizmos.DrawRay(Position + Direction, L * ArrowHeadLength);
		}

		/// <summary>Draws an Arrow as Debug.</summary>
		/// <param name="Position">The origin of the Arrow.</param>
		/// <param name="Direction">The direction of the Arrow.</param>
		/// <param name="ArrowHeadLength">The length of the Arrow head.</param>
		/// <param name="ArrowHeadAngle">The angle of the Arrow head.</param>
		public static void DebugArrow(Vector3 Position, Vector3 Direction, float ArrowHeadLength = .25f, float ArrowHeadAngle = 20f)
		{
			Debug.DrawRay(Position, Direction);

			Vector3 R = Quaternion.LookRotation(Direction) * Quaternion.Euler(0, 180 + ArrowHeadAngle, 0) * Vector3.forward;
			Vector3 L = Quaternion.LookRotation(Direction) * Quaternion.Euler(0, 180 - ArrowHeadAngle, 0) * Vector3.forward;
			Debug.DrawRay(Position + Direction, R * ArrowHeadLength);
			Debug.DrawRay(Position + Direction, L * ArrowHeadLength);
		}

		/// <summary>Draws an Arrow as Debug.</summary>
		/// <param name="Position">The origin of the Arrow.</param>
		/// <param name="Direction">The direction of the Arrow.</param>
		/// <param name="Colour">The colour of the Arrow.</param>
		/// <param name="ArrowHeadLength">The length of the Arrow head.</param>
		/// <param name="ArrowHeadAngle">The angle of the Arrow head.</param>
		public static void DebugArrow(Vector3 Position, Vector3 Direction, Color Colour, float ArrowHeadLength = .25f, float ArrowHeadAngle = 20f)
		{
			Debug.DrawRay(Position, Direction, Colour);

			Vector3 R = Quaternion.LookRotation(Direction) * Quaternion.Euler(0, 180 + ArrowHeadAngle, 0) * Vector3.forward;
			Vector3 L = Quaternion.LookRotation(Direction) * Quaternion.Euler(0, 180 - ArrowHeadAngle, 0) * Vector3.forward;
			Debug.DrawRay(Position + Direction, R * ArrowHeadLength, Colour);
			Debug.DrawRay(Position + Direction, L * ArrowHeadLength, Colour);
		}
	}
}