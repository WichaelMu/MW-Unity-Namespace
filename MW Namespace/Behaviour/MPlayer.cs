using System.Collections;
using UnityEngine;
using MW.Vector;

namespace MW.Behaviour
{
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

		public virtual void FixedUpdate()
		{
			if (HasStoppedReceivingMovementInput())
				return;

			Rigidbody.MovePosition(Position + Velocity * Time.fixedDeltaTime);
		}

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

		public float GetSpeed()
		{
			return Rigidbody.velocity.magnitude;
		}

		public void GetSpeedAndDirection(out float Speed, out MVector Direction)
		{
			Speed = GetSpeed();
			Direction = Rigidbody.velocity;
		}

		#endregion


	}
}
