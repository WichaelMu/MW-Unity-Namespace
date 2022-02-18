using UnityEngine;
using MW.Kinetic;
using MW.Easing;
using MW.Vector;
using MW.Diagnostics;
using MW.Core;
using MW.Enums;

namespace MW.Math
{

	public static class Mathematics
	{

		/// <param name="EEquation">The EEquation to use to accelerate.</param>
		/// <param name="fCurrentSpeed">The current speed of the acceleration.</param>
		/// <param name="fRateOfAcceleration">The rate to accelerate towards to terminal from current speed.</param>
		/// <param name="fTerminal">The maximum speed.</param>
		/// <returns>The acceleration value using EEquation, using the current speed and rate of acceleration towards terminal over Time.deltaTime.</returns>
		public static float Acceleration(EEquation EEquation, float fCurrentSpeed, float fRateOfAcceleration, float fTerminal)
		{
			fTerminal *= Kinematics.kVelocityRatio;

			if (fRateOfAcceleration == 0)
				Log.E(nameof(fRateOfAcceleration) + " cannot be zero");

			return Mathf.Clamp(Mathf.Lerp(fCurrentSpeed / Time.deltaTime, fTerminal, Interpolate.Ease(EEquation, 0, 1, fRateOfAcceleration)), 0, fTerminal);
		}

		public static float Deceleration(float fCurrentSpeed, float fTargetVelocity = 0)
		{
			return -((fTargetVelocity - fCurrentSpeed) / Time.deltaTime);
		}

		static float fAR = 0;

		/// <param name="RSelf">The Rigidbody to calculate an acceleration rate.</param>
		/// <param name="fSpeed">The current speed of the Rigidbody in EUnit.MetresPerSecond.</param>
		/// <returns>The float rate of movement in EUnit.MetresPerSecond.</returns>
		public static float AccelerationRate(Rigidbody RSelf, float fSpeed)
		{
			float a = RSelf.velocity.magnitude - fSpeed / Time.deltaTime;
			float fAccelerationRate = (a - fAR) * -1;
			fAR = a;
			return fAccelerationRate;
		}

