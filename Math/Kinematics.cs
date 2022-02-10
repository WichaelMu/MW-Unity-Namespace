using UnityEngine;
using MW.Easing;

namespace MW.Kinetic
{
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
}
