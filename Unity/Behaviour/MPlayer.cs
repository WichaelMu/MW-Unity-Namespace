using System.Collections;
using UnityEngine;
using MW.Vector;

namespace MW.Behaviour
{
	[RequireComponent(typeof(Rigidbody))]
	public class MPlayer : MonoBehaviour
	{
		public MVector Position { get => transform.position; set { transform.position = value; } }

		float MovementSpeed = 1;
		float InitialMovementSpeed;

		Rigidbody Rigidbody;

		public virtual void Awake()
		{
			Rigidbody = GetComponent<Rigidbody>();

			InitialMovementSpeed = MovementSpeed;
		}

		#region Player Movement

		/// <summary>Moves this player forward. Default direction is transform.forward.</summary>
		/// <param name="Throw">Input vector.</param>
		public virtual void ForwardInput(float Throw)
		{
			Rigidbody.MovePosition(Position + transform.forward * Throw * MovementSpeed * Time.deltaTime);
		}

		/// <summary>Moves this player right. Default direction is transform.right.</summary>
		/// <param name="Throw">Input vector.</param>
		public virtual void RightInput(float Throw)
		{
			Rigidbody.MovePosition(Position + transform.right * Throw * MovementSpeed * Time.deltaTime);
		}

		/// <summary>Adds force upwards to this player. Default direction is MVector.Up.</summary>
		/// <param name="Force">The amount of force to apply.</param>
		public virtual void Jump(float Force)
		{
			Rigidbody.AddForce(MVector.Up * Force);
		}

		public void SetMovementSpeed(float NewMovementSpeed)
		{
			MovementSpeed = NewMovementSpeed;
		}

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

		public void ResetMovementSpeed()
		{
			MovementSpeed = InitialMovementSpeed;
		}

		#endregion


	}
}
