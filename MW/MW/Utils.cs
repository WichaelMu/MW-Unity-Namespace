﻿using System;
using System.Collections;
using System.Runtime.CompilerServices;
using static MW.Math.Mathematics;
using static MW.Math.Magic.Fast;
using static MW.FMath;
#if RELEASE
using MW.Extensions;
using UnityEngine;
#endif // RELEASE

namespace MW
{
	/// <summary>Sorting delegate function.</summary>
	/// <typeparam name="T">Type to compare.</typeparam>
	/// <param name="L">Left T.</param>
	/// <param name="R">Right T.</param>
	/// <returns>1 if L > R. -1 if L &lt; R. 0 if L == R.</returns>
	public delegate int SortFunc<in T>(T L, T R);

	/// <summary>Helper Variables and Functions.</summary>
	/// <decorations decor="public static class"></decorations>
	public static class Utils
	{
#if RELEASE
		[Obsolete("This method has been deprecated due to limitations with EDirection. Use InFOV(Vector3, Vector3, Vector3, float) instead!")]
		public static bool InFOV(EDirection Face, Transform Self, Transform Target, float SearchAngle)
		{
			switch (Face)
			{
				case EDirection.Forward:
					return FAngle(Self.forward, (Target.position - Self.position).FNormalise()) < SearchAngle;
				case EDirection.Right:
					return FAngle(Self.right, (Target.position - Self.position).FNormalise()) < SearchAngle;
				case EDirection.Back:
					return FAngle(-Self.forward, (Target.position - Self.position).FNormalise()) < SearchAngle;
				case EDirection.Left:
					return FAngle(-Self.right, (Target.position - Self.position).FNormalise()) < SearchAngle;
				case EDirection.Up:
					return FAngle(Self.up, (Target.position - Self.position).FNormalise()) < SearchAngle;
				case EDirection.Down:
					return FAngle(-Self.up, (Target.position - Self.position).FNormalise()) < SearchAngle;
				default:
					Debug.LogWarning("There was a problem in determining a face direction");
					return false;
			}
		}

		[Obsolete("This method has been deprecated due to limitations with EDirection. Use InFOV(Vector3, Transform, Vector3, float) instead!")]
		public static bool InFOV(EDirection Face, Transform Self, Vector3 Target, float SearchAngle)
		{
			switch (Face)
			{
				case EDirection.Forward:
					return FAngle(Self.forward, (Target - Self.position).FNormalise()) < SearchAngle;
				case EDirection.Right:
					return FAngle(Self.right, (Target - Self.position).FNormalise()) < SearchAngle;
				case EDirection.Back:
					return FAngle(-Self.forward, (Target - Self.position).FNormalise()) < SearchAngle;
				case EDirection.Left:
					return FAngle(-Self.right, (Target - Self.position).FNormalise()) < SearchAngle;
				case EDirection.Up:
					return FAngle(Self.up, (Target - Self.position).FNormalise()) < SearchAngle;
				case EDirection.Down:
					return FAngle(-Self.up, (Target - Self.position).FNormalise()) < SearchAngle;
				default:
					Debug.LogWarning("There was a problem in determining a face direction");
					return false;
			}
		}

		/// <summary>If Self can see Target within SearchAngle degrees while facing Face.</summary>
		/// <decorations decor="public static bool"></decorations>
		/// <param name="Face">The direction Self is facing.</param>
		/// <param name="Self">The Transform searching for Target.</param>
		/// <param name="Target">The position to look out for.</param>
		/// <param name="SearchAngle">The maximum degrees to search for Target.</param>
		/// <returns>True if Self can see Target within SearchAngle while facing Face.</returns>
		public static bool InFOV(Vector3 Face, Vector3 Self, Vector3 Target, float SearchAngle)
		{
			Vector3 Direction = (Target - Self).FNormalise();
			FVector FFace = FVector.Clone(ref Face);
			FVector FDirection = FVector.Clone(ref Direction);

			return FVector.Angle(FFace, FDirection) < SearchAngle;
		}

