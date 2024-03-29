﻿using System;
using static MW.Math.Magic.Fast;
using System.Runtime.CompilerServices;
#if RELEASE
using MW.Math;
using UnityEngine;
using static MW.FMath;
#endif // RELEASE
using MW.Extensions;

namespace MW
{
	/// <summary>A <see cref="UnityEngine.Quaternion"/> rotation implementation defined by Pitch, Yaw and Roll in degrees.</summary>
	/// <docs>A Quaternion rotation implementation defined by Pitch, Yaw and Roll in degrees.</docs>
	/// <decorations decor="[Serializable] public struct"></decorations>
	[Serializable]
	public struct MRotator
	{
		/// <summary>The rotation in degrees around the X axis. 0 = Forward, +Up, -Down.</summary>
		/// <decorations decor="public float"></decorations>
		public float Pitch;
		/// <summary>The rotation in degrees around the Y axis. 0 = Straight, +Right, -Left.</summary>
		/// <decorations decor="public float"></decorations>
		public float Yaw;
		/// <summary>The rotation in degrees around the Z axis. 0 = Up is 12 o'clock, +Clockwise, -Counter-clockwise.</summary>
		/// <decorations decor="public float"></decorations>
		public float Roll;

		static readonly MRotator Zero = new MRotator(0, 0, 0);
		/// <summary>An MRotator with no rotation.</summary>
		/// <decorations decor="public static readonly MRotator"></decorations>
		public static readonly MRotator Neutral = Zero;

		/// <summary>Rotator floating-point precision.</summary>
		public const float kEpsilon = MVector.kEpsilon;

		/// <summary>Makes a rotation with Pitch, Yaw and Roll.</summary>
		/// <param name="P">Pitch.</param>
		/// <param name="Y">Yaw.</param>
		/// <param name="R">Roll.</param>
		public MRotator(float P, float Y, float R)
		{
			Pitch = P;
			Yaw = Y;
			Roll = R;
		}

		/// <summary>Makes a rotation with an MVector.</summary>
		/// <param name="PYR">An MVector where X = Pitch, Y = Yaw, and Z = Roll.</param>
		public MRotator(MVector PYR) : this(PYR.X, PYR.Y, PYR.Z) { }

#if RELEASE
		/// <summary>Makes an MRotator with a Quaternion.</summary>
		/// <param name="Quaternion">The Quaternion defining the Pitch, Yaw, and Roll.</param>
		public MRotator(Quaternion Quaternion)
		{
			this = Quaternion.MakeRotator();
		}

		/// <summary>Computes a <see cref="UnityEngine.Quaternion"/> with a rotation of Pitch, Yaw and Roll.</summary>
		/// <docs>Computes a Quaternion with a rotation of Pitch, Yaw and Roll.</docs>
		/// <decorations decor="public Quaternion"></decorations>
		/// <returns>A Quaternion with Pitch, Yaw, Roll.</returns>
		public Quaternion Quaternion()
		{
			const float kDegToRadOver2 = FMath.D2R * .5f;

			float InternalPitch = ModTowardsZero(-Yaw, 360.0f) * kDegToRadOver2;
			float InternalYaw = ModTowardsZero(-Roll, 360.0f) * kDegToRadOver2;
			float InternalRoll = ModTowardsZero(Pitch, 360.0f) * kDegToRadOver2;

			Mathematics.SinCos(out float SinePitch, out float CosinePitch, InternalPitch);
			Mathematics.SinCos(out float SineYaw, out float CosineYaw, InternalYaw);
			Mathematics.SinCos(out float SineRoll, out float CosineRoll, InternalRoll);

			Quaternion Q = new Quaternion();
			Q.x = CosineRoll * SinePitch * SineYaw - SineRoll * CosinePitch * CosineYaw;
			Q.y = -CosineRoll * SinePitch * CosineYaw - SineRoll * CosinePitch * SineYaw;
			Q.z = CosineRoll * CosinePitch * SineYaw - SineRoll * SinePitch * CosineYaw;
			Q.w = CosineRoll * CosinePitch * CosineYaw + SineRoll * SinePitch * SineYaw;

			return Q;
		}

