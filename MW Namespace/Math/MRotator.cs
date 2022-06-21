using System;
using MW.Diagnostics;
using MW.Math;
using UnityEngine;

namespace MW
{
	/// <summary>A <see cref="UnityEngine.Quaternion"/> rotation implementation defined by Pitch, Yaw and Roll in degrees.</summary>
	/// <docs>A Quaternion rotation implementation defined by Pitch, Yaw and Roll in degrees.</docs>
	[Serializable]
	public struct MRotator
	{
		/// <summary>The rotation in degrees around the X axis. 0 = Forward, +Up, -Down.</summary>
		public float Pitch;
		/// <summary>The rotation in degrees around the Y axis. 0 = Straight, +Right, -Left.</summary>
		public float Yaw;
		/// <summary>The rotation in degrees around the Z axis. 0 = Up is 12 o'clock, +Clockwise, -Counter-clockwise.</summary>
		public float Roll;

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

		static readonly MRotator zero = new MRotator(0, 0, 0);
		/// <summary>An MRotator with no rotation.</summary>
		public static readonly MRotator Zero = zero;

		/// <summary>Computes a <see cref="UnityEngine.Quaternion"/> with a rotation of Pitch, Yaw and Roll.</summary>
		/// <docs>Computes a Quaternion with a rotation of Pitch, Yaw and Roll.</docs>
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
		/// <param name="DeltaPYR">The MRotator to add.</param>
		/// <returns>See Add(float DeltaPitch, float DeltaYaw, float DeltaRoll).</returns>
		public MRotator Add(MRotator DeltaPYR)
		{
			return Add(DeltaPYR.Pitch, DeltaPYR.Yaw, DeltaPYR.Roll);
		}

		/// <summary>The opposite of this MRotator over OpposeAxes, defined by <see cref="ERotationAxis"/>.</summary>
		/// <docs>The opposite of this MRotator over OpposeAxes.</docs>
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
		public MVector MVector()
		{
			return new MVector(Pitch, Yaw, Roll);
		}

		/// <summary>This MRotator % 360 on all components.</summary>
		public void Wrap360()
		{
			Pitch %= 360f;
			Yaw %= 360f;
			Roll %= 360f;
		}

		/// <summary>Adds two MRotators together.</summary>
		/// <param name="L">Left-side MRotator.</param>
		/// <param name="R">Right-side MRotator.</param>
		/// <returns>(MRotator L, MRotator R) => L.Add(R)</returns>
		public static MRotator operator +(MRotator L, MRotator R) => L.Add(R);

		/// <summary>Subtracts L from R.</summary>
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

		/// <summary>Converts Pitch, Yaw, Roll into its corresponding <see cref="UnityEngine.Quaternion"/>.</summary>
		/// <docs>Converts Pitch, Yaw, Roll into its corresponding Quaternion.</docs>
		/// <param name="Rotation">The rotation to convert to Quaternions.</param>
		public static implicit operator Quaternion(MRotator Rotation) => Rotation.Quaternion();

		/// <summary>A human-readable MRotator.</summary>
		/// <returns>"Pitch: " + Pitch + " Yaw: " + Yaw + " Roll: " + Roll</returns>
		public override string ToString()
		{
			return "Pitch: " + Pitch + " Yaw: " + Yaw + " Roll: " + Roll;
		}

		/// <summary>Rotation axes.</summary>
		/// <remarks>Uses bytes.</remarks>
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
