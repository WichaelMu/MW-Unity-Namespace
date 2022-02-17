using System;
using UnityEngine;

namespace MW
{
	public static class Utils
	{
		/// <summary>Shorthand for writing / 1000. (Always faster to multiply than to divide)</summary>
		public const float kThousandth = .001f;
		/// <summary>Shorthand for writing / 100. (Always faster to multiply than to divide)</summary>
		public const float kHundreth = .01f;
		/// <summary>Shorthand for writing / 10. (Always faster to multiply than to divide)</summary>
		public const float k10Percent = .1f;
		/// <summary>Shorthand for writing / 4. (Always faster to multiply than to divide)</summary>
		public const float kQuarter = .25f;
		/// <summary>Shorthand for writing / 2. (Always faster to multiply than to divide)</summary>
		public const float kHalf = .5f;
		/// <summary>Shorthand for writing / 3. (Always faster to multiply than to divide)</summary>
		public const float kOneThird = .3333333333333333333333333333333333f;
		/// <summary>Shorthand for writing 1.6 recurring. (Always faster to multiply than to divide)</summary>
		public const float kTwoThirds = kOneThird * 2;
		/// <summary>The golden ratio.</summary>
		public const float kPhi = 1.6180339887498948482045868343656381f;
		/// <summary>Euler's number. (e)</summary>
		public const float kE = 2.71828182845904523536f;
		/// <summary>Shorthand for writing UnityEngine.Mathf.Sqrt(2). (Always faster to multiply than to divide)</summary>
		public const float kSqrt2 = 1.4142135623730950488016887242097f;
		/// <summary>Shorthand for writing UnityEngine.Mathf.Sqrt(3). (Always faster to multiply than to divide)</summary>
		public const float kSqrt3 = 1.7320508075688772935274463415059f;
		/// <summary>Shorthand for writing 1 / Mathf.PI.</summary>
		public const float kInversePI = .31830988618379067153776752674503f;
		/// <summary>Shorthand for writing Mathf.PI * kHalf.</summary>
		public const float kHalfPI = 1.5707963267948966192313216916398f;

		/// <summary>The ratio between 1 and 255.</summary>
		public const float k1To255RGB = 0.0039215686274509803921568627451F;

		/// <summary>If self can see Transform target within SearchAngle degrees while facing EDirection.</summary>
		/// <param name="dirFace">The EDirection self is facing.</param>
		/// <param name="ASelf">The Transform searching for target.</param>
		/// <param name="ATarget">The Transform to look out for.</param>
		/// <param name="fSearchAngle">The maximum degrees to search for target.</param>
		public static bool InFOV(EDirection dirFace, Transform ASelf, Transform ATarget, float fSearchAngle)
		{

			switch (dirFace)
			{
				case EDirection.Forward:
					return Vector3.Angle(ASelf.forward, ATarget.position - ASelf.position) < fSearchAngle;
				case EDirection.Right:
					return Vector3.Angle(ASelf.right, ATarget.position - ASelf.position) < fSearchAngle;
				case EDirection.Back:
					return Vector3.Angle(-ASelf.forward, ATarget.position - ASelf.position) < fSearchAngle;
				case EDirection.Left:
					return Vector3.Angle(-ASelf.right, ATarget.position - ASelf.position) < fSearchAngle;
				case EDirection.Up:
					return Vector3.Angle(ASelf.up, ATarget.position - ASelf.position) < fSearchAngle;
				case EDirection.Down:
					return Vector3.Angle(-ASelf.up, ATarget.position - ASelf.position) < fSearchAngle;
				default:
					Debug.LogWarning("There was a problem in determining a face direction");
					return false;
			}
		}

