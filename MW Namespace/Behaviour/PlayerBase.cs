using MW.Kinetic;
using MW.Math;
using System;
using System.Collections;
using UnityEngine;

namespace MW.Behaviour
{
	/// <summary>The base class for <see cref="MPlayer"/> and <see cref="MPlayer2D"/>.</summary>
	/// <docs>The base class for MPlayer and MPlayer2D.</docs>
	/// <decorations decor="public class : MonoBehaviour"></decorations>
	public class PlayerBase : MBehaviour
	{
		/// <summary>Called when damage is taken. First float is new Health, second is the inflicting damage.</summary>
		/// <decorations decor="public Action{float, float}"></decorations>
		public Action<float, float> OnTakeDamage;

		[Header("Player Settings")]

		[SerializeField] float Health;
		internal float InitialHealth;

		/// <summary>The movement speed of this Player.</summary>
		/// <decorations decor="public float"></decorations>
		[Tooltip("The normal movement speed for this player.")]
		public float MovementSpeed = 1;
		protected float InitialMovementSpeed;
		protected MVector Velocity;
		bool bStopReceivingMovementInput = false;

		internal IntervalInformation IntervalRecorder;
		internal IntervalInformation FixedIntervalRecorder;
		[HideInInspector] public float TimeSpawned;

		public override void Awake()
		{
			base.Awake();

			InitialisePlayer();
		}

		public virtual void FixedUpdate()
		{
			FixedIntervalRecorder.Record(this);
		}

		/// <summary>Initialises this player's settings.</summary>
		/// <decorations decor="public void"></decorations>
		public void InitialisePlayer()
		{
			IntervalRecorder = new IntervalInformation();
			FixedIntervalRecorder = new IntervalInformation();

			InitialMovementSpeed = MovementSpeed;
			InitialHealth = Health;

			TimeSpawned = Time.time;
		}

		#region Movement Speed

		/// <summary>Sets MovementSpeed to NewMovementSpeed.</summary>
		/// <remarks>Also updates the default, InitialMovementSpeed. TemporaryMovementSpeed will revert to NewMovementSpeed.</remarks>
		/// <decorations decor="public void"></decorations>
		/// <param name="NewMovementSpeed">The new Movement Speed of this player.</param>
		public void SetMovementSpeed(float NewMovementSpeed)
		{
			MovementSpeed = NewMovementSpeed;
			InitialMovementSpeed = NewMovementSpeed;
		}

		/// <summary>Temporarily modifies this player's MovementSpeed.</summary>
		/// <remarks>Calling StopCoroutine on the returned IEnumerator will not reset the player's MovementSpeed.</remarks>
		/// <decorations decor="public IEnumerator"></decorations>
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

		/// <summary>Set this player's MovementSpeed to InitialMovementSpeed.</summary>
		/// <decorations decor="public void"></decorations>
		public void ResetMovementSpeed()
		{
			MovementSpeed = InitialMovementSpeed;
		}

		#endregion

		#region Movement

		/// <summary>Registers movement from input.</summary>
		/// <remarks>Needs to be overridden from <see cref="PlayerBase"/>.</remarks>
		/// <decorations decor="public virtual void"></decorations>
		/// <param name="ForwardThrow">Horizontal input.</param>
		/// <param name="RightThrow">Vertical input.</param>
		/// <exception cref="NotImplementedException"></exception>
		public virtual void MovementInput(float ForwardThrow, float RightThrow)
		{
			throw new NotImplementedException(nameof(MovementInput) + " must be overridden!");
		}

		/// <summary>Set this Player's ability to receive <see cref="MovementInput(float, float)"/>.</summary>
		/// <docs>Set this Player's ability to receive MovementInput(float, float).</docs>
		/// <decorations decor="public void"></decorations>
		/// <param name="bStopReceivingInput">True if this Player should stop receiving input.</param>
		public void ReceiveMovementInput(bool bStopReceivingInput)
		{
			bStopReceivingMovementInput = bStopReceivingInput;
		}

		/// <summary>Whether or not this Player is allowed to receive <see cref="MovementInput(float, float)"/>.</summary>
		/// <docs>Whether or not this Player is allowed to receive MovementInput(float, float).</docs>
		/// <decorations decor="public bool"></decorations>
		/// <returns>True if this Player is not allowed to receive MovementInput(float, float).</returns>
		public bool HasStoppedReceivingMovementInput()
		{
			return bStopReceivingMovementInput;
		}

		/// <summary>Records this Player on this frame.</summary>
		/// <decorations decor="public void"></decorations>
		public void RecordInterval()
		{
			IntervalRecorder.Record(this);
		}