		/// <param name="RSelf">The Rigidbody to read a speed from.</param>
		/// <param name="UUnit">The desired EUnit of measurement.</param>
		/// <returns>A speed reading from self in EUnit of measurement.</returns>
		public static float Speed(Rigidbody RSelf, EUnit UUnit = EUnit.MetresPerSecond)
		{
			float speed = RSelf.velocity.magnitude;

			switch (UUnit)
			{
				case EUnit.KilometrsePerHour:
					return speed * 3.6f;
				case EUnit.MilesPerHour:
					return speed * 2.23694f;
				case EUnit.KilometresPerSecond:
					return speed * .001f;
				case EUnit.MetresPerHour:
					return speed * 3600;
				case EUnit.FeetPerSecond:
					return speed * 3.28084f;
				case EUnit.FeetPerHour:
					return speed * 11811.02362f;
				case EUnit.MilesPerSecond:
					return speed * 1609.34f;
				case EUnit.MetresPerSecond:
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
		public static MVector PredictiveProjectile(Rigidbody RSelf, Rigidbody RBTarget)
		{
			//  The approximate conversion from velocity in an rb.AddForce is the 2/3 of the force being applied.

			//  At a velocity of 950, the cannon travels at ~633 m/s.
			//  ~2279 kmph.

			float fSecondsPerKM = 1000 / (RSelf.velocity.magnitude * Utils.kTwoThirds);

			//  Distance between the RSelf and RBTarget in thousands.
			float fDistanceBetweenPlayer = Vector3.Distance(RSelf.position, RBTarget.position) * Utils.kThousandth;

			MVector vForwardPrediction = new MVector(RBTarget.velocity * fSecondsPerKM * fDistanceBetweenPlayer);

			return vForwardPrediction;
		}

		/// <summary>Whether nNumber is a power of two.</summary>
		/// <param name="nNumber">The number to check.</param>
		public static bool IsPowerOfTwo(int nNumber) => (nNumber & (nNumber - 1)) == 0;

		/// <summary>The greatest common divisor of na and nb.</summary>
		public static int GreatestCommonDivisor(int na, int nb)
		{
			while (nb != 0)
			{
				int t = nb;
				nb = na % nb;
				na = t;
			}

			return na;
		}

		/// <summary>The lowest common multiple of na and nb.</summary>
		public static int LowestCommonMultiple(int na, int nb)
		{
			int GCD = GreatestCommonDivisor(na, nb);
			return GCD == 0 ? 0 : (na / GCD) * nb;
		}

		/// <summary>Wraps f between fMin and fMax.</summary>
		/// <param name="f">The float number to wrap.</param>
		/// <param name="fMin">The minimum value to wrap.</param>
		/// <param name="fMax">The maximum value to wrap.</param>
		public static float Wrap(float f, float fMin, float fMax)
		{
			float s = fMax - fMin;
			float e = f;
			while (e < fMin)
			{
				e += s;
			}

			while (e > fMax)
			{
				e -= s;
			}

			return e;
		}

		/// <summary>Wraps n between nMin and nMax.</summary>
		/// <param name="n">The float number to wrap.</param>
		/// <param name="nMin">The minimum value to wrap.</param>
		/// <param name="nMax">The maximum value to wrap.</param>
		public static float Wrap(int n, int nMin, int nMax)
		{
			float s = nMax - nMin;
			float e = n;
			while (e < nMin)
			{
				e += s;
			}

			while (e > nMax)
			{
				e -= s;
			}

			return e;
		}

		/// <summary>Whether v1 is parallel to v2 within fParallelThreshold.</summary>
		/// <param name="v1">Whether this vector is parallel to the other.</param>
		/// <param name="v2">Whether this vector is parallel to the other.</param>
		/// <param name="fParallelThreshold">The threshold to consider parallel vectors.</param>
		public static bool Parallel(MVector v1, MVector v2, float fParallelThreshold = 0.999845f)
		    => Mathf.Abs(v1 | v2) >= fParallelThreshold;

		/// <summary>Whether vVector has been normalised.</summary>
		/// <param name="vVector">The vector to check.</param>
		public static bool IsNormalised(MVector vVector) => Mathf.Abs(1f - vVector.SqrMagnitude) < .01f;

		/// <summary>The angle in degrees pointing towards vDirection using the X-Axis and Z-Axis. (For 3D space)</summary>
		/// <param name="vDirection">The direction to calculate an angle towards.</param>
		public static float AngleFromVector3D(Vector3 vDirection)
		{
			return Mathf.Atan2(vDirection.x, vDirection.z) * Mathf.Rad2Deg;
		}

		/// <summary>The angle in degrees pointing towards vDirection using the X-Axis and Y-Axis. (For 2D space)</summary>
		/// <param name="vDirection">The direction to calculate an angle towards.</param>
		public static float AngleFromVector2D(Vector3 vDirection)
		{
			return -Mathf.Atan2(vDirection.x, vDirection.y) * Mathf.Rad2Deg;
		}

		/// <summary>Returns a normalised MVector at fDegrees, relative to dirForward.</summary>
		/// <param name="fDegrees">The angle offset.</param>
		/// <param name="dirForward">The forward direction.</param>
		public static MVector VectorFromAngle(float fDegrees, EDirection dirForward)
		{
			return dirForward switch
			{
				EDirection.Forward => new MVector(Mathf.Sin(fDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(fDegrees * Mathf.Deg2Rad)),
				EDirection.Right => new MVector(Mathf.Cos(-fDegrees * Mathf.Deg2Rad), 0, Mathf.Sin(-fDegrees * Mathf.Deg2Rad)),
				EDirection.Up => new MVector(Mathf.Sin(-fDegrees * Mathf.Deg2Rad), Mathf.Cos(-fDegrees * Mathf.Deg2Rad), 0),

				EDirection.Back => -VectorFromAngle(fDegrees, EDirection.Forward),
				EDirection.Left => -VectorFromAngle(fDegrees, EDirection.Right),
				EDirection.Down => -VectorFromAngle(fDegrees, EDirection.Up),
				_ => MVector.Forward,
			};
		}

		/// <summary>The 11-Degree Minimax Approximation Sine and 10-Degree Minimax Approximation Cosine over an angle.</summary>
		/// <param name="fSine">The Sine result over fValue.</param>
		/// <param name="fCosine">The Cosine result over fValue.</param>
		/// <param name="fValue">The angle.</param>
		public static void SinCos(out float fSine, out float fCosine, float fValue)
		{
			float quotient = (Utils.kInversePI * 0.5f) * fValue;
			if (fValue >= 0.0f)
			{
				quotient = (int)(quotient + 0.5f);
			}
			else
			{
				quotient = (int)(quotient - 0.5f);
			}
			float y = fValue - (2.0f * Mathf.PI) * quotient;

			// Map y to [-PI / 2, PI / 2] with Sin(y) = Sin(Value).
			float sign;
			if (y > Utils.kHalfPI)
			{
				y = Mathf.PI - y;
				sign = -1.0f;
			}
			else if (y < -Utils.kHalfPI)
			{
				y = -Mathf.PI - y;
				sign = -1.0f;
			}
			else
			{
				sign = +1.0f;
			}

			float y2 = y * y;

			// 11-degree minimax approximation
			fSine = (((((-2.3889859e-08f * y2 + 2.7525562e-06f) * y2 - 0.00019840874f) * y2 + 0.0083333310f) * y2 - 0.16666667f) * y2 + 1.0f) * y;

			// 10-degree minimax approximation
			float p = ((((-2.6051615e-07f * y2 + 2.4760495e-05f) * y2 - 0.0013888378f) * y2 + 0.041666638f) * y2 - 0.5f) * y2 + 1.0f;
			fCosine = sign * p;
		}
	}
}
