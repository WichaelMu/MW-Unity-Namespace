using static MW.Utils;
using static MW.Math.Magic.Fast;
#if RELEASE
using static MW.Diagnostics.Arrow;
using UnityEngine;
#endif // RELEASE

namespace MW.Kinetic
{
	/// <summary></summary>
	/// <decorations decor="public static class"></decorations>
	public static class Kinematics
	{
		/// <summary>Convert inspector speed to m/s.</summary>
		/// <decorations decor="public const int"></decorations>
		public const int kVelocityRatio = 50;

#if RELEASE
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
#endif // RELEASE

		/// <summary>Calculates a launch velocity towards a target at a given speed.</summary>
		/// <decorations decor="public static bool"></decorations>
		/// <param name="LaunchVelocity">The out velocity of the launch.</param>
		/// <param name="Origin">Where the launch will begin.</param>
		/// <param name="Target">The intended destination of the launched projectile.</param>
		/// <param name="LaunchSpeed">The speed of the launched projectile.</param>
		/// <param name="bFavourHighArc">Should the launched projectile attain the maximum height?</param>
		/// <param name="bIs3D">True if using 3D physics.</param>
		/// <param name="bDrawDebug">True to draw debug lines of the arc.</param>
		/// <returns>True if a solution to hit Target from Origin at LaunchSpeed exists.</returns>
		public static bool LaunchTowards(out MVector LaunchVelocity, MVector Origin, MVector Target, float LaunchSpeed, bool bFavourHighArc, bool bIs3D = true
#if RELEASE
			, bool bDrawDebug = false
#endif // RELEASE
			)
		{
			MVector RelativeTargetPosition = Target - Origin;
			MVector DirectionToTargetIgnoringAltitude = RelativeTargetPosition.XZ.Normalised;
			float DisplacementToTargetIgnoringAltitude = RelativeTargetPosition.XZ.Magnitude;

			float YDisplacement = RelativeTargetPosition.Y;

			float LaunchSpeedSquared = LaunchSpeed * LaunchSpeed;

			float Gravity = -F_GetGravity(
#if RELEASE
				bIs3D
#endif // RELEASE
				);
			float Radicand = (LaunchSpeedSquared * LaunchSpeedSquared) - Gravity * ((Gravity * (DisplacementToTargetIgnoringAltitude * DisplacementToTargetIgnoringAltitude)) + (2f * YDisplacement * LaunchSpeedSquared));
			if (Radicand < 0f)
			{
				LaunchVelocity = MVector.NaN;
				return false;
			}

			float Sqrt = FSqrt(Radicand);

			float GravityByDistance = 1 * FInverse(Gravity * DisplacementToTargetIgnoringAltitude);

			float Solution1 = (LaunchSpeedSquared + Sqrt) * GravityByDistance;
			float Solution2 = (LaunchSpeedSquared - Sqrt) * GravityByDistance;

			float ASqr1 = (Solution1 * Solution1) + 1f;
			float BSqr1 = (Solution2 * Solution2) + 1f;

			float ADisplacement = LaunchSpeedSquared * FInverse(ASqr1);
			float BDisplacement = LaunchSpeedSquared * FInverse(BSqr1);

			float DisplacementArcPreference = bFavourHighArc ? Min(ADisplacement, BDisplacement) : Max(ADisplacement, BDisplacement);
			float Sign = bFavourHighArc
				? (ADisplacement < BDisplacement)
					? FMath.Sign(Solution1)
					: FMath.Sign(Solution2)

				: (ADisplacement > BDisplacement)
					? FMath.Sign(Solution1)
					: FMath.Sign(Solution2)
			;

			float PreferenceMagnitude = FSqrt(DisplacementArcPreference);
			float LaunchHeight = FSqrt(LaunchSpeedSquared - DisplacementArcPreference);

			LaunchVelocity = (PreferenceMagnitude * DirectionToTargetIgnoringAltitude) + (LaunchHeight * Sign * MVector.Up);

#if RELEASE
			if (bDrawDebug)
			{
				const float kResolution = .033333f;
				MVector DebugOrigin = Origin;
				for (float TimeStep = 0f; TimeStep < 1f; TimeStep += kResolution)
				{
					float TimeInFlight = (TimeStep + kResolution) * DisplacementToTargetIgnoringAltitude * FInverse(PreferenceMagnitude);

					MVector DebugDestination = Origin + TimeInFlight * LaunchVelocity + new MVector(0f, .5f * -Gravity * TimeInFlight * TimeInFlight);

					Debug.DrawLine(DebugOrigin, DebugDestination, Color.green);
					DebugOrigin = DebugDestination;
				}
			}
#endif // RELEASE

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
		/// <param name="bLaunchRegardless">True to ignore the height limitation and compute a velocity anyway.</param>
		/// <docreturns>The velocity required to launch a projectile from Origin to Target, or MVector.NaN if impossible.</docreturns>
		/// <returns>The velocity required to launch a projectile from Origin to Target, or <see cref="MVector.NaN"/> if impossible.</returns>
		public static MVector LaunchTowards(MVector Origin, MVector Target, float TargetHeight, out float Time,
#if RELEASE
			bool b3DGravity = true, 
#endif // RELEASE
			bool bLaunchRegardless = false)
		{
			return LaunchTowards(Origin, Target, TargetHeight, F_GetGravity(
#if RELEASE
				b3DGravity
#endif // RELEASE
				), out Time, bLaunchRegardless
#if RELEASE
				, b3DGravity
#endif // RELEASE
				);
		}

		/// <summary>Computes a velocity to launch a Rigidbody from Origin to Target achieving a TargetHeight.</summary>
		/// <remarks>Will not compute if TargetHeight cannot reach To.y and bLaunchRegardless is false.</remarks>
		/// <decorations decor="public static MVector"></decorations>
		/// <param name="Origin">Where to launch from.</param>
		/// <param name="Target">Where to launch to.</param>
		/// <param name="TargetHeight">The apex.</param>
		/// <param name="GravityMagnitude">True if using 3D Physics.</param>
		/// <param name="Time">Point on launch trajectory at a time. Flight time.</param>
		/// <param name="bLaunchRegardless">True to ignore the height limitation and compute a velocity anyway.</param>
		/// <param name="b3DGravity">True if using 3D Physics.</param>
		/// <docreturns>The velocity required to launch a projectile from Origin to Target, or MVector.NaN if impossible.</docreturns>
		/// <returns>The velocity required to launch a projectile from Origin to Target, or <see cref="MVector.NaN"/> if impossible.</returns>
		public static MVector LaunchTowards(MVector Origin, MVector Target, float TargetHeight, float GravityMagnitude, out float Time, bool bLaunchRegardless
#if RELEASE
			, bool b3DGravity = true
#endif // RELEASE
			)
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
			float InverseGravity = 1f * FInverse(GravityMagnitude);

			MVector VY = ComputeJumpVelocity(MVector.Up, TargetHeight,
#if RELEASE
				b3DGravity
#else
				F_GetGravity()
#endif // RELEASE
				);
			Time = (FSqrt(-2f * TargetHeight * InverseGravity) + FSqrt(2 * (DeltaY - TargetHeight) * InverseGravity));
			MVector VXZ = FInverse(Time) * DeltaXZ;

			MVector LaunchVelocity = -FMath.Sign(GravityMagnitude) * VY + VXZ;

			return LaunchVelocity;
		}

