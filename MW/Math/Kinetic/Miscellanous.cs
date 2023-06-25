#if RELEASE
using UnityEngine;

namespace MW.Kinetic
{
	/// <summary></summary>
	/// <decorations decor="public static class"></decorations>
	public static class Miscellanous
	{
		/// <summary>The direction in which to avoid colliding with obstacles.</summary>
		/// <decorations decor="public static Vector3"></decorations>
		/// <param name="Self">The transform wanting to avoid collisions.</param>
		/// <param name="Angle">The angle to search for potential collisions.</param>
		/// <param name="SearchDistance">The distance to search for collisions.</param>
		/// <param name="Obstacles">The layer to avoid colliding with.</param>
		/// <param name="bDebug">[EDITOR ONLY] Draw lines of the collision avoidance search. Red is the closest collision. Blue is the moving forward direction.</param>
		public static Vector3 CollisionAvoidance(Transform Self, float Angle, float SearchDistance, LayerMask Obstacles, bool bDebug)
		{
			Collider[] colliders = Physics.OverlapSphere(Self.position, SearchDistance, Obstacles);

			float min = float.MaxValue;
			Vector3 dir = Self.forward;

			for (int i = 0; i < colliders.Length; i++)
			{
				Vector3 closestPoint = colliders[i].ClosestPoint(Self.position);
				if (Utils.InFOV(MVector.Forward, Self, closestPoint, Angle))
				{
					float distance = Vector3.Distance(Self.position, closestPoint);
					if (distance < min)
					{
						min = distance;
						dir = closestPoint;
					}
				}
			}


			Vector3 flipped = Vector3.Reflect(Self.forward, (dir - Self.position).normalized);

			Physics.Raycast(Self.position, flipped, out RaycastHit hit, 500000f, Obstacles);

			if (bDebug)
			{
				Debug.DrawRay(Self.position, (dir - Self.position).normalized * SearchDistance, Color.red);
				Debug.DrawRay(Self.position, (flipped - Self.position).normalized, Color.blue);
			}

			return hit.point;
		}
	}
}
#endif // RELEASE