		/// <summary>If Self can see Target within SearchAngle degrees while facing Face.</summary>
		/// <decorations decor="public static bool"></decorations>
		/// <param name="Face">The direction Self is facing.</param>
		/// <param name="Self">The Transform searching for Target.</param>
		/// <param name="Target">The position to look out for.</param>
		/// <param name="SearchAngle">The maximum degrees to search for Target.</param>
		/// <returns>True if Self can see Target within SearchAngle while facing Face.</returns>
		public static bool InFOV(Vector3 Face, Transform Self, Vector3 Target, float SearchAngle) => InFOV(Face, Self.position, Target, SearchAngle);

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
#endif // RELEASE

		///<summary>The Value rounded to DecimalPlaces.</summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Value">The value to be rounded.</param>
		/// <param name="DecimalPlaces">The decimal places to be included.</param>
		public static float RoundToDP(float Value, int DecimalPlaces = 2)
		{
			if (DecimalPlaces == 0)
			{
				return RoundInt(Value);
			}

			float fFactor = Power(10, DecimalPlaces);
			return Round(Value * fFactor) / fFactor;
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
				CallbackTrue?.Invoke();
			else
				CallbackFalse?.Invoke();
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
				Limit = FAbs(Limit);
			}

			return From + Limit > Value && Value > From - Limit;
		}

#if RELEASE
		/// <summary>The largest Vector3 between L and R, according to <see cref="Vector3.sqrMagnitude"/>.</summary>
		/// <docs>The largest Vector3 between L and R, according to Vector3.sqrMagnitude.</docs>
		/// <decorations decor="public static Vector3"></decorations>
		/// <param name="L"></param>
		/// <param name="R"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Max(Vector3 L, Vector3 R)
		{
			return L.sqrMagnitude < R.sqrMagnitude ? R : L;
		}

