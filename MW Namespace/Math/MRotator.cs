using MW.Diagnostics;
using MW.Extensions;
using MW.Math;
using MW.Math.Magic;
using System;
using UnityEngine;
using static MW.Utils;

namespace MW
{
	/// <summary>A <see cref="UnityEngine.Quaternion"/> rotation implementation defined by Pitch, Yaw and Roll in degrees.</summary>
	/// <docs>A Quaternion rotation implementation defined by Pitch, Yaw and Roll in degrees.</docs>
	/// <decorations decor="[Serializable] public struct"></decorations>
	[Serializable]
	public struct MRotator
	{
		/// <summary>The rotation in degrees around the X axis. 0 = Forward, +Up, -Down.</summary>
		/// <decorations decor="public static float"></decorations>
		public float Pitch;
		/// <summary>The rotation in degrees around the Y axis. 0 = Straight, +Right, -Left.</summary>
		/// <decorations decor="public static float"></decorations>
		public float Yaw;
		/// <summary>The rotation in degrees around the Z axis. 0 = Up is 12 o'clock, +Clockwise, -Counter-clockwise.</summary>
		/// <decorations decor="public static float"></decorations>
		public float Roll;

		static readonly MRotator zero = new MRotator(0, 0, 0);
		/// <summary>An MRotator with no rotation.</summary>
		/// <decorations decor="public static readonly MVector"></decorations>
		public static readonly MRotator Zero = zero;

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
			const float kDegToRadOver2 = Mathf.Deg2Rad * .5f;

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
				R.Yaw = 2f * Mathf.Atan2(Q.y, Q.x);
				R.Pitch = kHalfPI;
				R.Roll = 0f;
				return R.NormaliseAngles();
			}
			else if (SingularityTest < .4995f * Unit)
			{
				R.Yaw = -2f * Mathf.Atan2(Q.y, Q.x);
				R.Pitch = kHalfPI;
				R.Roll = 0f;
				return R.NormaliseAngles();
			}

