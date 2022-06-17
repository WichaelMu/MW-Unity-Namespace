using System;
using UnityEngine;

namespace MW
{
	/// <summary>Helper Variables and Functions.</summary>
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
		public const float kTwoThirds = .6666666666666666666666666666666666f;
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

		/// <summary>If Self can see Target within SearchAngle degrees while facing EDirection.</summary>
		/// <param name="Face">The EDirection self is facing.</param>
		/// <param name="Self">The Transform searching for target.</param>
		/// <param name="Target">The Transform to look out for.</param>
		/// <param name="SearchAngle">The maximum degrees to search for target.</param>
		public static bool InFOV(EDirection Face, Transform Self, Transform Target, float SearchAngle)
		{

			switch (Face)
			{
				case EDirection.Forward:
					return Vector3.Angle(Self.forward, Target.position - Self.position) < SearchAngle;
				case EDirection.Right:
					return Vector3.Angle(Self.right, Target.position - Self.position) < SearchAngle;
				case EDirection.Back:
					return Vector3.Angle(-Self.forward, Target.position - Self.position) < SearchAngle;
				case EDirection.Left:
					return Vector3.Angle(-Self.right, Target.position - Self.position) < SearchAngle;
				case EDirection.Up:
					return Vector3.Angle(Self.up, Target.position - Self.position) < SearchAngle;
				case EDirection.Down:
					return Vector3.Angle(-Self.up, Target.position - Self.position) < SearchAngle;
				default:
					Debug.LogWarning("There was a problem in determining a face direction");
					return false;
			}
		}

		/// <summary>If Self can see Target within SearchAngle degrees while facing EDirection.</summary>
		/// <param name="Face">The EDirection self is facing.</param>
		/// <param name="Self">The Transform searching for target.</param>
		/// <param name="Target">The Vector3 position to look out for.</param>
		/// <param name="SearchAngle">The maximum degrees to search for target.</param>
		public static bool InFOV(EDirection Face, Transform Self, Vector3 Target, float SearchAngle)
		{

			switch (Face)
			{
				case EDirection.Forward:
					return Vector3.Angle(Self.forward, Target - Self.position) < SearchAngle;
				case EDirection.Right:
					return Vector3.Angle(Self.right, Target - Self.position) < SearchAngle;
				case EDirection.Back:
					return Vector3.Angle(-Self.forward, Target - Self.position) < SearchAngle;
				case EDirection.Left:
					return Vector3.Angle(-Self.right, Target - Self.position) < SearchAngle;
				case EDirection.Up:
					return Vector3.Angle(Self.up, Target - Self.position) < SearchAngle;
				case EDirection.Down:
					return Vector3.Angle(-Self.up, Target - Self.position) < SearchAngle;
				default:
					Debug.LogWarning("There was a problem in determining a face direction");
					return false;
			}
		}

		/// <summary>If Self has an unobstructed line of sight to To.</summary>
		/// <param name="Self">The Vector3 position to look from.</param>
		/// <param name="To">The Vector3 position to look to.</param>
		/// <param name="Obstacles">The LayerMask obstacles to consider obtrusive.</param>
		public static bool LineOfSight(Vector3 Self, Vector3 To, LayerMask Obstacles)
		{
			return !Physics.Linecast(Self, To, Obstacles);
		}

		/// <summary>If Self has an unobstructed line of sight to To.</summary>
		/// <param name="Self">The Vector3 position to look from.</param>
		/// <param name="To">The Vector3 position to look to.</param>
		public static bool LineOfSight(Vector3 Self, Vector3 To)
		{
			return !Physics.Linecast(Self, To);
		}

		///<summary>The Value rounded to DecimalPlaces.</summary>
		/// <param name="Value">The value to be rounded.</param>
		/// <param name="DecimalPlaces">The decimal places to be included.</param>
		public static float RoundToDP(float Value, int DecimalPlaces = 2)
		{
			if (DecimalPlaces == 0)
			{
				Debug.LogWarning("Use Mathf.RoundToInt(" + nameof(Value) + ") instead.");
				return Mathf.RoundToInt(Value);
			}

			if (DecimalPlaces <= 0)
			{
				Debug.LogWarning("Please use a number greater than 0. Rounding to 2 instead.");
				DecimalPlaces = 2;
			}

			float fFactor = Mathf.Pow(10, DecimalPlaces);
			return Mathf.Round(Value * fFactor) / fFactor;
		}

		/// <summary>Flip-Flops bBool.</summary>
		/// <param name="bBool"></param>
		public static void FlipFlop(ref bool bBool)
		{
			bBool = !bBool;
		}

		/// <summary>Flip-Flops bBool.</summary>
		/// <param name="bBool"></param>
		/// <param name="CallbackTrue">The method to call if the flip-flop is true.</param>
		/// <param name="CallbackFalse">The method to call if the flip-flop is false.</param>
		public static void FlipFlop(ref bool bBool, Action CallbackTrue, Action CallbackFalse)
		{
			bBool = !bBool;

			if (bBool)
				CallbackTrue();
			else
				CallbackFalse();
		}

