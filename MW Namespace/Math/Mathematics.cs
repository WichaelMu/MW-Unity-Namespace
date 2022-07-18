using MW.Diagnostics;
using MW.Easing;
using MW.Kinetic;
using MW.Math.Magic;
using UnityEngine;
using static MW.Utils;

namespace MW.Math
{
	/// <summary></summary>
	/// <decorations decor="public static class"></decorations>
	public static class Mathematics
	{

		/// <param name="Equation">The EEquation to use to accelerate.</param>
		/// <decorations decor="public static float"></decorations>
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

		/// <summary>The rate of deceleration.</summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="CurrentSpeed"></param>
		/// <param name="TargetVelocity"></param>
		/// <returns></returns>
		public static float Deceleration(float CurrentSpeed, float TargetVelocity = 0)
		{
			return -((TargetVelocity - CurrentSpeed) / Time.deltaTime);
		}

		static float fAR = 0;

		/// <decorations decor="[Obsolete] public static float"></decorations>
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
		/// <decorations decor="public static float"></decorations>
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
		/// <decorations decor="public static float"></decorations>
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
		/// <decorations decor="public static MVector"></decorations>
		/// <param name="Projectile">The Rigidbody predicting the movement of RBTarget.</param>
		/// <param name="Target">The Rigidbody to predict.</param>
		public static MVector PredictiveProjectile(Rigidbody Projectile, Rigidbody Target)
		{
			//  The approximate conversion from velocity in an rb.AddForce is the 2/3 of the force being applied.

			//  At a velocity of 950, the cannon travels at ~633 m/s.
			//  ~2279 kmph.

			float fSecondsPerKM = 1000 / (Projectile.velocity.magnitude * Utils.kTwoThirds);

			//  Distance between the RSelf and RBTarget in thousands.
			float fDistanceBetweenPlayer = Vector3.Distance(Projectile.position, Target.position) * .001f;

			MVector vForwardPrediction = new MVector(Target.velocity * fSecondsPerKM * fDistanceBetweenPlayer);

			return vForwardPrediction;
		}

		/// <summary>Predicts the path of a target's movement for a projectile to be launched.</summary>
		/// <decorations decor="public static MVector"></decorations>
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

			float DistanceFromCallerToTarget = MVector.Distance(LaunchPosition, TargetPosition) * .001f;
			MVector ForwardPrediction = (1000 / ConstantMoveSpeed) * DistanceFromCallerToTarget * TargetVelocity;

			return TargetPosition + ForwardPrediction;
		}

		/// <summary>Whether Number is a power of two.</summary>
		/// <decorations decor="public static bool"></decorations>
		/// <param name="Number">The number to check.</param>
		/// <returns>Number &amp; (Number - 1) == 0.</returns>
		public static bool IsPowerOfTwo(int Number) => (Number & (Number - 1)) == 0;

		/// <summary>The greatest common divisor of A and B.</summary>
		/// <decorations decor="public static int"></decorations>
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

		/// <summary>The lowest common multiple of A and B.</summary>
		/// <decorations decor="public static int"></decorations>
		public static int LowestCommonMultiple(int A, int B)
		{
			int GCD = GreatestCommonDivisor(A, B);
			return GCD == 0 ? 0 : (A / GCD) * B;
		}

		/// <summary>Wraps f between Min and Max.</summary>
		/// <decorations decor="public static float"></decorations>
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
		/// <decorations decor="public static float"></decorations>
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
		/// <decorations decor="public static bool"></decorations>
		/// <param name="M1">Whether this vector is parallel to the other.</param>
		/// <param name="M2">Whether this vector is parallel to the other.</param>
		/// <param name="ParallelThreshold">The threshold to consider parallel vectors.</param>
		public static bool Parallel(MVector M1, MVector M2, float ParallelThreshold = 0.999845f)
		    => Mathf.Abs(M1 | M2) >= ParallelThreshold;

		/// <summary>Whether Vector has been normalised.</summary>
		/// <decorations decor="public static bool"></decorations>
		/// <param name="Vector">The vector to check.</param>
		public static bool IsNormalised(MVector Vector) => Mathf.Abs(1f - Vector.SqrMagnitude) < .01f;

		/// <summary>The angle in degrees pointing towards Direction using the X-Axis and Z-Axis. (For 3D space)</summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Direction">The direction to calculate an angle towards.</param>
		public static float AngleFromVector3D(Vector3 Direction)
		{
			return Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg;
		}

		/// <summary>The angle in degrees pointing towards Direction using the X-Axis and Y-Axis. (For 2D space)</summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Direction">The direction to calculate an angle towards.</param>
		public static float AngleFromVector2D(Vector3 Direction)
		{
			return -Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg;
		}

		/// <summary>Returns a normalised MVector at Degrees, relative to ForwardDirection.</summary>
		/// <decorations decor="public static MVector"></decorations>
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
		/// <decorations decor="public static void"></decorations>
		/// <param name="Sine">The Sine result over AngleInDegrees.</param>
		/// <param name="Cosine">The Cosine result over AngleInDegrees.</param>
		/// <param name="Angle">The angle in degrees.</param>
		public static void SinCos(out float Sine, out float Cosine, float Angle)
		{
			float Quotient = (kInversePI * 0.5f) * Angle;
			if (Angle >= 0.0f)
			{
				Quotient = (int)(Quotient + 0.5f);
			}
			else
			{
				Quotient = (int)(Quotient - 0.5f);
			}

			float A = Angle - (2.0f * Mathf.PI) * Quotient;

			// Map A to [-PI / 2, PI / 2] with Sin(A) = Sin(Value).
			float Sign;
			if (A > kHalfPI)
			{
				A = Mathf.PI - A;
				Sign = -1.0f;
			}
			else if (A < -kHalfPI)
			{
				A = -Mathf.PI - A;
				Sign = -1.0f;
			}
			else
			{
				Sign = +1.0f;
			}

			float A2 = A * A;

			// 11-degree minimax approximation
			Sine = (((((-2.3889859e-08f * A2 + 2.7525562e-06f) * A2 - 0.00019840874f) * A2 + 0.0083333310f) * A2 - 0.16666667f) * A2 + 1.0f) * A;

			// 10-degree minimax approximation
			Cosine = Sign * ((((-2.6051615e-07f * A2 + 2.4760495e-05f) * A2 - 0.0013888378f) * A2 + 0.041666638f) * A2 - 0.5f) * A2 + 1.0f;
		}

		/// <summary>Calculates the Square Distance between two Vector3s.</summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="L"></param>
		/// <param name="R"></param>
		/// <returns>The Square Distance between L and R.</returns>
		public static float SqrDistance(Vector3 L, Vector3 R)
		{
			float x = L.x - R.x;
			float y = L.y - R.y;
			float z = L.z - R.z;

			return x * x + y * y + z * z;
		}

		/// <summary>A fast version of <see cref="Vector3.Distance"/>.</summary>
		/// <docs>A fast version of Vector3.Distance().</docs>
		/// <decorations decor="public static float"></decorations>
		/// <param name="L"></param>
		/// <param name="R"></param>
		/// <returns>The distance between L and R.</returns>
		public static float Distance(Vector3 L, Vector3 R)
		{
			return Fast.Sqrt(SqrDistance(L, R));
		}
	}
}