		/// <summary>Computes a <see cref="UnityEngine.Quaternion"/> with a rotation combining T's Rotation with Pitch, Yaw, and Roll.</summary>
		/// <docs>Computes a Quaternion with a rotation of Pitch, Yaw and Roll.</docs>
		/// <decorations decor="public Quaternion"></decorations>
		/// <param name="T">The Transform to combine with.</param>
		/// <returns>A Quaternion combining T's Rotation with this Pitch, Yaw, and Roll.</returns>
		public Quaternion Quaternion(Transform T)
		{
			return T.rotation * Quaternion();
		}

		/// <summary>Computes a Rotator combining T's Rotation with Pitch, Yaw, and Roll.</summary>
		/// <decorations decor="public MRotator"></decorations>
		/// <param name="T">The Transform to combine with.</param>
		/// <returns>An MRotator combining T's Rotation with this Pitch, Yaw, and Roll.</returns>
		public MRotator Rotation(Transform T)
		{
			return Quaternion(T).MakeRotator();
		}

		/// <summary>Computes a Rotator relative to a directional Vector and Up Vector.</summary>
		/// <remarks>
		/// Suppose UpVector is the Positive Y-Axis.
		/// <br></br>
		/// Pitch would result in a rotation about the X-Axis.
		/// <br></br>
		/// Yaw would result in a rotation about the Y-Axis.
		/// <br></br>
		/// Roll would result in a rotation about the Z-Axis.
		/// <br></br><br></br>
		/// If UpVector was defined as the Positive X-Axis, Pitch and Yaw components
		/// would be swapped. Roll would remain unaffected.
		/// </remarks>
		/// <decorations decor="public MRotator"></decorations>
		/// <param name="Vector">Relative facing direction.</param>
		/// <param name="UpVector">Global Up Vector.</param>
		/// <returns>An MRotator rotated relative to a Vector with respect to Up.</returns>
		public MRotator Relative(MVector Vector, MVector UpVector)
		{
			MVector RightVector = Vector ^ UpVector;
			MVector ForwardVector = RightVector ^ UpVector;

			MVector Rotation = Vector.RotateVector(Pitch, RightVector);
			Rotation = Rotation.RotateVector(Yaw, UpVector);
			Rotation = Rotation.RotateVector(Roll, ForwardVector);

			return Rotation.Rotation();
		}

		/// <summary>Converts a <see cref="UnityEngine.Quaternion"/> to an MRotator.</summary>
		/// <docs>Converts a Quaternion to an MRotator.</docs>
		/// <decorations decor="public static MRotator"></decorations>
		/// <param name="Q">The Quaternion to extract Pitch, Yaw and Roll.</param>
		/// <docreturns>An MRotator rotated according to a Quaternion.</docreturns>
		/// <returns>An MRotator rotated according to a <see cref="UnityEngine.Quaternion"/>.</returns>
		public static MRotator Rotator(Quaternion Q)
		{
			float W2 = Q.w * Q.w;
			float X2 = Q.x * Q.x;
			float Y2 = Q.y * Q.y;
			float Z2 = Q.z * Q.z;

			float Unit = X2 + Y2 + Z2 + W2;
			float SingularityTest = Q.x * Q.w - Q.y * Q.z;

			MRotator R;
			if (SingularityTest > .4995f * Unit)
			{
				R.Yaw = 2f * FArcTangent2(Q.y, Q.x);
				R.Pitch = kHalfPI;
				R.Roll = 0f;
				return R.NormaliseAngles();
			}
			else if (SingularityTest < .4995f * Unit)
			{
				R.Yaw = -2f * FArcTangent2(Q.y, Q.x);
				R.Pitch = kHalfPI;
				R.Roll = 0f;
				return R.NormaliseAngles();
			}

			R.Yaw = FArcTangent2(2f * Q.x * Q.w + 2f * Q.y * Q.z, 1 - 2f * (Q.z * Q.z + Q.w * Q.w));
			R.Pitch = FArcSine(2f * (Q.x * Q.z - Q.w * Q.y));
			R.Roll = FArcTangent2(2f * Q.x * Q.y + 2f * Q.z * Q.w, 1 - 2f * (Q.y * Q.y + Q.z * Q.z));
			return R.NormaliseAngles();
		}

