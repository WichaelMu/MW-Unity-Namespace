using System;
using UnityEngine;

namespace MW.General {


    public static class Generic {

        public const float k10Percent = .1f;
        public const float kQuarter = .25f;
        public const float kHalf = .5f;
        public const float kThreeQuarters = .75f;
        public const float kOneThird = .3333333333333333333333333333333333f;
        public const float kTwoThirds = kOneThird * 2;

        /// <summary>The ratio between 1 and 255.</summary>
        public const float k1To255RGB = 0.0039215686274509803921568627451F;

        /// <summary>If self can see target within SearchAngle degrees while facing face.</summary>
        /// <param name="face">The direction self is facing.</param>
        /// <param name="self">The transform searching for target.</param>
        /// <param name="target">The transform to look out for.</param>
        /// <param name="SearchAngle">The maximum degrees to search for target.</param>
        public static bool InFOV(Direction face, Transform self, Transform target, float SearchAngle) {

            switch (face) {
                case Direction.forward:
                    return Vector3.Angle(self.forward, target.position - self.position) < SearchAngle;
                case Direction.right:
                    return Vector3.Angle(self.right, target.position - self.position) < SearchAngle;
                case Direction.back:
                    return Vector3.Angle(-self.forward, target.position - self.position) < SearchAngle;
                case Direction.left:
                    return Vector3.Angle(-self.right, target.position - self.position) < SearchAngle;
                case Direction.up:
                    return Vector3.Angle(self.up, target.position - self.position) < SearchAngle;
                case Direction.down:
                    return Vector3.Angle(-self.up, target.position - self.position) < SearchAngle;
                default:
                    Debug.LogWarning("There was a problem in determining a face direction");
                    return false;
            }
        }

        /// <summary>If self can see target within SearchAngle degrees while facing face.</summary>
        /// <param name="face">The direction self is facing.</param>
        /// <param name="self">The transform searching for target.</param>
        /// <param name="target">The position to look out for.</param>
        /// <param name="SearchAngle">The maximum degrees to search for target.</param>
        public static bool InFOV(Direction face, Transform self, Vector3 target, float SearchAngle) {

            switch (face) {
                case Direction.forward:
                    return Vector3.Angle(self.forward, target - self.position) < SearchAngle;
                case Direction.right:
                    return Vector3.Angle(self.right, target - self.position) < SearchAngle;
                case Direction.back:
                    return Vector3.Angle(-self.forward, target - self.position) < SearchAngle;
                case Direction.left:
                    return Vector3.Angle(-self.right, target - self.position) < SearchAngle;
                case Direction.up:
                    return Vector3.Angle(self.up, target - self.position) < SearchAngle;
                case Direction.down:
                    return Vector3.Angle(-self.up, target - self.position) < SearchAngle;
                default:
                    Debug.LogWarning("There was a problem in determining a face direction");
                    return false;
            }
        }

        /// <summary>If self has a line of sight to to.</summary>
        /// <param name="self">The position to look from.</param>
        /// <param name="to">The position to look to.</param>
        /// <param name="obstacles">The obstacles to consider obtrusive.</param>
        public static bool LineOfSight(Vector3 self, Vector3 to, LayerMask obstacles) {
            return !Physics.Linecast(self, to, obstacles);
        }

        /// <summary>If self has a line of sight to to.</summary>
        public static bool LineOfSight(Vector3 self, Vector3 to) {
            return !Physics.Linecast(self, to);
        }

        ///<summary>The fValue rounded to dp decimal places.</summary>
        /// <param name="fValue">The value to be rounded.</param>
        /// <param name="dp">The decimal places to be included.</param>
        public static float RoundToDP(float fValue, int dp = 2) {
            if (dp == 0) {
                Debug.LogWarning("Use Mathf.RoundToInt(" + nameof(fValue) + ") instead.");
                return Mathf.RoundToInt(fValue);
            }

            if (dp < 0) {
                Debug.LogWarning("Please use a number greater than 0. Rounding to 2 instead.");
                dp = 2;
            }

            int fFactor = (int)Mathf.Pow(10, dp);
            return Mathf.Round(fValue * fFactor) / fFactor;
        }

