using UnityEngine;
using MW.Easing;
using MW.General;

namespace MW.MWPhysics {


    public static class Kinematics {
        /// <summary>Convert inspector speed to m/s.</summary>
        public const int kVelocityRatio = 50;

        /// <summary>
        /// If the distance between from and to is less than or EqualTo detection.
        /// </summary>
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

        /// <summary>
        /// If the distance between from and to is less than or EqualTo detection.
        /// </summary>
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

        /// <summary>
        /// Moves self towards target while moving at velocity with a maximum turn angle of TurnRadius degrees.
        /// </summary>
        /// <param name="RBSelf">The Rigidbody to move.</param>
        /// <param name="TTarget">The Transform destination.</param>
        /// <param name="fVelocity">The rate at which self moves towards target.</param>
        /// <param name="fTurnRadius">The maximum degrees self can turn towards target.</param>
        public static void HomeTowards(Rigidbody RBSelf, Transform TTarget, float fVelocity, float fTurnRadius) {

            Transform _self = RBSelf.transform;
            RBSelf.velocity = _self.forward * fVelocity * Time.deltaTime;
            RBSelf.MoveRotation(Quaternion.RotateTowards(_self.rotation, Quaternion.LookRotation(TTarget.position - _self.position), fTurnRadius));
        }

        /// <summary>
        /// Moves self towards target while moving at velocity with a maximum turn angle of TurnRadius degrees.
        /// </summary>
        /// <param name="RBSelf">The Rigidbody to move.</param>
        /// <param name="vTarget">The destination coordinates.</param>
        /// <param name="fVelocity">The rate at which self moves towards target.</param>
        /// <param name="fTurnRadius">The maximum degrees self can turn towards target.</param>
        public static void HomeTowards(Rigidbody RBSelf, Vector3 vTarget, float fVelocity, float fTurnRadius) {

            Transform _self = RBSelf.transform;
            RBSelf.velocity = _self.forward * fVelocity * Time.deltaTime;
            RBSelf.MoveRotation(Quaternion.RotateTowards(_self.rotation, Quaternion.LookRotation(vTarget - _self.position, _self.up), fTurnRadius));
        }
    }

    public static class Mathematics {

        /// <param name="EEquation">The equation to use to accelerate.</param>
        /// <param name="fCurrentSpeed">The current speed of the acceleration.</param>
        /// <param name="fRateOfAcceleration">The rate to accelerate towards to terminal from current speed.</param>
        /// <param name="fTerminal">The maximum speed.</param>
        /// <returns>The acceleration value using Easing equation, using the current speed and rate of acceleration towards terminal by over time.</returns>
        public static float Acceleration(EEquation EEquation, float fCurrentSpeed, float fRateOfAcceleration, float fTerminal) {
            fTerminal *= Kinematics.kVelocityRatio;

            if (fRateOfAcceleration == 0)
                Debug.LogError(nameof(fRateOfAcceleration) + " cannot be zero");

            return Mathf.Clamp(Mathf.Lerp(fCurrentSpeed / Time.deltaTime, fTerminal, Interpolate.Ease(EEquation, 0, 1, fRateOfAcceleration)), 0, fTerminal);
        }

        public static float Deceleration(float fCurrentSpeed, float fTargetVelocity = 0) {
            return -((fTargetVelocity - fCurrentSpeed) / Time.deltaTime);
        }

        static float fAR = 0;

        /// <param name="RBSelf">The rigidbody to calculate an acceleration rate.</param>
        /// <param name="fSpeed">The current speed of the rigidbody.</param>
        /// <returns>The float rate of movement in metres per second.</returns>
        public static float AccelerationRate(Rigidbody RBSelf, float fSpeed) {
            float a = RBSelf.velocity.magnitude - fSpeed / Time.deltaTime;
            float fAccelerationRate = (a - fAR) * -1;
            fAR = a;
            return fAccelerationRate;
        }

        /// <param name="RBSelf">The Rigidbody to read a speed from.</param>
        /// <param name="UUnit">The desired unit of measurement.</param>
        /// <returns>A speed reading from self in unit of measurement.</returns>
        public static float Speed(Rigidbody RBSelf, Units UUnit = Units.MetresPerSecond) {
            float speed = RBSelf.velocity.magnitude;

            switch (UUnit) {
                case Units.MetresPerSecond:
                    return speed;
                case Units.KilometrsePerHour:
                    return speed * 3.6f;
                case Units.MilesPerHour:
                    return speed * 2.23694f;
                case Units.KilometresPerSecond:
                    return speed * .001f;
                case Units.MetresPerHour:
                    return speed * 3600;
                case Units.FeetPerSecond:
                    return speed * 3.28084f;
                case Units.FeetPerHour:
                    return speed * 11811.02362f;
                case Units.MilesPerSecond:
                    return speed * 1609.34f;
                default:
                    Debug.LogWarning("Failed to convert speed to: " + nameof(UUnit) + "\nReturning metres per second.");
                    return speed;
            }
        }

        /// <summary>The position to intercept RBTarget relative to RBSelf.</summary>
        /// <param name="RBSelf">The Rigidbody predicting the movement of RBTarget.</param>
        /// <param name="RBTarget">The Rigidbody to predict.</param>
        public static Vector3 PredictiveProjectile(Rigidbody RBSelf, Rigidbody RBTarget) {
            //  The approximate conversion from velocity in an rb.AddForce is the 2/3 of the force being applied.

            //  At a velocity of 950, the cannon travels at ~633 m/s.
            //  ~2279 kmph.

            float fSecondsPerKM = 1000 / (RBSelf.velocity.magnitude * Generic.kTwoThirds);

            //  Distance between the RBSelf and RBTarget in thousands.
            float fDistanceBetweenPlayer = Vector3.Distance(RBSelf.position, RBTarget.position) * Generic.kThousandth;

            Vector3 vForwardPrediction = RBTarget.velocity * fSecondsPerKM * fDistanceBetweenPlayer;

            return vForwardPrediction;
        }

