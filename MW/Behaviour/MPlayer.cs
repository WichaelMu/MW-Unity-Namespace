using UnityEngine;

namespace MW.Behaviour
{
	/// <summary>The base class of a Player in a three-dimensional world. Extends <see cref="PlayerBase"/>.</summary>
	/// <docs>The base class of a Player in a three-dimensional world.</docs>
	/// <decorations decor="[RequireComponent{Rigidbody}] public class : PlayerBase"></decorations>
	[RequireComponent(typeof(Rigidbody))]
	public class MPlayer : PlayerBase
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

			Rigidbody.MovePosition(Position + (Time.fixedDeltaTime * Velocity));
		}

		/// <summary>The default implementation for <see cref="MovementInput(float, float)"/>.</summary>
		/// <docs>The default implementation for movement input.</docs>
		/// <decorations decor="public override void"></decorations>
		/// <param name="ForwardThrow">Forward input. Default is Z axis.</param>
		/// <param name="RightThrow">Right input. Default is X axis.</param>
		public override void MovementInput(float ForwardThrow, float RightThrow)
		{
			if (HasStoppedReceivingMovementInput())
				return;

			Velocity = new MVector(RightThrow, 0, ForwardThrow).Normalise();
			Velocity = MovementSpeed * Velocity;
		}

		/// <summary>Adds force upwards to this player. Default direction is <see cref="MVector.Up"/>.</summary>
		/// <docs>Adds force upwards to this player. Default direction is MVector.Up.</docs>
		/// <decorations decor="public virtual void"></decorations>
		/// <remarks>Uses Rigidbody.AddForce(Vector3) to enforce jumping, by default.</remarks>
		/// <param name="Force">The amount of force to apply.</param>
		public virtual void Jump(float Force)
		{
			Rigidbody.AddForce(Force * MVector.Up);
		}

		/// <summary>The <see cref="Rigidbody.velocity"/> of this Player, respective to <see cref="Rigidbody"/>.</summary>
		/// <docs>The velocity of this Player, respective to attached Rigidbody.</docs>
		/// <decorations decor="public float"></decorations>
		/// <returns>The speed in Unity-units per second.</returns>
		public float GetSpeed()
		{
			return Rigidbody.velocity.magnitude;
		}

		/// <summary>Gets the speed and direction of this Player.</summary>
		/// <decorations decor="public void"></decorations>
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