		/// <summary>If self can see Transform target within SearchAngle degrees while facing EDirection.</summary>
		/// <param name="dirFace">The EDirection self is facing.</param>
		/// <param name="ASelf">The Transform searching for target.</param>
		/// <param name="vTarget">The Vector3 position to look out for.</param>
		/// <param name="fSearchAngle">The maximum degrees to search for target.</param>
		public static bool InFOV(EDirection dirFace, Transform ASelf, Vector3 vTarget, float fSearchAngle)
		{

			switch (dirFace)
			{
				case EDirection.Forward:
					return Vector3.Angle(ASelf.forward, vTarget - ASelf.position) < fSearchAngle;
				case EDirection.Right:
					return Vector3.Angle(ASelf.right, vTarget - ASelf.position) < fSearchAngle;
				case EDirection.Back:
					return Vector3.Angle(-ASelf.forward, vTarget - ASelf.position) < fSearchAngle;
				case EDirection.Left:
					return Vector3.Angle(-ASelf.right, vTarget - ASelf.position) < fSearchAngle;
				case EDirection.Up:
					return Vector3.Angle(ASelf.up, vTarget - ASelf.position) < fSearchAngle;
				case EDirection.Down:
					return Vector3.Angle(-ASelf.up, vTarget - ASelf.position) < fSearchAngle;
				default:
					Debug.LogWarning("There was a problem in determining a face direction");
					return false;
			}
		}

		/// <summary>If self has an unobstructed line of sight to to.</summary>
		/// <param name="vSelf">The Vector3 position to look from.</param>
		/// <param name="vTo">The Vector3 position to look to.</param>
		/// <param name="lmObstacles">The LayerMask obstacles to consider obtrusive.</param>
		public static bool LineOfSight(Vector3 vSelf, Vector3 vTo, LayerMask lmObstacles)
		{
			return !Physics.Linecast(vSelf, vTo, lmObstacles);
		}

		/// <summary>If Vector3 self has an unobstructed line of sight to to.</summary>
		/// <param name="vSelf">The Vector3 position to look from.</param>
		/// <param name="vTo">The Vector3 position to look to.</param>
		public static bool LineOfSight(Vector3 vSelf, Vector3 vTo)
		{
			return !Physics.Linecast(vSelf, vTo);
		}

		///<summary>The fValue rounded to dp decimal places.</summary>
		/// <param name="fValue">The value to be rounded.</param>
		/// <param name="nDP">The decimal places to be included.</param>
		public static float RoundToDP(float fValue, int nDP = 2)
		{
			if (nDP == 0)
			{
				Debug.LogWarning("Use Mathf.RoundToInt(" + nameof(fValue) + ") instead.");
				return Mathf.RoundToInt(fValue);
			}

			if (nDP <= 0)
			{
				Debug.LogWarning("Please use a number greater than 0. Rounding to 2 instead.");
				nDP = 2;
			}

			float fFactor = Mathf.Pow(10, nDP);
			return Mathf.Round(fValue * fFactor) / fFactor;
		}

		/// <summary>Flip-Flops Bool.</summary>
		/// <param name="bBool"></param>
		public static void FlipFlop(ref bool bBool)
		{
			bBool = !bBool;
		}

