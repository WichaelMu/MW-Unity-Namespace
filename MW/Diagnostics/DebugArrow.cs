#if RELEASE
using UnityEngine;

namespace MW.Diagnostics
{
	/// <summary>Debugger utility for drawing Gizmo and Debug Arrows.</summary>
	public static class Arrow
	{
		#region Vector3

		/// <summary>Draws an Arrow as a Gizmo.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Position">The origin of the Arrow.</param>
		/// <param name="Direction">The direction of the Arrow.</param>
		/// <param name="ArrowHeadLength">The length of the Arrow head.</param>
		/// <param name="ArrowHeadAngle">The angle of the Arrow head.</param>
		public static void GizmoArrow(Vector3 Position, Vector3 Direction, float ArrowHeadLength = .25f, float ArrowHeadAngle = 20f)
		{
			GizmoArrow(Position, Direction, Color.white, ArrowHeadLength, ArrowHeadAngle);
		}

		/// <summary>Draws an Arrow as a Gizmo.</summary>
		/// <decorations decor="public static void"></decorations>
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
		/// <decorations decor="public static void"></decorations>
		/// <param name="Position">The origin of the Arrow.</param>
		/// <param name="Direction">The direction of the Arrow.</param>
		/// <param name="ArrowHeadLength">The length of the Arrow head.</param>
		/// <param name="ArrowHeadAngle">The angle of the Arrow head.</param>
		/// <param name="Duration">The Duration in seconds in which to show this arrow.</param>
		public static void DebugArrow(Vector3 Position, Vector3 Direction, float ArrowHeadLength = .25f, float ArrowHeadAngle = 20f, float Duration = .02f)
		{
			DebugArrow(Position, Direction, Color.white, ArrowHeadLength, ArrowHeadAngle, Duration);
		}

		/// <summary>Draws an Arrow as Debug.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Position">The origin of the Arrow.</param>
		/// <param name="Direction">The direction of the Arrow.</param>
		/// <param name="Colour">The colour of the Arrow.</param>
		/// <param name="ArrowHeadLength">The length of the Arrow head.</param>
		/// <param name="ArrowHeadAngle">The angle of the Arrow head.</param>
		/// <param name="Duration">The Duration in seconds in which to show this arrow.</param>
		public static void DebugArrow(Vector3 Position, Vector3 Direction, Color Colour, float ArrowHeadLength = .25f, float ArrowHeadAngle = 20f, float Duration = .02f)
		{
			Debug.DrawRay(Position, Direction, Colour, Duration);

			Vector3 R = Quaternion.LookRotation(Direction) * Quaternion.Euler(0, 180 + ArrowHeadAngle, 0) * Vector3.forward;
			Vector3 L = Quaternion.LookRotation(Direction) * Quaternion.Euler(0, 180 - ArrowHeadAngle, 0) * Vector3.forward;
			Debug.DrawRay(Position + Direction, R * ArrowHeadLength, Colour, Duration);
			Debug.DrawRay(Position + Direction, L * ArrowHeadLength, Colour, Duration);
		}

		#endregion
		#region MVector

		/// <summary>Draws an Arrow as a Gizmo.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Position">The origin of the Arrow.</param>
		/// <param name="Direction">The direction of the Arrow.</param>
		/// <param name="ArrowHeadLength">The length of the Arrow head.</param>
		/// <param name="ArrowHeadAngle">The angle of the Arrow head.</param>
		public static void GizmoArrow(MVector Position, MVector Direction, float ArrowHeadLength = .25f, float ArrowHeadAngle = 20f)
		{
			GizmoArrow(Position, Direction, Color.white, ArrowHeadLength, ArrowHeadAngle);
		}

		/// <summary>Draws an Arrow as a Gizmo.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Position">The origin of the Arrow.</param>
		/// <param name="Direction">The direction of the Arrow.</param>
		/// <param name="Colour">The colour of the Arrow.</param>
		/// <param name="ArrowHeadLength">The length of the Arrow head.</param>
		/// <param name="ArrowHeadAngle">The angle of the Arrow head.</param>
		public static void GizmoArrow(MVector Position, MVector Direction, Color Colour, float ArrowHeadLength = .25f, float ArrowHeadAngle = 20f)
		{
			Gizmos.color = Colour;
			Gizmos.DrawRay(Position, Direction);

			MVector R = Quaternion.LookRotation(Direction) * Quaternion.Euler(0, 180 + ArrowHeadAngle, 0) * MVector.Forward;
			MVector L = Quaternion.LookRotation(Direction) * Quaternion.Euler(0, 180 - ArrowHeadAngle, 0) * MVector.Forward;
			Gizmos.DrawRay(Position + Direction, ArrowHeadLength * R);
			Gizmos.DrawRay(Position + Direction, ArrowHeadLength * L);
		}

		/// <summary>Draws an Arrow as Debug.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Position">The origin of the Arrow.</param>
		/// <param name="Direction">The direction of the Arrow.</param>
		/// <param name="ArrowHeadLength">The length of the Arrow head.</param>
		/// <param name="ArrowHeadAngle">The angle of the Arrow head.</param>
		/// <param name="Duration">The Duration in seconds in which to show this arrow.</param>
		public static void DebugArrow(MVector Position, MVector Direction, float ArrowHeadLength = .25f, float ArrowHeadAngle = 20f, float Duration = .02f)
		{
			DebugArrow(Position, Direction, Color.white, ArrowHeadLength, ArrowHeadAngle, Duration);
		}

		/// <summary>Draws an Arrow as Debug.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Position">The origin of the Arrow.</param>
		/// <param name="Direction">The direction of the Arrow.</param>
		/// <param name="Colour">The colour of the Arrow.</param>
		/// <param name="ArrowHeadLength">The length of the Arrow head.</param>
		/// <param name="ArrowHeadAngle">The angle of the Arrow head.</param>
		/// <param name="Duration">The Duration in seconds in which to show this arrow.</param>
		public static void DebugArrow(MVector Position, MVector Direction, Color Colour, float ArrowHeadLength = .25f, float ArrowHeadAngle = 20f, float Duration = .02f)
		{
			Debug.DrawRay(Position, Direction, Colour, Duration);

			MVector R = Quaternion.LookRotation(Direction) * Quaternion.Euler(0, 180 + ArrowHeadAngle, 0) * MVector.Forward;
			MVector L = Quaternion.LookRotation(Direction) * Quaternion.Euler(0, 180 - ArrowHeadAngle, 0) * MVector.Forward;
			Debug.DrawRay(Position + Direction, ArrowHeadLength * R, Colour, Duration);
			Debug.DrawRay(Position + Direction, ArrowHeadLength * L, Colour, Duration);
		}

		#endregion
	}
}
#endif // RELEASE