		/// <summary>Computes a velocity to launch a Rigidbody from Origin to Target achieving a TargetHeight.</summary>
		/// <remarks>Will not compute if TargetHeight cannot reach To.y and bLaunchRegardless is false.</remarks>
		/// <decorations decor="public static MVector"></decorations>
		/// <param name="Origin">Where to launch from.</param>
		/// <param name="Target">Where to launch to.</param>
		/// <param name="TargetHeight">The apex.</param>
		/// <param name="GravityMagnitude">True if using 3D Physics.</param>
		/// <param name="Time">Point on launch trajectory at a time. Flight time.</param>
		/// <param name="Arc">Out the points in a line where the projectile will follow from Origin to Target.</param>
		/// <param name="ArcResolution">The number of points for the Arc.</param>
		/// <param name="bLaunchRegardless">True to ignore the height limitation and compute a velocity anyway.</param>
		/// <param name="b3DGravity">True if using 3D Physics.</param>
		/// <docreturns>The velocity required to launch a projectile from Origin to Target, or MVector.NaN if impossible.</docreturns>
		/// <returns>The velocity required to launch a projectile from Origin to Target, or <see cref="MVector.NaN"/> if impossible.</returns>
		public static MVector LaunchTowards(MVector Origin, MVector Target, float TargetHeight, float GravityMagnitude, out float Time, out MVector[] Arc, int ArcResolution = 30, bool bLaunchRegardless = false
#if RELEASE
			, bool b3DGravity = true
#endif // RELEASE
			)
		{
			MVector LaunchVelocity = LaunchTowards(Origin, Target, TargetHeight, GravityMagnitude, out Time,
#if RELEASE
				b3DGravity, 
#endif // RELEASE
				bLaunchRegardless);
			Arc = GetArc(LaunchVelocity, Time, ArcResolution
#if RELEASE
				, b3DGravity
#endif // RELEASE
				);

			return LaunchVelocity;
		}