		static float ModTowardsZero(float X, float Y)
		{
			float AbsY = FAbs(Y);
			if (AbsY <= 1e-8f)
			{
				Diagnostics.Log.E("The Absolute value of Y is <= 1E-08F. Input Y: ", Y);
				return 0f;
			}

			float Div = X / Y;
			float Quotient = FAbs(Div) < 8388608.0f ? (float)System.Math.Truncate(Div) : Div;
			float IntPortion = Y * Quotient;
			if (FAbs(IntPortion) > FAbs(X))
			{
				IntPortion = X;
			}

			float Result = X - IntPortion;

			Clamp(ref Result, -AbsY, AbsY);

			return Result;
		}

		/// <summary>Gets the rotation required to look towards a direction with an up.</summary>
		/// <decorations decor="public static MRotator"></decorations>
		/// <param name="Direction">The direction to look towards.</param>
		/// <param name="Up">The up vector to rotate about.</param>
		/// <returns>An MRotator rotation towards Direction about Up.</returns>
		public static MRotator LookRotation(MVector Direction, MVector Up)
		{
			MVector Forward = Direction;
			Forward.Normalise();
			Up -= ((Up | Forward) * Forward);
			Up.Normalise();

			MVector V1 = Up ^ Forward;
			MVector V2 = Forward ^ V1;

			float F00 = V1.X; float F10 = V2.X; float F20 = Forward.X;
			float F01 = V1.Y; float F11 = V2.Y; float F21 = Forward.Y;
			float F02 = V1.Z; float F12 = V2.Z; float F22 = Forward.Z;

			float Diag = F00 + F11 + F22;

			Quaternion Q;
			if (Diag > 0f)
			{
				float Sqrt = FSqrt(Diag + 1f);
				Q.w = Sqrt * .5f;
				Sqrt = .5f * FInverse(Sqrt);
				Q.x = (F12 - F21) * Sqrt;
				Q.y = (F20 - F02) * Sqrt;
				Q.z = (F01 - F10) * Sqrt;
				return Rotator(Q);
			}

			if (F00 >= F11 && F00 >= F22)
			{
				float Sqrt = FSqrt(((1f + F00) - F11) - F22);
				float Inverse = .5f * FInverse(Sqrt);
				Q.x = .5f * Sqrt;
				Q.y = (F01 + F10) * Inverse;
				Q.z = (F02 + F20) * Inverse;
				Q.w = (F12 - F21) * Inverse;
				return Rotator(Q);
			}

			if (F11 > F22)
			{
				float Sqrt = FSqrt(((1f + F11) - F00) - F22);
				float Inverse = .5f * FInverse(Sqrt);
				Q.x = (F10 + F01) * Inverse;
				Q.y = .5f * Sqrt;
				Q.z = (F21 + F12) * Inverse;
				Q.w = (F20 - F02) * Inverse;
				return Rotator(Q);
			}

			{
				float Sqrt = FSqrt(((1f + F22) - F00 - F11));
				float Inverse = .5f * FInverse(Sqrt);
				Q.x = (F20 + F02) * Inverse;
				Q.y = (F21 + F12) * Inverse;
				Q.z = .5f * Sqrt;
				Q.w = (F01 - F10) * Inverse;
				return Rotator(Q);
			}
		}
#endif // RELEASE

		/// <summary>Adds a rotation to the respective rotation component.</summary>
		/// <decorations decor="public MRotator"></decorations>
		/// <returns>This MRotator after adding the deltas.</returns>
		public MRotator Add(float DeltaPitch, float DeltaYaw, float DeltaRoll)
		{
			Pitch += DeltaPitch;
			Yaw += DeltaYaw;
			Roll += DeltaRoll;

			Wrap360();

			return this;
		}

		/// <summary>Adds an MRotator to this MRotator on their respective components.</summary>
		/// <decorations decor="public MRotator"></decorations>
		/// <param name="DeltaPYR">The MRotator to add.</param>
		/// <returns>See Add(float DeltaPitch, float DeltaYaw, float DeltaRoll).</returns>
		public MRotator Add(MRotator DeltaPYR)
		{
			return Add(DeltaPYR.Pitch, DeltaPYR.Yaw, DeltaPYR.Roll);
		}

