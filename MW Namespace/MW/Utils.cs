using System;
using UnityEngine;
using MW.Math.Magic;

namespace MW
{
	/// <summary>Helper Variables and Functions.</summary>
	/// <decorations decor="public static class"></decorations>
	public static class Utils
	{
		/// <summary>Shorthand for writing / 3.</summary>
		/// <decorations decor="public const float"></decorations>
		public const float kOneThird = .3333333333333333333333333333333333f;
		/// <summary>Shorthand for writing / 1.5.</summary>
		/// <decorations decor="public const float"></decorations>
		public const float kTwoThirds = .6666666666666666666666666666666666f;
		/// <summary>The golden ratio.</summary>
		/// <decorations decor="public const float"></decorations>
		public const float kPhi = 1.6180339887498948482045868343656381f;
		/// <summary>Euler's number. (e)</summary>
		/// <decorations decor="public const float"></decorations>
		public const float kE = 2.71828182845904523536f;
		/// <summary>Shorthand for writing UnityEngine.Mathf.Sqrt(2).</summary>
		/// <decorations decor="public const float"></decorations>
		public const float kSqrt2 = 1.4142135623730950488016887242097f;
		/// <summary>Shorthand for writing UnityEngine.Mathf.Sqrt(3).</summary>
		/// <decorations decor="public const float"></decorations>
		public const float kSqrt3 = 1.7320508075688772935274463415059f;
		/// <summary>Shorthand for writing 1 / Mathf.PI.</summary>
		/// <decorations decor="public const float"></decorations>
		public const float kInversePI = .31830988618379067153776752674503f;
		/// <summary>Shorthand for writing Mathf.PI * kHalf.</summary>
		/// <decorations decor="public const float"></decorations>
		public const float kHalfPI = 1.5707963267948966192313216916398f;
		/// <summary>Shorthand for writing Mathf.PI * 2f</summary>
		/// <decorations decor="public const float"></decorations>
		public const float k2PI = 6.283185307179586476925286766559f;
		/// <summary>The conversion from 0-1 to 0-255.</summary>
		/// <decorations decor="public const float"></decorations>
		public const float k1To255RGB = 0.0039215686274509803921568627451F;

		/// <summary>If Self can see Target within SearchAngle degrees while facing EDirection.</summary>
		/// <decorations decor="public static bool"></decorations>
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
		/// <decorations decor="public static bool"></decorations>
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
		/// <decorations decor="public static bool"></decorations>
		/// <param name="Self">The Vector3 position to look from.</param>
		/// <param name="To">The Vector3 position to look to.</param>
		/// <param name="Obstacles">The LayerMask obstacles to consider obtrusive.</param>
		public static bool LineOfSight(Vector3 Self, Vector3 To, LayerMask Obstacles)
		{
			return !Physics.Linecast(Self, To, Obstacles);
		}

		/// <summary>If Self has an unobstructed line of sight to To.</summary>
		/// <decorations decor="public static bool"></decorations>
		/// <param name="Self">The Vector3 position to look from.</param>
		/// <param name="To">The Vector3 position to look to.</param>
		public static bool LineOfSight(Vector3 Self, Vector3 To)
		{
			return !Physics.Linecast(Self, To);
		}

		///<summary>The Value rounded to DecimalPlaces.</summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Value">The value to be rounded.</param>
		/// <param name="DecimalPlaces">The decimal places to be included.</param>
		public static float RoundToDP(float Value, int DecimalPlaces = 2)
		{
			if (DecimalPlaces == 0)
			{
				return Mathf.RoundToInt(Value);
			}

			float fFactor = Mathf.Pow(10, DecimalPlaces);
			return Mathf.Round(Value * fFactor) / fFactor;
		}

		/// <summary>Flip-Flops bBool.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="bBool"></param>
		public static void FlipFlop(ref bool bBool)
		{
			bBool = !bBool;
		}

		/// <summary>Flip-Flops bBool.</summary>
		/// <decorations decor="public static void"></decorations>
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
		/// <decorations decor="public static bool"></decorations>
		/// <param name="Value">The value to check.</param>
		/// <param name="From">The value to compare.</param>
		/// <param name="Limit">The limits to consider.</param>
		public static bool IsWithin(float Value, float From, float Limit)
		{
			if (Limit == 0)
			{
				//Debug.LogWarning("Use the '== 0' comparison operator instead.");
				return Value == From;
			}
			if (Limit < 0)
			{
				//Debug.LogWarning("Please use a positive number");
				Limit = Mathf.Abs(Limit);
			}

			return From + Limit > Value && Value > From - Limit;
		}