		/// <summary>Gets the Arc of a launched projectile, given the time.</summary>
		/// <decorations decor="public static MVector[]"></decorations>
		/// <param name="LaunchVelocity">The speed and direction of the launched projectile.</param>
		/// <param name="Time">Flight time.</param>
		/// <param name="ArcResolution">The number of points for the Arc.</param>
		/// <param name="bIs3D">True if using 3D Physics.</param>
		/// <returns>The trajectory of a projectile travelling at LaunchVelocity will travel through.</returns>
		public static MVector[] GetArc(MVector LaunchVelocity, float Time, int ArcResolution = 30
#if RELEASE
			, bool bIs3D = true
#endif // RELEASE
			)
		{
			if (ArcResolution <= 0)
				throw new System.ArgumentException($"{nameof(ArcResolution)} must be greater than zero!");

			if (Time <= MVector.kEpsilon)
				return System.Array.Empty<MVector>();

			MVector[] RetVal = new MVector[ArcResolution];

			float InverseResolutionTime = FInverse(ArcResolution * Time);

			for (int i = 0; i <= ArcResolution; ++i)
			{
				float Simulation = i * InverseResolutionTime;

				MVector Arc = Simulation * LaunchVelocity + Simulation * Simulation * .5f * V_GetGravity(
#if RELEASE
					bIs3D
#endif // RELEASE
					);

				RetVal[i] = Arc;
			}

			return RetVal;
		}

#if RELEASE
		/// <summary>The Arc of a Projectile travelling through the world.</summary>
		/// <decorations decor="public static MArray&lt;ProjectileArcTracer&gt;"></decorations>
		/// <param name="Physics">The Rigidbody associated with a Projectile.</param>
		/// <param name="StartPosition">The initial position of the Projectile.</param>
		/// <param name="Gravity">The pull of Gravity affecting this Projectile.</param>
		/// <param name="Resolution">The number of points to sample the trajectory.</param>
		/// <param name="bDrawDebug">True to draw debug lines of the Projectile's trajectory.</param>
		/// <param name="MaxSimulationTime">The maximum time-step for the trajectory simulation.</param>
		/// <returns>An array of this Projectile's Position and velocity at the given Time.</returns>
		public static MArray<ProjectileArcTracer> GetProjectileArc(Rigidbody Physics, Vector3 StartPosition, float Gravity, int Resolution = 30, bool bDrawDebug = false, float MaxSimulationTime = 100f)
		{
			float SubstepDeltaTime = 1f * FInverse(Resolution);
			MVector Velocity = Physics.velocity;
			MVector End = StartPosition;
			float Time = 0f;

			MArray<ProjectileArcTracer> RetVal = new MArray<ProjectileArcTracer>(Resolution);
			while (Time < MaxSimulationTime)
			{
				float ActualStepDeltaTime = Min(MaxSimulationTime - Time, SubstepDeltaTime);
				Time += ActualStepDeltaTime;

				StartPosition = End;
				MVector OldVelocity = Velocity;
				Velocity = OldVelocity + new MVector(0f, Gravity * ActualStepDeltaTime, 0f);
				End = StartPosition + .5f * ActualStepDeltaTime * (OldVelocity + Velocity);

				RetVal.Push(new ProjectileArcTracer(End, Velocity, Time));
			}

			if (bDrawDebug)
			{
				for (int i = 0; i < RetVal.Num - 1; ++i)
					Debug.DrawLine(RetVal[i].Position, RetVal[i + 1].Position, Color.Lerp(Color.green, Color.red, 0.06667f * RetVal[i].Velocity.Magnitude), 5f);
			}

			return RetVal;
		}
#endif // RELEASE

