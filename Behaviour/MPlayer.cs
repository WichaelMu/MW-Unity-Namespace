using System.Collections;
using UnityEngine;
using MW.Vector;

namespace MW.Behaviour
{
	[RequireComponent(typeof(Rigidbody))]
	public class MPlayer : MonoBehaviour
	{
		/// <summary>The world position of this player.</summary>
		public MVector Position { get => transform.position; set { transform.position = value; } }

		[Tooltip("The normal movement speed for this player.")]
		public float MovementSpeed = 1;
		float InitialMovementSpeed;

		Rigidbody Rigidbody;

		public virtual void Awake()
		{
			Rigidbody = GetComponent<Rigidbody>();

			InitialMovementSpeed = MovementSpeed;
		}

		#region Player Movement

		/// <summary>Moves this player forward. Default direction is transform.forward.</summary>
		/// <remarks>Uses UnityEngine.Rigibody.MovePosition to enforce movement.</remarks>
		/// <param name="Throw">Input vector.</param>
		public virtual void ForwardInput(float Throw)
		{
			Rigidbody.MovePosition(Position + transform.forward * Throw * MovementSpeed * Time.deltaTime);
		}

		/// <summary>Moves this player right. Default direction is transform.right.</summary>
		/// <remarks>Uses UnityEngine.Rigibody.MovePosition to enforce movement.</remarks>
		/// <param name="Throw">Input vector.</param>
		public virtual void RightInput(float Throw)
		{
			Rigidbody.MovePosition(Position + transform.right * Throw * MovementSpeed * Time.deltaTime);
		}

		/// <summary>Adds force upwards to this player. Default direction is MVector.Up.</summary>
		/// <remarks>Uses UnityEngine.Rigibody.AddForce.</remarks>
		/// <param name="Force">The amount of force to apply.</param>
		public virtual void Jump(float Force)
		{
			Rigidbody.AddForce(MVector.Up * Force);
		}

		/// <summary>Sets MovementSpeed to NewMovementSpeed.</summary>
		/// <remarks>Also updates the default, initial movement speed. TemporaryMovementSpeed(float, float) will revert to NewMovementSpeed.</remarks>
		/// <param name="NewMovementSpeed">The new Movement Speed of this player.</param>
		public void SetMovementSpeed(float NewMovementSpeed)
		{
			MovementSpeed = NewMovementSpeed;
			InitialMovementSpeed = NewMovementSpeed;
		}

		/// <summary>Temporarily modifies this player's MovementSpeed.</summary>
		/// <remarks>Calling StopCoroutine on the returned IEnumerator will not reset the player's MovementSpeed.</remarks>
		/// <param name="TemporaryMovementSpeed">The temporary MovementSpeed.</param>
		/// <param name="Duration">The time in seconds in which TemporaryMovementSpeed will be in effect.</param>
		/// <returns>The IEnumerator that handles timing.</returns>
		public IEnumerator TemporaryMovementSpeed(float TemporaryMovementSpeed, float Duration)
		{
			IEnumerator Temporary = Internal_TemporaryMovementSpeed(TemporaryMovementSpeed, Duration);

			return Temporary;
		}

		IEnumerator Internal_TemporaryMovementSpeed(float TemporaryMovementSpeed, float Duration)
		{
			MovementSpeed = TemporaryMovementSpeed;

			yield return new WaitForSeconds(Duration);

			MovementSpeed = InitialMovementSpeed;
		}

		/// <summary>Set this player's MovementSpeed to the initial.</summary>
		public void ResetMovementSpeed()
		{
			MovementSpeed = InitialMovementSpeed;
		}

		#endregion


	}
}
