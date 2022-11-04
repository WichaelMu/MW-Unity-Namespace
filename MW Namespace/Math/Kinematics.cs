using MW.Math.Magic;
using UnityEngine;

namespace MW.Kinetic
{
	/// <summary></summary>
	/// <decorations decor="public static class"></decorations>
	public static class Kinematics
	{
		/// <summary>Convert inspector speed to m/s.</summary>
		/// <decorations decor="public const int"></decorations>
		public const int kVelocityRatio = 50;

		/// <summary>If the distance between From and To is &lt;= Tolerance.</summary>
		/// <decorations decor="public static bool"></decorations>
		/// <param name="From">The reference Vector3 to compare.</param>
		/// <param name="To">The target Vector3 to compare.</param>
		/// <param name="Tolerance">The range that is considered if From has 'reached' To.</param>
		/// <returns>True if the distance between From and To are &lt;= Tolerance.</returns>
		public static bool HasReached(Vector3 From, Vector3 To, float Tolerance = .1f)
		{
			Vector3 Difference = From - To;

			return Difference.x <= Tolerance && Difference.y <= Tolerance && Difference.z <= Tolerance;
		}

		/// <summary>Moves Rigidbody towards target while moving at velocity with a maximum turn angle of MaxDegreesDeltaPerFrame.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Rigidbody">The Rigidbody to move.</param>
		/// <param name="Target">The Transform destination.</param>
		/// <param name="Velocity">The rate at which self moves towards target.</param>
		/// <param name="MaxDegreesDeltaPerFrame">The maximum degrees self can turn towards target per frame.</param>
		public static void HomeTowards(Rigidbody Rigidbody, Transform Target, float Velocity, float MaxDegreesDeltaPerFrame)
		{
			HomeTowards(Rigidbody, Target.position, Velocity, MaxDegreesDeltaPerFrame);
		}

		/// <summary>Moves Rigidbody towards target while moving at velocity with a maximum turn angle of MaxDegreesDeltaPerFrame.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Rigidbody">The Rigidbody to move.</param>
		/// <param name="Target">The destination coordinates.</param>
		/// <param name="Velocity">The rate at which self moves towards target.</param>
		/// <param name="MaxDegreesDeltaPerFrame">The maximum degrees self can turn towards target per frame.</param>
		public static void HomeTowards(Rigidbody Rigidbody, Vector3 Target, float Velocity, float MaxDegreesDeltaPerFrame)
		{
			Transform _self = Rigidbody.transform;
			Rigidbody.velocity = _self.forward * Velocity * Time.deltaTime;
			Rigidbody.MoveRotation(Quaternion.RotateTowards(_self.rotation, Quaternion.LookRotation(Target - _self.position, _self.up), MaxDegreesDeltaPerFrame));
		}

		/// <summary>Moves Rigidbody towards target while moving at velocity with a maximum turn angle of MaxDegreesDeltaPerFrame.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Rigidbody">The Rigidbody2D to move.</param>
		/// <param name="Target">The Transform destination.</param>
		/// <param name="Velocity">The rate at which self moves towards target.</param>
		/// <param name="MaxDegreesDeltaPerFrame">The maximum degrees self can turn towards target per frame.</param>
		public static void HomeTowards(Rigidbody2D Rigidbody, Transform Target, float Velocity, float MaxDegreesDeltaPerFrame)
		{
			HomeTowards(Rigidbody, Target.position, Velocity, MaxDegreesDeltaPerFrame);
		}

		/// <summary>Moves Rigidbody towards target while moving at velocity with a maximum turn angle of MaxDegreesDeltaPerFrame.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Rigidbody">The Rigidbody2D to move.</param>
		/// <param name="Target">The destination coordinates.</param>
		/// <param name="Velocity">The rate at which self moves towards target.</param>
		/// <param name="MaxDegreesDeltaPerFrame">The maximum degrees self can turn towards target per frame.</param>
		public static void HomeTowards(Rigidbody2D Rigidbody, Vector3 Target, float Velocity, float MaxDegreesDeltaPerFrame)
		{
			Transform _Self = Rigidbody.transform;
			Rigidbody.velocity = _Self.up * Velocity * Time.deltaTime;
			Rigidbody.MoveRotation(Quaternion.RotateTowards(_Self.rotation, Quaternion.LookRotation(Target - _Self.position, -_Self.forward), MaxDegreesDeltaPerFrame));
		}