		/// <summary>The Arc of a Projectile travelling through the world.</summary>
		/// <decorations decor="public static MArray&lt;ProjectileArcTracer&gt;"></decorations>
		/// <param name="Velocity">The speed and direction of this Projectile.</param>
		/// <param name="StartPosition">The initial position of the Projectile.</param>
		/// <param name="Gravity">The pull of Gravity affecting this Projectile.</param>
		/// <param name="Resolution">The number of points to sample the trajectory.</param>
		/// <param name="bDrawDebug">True to draw debug lines of the Projectile's trajectory.</param>
		/// <param name="MaxSimulationTime">The maximum time-step for the trajectory simulation.</param>
		/// <returns>An array of this Projectile's Position and velocity at the given Time.</returns>
		public static MArray<ProjectileArcTracer> GetProjectileArc(MVector Velocity, MVector StartPosition, float Gravity, int Resolution = 30,
#if RELEASE
			bool bDrawDebug = false, 
#endif // RELEASE
			float MaxSimulationTime = 100f)
		{
			float SubstepDeltaTime = 1f * FInverse(Resolution);
			MVector End = StartPosition;
			float Time = 0f;

			MArray<ProjectileArcTracer> RetVal = new MArray<ProjectileArcTracer>(Resolution);
			while (Time < MaxSimulationTime)
			{
				float ActualStepDeltaTime = Min(MaxSimulationTime - Time, SubstepDeltaTime);
				Time += ActualStepDeltaTime;

				StartPosition = End;
				MVector OldVelocity = Velocity;
				Velocity = OldVelocity + new MVector(0f, Gravity * ActualStepDeltaTime, 0f);
				End = StartPosition + .5f * ActualStepDeltaTime * (OldVelocity + Velocity);

				RetVal.Push(new ProjectileArcTracer(End, Velocity, Time));
			}

#if RELEASE
			if (bDrawDebug)
			{
				for (int i = 0; i < RetVal.Num - 1; ++i)
					Debug.DrawLine(RetVal[i].Position, RetVal[i + 1].Position, Color.Lerp(Color.green, Color.red, 0.06667f * RetVal[i].Velocity.Magnitude), 5f);
			}
#endif // RELEASE

			return RetVal;
		}

#if RELEASE
		/// <summary>The Arc of a Projectile travelling through the world.</summary>
		/// <decorations decor="public static bool"></decorations>
		/// <param name="Physics">The Rigidbody associated with a Projectile.</param>
		/// <param name="StartPosition">The initial position of the Projectile.</param>
		/// <param name="Gravity">The pull of Gravity affecting this Projectile.</param>
		/// <param name="Trajectory">Outs an array of this Projectile's Position and velocity at the given Time.</param>
		/// <param name="ProjectileRadius">The radius of this Projectile to consider for Collisions.</param>
		/// <param name="CollisionLayer">The Layer to identify Collisions.</param>
		/// <param name="Collisions">Outs an array of the Collisions this Projectile will encounter along the trajectory.</param>
		/// <param name="bStopOnCollision">True to stop simulating and calculating the trajectory upon Collision.</param>
		/// <param name="Resolution">The number of points to sample the trajectory.</param>
		/// <param name="bDrawDebug">True to draw debug lines of the Projectile's trajectory and Collisions.</param>
		/// <param name="MaxSimulationTime">The maximum time-step for the trajectory simulation.</param>
		/// <returns>True if this Projectile will Collide with something in CollisionLayer.</returns>
		public static bool GetProjectileArc(Rigidbody Physics, Vector3 StartPosition, float Gravity, out MArray<ProjectileArcTracer> Trajectory, float ProjectileRadius, LayerMask CollisionLayer, out MArray<ProjectileArcCollision> Collisions, bool bStopOnCollision = true, int Resolution = 30, bool bDrawDebug = false, float MaxSimulationTime = 100f)
		{
			float SubstepDeltaTime = 1f * FInverse(Resolution);
			MVector Velocity = Physics.velocity;
			MVector End = StartPosition;
			float Time = 0f;

			Trajectory = new MArray<ProjectileArcTracer>(Resolution);
			Collisions = new MArray<ProjectileArcCollision>();
			while (Time < MaxSimulationTime)
			{
				float ActualStepDeltaTime = Min(MaxSimulationTime - Time, SubstepDeltaTime);
				Time += ActualStepDeltaTime;

				StartPosition = End;
				MVector OldVelocity = Velocity;
				Velocity = OldVelocity + new MVector(0f, Gravity * ActualStepDeltaTime, 0f);
				End = StartPosition + .5f * ActualStepDeltaTime * (OldVelocity + Velocity);

				Collider[] Collision = UnityEngine.Physics.OverlapSphere(End, ProjectileRadius, CollisionLayer);
				for (int i = 0; i < Collision.Length; ++i)
					if (Collision[i].gameObject != Physics.gameObject)
						Collisions.PushUnique(new ProjectileArcCollision(End, Velocity, Collision[i]));

				if (bStopOnCollision && Collision.Length > 0)
					break;

				Trajectory.Push(new ProjectileArcTracer(End, Velocity, Time));
			}

			if (bDrawDebug)
			{
				for (int i = 0; i < Trajectory.Num - 1; ++i)
					Debug.DrawLine(Trajectory[i].Position, Trajectory[i + 1].Position, Color.Lerp(Color.green, Color.red, 0.06667f * Trajectory[i].Velocity.Magnitude), 5f);

				for (int i = 0; i < Collisions.Num; ++i)
					DebugArrow(Collisions[i].Point + MVector.One, -MVector.Up, Color.blue);
			}

			return Collisions.Num != 0;
		}

