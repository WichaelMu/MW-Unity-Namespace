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
        /// <param name="dirFace">The direction self is facing.</param>
        /// <param name="TSelf">The transform searching for target.</param>
        /// <param name="TTarget">The transform to look out for.</param>
        /// <param name="fSearchAngle">The maximum degrees to search for target.</param>
        public static bool InFOV(Direction dirFace, Transform TSelf, Transform TTarget, float fSearchAngle) {

            switch (dirFace) {
                case Direction.forward:
                    return Vector3.Angle(TSelf.forward, TTarget.position - TSelf.position) < fSearchAngle;
                case Direction.right:
                    return Vector3.Angle(TSelf.right, TTarget.position - TSelf.position) < fSearchAngle;
                case Direction.back:
                    return Vector3.Angle(-TSelf.forward, TTarget.position - TSelf.position) < fSearchAngle;
                case Direction.left:
                    return Vector3.Angle(-TSelf.right, TTarget.position - TSelf.position) < fSearchAngle;
                case Direction.up:
                    return Vector3.Angle(TSelf.up, TTarget.position - TSelf.position) < fSearchAngle;
                case Direction.down:
                    return Vector3.Angle(-TSelf.up, TTarget.position - TSelf.position) < fSearchAngle;
                default:
                    Debug.LogWarning("There was a problem in determining a face direction");
                    return false;
            }
        }

        /// <summary>If self can see target within SearchAngle degrees while facing face.</summary>
        /// <param name="dirFace">The direction self is facing.</param>
        /// <param name="TSelf">The transform searching for target.</param>
        /// <param name="vTarget">The position to look out for.</param>
        /// <param name="fSearchAngle">The maximum degrees to search for target.</param>
        public static bool InFOV(Direction dirFace, Transform TSelf, Vector3 vTarget, float fSearchAngle) {

            switch (dirFace) {
                case Direction.forward:
                    return Vector3.Angle(TSelf.forward, vTarget - TSelf.position) < fSearchAngle;
                case Direction.right:
                    return Vector3.Angle(TSelf.right, vTarget - TSelf.position) < fSearchAngle;
                case Direction.back:
                    return Vector3.Angle(-TSelf.forward, vTarget - TSelf.position) < fSearchAngle;
                case Direction.left:
                    return Vector3.Angle(-TSelf.right, vTarget - TSelf.position) < fSearchAngle;
                case Direction.up:
                    return Vector3.Angle(TSelf.up, vTarget - TSelf.position) < fSearchAngle;
                case Direction.down:
                    return Vector3.Angle(-TSelf.up, vTarget - TSelf.position) < fSearchAngle;
                default:
                    Debug.LogWarning("There was a problem in determining a face direction");
                    return false;
            }
        }

        /// <summary>If self has a line of sight to to.</summary>
        /// <param name="vSelf">The position to look from.</param>
        /// <param name="vTo">The position to look to.</param>
        /// <param name="lmObstacles">The obstacles to consider obtrusive.</param>
        public static bool LineOfSight(Vector3 vSelf, Vector3 vTo, LayerMask lmObstacles) {
            return !Physics.Linecast(vSelf, vTo, lmObstacles);
        }

        /// <summary>If self has a line of sight to to.</summary>
        public static bool LineOfSight(Vector3 vSelf, Vector3 vTo) {
            return !Physics.Linecast(vSelf, vTo);
        }

        ///<summary>The fValue rounded to dp decimal places.</summary>
        /// <param name="fValue">The value to be rounded.</param>
        /// <param name="nDP">The decimal places to be included.</param>
        public static float RoundToDP(float fValue, int nDP = 2) {
            if (nDP == 0) {
                Debug.LogWarning("Use Mathf.RoundToInt(" + nameof(fValue) + ") instead.");
                return Mathf.RoundToInt(fValue);
            }

            if (nDP < 0) {
                Debug.LogWarning("Please use a number greater than 0. Rounding to 2 instead.");
                nDP = 2;
            }

            int fFactor = (int)Mathf.Pow(10, nDP);
            return Mathf.Round(fValue * fFactor) / fFactor;
        }

        /// <summary>Flip-Flops Bool.</summary>
        /// <param name="bBool"></param>
        public static void FlipFlop(ref bool bBool) {
            bBool = !bBool;
        }

        /// <summary>Flip-Flops Bool.</summary>
        /// <param name="bBool"></param>
        /// <param name="ACallbackTrue">The method to call if the flip-flop is true.</param>
        /// <param name="ACallbackFalse">The method to call if the flip-flop is false.</param>
        public static void FlipFlop(ref bool bBool, Action ACallbackTrue, Action ACallbackFalse) {
            bBool = !bBool;

            if (bBool)
                ACallbackTrue();
            else
                ACallbackFalse();
        }

        /// <summary>If value is within the +- limit of from.</summary>
        /// <param name="fValue">The value to check.</param>
        /// <param name="fFrom">The value to compare.</param>
        /// <param name="fLimit">The limits to consider.</param>
        public static bool IsWithin(float fValue, float fFrom, float fLimit) {
            if (fLimit == 0) {
                Debug.LogWarning("Use the '== 0' comparison operator instead.");
                return fValue == fFrom;
            }
            if (fLimit < 0) {
                Debug.LogWarning("Please use a positive number");
                fLimit = Mathf.Abs(fLimit);
            }

            return (fFrom + fLimit > fValue) && (fValue > fFrom - fLimit);
        }

        /// <summary>The largest vector between L and R, according to magnitude.</summary>
        /// <param name="vL"></param>
        /// <param name="vR"></param>
        public static Vector3 Max(Vector3 vL, Vector3 vR) {
            return (vL.magnitude < vR.magnitude) ? vR : vL;
        }

        /// <summary>The smallest vector vector between L and R, according to magnitude.</summary>
        /// <param name="vL"></param>
        /// <param name="vR"></param>
        public static Vector3 Min(Vector3 vL, Vector3 vR) {
            return (vL.magnitude > vR.magnitude) ? vR : vL;
        }

        /// <summary>Returns the n'th Fibonacci number.</summary>
        /// <param name="n"></param>
        public static int Fibonacci(int n) {
            if (n <= 2)
                return 1;
            return Fibonacci(n - 1) + Fibonacci(n - 2);
        }

        /// <summary>Generates spherical points with an equal distribution.</summary>
        /// <param name="nResolution">The number of points to generate.</param>
        /// <param name="fGoldenRationModifier">Adjusts the golden ratio.</param>
        /// <returns>The Vector3[] points for the sphere.</returns>
        public static Vector3[] GenerateEqualSphere(int nResolution, float fGoldenRationModifier) {
            Vector3[] vDirections = new Vector3[nResolution];

            float fPhi = (1 + Mathf.Sqrt(fGoldenRationModifier) * .5f);
            float fInc = Mathf.PI * 2 * fPhi;

            for (int i = 0; i < nResolution; i++) {
                float t = (float)i / nResolution;
                float incline = Mathf.Acos(1 - 2 * t);
                float azimuth = fInc * i;

                float x = Mathf.Sin(incline) * Mathf.Cos(azimuth);
                float y = Mathf.Sin(incline) * Mathf.Sin(azimuth);
                float z = Mathf.Cos(incline);

                vDirections[i] = new Vector3(x, y, z);
            }

            return vDirections;
        }

        /// <summary>Generates the points to 'bridge' origin and target together at a height as an arc.</summary>
        /// <param name="vOrigin">The starting point of the bridge.</param>
        /// <param name="vTarget">The ending point of the bridge.</param>
        /// <param name="nResolution">The number of points for the bridge.</param>
        /// <param name="fHeight">The maximum height of the bridge.</param>
        /// <returns>The Vector3[] points for the bridge.</returns>
        public static Vector3[] Bridge(Vector3 vOrigin, Vector3 vTarget, int nResolution, float fHeight) {
            Vector3[] points = new Vector3[nResolution];

            Vector3 dirToTarget = (vTarget - vOrigin).normalized;

            float theta = 0f;
            float horizontalIncrement = 2 * Mathf.PI / nResolution;
            float resolutionToDistance = nResolution * .5f - 1;
            float distanceIncrement = Vector3.Distance(vTarget, vOrigin) / resolutionToDistance;

            int k = 0;

            for (float i = 0; i < nResolution; i += distanceIncrement) {
                float y = Mathf.Sin(theta);

                if (y < 0)
                    break;

                Vector3 point = new Vector3 {
                    x = dirToTarget.x * i,
                    y = y * fHeight,
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