		/// <summary>Calculates a launch velocity towards a target at a given speed.</summary>
		/// <decorations decor="public static bool"></decorations>
		/// <param name="LaunchVelocity">The out velocity of the launch.</param>
		/// <param name="Origin">Where the launch will begin.</param>
		/// <param name="Target">The intended destination of the launched projectile.</param>
		/// <param name="LaunchSpeed">The speed of the launched projectile.</param>
		/// <param name="bFavourHighArc">Should the launched projectile attain the maximum height?</param>
		/// <returns>True if a solution to hit Target from Origin at LaunchSpeed exists.</returns>
		public static bool LaunchTowards(out MVector LaunchVelocity, Vector3 Origin, Vector3 Target, float LaunchSpeed, bool bFavourHighArc)
		{
			MVector RelativeTargetPosition = Target - Origin;
			MVector DirectionToTargetIgnoringAltitude = RelativeTargetPosition.XZ.Normalised;
			float DisplacementToTargetIgnoringAltitude = new MVector(RelativeTargetPosition.X, RelativeTargetPosition.Z).Magnitude;

			float YDisplacement = RelativeTargetPosition.Y;

			float LaunchSpeedSquared = LaunchSpeed * LaunchSpeed;

			float Gravity = -Physics.gravity.y;
			float Radicand = (LaunchSpeedSquared * LaunchSpeedSquared) - Gravity * ((Gravity * (DisplacementToTargetIgnoringAltitude * DisplacementToTargetIgnoringAltitude)) + (2f * YDisplacement * LaunchSpeedSquared));
			if (Radicand < 0f)
			{
				LaunchVelocity = MVector.Zero;
				return false;
			}

			float Sqrt = Fast.FSqrt(Radicand);

			float GravityByDistance = 1 / (Gravity * DisplacementToTargetIgnoringAltitude);

			float Solution1 = (LaunchSpeedSquared + Sqrt) * GravityByDistance;
			float Solution2 = (LaunchSpeedSquared - Sqrt) * GravityByDistance;

			float ASqr1 = (Solution1 * Solution1) + 1f;
			float BSqr1 = (Solution2 * Solution2) + 1f;

			float ADisplacement = LaunchSpeedSquared / ASqr1;
			float BDisplacement = LaunchSpeedSquared / BSqr1;

			float DisplacementArcPreference = bFavourHighArc ? Mathf.Min(ADisplacement, BDisplacement) : Mathf.Max(ADisplacement, BDisplacement);
			float Sign = bFavourHighArc
				? (ADisplacement < BDisplacement)
					? Mathf.Sign(Solution1)
					: Mathf.Sign(Solution2)

				: (ADisplacement > BDisplacement)
					? Mathf.Sign(Solution1)
					: Mathf.Sign(Solution2)
			;

			float PreferenceMagnitude = Fast.FSqrt(DisplacementArcPreference);
			float LaunchHeight = Fast.FSqrt(LaunchSpeedSquared - DisplacementArcPreference);

			LaunchVelocity = (PreferenceMagnitude * DirectionToTargetIgnoringAltitude) + (LaunchHeight * Sign * MVector.Up);
			return true;
		}

		/// <summary>Computes a velocity to launch a Rigidbody from Origin to Target achieving a TargetHeight.</summary>
		/// <remarks>Will not compute if TargetHeight cannot reach To.y and bLaunchRegardless is false.</remarks>
		/// <decorations decor="public static MVector"></decorations>
		/// <param name="Origin">Where to launch from.</param>
		/// <param name="Target">Where to launch to.</param>
		/// <param name="TargetHeight">The apex.</param>
		/// <param name="Time">Point on launch trajectory at a time. Flight time.</param>
		/// <param name="b3DGravity">True if using 3D Physics.</param>
		/// <param name="bLaunchRegardless">
		/// True to ignore the height limitation and compute a velocity anyway.
		/// May be inaccurate.
		/// </param>
		/// <docreturns>The velocity required to launch a projectile from Origin to Target, or MVector.NaN if impossible.</docreturns>
		/// <returns>The velocity required to launch a projectile from Origin to Target, or <see cref="MVector.NaN"/> if impossible.</returns>
		public static MVector LaunchTowards(MVector Origin, MVector Target, float TargetHeight, out float Time, bool b3DGravity = true, bool bLaunchRegardless = false)
		{
			return LaunchTowards(Origin, Target, TargetHeight, GetGravity(b3DGravity), out Time, bLaunchRegardless, b3DGravity);
		}

