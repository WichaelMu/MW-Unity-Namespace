using MW.Diagnostics;
using MW.Easing;
using MW.Kinetic;
using UnityEngine;

namespace MW.Math
{
	/// <summary></summary>
	public static class Mathematics
	{

		/// <param name="Equation">The EEquation to use to accelerate.</param>
		/// <param name="CurrentSpeed">The current speed of the acceleration.</param>
		/// <param name="RateOfAcceleration">The rate to accelerate towards to terminal from current speed.</param>
		/// <param name="Terminal">The maximum speed.</param>
		/// <returns>The acceleration value using EEquation, using the current speed and rate of acceleration towards terminal over Time.deltaTime.</returns>
		public static float Acceleration(EEquation Equation, float CurrentSpeed, float RateOfAcceleration, float Terminal)
		{
			Terminal *= Kinematics.kVelocityRatio;

			if (RateOfAcceleration == 0)
				Log.E(nameof(RateOfAcceleration) + " cannot be zero");

			return Mathf.Clamp(Mathf.Lerp(CurrentSpeed / Time.deltaTime, Terminal, Interpolate.Ease(Equation, 0, 1, RateOfAcceleration)), 0, Terminal);
		}

		public static float Deceleration(float CurrentSpeed, float TargetVelocity = 0)
		{
			return -((TargetVelocity - CurrentSpeed) / Time.deltaTime);
		}

		static float fAR = 0;

		/// <param name="Self">The Rigidbody to calculate an acceleration rate.</param>
		/// <param name="Speed">The current speed of the Rigidbody in EUnit.MetresPerSecond.</param>
		/// <returns>The float rate of movement in EUnit.MetresPerSecond.</returns>
		[System.Obsolete("Use AccelerationRate(LastFrameInformation, ThisFrameInformation, DeltaTime) instead.")]
		public static float AccelerationRate(Rigidbody Self, float Speed)
		{
			float a = Self.velocity.magnitude - Speed / Time.deltaTime;
			float fAccelerationRate = (a - fAR) * -1;
			fAR = a;
			return fAccelerationRate;
		}

		/// <summary>The rate of acceleration in m/s^2.</summary>
		/// <param name="LastPosition"></param>
		/// <param name="ThisPosition"></param>
		/// <param name="DeltaTime">The difference in time between LastPosition and ThisPosition in seconds.</param>
		/// <returns>The rate of acceleration if an object went from LastPosition to ThisPosition in DeltaTime.</returns>
		public static float AccelerationRate(MVector LastPosition, MVector ThisPosition, float DeltaTime)
		{
			float DeltaPos = (ThisPosition - LastPosition).Magnitude;

			return DeltaPos / DeltaTime;
		}

		/// <summary>Converts a <see cref="Rigidbody"/>'s speed from metres per second to <see cref="EUnit"/>.</summary>
		/// <docs>Converts a Rigidbody's speed from metres per second to UUnit.</docs>
		/// <param name="Self">The Rigidbody to read a speed from.</param>
		/// <param name="Unit">The desired EUnit of measurement.</param>
		/// <returns>A speed reading from self in EUnit of measurement.</returns>
		public static float Speed(Rigidbody Self, EUnit Unit = EUnit.KilometersPerHour)
		{
			float speed = Self.velocity.magnitude;

			switch (Unit)
			{
				case EUnit.KilometersPerHour:
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
					Debug.LogWarning("Failed to convert speed to: " + nameof(Unit) + "\nReturning metres per second.");
					return speed;
			}
		}

		/// <summary>The direction to intercept Target relative to Projectile.</summary>
		/// <remarks>Requires movement in BOTH Rigidbodies. This function was designed specifically for aircraft.</remarks>
		/// <param name="Projectile">The Rigidbody predicting the movement of RBTarget.</param>
		/// <param name="Target">The Rigidbody to predict.</param>
		public static MVector PredictiveProjectile(Rigidbody Projectile, Rigidbody Target)
		{
			//  The approximate conversion from velocity in an rb.AddForce is the 2/3 of the force being applied.

			//  At a velocity of 950, the cannon travels at ~633 m/s.
			//  ~2279 kmph.

			float fSecondsPerKM = 1000 / (Projectile.velocity.magnitude * Utils.kTwoThirds);

			//  Distance between the RSelf and RBTarget in thousands.
			float fDistanceBetweenPlayer = Vector3.Distance(Projectile.position, Target.position) * Utils.kThousandth;

			MVector vForwardPrediction = new MVector(Target.velocity * fSecondsPerKM * fDistanceBetweenPlayer);

			return vForwardPrediction;
		}

		/// <summary>Predicts the path of a target's movement for a projectile to be launched.</summary>
		/// <param name="LaunchPosition">World location of where a projectile will be launched.</param>
		/// <param name="ConstantMoveSpeed">The movement speed of the projectile that will be launched.</param>
		/// <param name="TargetVelocity">The direction of where the target is heading and the speed it is travelling at.</param>
		/// <param name="TargetPosition">The world location of the target.</param>
		/// <returns>A world location of where a projectile should aim towards so that it intercepts the target.</returns>
		public static MVector PredictiveProjectile(MVector LaunchPosition, float ConstantMoveSpeed, MVector TargetVelocity, MVector TargetPosition)
		{
			// If the target is not moving, return the target's current position.
			if (TargetVelocity.SqrMagnitude < MVector.kEpsilon)
			{
				return TargetPosition;
			}

			float DistanceFromCallerToTarget = MVector.Distance(LaunchPosition, TargetPosition) * Utils.kThousandth;
			MVector ForwardPrediction = (1000 / ConstantMoveSpeed) * DistanceFromCallerToTarget * TargetVelocity;

			return TargetPosition + ForwardPrediction;
		}