		/// <summary>If Value is within the +- Limit of From.</summary>
		/// <param name="Value">The value to check.</param>
		/// <param name="From">The value to compare.</param>
		/// <param name="Limit">The limits to consider.</param>
		public static bool IsWithin(float Value, float From, float Limit)
		{
			if (Limit == 0)
			{
				Debug.LogWarning("Use the '== 0' comparison operator instead.");
				return Value == From;
			}
			if (Limit < 0)
			{
				Debug.LogWarning("Please use a positive number");
				Limit = Mathf.Abs(Limit);
			}

			return From + Limit > Value && Value > From - Limit;
		}

		/// <summary>The largest Vector3 between L and R, according to <see cref="Vector3.sqrMagnitude"/>.</summary>
		/// <docs>The largest Vector3 between L and R, according to Vector3.sqrMagnitude.</docs>
		/// <param name="L"></param>
		/// <param name="R"></param>
		public static Vector3 Max(Vector3 L, Vector3 R)
		{
			return L.sqrMagnitude < R.sqrMagnitude ? R : L;
		}

		/// <summary>The largest Vector3 between L and R, according to <see cref="Vector3.sqrMagnitude"/>.</summary>
		/// <docs>The largest Vector3 between L and R, according to Vector3.sqrMagnitude.</docs>
		/// <param name="L"></param>
		/// <param name="R"></param>
		public static Vector3 Min(Vector3 L, Vector3 R)
		{
			return L.sqrMagnitude > R.sqrMagnitude ? R : L;
		}

		static int[] fib_dp;

		/// <summary>Returns the N'th Fibonacci number.</summary>
		/// <param name="N"></param>
		public static int Fibonacci(int N)
		{
			if (fib_dp == null)
				fib_dp = new int[int.MaxValue];
			else if (fib_dp[N] != 0)
				return fib_dp[N];
			if (N <= 2)
				return 1;

			fib_dp[N] = Fibonacci(N - 1) + Fibonacci(N - 2);

			return fib_dp[N];
		}

		/// <summary>Generates spherical points with an equal distribution.</summary>
		/// <param name="Resolution">The number of points to generate.</param>
		/// <param name="GoldenRatioModifier">Adjusts the golden ratio.</param>
		/// <returns>The Vector3[] points for the sphere.</returns>
		public static Vector3[] GenerateEqualSphere(int Resolution, float GoldenRatioModifier)
		{
			Vector3[] vDirections = new Vector3[Resolution];

			float fPhi = 1 + Mathf.Sqrt(GoldenRatioModifier) * .5f;
			float fInc = Mathf.PI * 2 * fPhi;

			for (int i = 0; i < Resolution; i++)
			{
				float t = (float)i / Resolution;
				float incline = Mathf.Acos(1 - 2 * t);
				float azimuth = fInc * i;

				float x = Mathf.Sin(incline) * Mathf.Cos(azimuth);
				float y = Mathf.Sin(incline) * Mathf.Sin(azimuth);
				float z = Mathf.Cos(incline);

				vDirections[i] = new Vector3(x, y, z);
			}

			return vDirections;
		}

		/// <summary>Generates the points to 'bridge' Origin and Target together at a Height as an arc.</summary>
		/// <param name="Origin">The Vector3 starting point of the bridge.</param>
		/// <param name="Target">The Vector3 ending point of the bridge.</param>
		/// <param name="Resolution">The number of points for the bridge.</param>
		/// <param name="Height">The maximum height of the bridge.</param>
		/// <returns>The Vector3[] points for the bridge.</returns>
		public static Vector3[] Bridge(Vector3 Origin, Vector3 Target, int Resolution, float Height)
		{
			Vector3[] points = new Vector3[Resolution];

			Vector3 dirToTarget = (Target - Origin).normalized;

			float theta = 0f;
			float horizontalIncrement = 2 * Mathf.PI / Resolution;
			float resolutionToDistance = Resolution * .5f - 1;
			float distanceIncrement = Vector3.Distance(Target, Origin) / resolutionToDistance;

			int k = 0;

			for (float i = 0; i < Resolution; i += distanceIncrement)
			{
				float y = Mathf.Sin(theta);

				if (y < 0)
					break;

				Vector3 point = new Vector3
				{
					x = dirToTarget.x * i,
					y = y * Height,
					z = dirToTarget.z * i
				};

				points[k] = point;
				theta += horizontalIncrement;

				k++;
			}

			return points;
		}

		public static string AsString(object Convert) => Convert.ToString();

		/// <summary>Mirrors Number about Minimum and Maximum, inclusive.</summary>
		/// <param name="Number">The number to anchor a reflection.</param>
		/// <param name="Minimum">The minimum number that can be reflected.</param>
		/// <param name="Maximum">The maximum number that can be reflected.</param>
		/// <returns>The reflected number.</returns>
		public static float MirrorNumber(float Number, float Minimum, float Maximum) => Minimum + Maximum - Number;

		/// <summary>Mirrors Number about Minimum and Maximum, inclusive. Not to be confused with <see cref="MArray{T}.Reflect(int, int)"/>.</summary>
		/// <param name="Number">The number to anchor a reflection.</param>
		/// <param name="Minimum">The minimum number that can be reflected.</param>
		/// <param name="Maximum">The maximum number that can be reflected.</param>
		/// <returns>The reflected number.</returns>
		public static int MirrorNumber(int Number, int Minimum, int Maximum) => Minimum + Maximum - Number;
	}
}