		/// <summary>The opposite of this MRotator over OpposeAxes, defined by <see cref="ERotationAxis"/>.</summary>
		/// <docs>The opposite of this MRotator over OpposeAxes.</docs>
		/// <decorations decor="public MRotator"></decorations>
		/// <param name="OpposeAxes">The axes to oppose.</param>
		/// <returns>An MRotator in the opposite direction over the supplied axes.</returns>
		public MRotator Oppose(ERotationAxis OpposeAxes)
		{
			const float kDegreesToAdd = 180f;

			float PToAdd = kDegreesToAdd * ((OpposeAxes & ERotationAxis.Pitch) == ERotationAxis.Pitch ? 1 : 0);
			float YToAdd = kDegreesToAdd * ((OpposeAxes & ERotationAxis.Yaw) == ERotationAxis.Yaw ? 1 : 0);
			float RToAdd = kDegreesToAdd * ((OpposeAxes & ERotationAxis.Roll) == ERotationAxis.Roll ? 1 : 0);

			return Add(PToAdd, YToAdd, RToAdd);
		}

		/// <summary>Interpolates between two MRotators.</summary>
		/// <decorations decor="public static MRotator"></decorations>
		/// <param name="From">The beginning rotation.</param>
		/// <param name="To">The target rotation.</param>
		/// <param name="Alpha">The percentage of interpolation.</param>
		/// <param name="Equation">The equation to interpolate. Default is Linear.</param>
		/// <returns>An interpolated MRotator between From and To.</returns>
		public static MRotator Interpolate(MRotator From, MRotator To, float Alpha, EEquation Equation = EEquation.Linear)
		{
			float P = Easing.Interpolate.Ease(Equation, From.Pitch, To.Pitch, Alpha);
			float Y = Easing.Interpolate.Ease(Equation, From.Yaw, To.Yaw, Alpha);
			float R = Easing.Interpolate.Ease(Equation, From.Roll, To.Roll, Alpha);

			return new MRotator(P, Y, R);
		}

#if RELEASE
		/// <summary>Interpolates between two MRotators.</summary>
		/// <decorations decor="public static MRotator"></decorations>
		/// <param name="From">The beginning rotation.</param>
		/// <param name="To">The target rotation.</param>
		/// <param name="Alpha">The percentage of interpolation.</param>
		/// <param name="Curve">The curve describing the speed of the interpolation.</param>
		/// <returns>An interpolated MRotator between From and To.</returns>
		public static MRotator Interpolate(MRotator From, MRotator To, float Alpha, AnimationCurve Curve)
		{
			float Evaluation = Curve.Evaluate(Alpha);

			float P = Mathf.Lerp(From.Pitch, To.Pitch, Evaluation);
			float Y = Mathf.Lerp(From.Yaw, To.Yaw, Evaluation);
			float R = Mathf.Lerp(From.Roll, To.Roll, Evaluation);

			return new MRotator(P, Y, R);
		}
#endif // RELEASE

		/// <summary>An <see cref="MW.MVector"/> where X = Pitch, Y = Yaw, Z = Roll.</summary>
		/// <docs>An MVector where X = Pitch, Y = Yaw, Z = Roll.</docs>
		/// <decorations decor="public MVector"></decorations>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public MVector MV()
		{
			return new MVector(Pitch, Yaw, Roll);
		}

#if RELEASE
		/// <summary>Gets the direction this MRotator is facing when facing forward.</summary>
		/// <decorations decor="public MVector"></decorations>
		/// <param name="Forward">Forward orientation.</param>
		/// <returns>An MVector direction representing this Rotation.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public MVector Direction(MVector Forward)
		{
			return this * Forward;
		}

		/// <summary>This Rotator as a directional Vector.</summary>
		/// <decorations decor="public MVector"></decorations>
		/// <returns>An normalised MVector describing this MRotator.</returns>
		public MVector AsOrientationVector()
		{
			float InternalPitch = ModTowardsZero(Pitch, 360.0f) * D2R;
			float InternalYaw = ModTowardsZero(Yaw, 360.0f) * D2R;

			Mathematics.SinCos(out float SinePitch, out float CosinePitch, InternalPitch, true);
			Mathematics.SinCos(out float SineYaw, out float CosineYaw, InternalYaw, true);

			return new MVector(CosinePitch * SineYaw, SinePitch, CosinePitch * CosineYaw);
		}
#endif // RELEASE

		/// <summary>This MRotator % 360 on all components.</summary>
		/// <decorations decor="public void"></decorations>
		public void Wrap360()
		{
			Pitch %= 360f;
			Yaw %= 360f;
			Roll %= 360f;
		}