        /// <summary>Flip-Flops Bool.</summary>
        /// <param name="Bool"></param>
        public static void FlipFlop(ref bool Bool) {
            Bool = !Bool;
        }

        /// <summary>Flip-Flops Bool.</summary>
        /// <param name="Bool"></param>
        /// <param name="callbackTrue">The method to call if the flip-flop is true.</param>
        /// <param name="callbackFalse">The method to call if the flip-flop is false.</param>
        public static void FlipFlop(ref bool Bool, Action callbackTrue, Action callbackFalse) {
            Bool = !Bool;

            if (Bool)
                callbackTrue();
            else
                callbackFalse();
        }

        /// <summary>If value is within the +- limit of from.</summary>
        /// <param name="value">The value to check.</param>
        /// <param name="from">The value to compare.</param>
        /// <param name="limit">The limits to consider.</param>
        public static bool IsWithin(float value, float from, float limit) {
            if (limit == 0) {
                Debug.LogWarning("Use the '== 0' comparison operator instead.");
                return value == from;
            }
            if (limit < 0) {
                Debug.LogWarning("Please use a positive number");
                limit = Mathf.Abs(limit);
            }

            return (from + limit > value) && (value > from - limit);
        }

        /// <summary>The largest vector between L and R, according to magnitude.</summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        public static Vector3 Max(Vector3 L, Vector3 R) {
            return (L.magnitude < R.magnitude) ? R : L;
        }

        /// <summary>The smallest vector vector between L and R, according to magnitude.</summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        public static Vector3 Min(Vector3 L, Vector3 R) {
            return (L.magnitude > R.magnitude) ? R : L;
        }

        /// <summary>Returns the n'th Fibonacci number.</summary>
        /// <param name="n"></param>
        public static int Fibonacci(int n) {
            if (n <= 2)
                return 1;
            return Fibonacci(n - 1) + Fibonacci(n - 2);
        }

        /// <summary>Generates spherical points with an equal distribution.</summary>
        /// <param name="resolution">The number of points to generate.</param>
        /// <param name="goldenRationModifier">Adjusts the golden ratio.</param>
        /// <returns>The Vector3[] points for the sphere.</returns>
        public static Vector3[] GenerateEqualSphere(int resolution, float goldenRationModifier) {
            Vector3[] directions = new Vector3[resolution];

            float phi = (1 + Mathf.Sqrt(goldenRationModifier) * .5f);
            float inc = Mathf.PI * 2 * phi;

            for (int i = 0; i < resolution; i++) {
                float t = (float)i / resolution;
                float incline = Mathf.Acos(1 - 2 * t);
                float azimuth = inc * i;

                float x = Mathf.Sin(incline) * Mathf.Cos(azimuth);
                float y = Mathf.Sin(incline) * Mathf.Sin(azimuth);
                float z = Mathf.Cos(incline);

                directions[i] = new Vector3(x, y, z);
            }

            return directions;
        }

        /// <summary>Generates the points to 'bridge' origin and target together at a height as an arc.</summary>
        /// <param name="origin">The starting point of the bridge.</param>
        /// <param name="target">The ending point of the bridge.</param>
        /// <param name="resolution">The number of points for the bridge.</param>
        /// <param name="height">The maximum height of the bridge.</param>
        /// <returns>The Vector3[] points for the bridge.</returns>
        public static Vector3[] Bridge(Vector3 origin, Vector3 target, int resolution, float height) {
            Vector3[] points = new Vector3[resolution];

            Vector3 dirToTarget = (target - origin).normalized;

            float theta = 0f;
            float horizontalIncrement = 2 * Mathf.PI / resolution;
            float resolutionToDistance = resolution * .5f - 1;
            float distanceIncrement = Vector3.Distance(target, origin) / resolutionToDistance;

            int k = 0;

            for (float i = 0; i < resolution; i += distanceIncrement) {
                float y = Mathf.Sin(theta);

                if (y < 0)
                    break;

                Vector3 point = new Vector3 {
                    x = dirToTarget.x * i,
                    y = y * height,
                    z = dirToTarget.z * i
                };

                points[k] = point;
                theta += horizontalIncrement;

                k++;
            }

            return points;
        }
    }
}
