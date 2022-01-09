using UnityEngine;
using MW.Easing;
using MW.Math;

namespace MW.Kinetic {


    public static class Kinematics {
        /// <summary>Convert inspector speed to m/s.</summary>
        public const int kVelocityRatio = 50;

        /// <summary>If the distance between from and to is less than or EqualTo detection.</summary>
        /// <param name="vFrom">The reference coordinate to compare.</param>
        /// <param name="vTo">The target coordinate to compare.</param>
        /// <param name="fDistance">The range that is considered if from has 'reached' to.</param>
        /// <param name="bEqualTo">If this calculation needs to check if the distance between from and to are equal.</param>
        /// <returns></returns>
        public static bool HasReached(Vector3 vFrom, Vector3 vTo, float fDistance = .1f, bool bEqualTo = false) {
            if (bEqualTo)
                return Vector3.Distance(vFrom, vTo) < fDistance;
            return Vector3.Distance(vFrom, vTo) <= fDistance;
        }

        /// <summary>If the distance between from and to is less than or EqualTo detection.</summary>
        /// <param name="vFrom">The reference coordinate to compare.</param>
        /// <param name="vTo">The target coordinate to compare.</param>
        /// <param name="bEqualTo">If this calculation needs to check if the distance between from and to are equal.</param>
        /// <param name="fDistance">The range that is considered if from has 'reached' to.</param>
        /// <returns></returns>
        public static bool HasReached(Vector3 vFrom, Vector3 vTo, bool bEqualTo = false, float fDistance = .1f) {
            if (bEqualTo)
                return Vector3.Distance(vFrom, vTo) < fDistance;
            return Vector3.Distance(vFrom, vTo) <= fDistance;
        }

        /// <summary>Moves self towards target while moving at velocity with a maximum turn angle of TurnRadius degrees.</summary>
        /// <param name="RSelf">The Rigidbody to move.</param>
        /// <param name="ATarget">The Transform destination.</param>
        /// <param name="fVelocity">The rate at which self moves towards target.</param>
        /// <param name="fTurnRadius">The maximum degrees self can turn towards target.</param>
        public static void HomeTowards(Rigidbody RSelf, Transform ATarget, float fVelocity, float fTurnRadius) {

            Transform _self = RSelf.transform;
            RSelf.velocity = _self.forward * fVelocity * Time.deltaTime;
            RSelf.MoveRotation(Quaternion.RotateTowards(_self.rotation, Quaternion.LookRotation(ATarget.position - _self.position), fTurnRadius));
        }

        /// <summary>Moves self towards target while moving at velocity with a maximum turn angle of TurnRadius degrees.</summary>
        /// <param name="RSelf">The Rigidbody to move.</param>
        /// <param name="vTarget">The destination coordinates.</param>
        /// <param name="fVelocity">The rate at which self moves towards target.</param>
        /// <param name="fTurnRadius">The maximum degrees self can turn towards target.</param>
        public static void HomeTowards(Rigidbody RSelf, Vector3 vTarget, float fVelocity, float fTurnRadius) {

            Transform _self = RSelf.transform;
            RSelf.velocity = _self.forward * fVelocity * Time.deltaTime;
            RSelf.MoveRotation(Quaternion.RotateTowards(_self.rotation, Quaternion.LookRotation(vTarget - _self.position, _self.up), fTurnRadius));
        }
    }

    public static class Aerodynamics {
        /// <summary>The direction of natural air resistance.</summary>
        /// <param name="RSelf">The rigidbody to apply air resistance to.</param>
        public static Vector3 AirResistance(Rigidbody RSelf) {
            return -(.5f * Mathematics.Speed(RSelf) * Mathematics.Speed(RSelf) * RSelf.drag * RSelf.velocity.normalized);
        }

        /// <summary>The scale of lift applied to a wing with fWingArea travelling at fVelocity through a fluid at fDensity with fLiftCoefficient.</summary>
        /// <param name="fLiftCoefficient">The heuristic coefficient for lift.</param>
        /// <param name="fDensity">The density of the fluid.</param>
        /// <param name="fVelocity">The speed at which the wing is travelling.</param>
        /// <param name="fWingArea">The area of the wing.</param>
        public static float Lift(float fLiftCoefficient, float fDensity, float fVelocity, float fWingArea)
            => fLiftCoefficient * fWingArea * .5f * fDensity * fVelocity * fVelocity;
    }

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