		/// <summary>Gets the velocity of the Player, relative to the <see cref="LastIntervalInformation"/> interval.</summary>
		/// <docs>Gets the velocity of the Player, relative to the previous interval.</docs>
		/// <decorations decor="public MVector"></decorations>
		/// <returns>The velocity of this Player, relative to the previous interval.</returns>
		public MVector GetVelocity()
		{
			IntervalRecorder.Mark(this, out LastIntervalInformation Last, out ThisIntervalInformation This);

			return This.DeltaPosition(Last);
		}

		/// <summary>The rate of acceleration of this Player between <see cref="Time.fixedDeltaTime"/> intervals.</summary>
		/// <docs>The rate of acceleration of this Player between FixedUpdate intervals.</docs>
		/// <decorations decor="public float"></decorations>
		/// <returns>The rate of acceleration in metres per second, as per Time.fixedDeltaTime.</returns>
		public float GetAccelerationRate()
		{
			FixedIntervalRecorder.Mark(this, out LastIntervalInformation Last, out ThisIntervalInformation This);

			return Mathematics.AccelerationRate(Last.Position, This.Position, Time.fixedDeltaTime);
		}

		/// <summary>The G Force experienced by this Player between <see cref="FixedUpdate"/> intervals.</summary>
		/// <docs>The G Force experienced by this Player between FixedUpdate intervals.</docs>
		/// <decorations decor="public MVector"></decorations>
		/// <param name="Gravity">The force of gravity this Player experiences at a standstill.</param>
		/// <returns>The G Force experience by this Player as an MVector.</returns>
		public MVector V_ComputeGForce(MVector Gravity)
		{
			FixedIntervalRecorder.Mark(this, out LastIntervalInformation Last, out ThisIntervalInformation This);

			return Kinematics.V_GForce(Last.Position, This.Position, Time.fixedDeltaTime, Gravity);
		}

		/// <summary>The G Force experienced by this Player between <see cref="FixedUpdate"/> intervals.</summary>
		/// <docs>The G Force experienced by this Player between FixedUpdate intervals.</docs>
		/// <decorations decor="public float"></decorations>
		/// <param name="Gravity">The force of gravity this Player experiences at a standstill.</param>
		/// <returns>The G Force experienced by this player.</returns>
		public float F_ComputeGForce(MVector Gravity)
		{
			return V_ComputeGForce(Gravity).Magnitude;
		}

		#endregion

		#region Health And Death

		/// <summary>Get this Player's <see cref="Health"/>.</summary>
		/// <docs>Get this Player's Health.</docs>
		/// <decorations decor="public float"></decorations>
		/// <docreturns>Current health.</docreturns>
		/// <returns>Current <see cref="Health"/>.</returns>
		public float GetHealth()
		{
			return Health;
		}

		/// <summary>Deduct InDamage from this Player's <see cref="Health"/>.</summary>
		/// <docs>Deduct InDamage from this Player's Health.</docs>
		/// <decorations decor="public bool"></decorations>
		/// <param name="InDamage">The damage to inflict on this Player.</param>
		/// <docreturns>True if this Player IsDead.</docreturns>
		/// <returns>True if this Player <see cref="IsDead"/>.</returns>
		public bool TakeDamage(float InDamage)
		{
			Health -= InDamage;

			OnTakeDamage?.Invoke(GetHealth(), InDamage);

			return IsDead();
		}

		/// <summary>Deduct InDamage from this Player's <see cref="Health"/>.</summary>
		/// <docs>Deduct InDamage from this Player's Health.</docs>
		/// <decorations decor="public bool"></decorations>
		/// <param name="InDamage">The damage to inflict on this Player.</param>
		/// <param name="HealthPercentageRemaining">The percentage of health remaining after taking InDamage.</param>
		/// <docreturns>True if this Player IsDead.</docreturns>
		/// <returns>True if this Player <see cref="IsDead"/>.</returns>
		public bool TakeDamage(float InDamage, out float HealthPercentageRemaining)
		{
			bool bIsDead = TakeDamage(InDamage);
			HealthPercentageRemaining = GetHealth() / InitialHealth;

			return bIsDead;
		}

		/// <summary>If this Player's is considered dead.</summary>
		/// <decorations decor="public bool"></decorations>
		/// <docreturns>True if GetHealth &lt;= 0.</docreturns>
		/// <returns>True if <see cref="GetHealth"/> &lt;= 0.</returns>
		public bool IsDead()
		{
			return GetHealth() <= 0;
		}

		/// <summary>What should happen when this Player is removed from the game?</summary>
		/// <decorations decor="public virtual void"></decorations>
		public virtual void OnDestroy()
		{
			OnTakeDamage = null;
		}

		#endregion
	}
}
