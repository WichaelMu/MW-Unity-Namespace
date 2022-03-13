using System.Collections;
using UnityEngine;

namespace MW.Behaviour
{
	/// <summary>The base class of a Player in a three-dimensional world. Extends Player.</summary>
	[RequireComponent(typeof(Rigidbody))]
	public class MPlayer : Player
	{
		Rigidbody Rigidbody;

		public override void Awake()
		{
			InitialisePlayer();

			Rigidbody = GetComponent<Rigidbody>();
		}

		#region Player Movement

		public override void FixedUpdate()
		{
			if (HasStoppedReceivingMovementInput())
				return;

			Rigidbody.MovePosition(Position + Velocity * Time.fixedDeltaTime);
		}

		/// <summary>The default implementation for movement input.</summary>
		/// <param name="ForwardThrow">Forward input. Default is Z axis.</param>
		/// <param name="RightThrow">Right input. Default is X axis.</param>
		public override void MovementInput(float ForwardThrow, float RightThrow)
		{
			if (HasStoppedReceivingMovementInput())
				return;

			Velocity = new MVector(RightThrow, 0, ForwardThrow).Normalise();
			Velocity *= MovementSpeed;
		}

		/// <summary>Adds force upwards to this player. Default direction is MVector.Up.</summary>
		/// <remarks>Uses Rigidbody.AddForce(Vector3) to enforce jumping, by default.</remarks>
		/// <param name="Force">The amount of force to apply.</param>
		public virtual void Jump(float Force)
		{
			Rigidbody.AddForce(MVector.Up * Force);
		}

		/// <summary>The velocity of this Player, respective to attached Rigidbody.</summary>
		/// <returns>The speed in Unity-units per second.</returns>
		public float GetSpeed()
		{
			return Rigidbody.velocity.magnitude;
		}

		/// <summary>Gets the speed and direction of this Player.</summary>
		/// <param name="Speed">Out float speed in Unity-units per second.</param>
		/// <param name="Direction">Out MVector normalised direction this player is moving. Relative to the attached Rigidbody.</param>
		public void GetSpeedAndDirection(out float Speed, out MVector Direction)
		{
			Speed = GetSpeed();
			Direction = Rigidbody.velocity.normalized;
		}

		#endregion


	}
}
