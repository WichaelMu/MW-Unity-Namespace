using System;
using System.Collections;
using UnityEngine;
using MW.Vector;

namespace MW.Behaviour
{
	public class Player : MonoBehaviour
	{
		/// <summary>Called when damage is taken. First float is new <see cref="Health"/>, second is the inflicting damage.</summary>
		public Action<float, float> OnTakeDamage;

		/// <summary>The world position of this player.</summary>
		public MVector Position { get => transform.position; set { transform.position = value; } }

		[Header("Player Settings")]

		[SerializeField] float Health;
		internal float InitialHealth;

		[Tooltip("The normal movement speed for this player.")]
		public float MovementSpeed = 1;
		protected float InitialMovementSpeed;
		protected MVector Velocity;
		bool bStopReceivingMovementInput = false;

		public virtual void Awake()
		{
			InitialisePlayer();
		}

		/// <summary>Initialises this player's settings.</summary>
		public void InitialisePlayer()
		{
			InitialMovementSpeed = MovementSpeed;
			InitialHealth = Health;
		}

		#region Movement Speed

		/// <summary>Sets MovementSpeed to NewMovementSpeed.</summary>
		/// <remarks>Also updates the default, <see cref="InitialMovementSpeed"/>. <see cref="TemporaryMovementSpeed"/> will revert to NewMovementSpeed.</remarks>
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

		/// <summary>Set this player's <see cref="MovementSpeed"/> to <see cref="InitialMovementSpeed"/>.</summary>
		public void ResetMovementSpeed()
		{
			MovementSpeed = InitialMovementSpeed;
		}

		#endregion

		#region Movement

		/// <summary>Registers movement from input.</summary>
		/// <remarks>Needs to be overidden from <see cref="Player"/>.</remarks>
		/// <param name="ForwardThrow">Horizontal input.</param>
		/// <param name="RightThrow">Vertical input.</param>
		/// <exception cref="NotImplementedException"></exception>
		public virtual void MovementInput(float ForwardThrow, float RightThrow)
		{
			throw new NotImplementedException(nameof(MovementInput) + " must be overridden!");
		}

		/// <summary>Set this Player's ability to receive <see cref="MovementInput(float, float)"/>.</summary>
		/// <param name="bStopReceivingInput">True if this Player should stop receiving input.</param>
		public void ReceiveMovementInput(bool bStopReceivingInput)
		{
			bStopReceivingMovementInput = bStopReceivingInput;
		}

		/// <summary>Whether or not this Player is allowed to receive <see cref="MovementInput(float, float)"/>.</summary>
		/// <returns>True if this Player is not allowed to receive <see cref="MovementInput(float, float)"/>.</returns>
		public bool HasStoppedReceivingMovementInput()
		{
			return bStopReceivingMovementInput;
		}

		#endregion

		/// <summary>Get this Player's Health.</summary>
		/// <returns>Current health.</returns>
		public float GetHealth()
		{
			return Health;
		}

		/// <summary>Deduct InDamage from this Player's Health.</summary>
		/// <param name="InDamage">The damage to inflict on this Player.</param>
		/// <returns>True if this Player <see cref="IsDead"/>.</returns>
		public bool TakeDamage(float InDamage)
		{
			Health -= InDamage;

			OnTakeDamage?.Invoke(GetHealth(), InDamage);

			return IsDead();
		}

		/// <inheritdoc cref="TakeDamage(float)"/> <param name="InDamage"></param>
		/// <param name="HealthPercentageRemaining">The percentage of health remaining after taking InDamage.</param>
		public bool TakeDamage(float InDamage, out float HealthPercentageRemaining)
		{
			bool bIsDead = TakeDamage(InDamage);
			HealthPercentageRemaining = GetHealth() / InitialHealth;

			return bIsDead;
		}

		/// <summary>If this Player's is considered dead.</summary>
		/// <returns>True if <see cref="GetHealth"/> &lt;= 0.</returns>
		public bool IsDead()
		{
			return GetHealth() <= 0;
		}

		public virtual void OnDestroy()
		{
			OnTakeDamage = null;
		}
	}
}