        /// <summary>The position to intercept RBTarget relative to TSelf.</summary>
        /// <param name="TSelf">Transform who will be intercepting RBTarget.</param>
        /// <param name="RBTarget">The Rigidbody to predict.</param>
        /// <param name="fForce">The force applied to a projectile as a UnityEngine.ForceMode.Force.</param>
        public static Vector3 PredictiveProjectile(Transform TSelf, Rigidbody RBTarget, float fForce) {
            float fSecondsPerKM = 1000 / (fForce * Generic.kTwoThirds);

            //  Distance between the RBSelf and RBTarget in thousands.
            float fDistanceBetweenPlayer = Vector3.Distance(TSelf.position, RBTarget.position) * Generic.kThousandth;

            Vector3 vForwardPrediction = RBTarget.velocity * fSecondsPerKM * fDistanceBetweenPlayer;

            return vForwardPrediction;
        }
    }

    public static class Aerodynamics {
        /// <summary>The direction of natural air resistance.</summary>
        /// <param name="RBSelf">The rigidbody to apply air resistance to.</param>
        public static Vector3 AirResistance(Rigidbody RBSelf) {
            return -(.5f * Mathematics.Speed(RBSelf) * Mathematics.Speed(RBSelf) * RBSelf.drag * RBSelf.velocity.normalized);
        }
    }

    public static class PhysicsInterpolation {
        /// <summary>Interpolates between origin and destination using equation in duration.</summary>
        /// <param name="EEquation">The equation to use to interpolate.</param>
        /// <param name="vOrigin">The origin of the interpolation.</param>
        /// <param name="vDestination">The destination of this interpolation.</param>
        /// <param name="fDuration">The duration of the interpolation.</param>
        /// <param name="fStart">The starting value of equation.</param>
        /// <param name="fFinal">The final value of equation.</param>
        public static void V3Interpolate(EEquation EEquation, Vector3 vOrigin, Vector3 vDestination, float fDuration, float fStart = 0, float fFinal = 1) {
            Vector3.Lerp(vOrigin, vDestination, Interpolate.Ease(EEquation, fStart, fFinal, fDuration));
        }

        /// <summary>Interpolates between origin and destination using equation.</summary>
        /// <param name="EEquation">The equation to use to interpolate.</param>
        /// <param name="vOrigin">The origin of the interpolation.</param>
        /// <param name="vDestination">The destination of this interpolation.</param>
        /// <param name="fStart">The starting value of equation.</param>
        /// <param name="fFinal">The final value of equation.</param>
        public static void V3Interpolate(EEquation EEquation, Vector3 vOrigin, Vector3 vDestination, float fStart = 0, float fFinal = 1) {
            Vector3.Lerp(vOrigin, vDestination, Interpolate.Ease(EEquation, fStart, fFinal, 1));
        }

        /// <summary>Interpolates between origin and destination.</summary>
        /// <param name="EEquation">The equation to use for interpolation.</param>
        /// <param name="vOrigin">The origin of the interpolation.</param>
        /// <param name="vDestination">The destination of this interpolation.</param>
        public static void V3Interpolate(EEquation EEquation, Vector3 vOrigin, Vector3 vDestination) {
            Vector3.Lerp(vOrigin, vDestination, Interpolate.Ease(EEquation, 0, 1, .3f));
        }

        /// <summary>Interpolates between origin and destination at a constant rate.</summary>
        /// <param name="vOrigin">The origin of the interpolation.</param>
        /// <param name="vDestination">The destination of this interpolation.</param>
        public static void V3Interpolate(Vector3 vOrigin, Vector3 vDestination) {
            Vector3.Lerp(vOrigin, vDestination, Time.deltaTime);
        }

    }

    public static class Miscellanous {

        /// <summary>The direction in which to avoid colliding with obstacles.</summary>
        /// <param name="TSelf">The transform wanting to avoid collisions.</param>
        /// <param name="fAngle">The angle to search for potential collisions.</param>
        /// <param name="fSearchDistance">The distance to search for collisions.</param>
        /// <param name="lmObstacles">The layer to avoid colliding with.</param>
        /// <param name="bDebug">[EDITOR ONLY] Draw lines of the collision avoidance search. Red is the closest collision. Blue is the moving forward direction.</param>
        public static Vector3 CollisionAvoidance(Transform TSelf, float fAngle, float fSearchDistance, LayerMask lmObstacles, bool bDebug) {
            Collider[] colliders = Physics.OverlapSphere(TSelf.position, fSearchDistance, lmObstacles);

            float min = float.MaxValue;
            Vector3 dir = TSelf.forward;

            for (int i = 0; i < colliders.Length; i++) {
                Vector3 closestPoint = colliders[i].ClosestPoint(TSelf.position);
                if (Generic.InFOV(Direction.forward, TSelf, closestPoint, fAngle)) {
                    float distance = Vector3.Distance(TSelf.position, closestPoint);
                    if (distance < min) {
                        min = distance;
                        dir = closestPoint;
                    }
                }
            }


            Vector3 flipped = Vector3.Reflect(TSelf.forward, (dir - TSelf.position).normalized);

            Physics.Raycast(TSelf.position, flipped, out RaycastHit hit, 500000f, lmObstacles);

            if (bDebug) {
                Debug.DrawRay(TSelf.position, (dir - TSelf.position).normalized * fSearchDistance, Color.red);
                Debug.DrawRay(TSelf.position, (flipped - TSelf.position).normalized, Color.blue);
            }

            return hit.point;
        }
    }
}
