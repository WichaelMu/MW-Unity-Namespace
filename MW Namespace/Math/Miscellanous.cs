using UnityEngine;

namespace MW.Kinetic
{
        /// <summary></summary>
	public static class Miscellanous {

        /// <summary>The direction in which to avoid colliding with obstacles.</summary>
        /// <param name="ASelf">The transform wanting to avoid collisions.</param>
        /// <param name="fAngle">The angle to search for potential collisions.</param>
        /// <param name="fSearchDistance">The distance to search for collisions.</param>
        /// <param name="lmObstacles">The layer to avoid colliding with.</param>
        /// <param name="bDebug">[EDITOR ONLY] Draw lines of the collision avoidance search. Red is the closest collision. Blue is the moving forward direction.</param>
        public static Vector3 CollisionAvoidance(Transform ASelf, float fAngle, float fSearchDistance, LayerMask lmObstacles, bool bDebug) {
            Collider[] colliders = Physics.OverlapSphere(ASelf.position, fSearchDistance, lmObstacles);

            float min = float.MaxValue;
            Vector3 dir = ASelf.forward;

            for (int i = 0; i < colliders.Length; i++) {
                Vector3 closestPoint = colliders[i].ClosestPoint(ASelf.position);
                if (Utils.InFOV(EDirection.Forward, ASelf, closestPoint, fAngle)) {
                    float distance = Vector3.Distance(ASelf.position, closestPoint);
                    if (distance < min) {
                        min = distance;
                        dir = closestPoint;
                    }
                }
            }


            Vector3 flipped = Vector3.Reflect(ASelf.forward, (dir - ASelf.position).normalized);

            Physics.Raycast(ASelf.position, flipped, out RaycastHit hit, 500000f, lmObstacles);

            if (bDebug) {
                Debug.DrawRay(ASelf.position, (dir - ASelf.position).normalized * fSearchDistance, Color.red);
                Debug.DrawRay(ASelf.position, (flipped - ASelf.position).normalized, Color.blue);
            }

            return hit.point;
        }
    }
}
