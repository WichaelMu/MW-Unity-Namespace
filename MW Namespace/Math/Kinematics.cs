using UnityEngine;

namespace MW.Kinetic
{
	public static class Kinematics
	{
		/// <summary>Convert inspector speed to m/s.</summary>
		public const int kVelocityRatio = 50;

		/// <summary>If the distance between From and To is &lt;= Tolerance.</summary>
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
		/// <param name="Rigidbody">The Rigidbody to move.</param>
		/// <param name="Target">The Transform destination.</param>
		/// <param name="Velocity">The rate at which self moves towards target.</param>
		/// <param name="MaxDegreesDeltaPerFrame">The maximum degrees self can turn towards target per frame.</param>
		public static void HomeTowards(Rigidbody Rigidbody, Transform Target, float Velocity, float MaxDegreesDeltaPerFrame)
		{
			HomeTowards(Rigidbody, Target.position, Velocity, MaxDegreesDeltaPerFrame);
		}

		/// <summary>Moves Rigidbody towards target while moving at velocity with a maximum turn angle of MaxDegreesDeltaPerFrame.</summary>
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
		/// <param name="Rigidbody">The Rigidbody2D to move.</param>
		/// <param name="Target">The Transform destination.</param>
		/// <param name="Velocity">The rate at which self moves towards target.</param>
		/// <param name="MaxDegreesDeltaPerFrame">The maximum degrees self can turn towards target per frame.</param>
		public static void HomeTowards(Rigidbody2D Rigidbody, Transform Target, float Velocity, float MaxDegreesDeltaPerFrame)
		{
			HomeTowards(Rigidbody, Target.position, Velocity, MaxDegreesDeltaPerFrame);
		}

		/// <summary>Moves Rigidbody towards target while moving at velocity with a maximum turn angle of MaxDegreesDeltaPerFrame.</summary>
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
	}
}