		/// <summary>Whether Number is a power of two.</summary>
		/// <param name="Number">The number to check.</param>
		/// <returns>Number &amp; (Number - 1) == 0.</returns>
		public static bool IsPowerOfTwo(int Number) => (Number & (Number - 1)) == 0;

		/// <summary>The greatest common divisor of na and nb.</summary>
		public static int GreatestCommonDivisor(int A, int B)
		{
			while (B != 0)
			{
				int t = B;
				B = A % B;
				A = t;
			}

			return A;
		}

		/// <summary>The lowest common multiple of na and nb.</summary>
		public static int LowestCommonMultiple(int A, int B)
		{
			int GCD = GreatestCommonDivisor(A, B);
			return GCD == 0 ? 0 : (A / GCD) * B;
		}

		/// <summary>Wraps f between Min and Max.</summary>
		/// <param name="f">The float number to wrap.</param>
		/// <param name="Min">The minimum value to wrap.</param>
		/// <param name="Max">The maximum value to wrap.</param>
		public static float Wrap(float f, float Min, float Max)
		{
			float s = Max - Min;
			float e = f;
			while (e < Min)
			{
				e += s;
			}

			while (e > Max)
			{
				e -= s;
			}

			return e;
		}

		/// <summary>Wraps n between Min and Max.</summary>
		/// <param name="n">The float number to wrap.</param>
		/// <param name="Min">The minimum value to wrap.</param>
		/// <param name="Max">The maximum value to wrap.</param>
		public static float Wrap(int n, int Min, int Max)
		{
			float s = Max - Min;
			float e = n;
			while (e < Min)
			{
				e += s;
			}

			while (e > Max)
			{
				e -= s;
			}

			return e;
		}

		/// <summary>Whether M1 is parallel to M2 within ParallelThreshold.</summary>
		/// <param name="M1">Whether this vector is parallel to the other.</param>
		/// <param name="M2">Whether this vector is parallel to the other.</param>
		/// <param name="ParallelThreshold">The threshold to consider parallel vectors.</param>
		public static bool Parallel(MVector M1, MVector M2, float ParallelThreshold = 0.999845f)
		    => Mathf.Abs(M1 | M2) >= ParallelThreshold;

		/// <summary>Whether Vector has been normalised.</summary>
		/// <param name="Vector">The vector to check.</param>
		public static bool IsNormalised(MVector Vector) => Mathf.Abs(1f - Vector.SqrMagnitude) < .01f;

		/// <summary>The angle in degrees pointing towards Direction using the X-Axis and Z-Axis. (For 3D space)</summary>
		/// <param name="Direction">The direction to calculate an angle towards.</param>
		public static float AngleFromVector3D(Vector3 Direction)
		{
			return Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg;
		}

		/// <summary>The angle in degrees pointing towards Direction using the X-Axis and Y-Axis. (For 2D space)</summary>
		/// <param name="Direction">The direction to calculate an angle towards.</param>
		public static float AngleFromVector2D(Vector3 Direction)
		{
			return -Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg;
		}

		/// <summary>Returns a normalised MVector at Degrees, relative to ForwardDirection.</summary>
		/// <param name="Degrees">The angle offset.</param>
		/// <param name="ForwardDirection">The forward direction.</param>
		public static MVector VectorFromAngle(float Degrees, EDirection ForwardDirection)
		{
			return ForwardDirection switch
			{
				EDirection.Forward => new MVector(Mathf.Sin(Degrees * Mathf.Deg2Rad), 0, Mathf.Cos(Degrees * Mathf.Deg2Rad)),
				EDirection.Right => new MVector(Mathf.Cos(-Degrees * Mathf.Deg2Rad), 0, Mathf.Sin(-Degrees * Mathf.Deg2Rad)),
				EDirection.Up => new MVector(Mathf.Sin(-Degrees * Mathf.Deg2Rad), Mathf.Cos(-Degrees * Mathf.Deg2Rad), 0),

				EDirection.Back => -VectorFromAngle(Degrees, EDirection.Forward),
				EDirection.Left => -VectorFromAngle(Degrees, EDirection.Right),
				EDirection.Down => -VectorFromAngle(Degrees, EDirection.Up),
				_ => MVector.Forward,
			};
		}

		/// <summary>The 11-Degree Minimax Approximation Sine and 10-Degree Minimax Approximation Cosine over an angle.</summary>
		/// <param name="Sine">The Sine result over fValue.</param>
		/// <param name="Cosine">The Cosine result over fValue.</param>
		/// <param name="Value">The angle.</param>
		public static void SinCos(out float Sine, out float Cosine, float Value)
		{
			float quotient = (Utils.kInversePI * 0.5f) * Value;
			if (Value >= 0.0f)
			{
				quotient = (int)(quotient + 0.5f);
			}
			else
			{
				quotient = (int)(quotient - 0.5f);
			}
			float y = Value - (2.0f * Mathf.PI) * quotient;

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
			Sine = (((((-2.3889859e-08f * y2 + 2.7525562e-06f) * y2 - 0.00019840874f) * y2 + 0.0083333310f) * y2 - 0.16666667f) * y2 + 1.0f) * y;

			// 10-degree minimax approximation
			float p = ((((-2.6051615e-07f * y2 + 2.4760495e-05f) * y2 - 0.0013888378f) * y2 + 0.041666638f) * y2 - 0.5f) * y2 + 1.0f;
			Cosine = sign * p;
		}
	}
}