			R.Yaw = Mathf.Atan2(2f * Q.x * Q.w + 2f * Q.y * Q.z, 1 - 2f * (Q.z * Q.z + Q.w * Q.w));
			R.Pitch = Fast.ArcSine(2f * (Q.x * Q.z - Q.w * Q.y));
			R.Roll = Mathf.Atan2(2f * Q.x * Q.y + 2f * Q.z * Q.w, 1 - 2f * (Q.y * Q.y + Q.z * Q.z));
			return R.NormaliseAngles();
		}

		static float ModTowardsZero(float X, float Y)
		{
			float AbsY = Mathf.Abs(Y);
			if (AbsY <= 1e-8f)
			{
				Log.E("The Absolute value of Y is <= 1E-08F. Input Y: ", Y);
				return 0f;
			}

			float Div = X / Y;
			float Quotient = Mathf.Abs(Div) < 8388608.0f ? (float)System.Math.Truncate(Div) : Div;
			float IntPortion = Y * Quotient;
			if (Mathf.Abs(IntPortion) > Mathf.Abs(X))
			{
				IntPortion = X;
			}

			float Result = X - IntPortion;

			return Mathf.Clamp(Result, -AbsY, AbsY);
		}

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
		/// <param name="NormalisedAlpha">The percentage of interpolation, from zero to one.</param>
		/// <param name="Equation">The equation to interpolate. Default is Linear.</param>
		/// <returns>An interpolated MRotator between From and To.</returns>
		public static MRotator Interpolate(MRotator From, MRotator To, float NormalisedAlpha, EEquation Equation = EEquation.Linear)
		{
			if (NormalisedAlpha >= 1f)
				return To;

			float P = Easing.Interpolate.Ease(Equation, From.Pitch, To.Pitch, NormalisedAlpha);
			float Y = Easing.Interpolate.Ease(Equation, From.Yaw, To.Yaw, NormalisedAlpha);
			float R = Easing.Interpolate.Ease(Equation, From.Roll, To.Roll, NormalisedAlpha);

			return new MRotator(P, Y, R);
		}

		/// <summary>Interpolates between two MRotators.</summary>
		/// <decorations decor="public static MRotator"></decorations>
		/// <param name="From">The beginning rotation.</param>
		/// <param name="To">The target rotation.</param>
		/// <param name="NormalisedAlpha">The percentage of interpolation, from zero to one.</param>
		/// <param name="NormalisedCurve">The curve describing the speed of the interpolation.</param>
		/// <returns>An interpolated MRotator between From and To.</returns>
		public static MRotator Interpolate(MRotator From, MRotator To, float NormalisedAlpha, AnimationCurve NormalisedCurve)
		{
			if (NormalisedCurve.Evaluate(1f) != 1f || NormalisedCurve.Evaluate(0f) != 0f)
			{
				Log.E(nameof(NormalisedCurve), "is not normalised!\n", "Input Evaluation(0) ==", NormalisedCurve.Evaluate(0), "Input Evaluation(1) ==", NormalisedCurve.Evaluate(1f));
				return Zero;
			}

			if (NormalisedAlpha >= 1f)
				return To;

			float Evaluation = NormalisedCurve.Evaluate(NormalisedAlpha);

			float P = Mathf.Lerp(From.Pitch, To.Pitch, Evaluation);
			float Y = Mathf.Lerp(From.Yaw, To.Yaw, Evaluation);
			float R = Mathf.Lerp(From.Roll, To.Roll, Evaluation);

			return new MRotator(P, Y, R);
		}

		/// <summary>An <see cref="MW.MVector"/> where X = Pitch, Y = Yaw, Z = Roll.</summary>
		/// <docs>An MVector where X = Pitch, Y = Yaw, Z = Roll.</docs>
		/// <decorations decor="public MVector"></decorations>
		public MVector MV()
		{
			return new MVector(Pitch, Yaw, Roll);
		}

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

		/// <summary>Compares two MRotators for equality.</summary>
		/// <decorations decor="public static bool operator=="></decorations>
		/// <param name="Left">Left-side comparison.</param>
		/// <param name="Right">Right-side comparison.</param>
		/// <docreturns>True if the square difference between Left and Right is less than kEpsilon ^ 2.</docreturns>
		/// <returns>True if the square difference between Left and Right is less than <see cref="MVector.kEpsilon"/>^2.</returns>
		public static bool operator ==(MRotator Left, MRotator Right)
		{
			float P = Left.Pitch - Right.Pitch;
			float Y = Left.Yaw - Right.Yaw;
			float R = Left.Roll - Right.Roll;
			float Sqr = P * P + Y * Y + R * R;
			return Sqr < MVector.kEpsilon * MVector.kEpsilon;
		}

		/// <summary>Compares two MRotators for inequality.</summary>
		/// <decorations decor="public static bool operator!="></decorations>
		/// <param name="Left">Left-side comparison.</param>
		/// <param name="Right">Right-side comparison.</param>
		/// <returns>The opposite of operator ==.</returns>
		public static bool operator !=(MRotator Left, MRotator Right) => !(Left == Right);

		public override bool Equals(object O) => O is MVector V && Equals(V);
		public bool Equals(MRotator R) => this == R || Mathf.Abs(R.Pitch - Pitch + R.Yaw - Yaw + R.Roll - Roll) < 4f * MVector.kEpsilon;

		/// <summary>Converts Pitch, Yaw, Roll into its corresponding <see cref="UnityEngine.Quaternion"/>.</summary>
		/// <docs>Converts Pitch, Yaw, Roll into its corresponding Quaternion.</docs>
		/// <decorations decor="public static implicit operator Quaternion"></decorations>
		/// <param name="Rotation">The rotation to convert to Quaternions.</param>
		public static implicit operator Quaternion(MRotator Rotation) => Rotation.Quaternion();

		/// <summary>Hash code for use in Maps, Sets, MArrays, etc.</summary>
		/// <decorations decors="public override int"></decorations>
		/// <returns>GetHashCode() => Pitch.GetHashCode() ^ (Yaw.GetHashCode() &lt;&lt; 2) ^ (Roll.GetHashCode() &gt;&gt; 2)</returns>
		public override int GetHashCode() => Pitch.GetHashCode() ^ (Yaw.GetHashCode() << 2) ^ (Roll.GetHashCode() >> 2);

		/// <summary>A human-readable MRotator.</summary>
		/// <decorations decor="public override string"></decorations>
		/// <returns>"Pitch: " + Pitch + " Yaw: " + Yaw + " Roll: " + Roll</returns>
		public override string ToString()
		{
			return "Pitch: " + Pitch + " Yaw: " + Yaw + " Roll: " + Roll;
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