		/// <summary>Computes a velocity to launch a Rigidbody from Origin to Target achieving a TargetHeight.</summary>
		/// <remarks>Will not compute if TargetHeight cannot reach To.y and bLaunchRegardless is false.</remarks>
		/// <decorations decor="public static MVector"></decorations>
		/// <param name="Origin">Where to launch from.</param>
		/// <param name="Target">Where to launch to.</param>
		/// <param name="TargetHeight">The apex.</param>
		/// <param name="GravityMagnitude">True if using 3D Physics.</param>
		/// <param name="Time">Point on launch trajectory at a time. Flight time.</param>
		/// <param name="bLaunchRegardless">
		/// True to ignore the height limitation and compute a velocity anyway.
		/// May be inaccurate.
		/// </param>
		/// <param name="b3DGravity">True if using 3D Physics.</param>
		/// <docreturns>The velocity required to launch a projectile from Origin to Target, or MVector.NaN if impossible.</docreturns>
		/// <returns>The velocity required to launch a projectile from Origin to Target, or <see cref="MVector.NaN"/> if impossible.</returns>
		public static MVector LaunchTowards(MVector Origin, MVector Target, float TargetHeight, float GravityMagnitude, out float Time, bool bLaunchRegardless, bool b3DGravity = true)
		{
			float DeltaY = Target.Y - Origin.Y;

			if (DeltaY > TargetHeight)
			{
				if (bLaunchRegardless)
				{
					TargetHeight += DeltaY;
				}
				else
				{
					Time = -1f;
					return MVector.NaN;
				}
			}

			MVector DeltaXZ = new MVector(Target.X - Origin.X, 0f, Target.Z - Origin.Z);
			float InverseGravity = 1f / GravityMagnitude;

			MVector VY = ComputeJumpVelocity(MVector.Up, TargetHeight, b3DGravity);
			Time = (Fast.FSqrt(-2f * TargetHeight * InverseGravity) + Fast.FSqrt(2 * (DeltaY - TargetHeight) * InverseGravity));
			MVector VXZ = DeltaXZ / Time;

			MVector LaunchVelocity = -Mathf.Sign(GravityMagnitude) * VY + VXZ;

			return LaunchVelocity;
		}

		static float GetGravity(bool bIs3D) => bIs3D ? Physics.gravity.y : Physics2D.gravity.y;

		/// <summary>The G Force experienced by a GameObject between two positions over DeltaTime, under the pull of Gravity.</summary>
		/// <decorations decor="public static MVector"></decorations>
		/// <param name="LastPosition">The position before the current FixedUpdate call.</param>
		/// <param name="ThisPosition">The current position at this FixedUpdate call.</param>
		/// <param name="DeltaTime">The time between recording LastPosition and ThisPosition.</param>
		/// <param name="Gravity">The force of Gravity.</param>
		/// <returns>The direction/s of the G Force.</returns>
		public static MVector V_GForce(MVector LastPosition, MVector ThisPosition, float DeltaTime, MVector Gravity)
		{
			MVector DeltaPos = ThisPosition - LastPosition;

			float GravityMag = Gravity.Magnitude;
			DeltaTime *= GravityMag;

			return DeltaPos / (DeltaTime * GravityMag);
		}

		/// <summary>The G Force experienced by a GameObject between two positions over DeltaTime, under the pull of Gravity.</summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="LastPosition">The position before the current FixedUpdate call.</param>
		/// <param name="ThisPosition">The current position at this FixedUpdate call.</param>
		/// <param name="DeltaTime">The time between recording LastPosition and ThisPosition.</param>
		/// <param name="Gravity">The force of Gravity.</param>
		/// <returns>The G Force without an associated direction.</returns>
		public static float F_GForce(MVector LastPosition, MVector ThisPosition, float DeltaTime, MVector Gravity)
		{
			return V_GForce(LastPosition, ThisPosition, DeltaTime, Gravity).Magnitude;
		}

		/// <summary>Compute the required velocity to jump at TargetHeight.</summary>
		/// <decorations decor="public static MVector"></decorations>
		/// <param name="Up">Normalised direction of jumping.</param>
		/// <param name="TargetHeight">The peak height achieved by this velocity.</param>
		/// <param name="b3DGravity">True if using 3D Physics.</param>
		/// <returns>The velocity required to jump Up at TargetHeight high.</returns>
		public static MVector ComputeJumpVelocity(MVector Up, float TargetHeight, bool b3DGravity = true)
		{
			return ComputeJumpVelocity(Up, TargetHeight, GetGravity(b3DGravity));
		}

		/// <summary>Compute the required velocity to jump at TargetHeight.</summary>
		/// <decorations decor="public static MVector"></decorations>
		/// <param name="Up">Normalised direction of jumping.</param>
		/// <param name="TargetHeight">The peak height achieved by this velocity.</param>
		/// <param name="GravityMagnitude">The pull of gravity in opposite Up.</param>
		/// <returns>The velocity required to jump Up at TargetHeight high.</returns>
		public static MVector ComputeJumpVelocity(MVector Up, float TargetHeight, float GravityMagnitude)
		{
			float U = -2f * GravityMagnitude * TargetHeight;

			return Fast.FSqrt(U) * Up;
		}
	}
}
