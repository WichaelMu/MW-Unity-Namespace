using UnityEngine;
using MW.Easing;

namespace MW.MWPhysics {


    public static class MWPhysics {
        /// <summary>Convert inspector speed to m/s.</summary>
        public const int VelocityRatio = 50;

        /// <summary>
        /// If the distance between from and to is less than or EqualTo detection.
        /// </summary>
        /// <param name="from">The reference coordinate to compare.</param>
        /// <param name="to">The target coordinate to compare.</param>
        /// <param name="distance">The range that is considered if from has 'reached' to.</param>
        /// <param name="EqualTo">If this calculation needs to check if the distance between from and to are equal.</param>
        /// <returns></returns>
        public static bool HasReached(Vector3 from, Vector3 to, float distance = .1f, bool EqualTo = false) {
            if (EqualTo)
                return Vector3.Distance(from, to) < distance;
            return Vector3.Distance(from, to) <= distance;
        }

        /// <summary>
        /// If the distance between from and to is less than or EqualTo detection.
        /// </summary>
        /// <param name="from">The reference coordinate to compare.</param>
        /// <param name="to">The target coordinate to compare.</param>
        /// <param name="EqualTo">If this calculation needs to check if the distance between from and to are equal.</param>
        /// <param name="distance">The range that is considered if from has 'reached' to.</param>
        /// <returns></returns>
        public static bool HasReached(Vector3 from, Vector3 to, bool EqualTo = false, float distance = .1f) {
            if (EqualTo)
                return Vector3.Distance(from, to) < distance;
            return Vector3.Distance(from, to) <= distance;
        }

        /// <summary>
        /// Moves self towards target while moving at velocity with a maximum turn angle of TurnRadius degrees.
        /// </summary>
        /// <param name="self">The Rigidbody to move.</param>
        /// <param name="target">The Transform destination.</param>
        /// <param name="velocity">The rate at which self moves towards target.</param>
        /// <param name="TurnRadius">The maximum degrees self can turn towards target.</param>
        public static void HomeTowards(Rigidbody self, Transform target, float velocity, float TurnRadius) {

            Transform _self = self.transform;
            self.velocity = _self.forward * velocity * Time.deltaTime;
            self.MoveRotation(Quaternion.RotateTowards(_self.rotation, Quaternion.LookRotation(target.position - _self.position), TurnRadius));
        }

        /// <summary>
        /// Moves self towards target while moving at velocity with a maximum turn angle of TurnRadius degrees.
        /// </summary>
        /// <param name="self">The Rigidbody to move.</param>
        /// <param name="target">The destination coordinates.</param>
        /// <param name="velocity">The rate at which self moves towards target.</param>
        /// <param name="TurnRadius">The maximum degrees self can turn towards target.</param>
        public static void HomeTowards(Rigidbody self, Vector3 target, float velocity, float TurnRadius) {

            Transform _self = self.transform;
            self.velocity = _self.forward * velocity * Time.deltaTime;
            self.MoveRotation(Quaternion.RotateTowards(_self.rotation, Quaternion.LookRotation(target - _self.position, _self.up), TurnRadius));
        }

        /// <param name="equation">The equation to use to accelerate.</param>
        /// <param name="CurrentSpeed">The current speed of the acceleration.</param>
        /// <param name="RateOfAcceleration">The rate to accelerate towards to terminal from current speed.</param>
        /// <param name="terminal">The maximum speed.</param>
        /// <param name="time">The elapsed time.</param>
        /// <returns>The acceleration value using Easing equation, using the current speed and rate of acceleration towards terminal by over time.</returns>
        public static float Acceleration(Equation equation, float CurrentSpeed, float RateOfAcceleration, float terminal, float time) {
            terminal *= VelocityRatio;

            if (RateOfAcceleration == 0)
                Debug.LogError(nameof(RateOfAcceleration) + " cannot be zero");

            return Mathf.Clamp(Mathf.Lerp(CurrentSpeed / Time.deltaTime, terminal, Interpolate.Ease(equation, 0, 1, RateOfAcceleration)), 0, terminal);
        }

        public static float Deceleration(float CurrentSpeed, float TargetVelocity = 0) {
            return -((TargetVelocity - CurrentSpeed) / Time.deltaTime);
        }

        static float fAR = 0;

        /// <param name="self">The rigidbody to calculate an acceleration rate.</param>
        /// <param name="Speed">The current speed of the rigidbody.</param>
        /// <returns>The float rate of movement in metres per second.</returns>
        public static float AccelerationRate(Rigidbody self, float Speed) {
            float a = self.velocity.magnitude - Speed / Time.deltaTime;
            float fAccelerationRate = (a - fAR) * -1;
            fAR = a;
            return fAccelerationRate;
        }

