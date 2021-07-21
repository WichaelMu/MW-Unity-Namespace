using UnityEngine;
using MW.Kinetic;
using MW.Easing;
using MW.General;

namespace MW.Math {

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

        /// <param name="RSelf">The rigidbody to calculate an acceleration rate.</param>
        /// <param name="fSpeed">The current speed of the rigidbody.</param>
        /// <returns>The float rate of movement in metres per second.</returns>
        public static float AccelerationRate(Rigidbody RSelf, float fSpeed) {
            float a = RSelf.velocity.magnitude - fSpeed / Time.deltaTime;
            float fAccelerationRate = (a - fAR) * -1;
            fAR = a;
            return fAccelerationRate;
        }

        /// <param name="RSelf">The Rigidbody to read a speed from.</param>
        /// <param name="UUnit">The desired unit of measurement.</param>
        /// <returns>A speed reading from self in unit of measurement.</returns>
        public static float Speed(Rigidbody RSelf, EUnits UUnit = EUnits.MetresPerSecond) {
            float speed = RSelf.velocity.magnitude;

            switch (UUnit) {
                case EUnits.KilometrsePerHour:
                    return speed * 3.6f;
                case EUnits.MilesPerHour:
                    return speed * 2.23694f;
                case EUnits.KilometresPerSecond:
                    return speed * .001f;
                case EUnits.MetresPerHour:
                    return speed * 3600;
                case EUnits.FeetPerSecond:
                    return speed * 3.28084f;
                case EUnits.FeetPerHour:
                    return speed * 11811.02362f;
                case EUnits.MilesPerSecond:
                    return speed * 1609.34f;
                case EUnits.MetresPerSecond:
                    Debug.LogWarning("Use 'RSelf.velocity.magnitude' instead.");
                    return speed;
                default:
                    Debug.LogWarning("Failed to convert speed to: " + nameof(UUnit) + "\nReturning metres per second.");
                    return speed;
            }
        }

        /// <summary>The direction to intercept RBTarget relative to RSelf.</summary>
        /// <param name="RSelf">The Rigidbody predicting the movement of RBTarget.</param>
        /// <param name="RBTarget">The Rigidbody to predict.</param>
        public static Vector3 PredictiveProjectile(Rigidbody RSelf, Rigidbody RBTarget) {
            //  The approximate conversion from velocity in an rb.AddForce is the 2/3 of the force being applied.

            //  At a velocity of 950, the cannon travels at ~633 m/s.
            //  ~2279 kmph.

            float fSecondsPerKM = 1000 / (RSelf.velocity.magnitude * Generic.kTwoThirds);

            //  Distance between the RSelf and RBTarget in thousands.
            float fDistanceBetweenPlayer = Vector3.Distance(RSelf.position, RBTarget.position) * Generic.kThousandth;

            Vector3 vForwardPrediction = RBTarget.velocity * fSecondsPerKM * fDistanceBetweenPlayer;

            return vForwardPrediction;
        }

        /// <summary>Whether nNumber is a power of two.</summary>
        /// <param name="nNumber">The number to check.</param>
        public static bool IsPowerOfTwo(int nNumber) => (nNumber & (nNumber - 1)) == 0;

        public static int GreatestCommonDivisor(int na, int nb) {
            while (nb != 0) {
                int t = nb;
                nb = na % nb;
                na = t;
			}

            return na;
		}

        /// <summary>The lowest common multiple of na and nb.</summary>
        public static int LowestCommonMultiple(int na, int nb) {
            int GCD = GreatestCommonDivisor(na, nb);
            return GCD == 0 ? 0 : (na / GCD) * nb;
		}

        /// <summary>Wraps TN between TMin and TMax.</summary>
        /// <param name="TN">The dynamic number to wrap.</param>
        /// <param name="TMin">The minimum value to wrap.</param>
        /// <param name="TMax">The maximum value to wrap.</param>
        public static dynamic Wrap(dynamic TN, dynamic TMin, dynamic TMax) {
            dynamic s = TMax - TMin;
            dynamic e = TN;
            while (e < TMin) {
                e += s;
			}

            while (e > TMax) {
                e -= s;
			}

            return e;
		}

        /// <summary>Whether v1 is parallel to v2 within fParallelThreshold.</summary>
        /// <param name="v1">Whether this vector is parallel to the other.</param>
        /// <param name="v2">Whether this vector is parallel to the other.</param>
        /// <param name="fParallelThreshold">The threshold to consider parallel vectors.</param>
        public static bool Parallel(Vector3 v1, Vector3 v2, float fParallelThreshold = 0.999845f)
            => Mathf.Abs(Vector3.Dot(v1, v2)) >= fParallelThreshold;

        /// <summary>Whether vVector has been normalised.</summary>
        /// <param name="vVector">The vector to check.</param>
        public static bool IsNormalised(Vector3 vVector) => Mathf.Abs(1f - vVector.sqrMagnitude) < .01f;
    }
}