		/// <summary>Forces the Angles to be within 0 - 360 degrees.</summary>
		/// <decorations decor="public MRotator"></decorations>
		/// <returns>The normalised MRotator.</returns>
		public MRotator NormaliseAngles()
		{
			Wrap360();

			if (Pitch < 0f)
				Pitch += 360f;
			if (Yaw < 0f)
				Yaw += 360f;
			if (Roll < 0f)
				Roll += 360f;

			return this;
		}

		/// <summary>If this MRotator is Neutral, considering ZeroThreshold.</summary>
		/// <decorations decor="public bool"></decorations>
		/// <param name="ZeroThreshold">The threshold to consider Zero. Default = kEpsilon.</param>
		/// <returns>True if this MRotator is considered Neutral.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsZero(float ZeroThreshold = kEpsilon)
		{
			return Utils.IsZero(this, ZeroThreshold);
		}

		/// <summary>Checks R's components for NaN.</summary>
		/// <decorations decor="public static bool"></decorations>
		/// <param name="R">The vector to check.</param>
		/// <returns>True if R contains NaN.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool ContainsNaN(MRotator R)
			=> R.Pitch.IsIllegalFloat() || R.Yaw.IsIllegalFloat() || R.Roll.IsIllegalFloat();

		/// <summary>Adds two MRotators together.</summary>
		/// <decorations decor="public static MRotator operator+"></decorations>
		/// <param name="L">Left-side MRotator.</param>
		/// <param name="R">Right-side MRotator.</param>
		/// <returns>(MRotator L, MRotator R) => L.Add(R)</returns>
		public static MRotator operator +(MRotator L, MRotator R) => L.Add(R);

		/// <summary>Subtracts R from L.</summary>
		/// <decorations decor="public static MRotator operator-"></decorations>
		/// <param name="L">Left-side MRotator.</param>
		/// <param name="R">Right-side MRotator.</param>
		/// <returns>L.Add(-R).</returns>
		public static MRotator operator -(MRotator L, MRotator R)
		{
			R.Pitch *= -1f;
			R.Yaw *= -1f;
			R.Roll *= -1f;

			return L.Add(R);
		}

		/// <summary>Multiplies an MRotator by a Scalar value.</summary>
		/// <decorations decor="public static MRotator operator*"></decorations>
		/// <param name="R">Rotator.</param>
		/// <param name="S">Scalar.</param>
		/// <returns>R[P..Y..R] * S.</returns>
		public static MRotator operator *(MRotator R, float S)
		{
			R.Pitch *= S;
			R.Yaw *= S;
			R.Roll *= S;

			return R;
		}

#if RELEASE
		/// <summary>Apply Rotations L and R in sequence.</summary>
		/// <remarks>MRotator multiplications are not commutative. L * R != R * L.</remarks>
		/// <decorations decor="public static MRotator operator*"></decorations>
		/// <param name="L">First rotation.</param>
		/// <param name="R">Second rotation.</param>
		/// <returns>An MRotator L rotated by R.</returns>
		public static MRotator operator *(MRotator L, MRotator R)
		{
			Quaternion A = L.Quaternion();
			Quaternion B = R.Quaternion();

			Quaternion RetVal = new Quaternion(
				A.w * B.x + A.x * B.w + A.y * B.z - A.z * B.y,
				A.w * B.y + A.y * B.w + A.z * B.x - A.x * B.z,
				A.w * B.z + A.z * B.w + A.x * B.y - A.y * B.x,
				A.w * B.w - A.x * B.x - A.y * B.y - A.z * B.z
			);

			return new MRotator(RetVal);
		}
#endif // RELEASE

		/// <summary>Compares two MRotators for equality.</summary>
		/// <decorations decor="public static bool operator=="></decorations>
		/// <param name="Left">Left-side comparison.</param>
		/// <param name="Right">Right-side comparison.</param>
		/// <docreturns>True if the square difference between Left and Right is less than kEpsilon ^ 2.</docreturns>
		/// <returns>True if the square difference between Left and Right is less than <see cref="kEpsilon"/>^2.</returns>
		public static bool operator ==(MRotator Left, MRotator Right)
		{
			float P = Left.Pitch - Right.Pitch;
			float Y = Left.Yaw - Right.Yaw;
			float R = Left.Roll - Right.Roll;
			float Sqr = P * P + Y * Y + R * R;
			return Sqr < kEpsilon * kEpsilon;
		}