        /// <param name="self">The Rigidbody to read a speed from.</param>
        /// <param name="unit">The desired unit of measurement.</param>
        /// <returns>A speed reading from self in unit of measurement.</returns>
        public static float Speed(Rigidbody self, Units unit = Units.MetresPerSecond) {
            float speed = self.velocity.magnitude;

            switch (unit) {
                case Units.MetresPerSecond:
                    Debug.LogWarning("Use 'self.velocity.magnitude' instead.");
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
                    Debug.LogWarning("Failed to convert speed to: " + nameof(unit) + "\nReturning metres per second.");
                    return speed;
            }
        }

        /// <summary>The direction of natural air resistance.</summary>
        /// <param name="self">The rigidbody to apply air resistance to.</param>
        public static Vector3 AirResitance(Rigidbody self) {
            return -(.5f * Speed(self) * Speed(self) * self.drag * self.velocity.normalized);
        }

        /// <summary>Interpolates between origin and destination using equation in duration.</summary>
        /// <param name="equation">The equation to use to interpolate.</param>
        /// <param name="origin">The origin of the interpolation.</param>
        /// <param name="destination">The destination of this interpolation.</param>
        /// <param name="duration">The duration of the interpolation.</param>
        /// <param name="start">The starting value of equation.</param>
        /// <param name="final">The final value of equation.</param>
        public static void V3Interpolate(Equation equation, Vector3 origin, Vector3 destination, float duration, float start = 0, float final = 1) {
            Vector3.Lerp(origin, destination, Interpolate.Ease(equation, start, final, duration));
        }

        /// <summary>Interpolates between origin and destination using equation.</summary>
        /// <param name="equation">The equation to use to interpolate.</param>
        /// <param name="origin">The origin of the interpolation.</param>
        /// <param name="destination">The destination of this interpolation.</param>
        /// <param name="start">The starting value of equation.</param>
        /// <param name="final">The final value of equation.</param>
        public static void V3Interpolate(Equation equation, Vector3 origin, Vector3 destination, float start = 0, float final = 1) {
            Vector3.Lerp(origin, destination, Interpolate.Ease(equation, start, final, 1));
        }

        /// <summary>Interpolates between origin and destination.</summary>
        /// <param name="equation">The equation to use for interpolation.</param>
        /// <param name="origin">The origin of the interpolation.</param>
        /// <param name="destination">The destination of this interpolation.</param>
        public static void V3Interpolate(Equation equation, Vector3 origin, Vector3 destination) {
            Vector3.Lerp(origin, destination, Interpolate.Ease(equation, 0, 1, .3f));
        }

        /// <summary>Interpolates between origin and destination at a constant rate.</summary>
        /// <param name="origin">The origin of the interpolation.</param>
        /// <param name="destination">The destination of this interpolation.</param>
        public static void V3Interpolate(Vector3 origin, Vector3 destination) {
            Vector3.Lerp(origin, destination, Time.deltaTime);
        }


        /// <summary>The direction in which to avoid colliding with obstacles.</summary>
        /// <param name="self">The transform wanting to avoid collisions.</param>
        /// <param name="angle">The angle to search for potential collisions.</param>
        /// <param name="searchDistance">The distance to search for collisions.</param>
        /// <param name="obstacles">The layer to avoid colliding with.</param>
        /// <param name="debug">[EDITOR ONLY] Draw lines of the collision avoidance search. Red is the closest collision. Blue is the moving forward direction.</param>
        public static Vector3 CollisionAvoidance(Transform self, float angle, float searchDistance, LayerMask obstacles, bool debug) {
            Collider[] colliders = Physics.OverlapSphere(self.position, searchDistance, obstacles);

            float min = float.MaxValue;
            Vector3 dir = self.forward;

            for (int i = 0; i < colliders.Length; i++) {
                Vector3 closestPoint = colliders[i].ClosestPoint(self.position);
                if (General.Generic.InFOV(Direction.forward, self, closestPoint, angle)) {
                    float distance = Vector3.Distance(self.position, closestPoint);
                    if (distance < min) {
                        min = distance;
                        dir = closestPoint;
                    }
                }
            }


            Vector3 flipped = Vector3.Reflect(self.forward, (dir - self.position).normalized);

            Physics.Raycast(self.position, flipped, out RaycastHit hit, 500000f, obstacles);

            if (debug) {
                Debug.DrawRay(self.position, (dir - self.position).normalized * searchDistance, Color.red);
                Debug.DrawRay(self.position, (flipped - self.position).normalized, Color.blue);
            }

            return hit.point;
        }
    }
}