		/// <summary>The largest Vector3 between L and R, according to <see cref="Vector3.sqrMagnitude"/>.</summary>
		/// <docs>The largest Vector3 between L and R, according to Vector3.sqrMagnitude.</docs>
		/// <decorations decor="public static Vector3"></decorations>
		/// <param name="L"></param>
		/// <param name="R"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Min(Vector3 L, Vector3 R)
		{
			return L.sqrMagnitude > R.sqrMagnitude ? R : L;
		}
#endif // RELEASE

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
		public static MVector[] GenerateEqualSphere(int Resolution, float GoldenRatioModifier)
		{
			MVector[] vDirections = new MVector[Resolution];

			float fPhi = 1 + FSqrt(GoldenRatioModifier) * .5f;
			float fInc = k2PI * fPhi;

			for (int i = 0; i < Resolution; i++)
			{
				float t = i * FInverse(Resolution);
				float Incline = FArcCosine(1 - 2 * t);
				float Azimuth = fInc * i;

				SinCos(out float InclineSine, out float InclineCosine, Incline);
				SinCos(out float AzimuthSine, out float AzimuthCosine, Azimuth);

				float X = InclineSine * AzimuthCosine;
				float Y = InclineSine * AzimuthSine;
				float Z = InclineCosine;

				vDirections[i] = new MVector(X, Y, Z);
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
		public static MVector[] Bridge(MVector Origin, MVector Target, int Resolution, float Height)
		{
			MVector[] Points = new MVector[Resolution];

			MVector DirectionToTarget = (Target - Origin).Normalised;

			float Theta = 0f;
			float HorizontalIncrement = k2PI * FInverse(Resolution);
			float ResolutionToDistance = Resolution * .5f - 1;
			float DistanceIncrement = (Target - Origin).Magnitude * FInverse(ResolutionToDistance);

			int k = 0;

			for (float i = 0; i < Resolution; i += DistanceIncrement)
			{
				float Y = Sine(Theta);

				if (Y < 0)
					break;

				MVector point = new MVector
				{
					X = DirectionToTarget.X * i,
					Y = Y * Height,
					Z = DirectionToTarget.Z * i
				};

				Points[k] = point;
				Theta += HorizontalIncrement;

				k++;
			}

			return Points;
		}

		/// <summary>Mirrors Number about Minimum and Maximum, inclusive.</summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Number">The number to anchor a reflection.</param>
		/// <param name="Minimum">The minimum number that can be reflected.</param>
		/// <param name="Maximum">The maximum number that can be reflected.</param>
		/// <returns>The reflected number.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float MirrorNumber(float Number, float Minimum, float Maximum) => Minimum + Maximum - Number;

		/// <summary>Mirrors Number about Minimum and Maximum, inclusive. Not to be confused with <see cref="MArray{T}.Reflect(int, int)"/>.</summary>
		/// <docs>Mirrors Number about Minimum and Maximum, inclusive. Not to be confused with MArray&lt;T&gt;.Reflect(int, int).</docs>
		/// <decorations decor="public static int"></decorations>
		/// <param name="Number">The number to anchor a reflection.</param>
		/// <param name="Minimum">The minimum number that can be reflected.</param>
		/// <param name="Maximum">The maximum number that can be reflected.</param>
		/// <returns>The reflected number.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int MirrorNumber(int Number, int Minimum, int Maximum) => Minimum + Maximum - Number;

		/// <summary>Swaps L and R.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="L"></param>
		/// <param name="R"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Swap<T>(ref T L, ref T R)
		{
			(R, L) = (L, R);
		}

		/// <summary>Clamps I to be between Min and Max.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="I">A reference to the Integer to clamp.</param>
		/// <param name="Min">The Minimum value of I.</param>
		/// <param name="Max">The Maximum value of I.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Clamp(ref int I, int Min, int Max)
		{
			if (Min > Max)
				Swap(ref Min, ref Max);

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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ClampMin(ref int I, int Min)
		{
			if (I < Min)
				I = Min;
		}

		/// <summary>Clamps I to not exceed Max.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="I">A reference to the Integer to clamp.</param>
		/// <param name="Max">The Maximum value I can be.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ClampMax(ref int I, int Max)
		{
			if (I > Max)
				I = Max;
		}

		/// <summary>Clamps F to not fall below Min.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="F">A reference to the Float to clamp.</param>
		/// <param name="Min">The Minimum value F can be.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ClampMin(ref float F, float Min)
		{
			if (F < Min)
				F = Min;
		}

		/// <summary>Clamps F to not exceed Max.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="F">A reference to the Float to clamp.</param>
		/// <param name="Max">The Maximum value F can be.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Max(float F1, float F2)
		{
			return F1 < F2 ? F2 : F1;
		}

		/// <summary>The smaller value between F1 and F2.</summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="F1"></param>
		/// <param name="F2"></param>
		/// <returns>The smaller of the two given floats.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Min(float F1, float F2)
		{
			return F1 < F2 ? F1 : F2;
		}

		/// <summary>The larger value between I1 and I2.</summary>
		/// <decorations decor="public static int"></decorations>
		/// <param name="I1"></param>
		/// <param name="I2"></param>
		/// <returns>The larger of the two given ints.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Max(int I1, int I2)
		{
			return I1 < I2 ? I2 : I1;
		}

		/// <summary>The smaller value between I1 and I2.</summary>
		/// <decorations decor="public static int"></decorations>
		/// <param name="I1"></param>
		/// <param name="I2"></param>
		/// <returns>The smaller of the two given ints.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Min(int I1, int I2)
		{
			return I1 < I2 ? I1 : I2;
		}

		/// <summary>Modifies I to be its Absolute Value.</summary>
		/// <remarks><see langword="ref"/> version of <see cref="FAbs(int)"/>.</remarks>
		/// <docremarks>Ref&amp; version of FastAbs().</docremarks>
		/// <decorations decor="public static void"></decorations>
		/// <param name="I">A reference to the int to modify.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Abs(ref int I) => I = FAbs(I);

		/// <summary>Modifies F to be its Absolute Value.</summary>
		/// <remarks><see langword="ref"/> version of <see cref="FAbs(float)"/></remarks>
		/// <docremarks>Ref&amp; version of FastAbs().</docremarks>
		/// <decorations decor="public static void"></decorations>
		/// <param name="F">A reference to the float to modify.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Abs(ref float F) => F = FAbs(F);

		/// <summary><see cref="FAbs(float)"/> on all V's components.</summary>
		/// <docs>Abs() on all V's components.</docs>
		/// <decorations decor="public static MVector"></decorations>
		/// <param name="V">The Vector to Abs.</param>
		/// <returns>An MVector with all positive components.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MVector Abs(MVector V) => V.Abs;

		/// <summary>Modifies V to be positive on all components.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="V">The Vector to modify.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Abs(ref MVector V) => V = V.Abs;

		/// <summary>Whether or not a float is considered to be zero.</summary>
		/// <decorations decor="public static bool"></decorations>
		/// <param name="F">The float to check.</param>
		/// <param name="Tolerance">The threshold for F to be considered zero.</param>
		/// <returns>True if F is +- Tolerance of zero.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsZero(float F, float Tolerance = MVector.kEpsilon)
		{
			return FAbs(F) < Tolerance;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsZero(MVector V, float Tolerance = MVector.kEpsilon)
		{
			return IsZero(V.X, Tolerance) && IsZero(V.Y, Tolerance) && IsZero(V.Z, Tolerance);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsZero(MRotator R, float Tolerance = MVector.kEpsilon)
		{
			return IsZero(R.Pitch, Tolerance) && IsZero(R.Yaw, Tolerance) && IsZero(R.Roll, Tolerance);
		}

#if RELEASE
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

		/// <summary>Gets the number of frames per second during runtime.</summary>
		/// <decorations decor="public static int"></decorations>
		/// <returns>The number of frames per second.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int FPS() => (int)FInverse(Time.unscaledDeltaTime);

		/// <summary>Executes a Delayed call to Function after Seconds by Owner.</summary>
		/// <decorations decor="public static IEnumerator"></decorations>
		/// <param name="Owner">The MonoBehaviour which owns this delayed call.</param>
		/// <param name="Seconds">The number of seconds to wait until Function is called.</param>
		/// <param name="Function">The Function to execute after Seconds has elapsed.</param>
		/// <returns>The IEnumerator responsible for handling this delayed call.</returns>
		public static IEnumerator Delay(MonoBehaviour Owner, float Seconds, Action Function)
		{
			if (Function == null)
				return null;

			if (Seconds < Time.deltaTime)
			{
				Function.Invoke();
				return null;
			}

			IEnumerator DelayedCall = Internal_Delay(Seconds, Function);

			Owner.StartCoroutine(DelayedCall);

			return DelayedCall;
		}

		/// <summary>Executes a Delayed call to Function after another Dependent delayed call by Owner.</summary>
		/// <decorations decor="public static IEnumerator"></decorations>
		/// <param name="Owner">The MonoBehaviour which owns this delayed call.</param>
		/// <param name="Dependent">The other delayed call to wait until Function can be executed.</param>
		/// <param name="Function">The Function to execute after Dependent has finished.</param>
		/// <returns>The IEnumerator responsible for handling this delayed call.</returns>
		public static IEnumerator Delay(MonoBehaviour Owner, IEnumerator Dependent, Action Function)
		{
			if (Function == null || Dependent == null)
				return null;

			IEnumerator DelayedCall = Internal_Delay(Dependent, Function);

			Owner.StartCoroutine(DelayedCall);

			return DelayedCall;
		}

		/// <summary>Executes a Delayed call to Function after a Dependent delayed call, followed by AdditionalSeconds, by Owner.</summary>
		/// <decorations decor="public static IEnumerator"></decorations>
		/// <param name="Owner">The MonoBehaviour which owns this delayed call.</param>
		/// <param name="Dependent">The other delayed call to wait until Function can be executed.</param>
		/// <param name="AdditionalSeconds">The number of seconds to wait after Dependent has finished.</param>
		/// <param name="Function">The Function to execute after Dependent has finished and AdditionalSeconds has elapsed.</param>
		/// <returns>The IEnumerator responsible for handling this delayed call.</returns>
		public static IEnumerator Delay(MonoBehaviour Owner, IEnumerator Dependent, float AdditionalSeconds, Action Function)
		{
			if (Function == null || Dependent == null)
				return null;

			if (AdditionalSeconds < Time.deltaTime)
			{
				Function.Invoke();
				return null;
			}

			IEnumerator DelayedCall = Internal_Delay(Dependent, AdditionalSeconds, Function);

			Owner.StartCoroutine(DelayedCall);

			return DelayedCall;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static IEnumerator Internal_Delay(float Seconds, Action Function)
		{
			yield return new WaitForSeconds(Seconds);

			Function.Invoke();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static IEnumerator Internal_Delay(IEnumerator Dependent, Action Function)
		{
			yield return Dependent;

			Function.Invoke();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static IEnumerator Internal_Delay(IEnumerator Dependent, float Seconds, Action Function)
		{
			yield return Dependent;
			yield return new WaitForSeconds(Seconds);

			Function.Invoke();
		}
#endif // RELEASE

		/// <summary>Compares two strings for similarity.</summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="BaseString">The string to compare against.</param>
		/// <param name="CompString">The string to compare to.</param>
		/// <param name="bCaseSensitive">True to consider character casing in the similarity calculation.</param>
		/// <param name="bConsiderSpaces">True to consider space ' ' characters in the similarity calculation.</param>
		/// <returns>A score between 0 to 1 depending on how similar CompString is to BaseString. 0 = Completely different, 1 = Identical.</returns>
		public static float Compare(string BaseString, string CompString, bool bCaseSensitive = false, bool bConsiderSpaces = false)
		{
			if (BaseString == CompString)
				return 1f;

			if (bCaseSensitive)
			{
				BaseString = BaseString.ToUpper();
				CompString = CompString.ToUpper();
			}

			if (!bConsiderSpaces)
			{
				BaseString = BaseString.Replace(" ", "");
				CompString = CompString.Replace(" ", "");
			}

			int BaseLength = BaseString.Length;
			int CompLength = CompString.Length;
			_ = (BaseLength < CompLength)
				? BaseString += new string(' ', CompLength - BaseLength)
				: CompString += new string(' ', BaseLength - CompLength);

			BaseLength = BaseString.Length;
			CompLength = CompString.Length;

			float MatchingCharacterRange = Max((BaseLength * .5f) - 1f, 0f);
			float Matches = 0f;

			bool[] BaseMatches = new bool[BaseLength];
			bool[] CompMatches = new bool[BaseLength];

			for (int i = 0; i < BaseLength; i++)
			{
				int Start = (int)Max(i - MatchingCharacterRange, 0f);
				int End = (int)Min(i + MatchingCharacterRange, BaseLength - 1f);
				for (int k = Start; k <= End; k++)
				{
					if (BaseString[i] == CompString[k] && !CompMatches[k])
					{
						Matches++;
						BaseMatches[i] = true;
						CompMatches[k] = true;
						break;
					}
				}
			}

			if (Matches == 0f)
				return 0f;

			int Transpositions = 0;
			int CompIndex = 0;
			for (int i = 0; i < BaseLength; i++)
			{
				if (BaseMatches[i])
				{
					while (!CompMatches[CompIndex])
						CompIndex++;

					if (BaseString[i] != CompString[CompIndex])
						Transpositions++;

					CompIndex++;
				}
			}

			Transpositions = (int)(Transpositions * .5f);

			float S1Length = FInverse(BaseLength);
			float S2Length = FInverse(CompLength);
			float InvMatches = FInverse(Matches);

			return ((Matches * S1Length) + (Matches * S2Length) + ((Matches - Transpositions) * InvMatches)) * kOneThird;
		}
	}
}