		/// <summary>Compares two MRotators for inequality.</summary>
		/// <decorations decor="public static bool operator!="></decorations>
		/// <param name="Left">Left-side comparison.</param>
		/// <param name="Right">Right-side comparison.</param>
		/// <returns>The opposite of operator ==.</returns>
		public static bool operator !=(MRotator Left, MRotator Right) => !(Left == Right);

		public override bool Equals(object O) => O is MVector V && Equals(V);
		public bool Equals(MRotator R) => this == R || FAbs(R.Pitch - Pitch + R.Yaw - Yaw + R.Roll - Roll) < 4f * kEpsilon;

#if RELEASE
		/// <summary>Converts Pitch, Yaw, Roll into its corresponding <see cref="UnityEngine.Quaternion"/>.</summary>
		/// <docs>Converts Pitch, Yaw, Roll into its corresponding Quaternion.</docs>
		/// <decorations decor="public static implicit operator Quaternion"></decorations>
		/// <param name="Rotation">The rotation to convert to Quaternions.</param>
		public static implicit operator Quaternion(MRotator Rotation) => Rotation.Quaternion();

		/// <summary>Rotate Vector by Rotation.</summary>
		/// <decorations decor="public static MVector operator*"></decorations>
		/// <param name="Rotation"></param>
		/// <param name="Vector"></param>
		/// <returns></returns>
		public static MVector operator *(MRotator Rotation, MVector Vector)
		{
			Quaternion R = Rotation.Quaternion();

			MVector XYZ = new MVector(R.x, R.y, R.z);
			MVector V0 = 2f * XYZ;
			MVector V1 = V0 * XYZ;
			MVector V2 = new MVector(R.x, R.x, R.y) * new MVector(V0.Y, V0.Z, V0.Z);
			MVector V3 = new MVector(R.w) * V0;

			MVector RetVal;
			RetVal.X = (1f - (V1.Y + V1.Z)) * Vector.X + (V2.X - V3.Z) * Vector.Y + (V2.Y + V3.Y) * Vector.Z;
			RetVal.Y = (V2.X + V3.Z) * Vector.X + (1f - (V1.X + V1.Z)) * Vector.Y + (V2.Z - V3.X) * Vector.Z;
			RetVal.Z = (V2.Y - V3.Y) * Vector.X + (V2.Z + V3.X) * Vector.Y + (1f - (V1.X + V1.Y)) * Vector.Z;

			return RetVal;
		}

		/// <summary>Rotate Vector by Rotation.</summary>
		/// <decorations decor="public static Vector3 operator*"></decorations>
		/// <param name="Rotation"></param>
		/// <param name="Vector"></param>
		/// <returns></returns>
		public static Vector3 operator *(MRotator Rotation, Vector3 Vector)
		{
			return Rotation * Vector.MV();
		}
#endif // RELEASE

		/// <summary>Hash code for use in Maps, Sets, MArrays, etc.</summary>
		/// <decorations decors="public override int"></decorations>
		/// <returns>GetHashCode() => Pitch.GetHashCode() ^ (Yaw.GetHashCode() &lt;&lt; 2) ^ (Roll.GetHashCode() &gt;&gt; 2)</returns>
		public override int GetHashCode() => Pitch.GetHashCode() ^ (Yaw.GetHashCode() << 2) ^ (Roll.GetHashCode() >> 2);

		/// <summary>A human-readable MRotator.</summary>
		/// <decorations decor="public override string"></decorations>
		/// <returns>"Pitch: " + Pitch + " Yaw: " + Yaw + " Roll: " + Roll</returns>
		public override string ToString()
		{
			return $"Pitch: {Pitch} Yaw: {Yaw} Roll: {Roll}";
		}

		/// <summary>Rotation axes.</summary>
		/// <remarks>Uses bytes.</remarks>
		/// <decorations decor="public enum : byte"></decorations>
		public enum ERotationAxis : byte
		{
			/// <summary>No Rotation axis.</summary>
			NoAxis = 0,
			/// <summary>Rotation axis describing Pitch.</summary>
			Pitch = 1,
			/// <summary>Rotation axis describing Yaw.</summary>
			Yaw = 2,
			/// <summary>Rotation axis describing Roll.</summary>
			Roll = 4
		}
	}
}