		/// <summary>The Arc of a Projectile travelling through the world.</summary>
		/// <decorations decor="public static bool"></decorations>
		/// <param name="Projectile">This Projectile; ignored when checking for Collisions.</param>
		/// <param name="Velocity">The speed and direction of this Projectile.</param>
		/// <param name="StartPosition">The initial position of the Projectile.</param>
		/// <param name="Gravity">The pull of Gravity affecting this Projectile.</param>
		/// <param name="Trajectory">Outs an array of this Projectile's Position and velocity at the given Time.</param>
		/// <param name="ProjectileRadius">The radius of this Projectile to consider for Collisions.</param>
		/// <param name="CollisionLayer">The Layer to identify Collisions.</param>
		/// <param name="Collisions">Outs an array of the Collisions this Projectile will encounter along the trajectory.</param>
		/// <param name="bStopOnCollision">True to stop simulating and calculating the trajectory upon Collision.</param>
		/// <param name="Resolution">The number of points to sample the trajectory.</param>
		/// <param name="bDrawDebug">True to draw debug lines of the Projectile's trajectory and Collisions.</param>
		/// <param name="MaxSimulationTime">The maximum time-step for the trajectory simulation.</param>
		/// <returns>True if this Projectile will Collide with something in CollisionLayer.</returns>
		public static bool GetProjectileArc(GameObject Projectile, MVector Velocity, Vector3 StartPosition, float Gravity, out MArray<ProjectileArcTracer> Trajectory, float ProjectileRadius, LayerMask CollisionLayer, out MArray<ProjectileArcCollision> Collisions, bool bStopOnCollision = true, int Resolution = 30, bool bDrawDebug = false, float MaxSimulationTime = 100f)
		{
			float SubstepDeltaTime = 1f * FInverse(Resolution);
			MVector End = StartPosition;
			float Time = 0f;

			Trajectory = new MArray<ProjectileArcTracer>(Resolution);
			Collisions = new MArray<ProjectileArcCollision>();
			while (Time < MaxSimulationTime)
			{
				float ActualStepDeltaTime = Min(MaxSimulationTime - Time, SubstepDeltaTime);
				Time += ActualStepDeltaTime;

				StartPosition = End;
				MVector OldVelocity = Velocity;
				Velocity = OldVelocity + new MVector(0f, Gravity * ActualStepDeltaTime, 0f);
				End = StartPosition + .5f * ActualStepDeltaTime * (OldVelocity + Velocity);

				Collider[] Collision = Physics.OverlapSphere(End, ProjectileRadius, CollisionLayer);
				for (int i = 0; i < Collision.Length; ++i)
					if (Collision[i].gameObject != Projectile)
						Collisions.PushUnique(new ProjectileArcCollision(End, Velocity, Collision[i]));

				if (bStopOnCollision && Collision.Length > 0)
					break;

				Trajectory.Push(new ProjectileArcTracer(End, Velocity, Time));
			}

			if (bDrawDebug)
			{
				for (int i = 0; i < Trajectory.Num - 1; ++i)
					Debug.DrawLine(Trajectory[i].Position, Trajectory[i + 1].Position, Color.Lerp(Color.green, Color.red, 0.06667f * Trajectory[i].Velocity.Magnitude), 5f);

				for (int i = 0; i < Collisions.Num; ++i)
					DebugArrow(Collisions[i].Point + MVector.One, -MVector.Up, Color.blue);
			}

			return Collisions.Num != 0;
		}
#endif // RELEASE

#if RELEASE
		static float F_GetGravity(bool bIs3D) => bIs3D ? Physics.gravity.y : Physics2D.gravity.y;
		static Vector3 V_GetGravity(bool bIs3D) => bIs3D ? Physics.gravity : Physics2D.gravity;
#else
		static float F_GetGravity() => 9.81f;
		static MVector V_GetGravity() => F_GetGravity() * MVector.Up;
#endif // RELEASE

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

#if RELEASE
		/// <summary>Compute the required velocity to jump at TargetHeight.</summary>
		/// <decorations decor="public static MVector"></decorations>
		/// <param name="Up">Normalised direction of jumping.</param>
		/// <param name="TargetHeight">The peak height achieved by this velocity.</param>
		/// <param name="b3DGravity">True if using 3D Physics.</param>
		/// <returns>The velocity required to jump Up at TargetHeight high.</returns>
		public static MVector ComputeJumpVelocity(MVector Up, float TargetHeight, bool b3DGravity = true)
		{
			return ComputeJumpVelocity(Up, TargetHeight, F_GetGravity(b3DGravity));
		}
#endif // RELEASE