		/// <summary>The largest Vector3 between L and R, according to <see cref="Vector3.sqrMagnitude"/>.</summary>
		/// <docs>The largest Vector3 between L and R, according to Vector3.sqrMagnitude.</docs>
		/// <decorations decor="public static Vector3"></decorations>
		/// <param name="L"></param>
		/// <param name="R"></param>
		public static Vector3 Max(Vector3 L, Vector3 R)
		{
			return L.sqrMagnitude < R.sqrMagnitude ? R : L;
		}

		/// <summary>The largest Vector3 between L and R, according to <see cref="Vector3.sqrMagnitude"/>.</summary>
		/// <docs>The largest Vector3 between L and R, according to Vector3.sqrMagnitude.</docs>
		/// <decorations decor="public static Vector3"></decorations>
		/// <param name="L"></param>
		/// <param name="R"></param>
		public static Vector3 Min(Vector3 L, Vector3 R)
		{
			return L.sqrMagnitude > R.sqrMagnitude ? R : L;
		}

		static int[] fib_dp;

		/// <summary>Returns the N'th Fibonacci number.</summary>
		/// <decorations decor="public static int"></decorations>
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
		/// <decorations decor="public static Vector3[]"></decorations>
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
		/// <decorations decor="public static Vector3[]"></decorations>
		/// <param name="Origin">The Vector3 starting point of the bridge.</param>
		/// <param name="Target">The Vector3 ending point of the bridge.</param>
		/// <param name="Resolution">The number of points for the bridge.</param>
		/// <param name="Height">The maximum height of the bridge.</param>
		/// <returns>The Vector3[] points for the bridge.</returns>
		public static Vector3[] Bridge(Vector3 Origin, Vector3 Target, int Resolution, float Height)
		{
			Vector3[] points = new Vector3[Resolution];

			Vector3 DirectionToTarget = (Target - Origin).normalized;

			float Theta = 0f;
			float HorizontalIncrement = 2 * Mathf.PI / Resolution;
			float ResolutionToDistance = Resolution * .5f - 1;
			float DistanceIncrement = Vector3.Distance(Target, Origin) / ResolutionToDistance;

			int k = 0;

			for (float i = 0; i < Resolution; i += DistanceIncrement)
			{
				float y = Mathf.Sin(Theta);

				if (y < 0)
					break;

				Vector3 point = new Vector3
				{
					x = DirectionToTarget.x * i,
					y = y * Height,
					z = DirectionToTarget.z * i
				};

				points[k] = point;
				Theta += HorizontalIncrement;

				k++;
			}

			return points;
		}

		public static string AsString(object Convert) => Convert.ToString();

		/// <summary>Mirrors Number about Minimum and Maximum, inclusive.</summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Number">The number to anchor a reflection.</param>
		/// <param name="Minimum">The minimum number that can be reflected.</param>
		/// <param name="Maximum">The maximum number that can be reflected.</param>
		/// <returns>The reflected number.</returns>
		public static float MirrorNumber(float Number, float Minimum, float Maximum) => Minimum + Maximum - Number;

		/// <summary>Mirrors Number about Minimum and Maximum, inclusive. Not to be confused with <see cref="MArray{T}.Reflect(int, int)"/>.</summary>
		/// <decorations decor="public static int"></decorations>
		/// <param name="Number">The number to anchor a reflection.</param>
		/// <param name="Minimum">The minimum number that can be reflected.</param>
		/// <param name="Maximum">The maximum number that can be reflected.</param>
		/// <returns>The reflected number.</returns>
		public static int MirrorNumber(int Number, int Minimum, int Maximum) => Minimum + Maximum - Number;

		/// <summary>Swaps L and R.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="L"></param>
		/// <param name="R"></param>
		public static void Swap<T>(ref T L, ref T R)
		{
			(R, L) = (L, R);
		}

		/// <summary>Clamps I to be between Min and Max.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="I">A reference to the Integer to clamp.</param>
		/// <param name="Min">The Minimum value of I.</param>
		/// <param name="Max">The Maximum value of I.</param>
		public static void Clamp(ref int I, int Min, int Max)
		{
			if (I < Min)
			{
				I = Min;
			}
			else if (I > Max)
			{
				I = Max;
			}
		}