		/// <summary>Flip-Flops Bool.</summary>
		/// <param name="bBool"></param>
		/// <param name="ACallbackTrue">The method to call if the flip-flop is true.</param>
		/// <param name="ACallbackFalse">The method to call if the flip-flop is false.</param>
		public static void FlipFlop(ref bool bBool, Action ACallbackTrue, Action ACallbackFalse)
		{
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
		public static bool IsWithin(float fValue, float fFrom, float fLimit)
		{
			if (fLimit == 0)
			{
				Debug.LogWarning("Use the '== 0' comparison operator instead.");
				return fValue == fFrom;
			}
			if (fLimit < 0)
			{
				Debug.LogWarning("Please use a positive number");
				fLimit = Mathf.Abs(fLimit);
			}

			return (fFrom + fLimit > fValue) && (fValue > fFrom - fLimit);
		}

		/// <summary>The largest Vector3 between L and R, according to Vector3.magnitude.</summary>
		/// <param name="vL"></param>
		/// <param name="vR"></param>
		public static Vector3 Max(Vector3 vL, Vector3 vR)
		{
			return (vL.magnitude < vR.magnitude) ? vR : vL;
		}

		/// <summary>The smallest Vector3 between L and R, according to Vector3.magnitude.</summary>
		/// <param name="vL"></param>
		/// <param name="vR"></param>
		public static Vector3 Min(Vector3 vL, Vector3 vR)
		{
			return (vL.magnitude > vR.magnitude) ? vR : vL;
		}

		static int[] fib_dp;

		/// <summary>Returns the n'th Fibonacci number.</summary>
		/// <param name="n"></param>
		public static int Fibonacci(int n)
		{
			if (fib_dp == null)
				fib_dp = new int[int.MaxValue];
			else if (fib_dp[n] != 0)
				return fib_dp[n];
			if (n <= 2)
				return 1;

			fib_dp[n] = Fibonacci(n - 1) + Fibonacci(n - 2);

			return fib_dp[n];
		}

		/// <summary>Generates spherical points with an equal distribution.</summary>
		/// <param name="nResolution">The number of points to generate.</param>
		/// <param name="fGoldenRationModifier">Adjusts the golden ratio.</param>
		/// <returns>The Vector3[] points for the sphere.</returns>
		public static Vector3[] GenerateEqualSphere(int nResolution, float fGoldenRationModifier)
		{
			Vector3[] vDirections = new Vector3[nResolution];

			float fPhi = (1 + Mathf.Sqrt(fGoldenRationModifier) * .5f);
			float fInc = Mathf.PI * 2 * fPhi;

			for (int i = 0; i < nResolution; i++)
			{
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
		/// <param name="vOrigin">The Vector3 starting point of the bridge.</param>
		/// <param name="vTarget">The Vector3 ending point of the bridge.</param>
		/// <param name="nResolution">The number of points for the bridge.</param>
		/// <param name="fHeight">The maximum height of the bridge.</param>
		/// <returns>The Vector3[] points for the bridge.</returns>
		public static Vector3[] Bridge(Vector3 vOrigin, Vector3 vTarget, int nResolution, float fHeight)
		{
			Vector3[] points = new Vector3[nResolution];

			Vector3 dirToTarget = (vTarget - vOrigin).normalized;

			float theta = 0f;
			float horizontalIncrement = 2 * Mathf.PI / nResolution;
			float resolutionToDistance = nResolution * .5f - 1;
			float distanceIncrement = Vector3.Distance(vTarget, vOrigin) / resolutionToDistance;

			int k = 0;

			for (float i = 0; i < nResolution; i += distanceIncrement)
			{
				float y = Mathf.Sin(theta);

				if (y < 0)
					break;

				Vector3 point = new Vector3
				{
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

		public static string AsString(object convert) => convert.ToString();

		/// <summary>Mirrors Number about Minimum and Maximum, inclusive.</summary>
		/// <param name="Number">The number to anchor a reflection.</param>
		/// <param name="Minimum">The minimum number that can be reflected.</param>
		/// <param name="Maximum">The maximum number that can be reflected.</param>
		/// <returns>The reflected number.</returns>
		public static float MirrorNumber(float Number, float Minimum, float Maximum) => (Minimum + Maximum) - Number;

		/// <summary>Mirrors Number about Minimum and Maximum, inclusive. Not to be confused with MArray{T}.Mirror(int, int).</summary>
		/// <param name="Number">The number to anchor a reflection.</param>
		/// <param name="Minimum">The minimum number that can be reflected.</param>
		/// <param name="Maximum">The maximum number that can be reflected.</param>
		/// <returns>The reflected number.</returns>
		public static int MirrorNumber(int Number, int Minimum, int Maximum) => (Minimum + Maximum) - Number;
	}
}