		/// <summary>Compute the required velocity to jump at TargetHeight.</summary>
		/// <decorations decor="public static MVector"></decorations>
		/// <param name="Up">Normalised direction of jumping.</param>
		/// <param name="TargetHeight">The peak height achieved by this velocity.</param>
		/// <param name="GravityMagnitude">The pull of gravity in opposite Up.</param>
		/// <returns>The velocity required to jump Up at TargetHeight high.</returns>
		public static MVector ComputeJumpVelocity(MVector Up, float TargetHeight, float GravityMagnitude)
		{
			float U = -2f * GravityMagnitude * TargetHeight;

			return FSqrt(U) * Up;
		}
	}

	/// <summary>Projectile trajectory information.</summary>
	/// <decorations decor="public struct"></decorations>
	public struct ProjectileArcTracer
	{
		/// <summary>The world-position of this trace at <see cref="Time"/>.</summary>
		/// <docs>Te world-position of this trace at Time.</docs>
		/// <decorations decor="public MVector"></decorations>
		public MVector Position;
		/// <summary>The speed and direction of this trace at <see cref="Time"/>.</summary>
		/// <docs>The speed and direction of this trace at Time.</docs>
		/// <decorations decor="public MVector"></decorations>
		public MVector Velocity;
		/// <summary>The <see cref="Position"/> and <see cref="Velocity"/> of the trajectory at this Time.</summary>
		/// <docs>The Position and Velocity of the trajectory at this Time.</docs>
		/// <decorations decor="public float"></decorations>
		public float Time;

		internal ProjectileArcTracer(MVector Position, MVector Velocity, float Time)
		{
			this.Position = Position;
			this.Velocity = Velocity;
			this.Time = Time;
		}
	}

#if RELEASE
	/// <summary>Projectile trajectory impacts information.</summary>
	/// <decorations decor="public struct"></decorations>
	public struct ProjectileArcCollision
	{
		/// <summary>The world-position of the Collision.</summary>
		/// <decorations decor="public MVector"></decorations>
		public MVector Point;
		/// <summary>The speed and direction of the Collision.</summary>
		/// <decorations decor="public MVector"></decorations>
		public MVector Velocity;
		/// <summary>The impacting Collider.</summary>
		/// <decorations decor="public Collider"></decorations>
		public Collider Collider;

		internal ProjectileArcCollision(MVector Point, MVector Velocity, Collider Collider)
		{
			this.Point = Point;
			this.Velocity = Velocity;
			this.Collider = Collider;
		}

		public override int GetHashCode() => Collider.GetHashCode();
	}
#endif // RELEASE
}