		/// <summary>Clamps I to not fall below Min.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="I">A reference to the Integer to clamp.</param>
		/// <param name="Min">The Minimum value I can be.</param>
		public static void ClampMin(ref int I, int Min)
		{
			if (I < Min)
				I = Min;
		}

		/// <summary>Clamps I to not exceed Max.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="I">A reference to the Integer to clamp.</param>
		/// <param name="Max">The Maximum value I can be.</param>
		public static void ClampMax(ref int I, int Max)
		{
			if (I > Max)
				I = Max;
		}

		/// <summary>Clamps F to not fall below Min.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="F">A reference to the Float to clamp.</param>
		/// <param name="Min">The Minimum value F can be.</param>
		public static void ClampMin(ref float F, float Min)
		{
			if (F < Min)
				F = Min;
		}

		/// <summary>Clamps F to not exceed Max.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="F">A reference to the Float to clamp.</param>
		/// <param name="Max">The Maximum value F can be.</param>
		public static void ClampMax(ref float F, float Max)
		{
			if (F > Max)
				F = Max;
		}

		/// <summary>Clamps F to be between Min and Max.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="F">A reference to the Float to clamp.</param>
		/// <param name="Min">The Minimum value of F.</param>
		/// <param name="Max">The Maximum value of F.</param>
		public static void Clamp(ref float F, float Min, float Max)
		{
			if (F < Min)
			{
				F = Min;
			}
			else if (F > Max)
			{
				F = Max;
			}
		}

		/// <summary>The larger value between F1 and F2.</summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="F1"></param>
		/// <param name="F2"></param>
		/// <returns>The larger of the two given floats.</returns>
		public static float Max(float F1, float F2)
		{
			return F1 < F2 ? F2 : F1;
		}

		/// <summary>The smaller value between F1 and F2.</summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="F1"></param>
		/// <param name="F2"></param>
		/// <returns>The smaller of the two given floats.</returns>
		public static float Min(float F1, float F2)
		{
			return F1 < F2 ? F1 : F2;
		}


		/// <summary>Modifies I to be its Absolute Value.</summary>
		/// <remarks><see langword="ref"/> version of <see cref="Fast.Abs(int)"/>.</remarks>
		/// <docremarks>Ref&amp; version of FastAbs().</docremarks>
		/// <decorations decor="public static void"></decorations>
		/// <param name="I">A reference to the int to modify.</param>
		public static void Abs(ref int I) => I = Fast.Abs(I);


		/// <summary>Modifies F to be its Absolute Value.</summary>
		/// <remarks><see langword="ref"/> version of <see cref="Fast.Abs(float)"/></remarks>
		/// <docremarks>Ref&amp; version of FastAbs().</docremarks>
		/// <decorations decor="public static void"></decorations>
		/// <param name="F">A reference to the float to modify.</param>
		public static void Abs(ref float F) => F = Fast.Abs(F);

		/// <summary><see cref="Fast.Abs(float)"/> on all V's components.</summary>
		/// <docs>Abs() on all V's components.</docs>
		/// <param name="V">The Vector to Abs.</param>
		/// <returns>An MVector with all positive components.</returns>
		public static MVector Abs(MVector V)
		{
			return new MVector
			{
				X = Fast.Abs(V.X),
				Y = Fast.Abs(V.Y),
				Z = Fast.Abs(V.Z)
			};
		}

		/// <summary>Modifies V to be positive on all components.</summary>
		/// <param name="V">The Vector to modify.</param>
		public static void Abs(ref MVector V)
		{
			Abs(ref V.X);
			Abs(ref V.Y);
			Abs(ref V.Z);
		}

		/// <summary>Locks or unlocks the Cursor and optionally hide it.</summary>
		/// <remarks>Unlocking the cursor will always enable the Cursor's visibility.</remarks>
		/// <decorations decor="public static void"></decorations>
		/// <param name="bLockCursor">True to lock the Cursor.</param>
		/// <param name="bHideCursorOnLock">True to hide the Cursor when it's locked.</param>
		public static void LockCursor(bool bLockCursor, bool bHideCursorOnLock = true)
		{
			if (bLockCursor)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = bHideCursorOnLock;
			}
			else
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		public static int FPS() => (int)(1f / Time.unscaledDeltaTime);
	}
}
