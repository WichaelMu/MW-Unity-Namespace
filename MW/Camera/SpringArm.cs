#if RELEASE
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using MW.Extensions;
using MW.Diagnostics;

namespace MW.CameraUtils
{
	/// <summary>A GameObject Orbit Utility class.</summary>
	[ExecuteAlways]
	public class MSpringArm : MonoBehaviour
	{
		/// <summary>True to show Debug Lines.</summary>
		[Header("Debug")]
		[SerializeField, Tooltip("True to show Debug Lines.")] bool bShowArmLine;

		/// <summary>The Transform this SpringArm will track.</summary>
		[Header("SpringArm Targets")]
		[Tooltip("The Transform this Spring Arm will track.")] public Transform Target;
		/// <summary>The Rotation of this Spring Arm's orbit around Target.</summary>
		[SerializeField, Space(10), Tooltip("The Rotation of this Spring Arm's orbit around Target.")]
		MRotator ArmRotation = new MRotator(-25f, 0, 0);
		/// <summary>The positional Offset this Spring Arm will take.</summary>
		[SerializeField, Tooltip("The positional Offset this Spring Arm will take.")] MVector Offset;

		/// <summary>The Angle Offset to apply when this Spring Arm Orients towards Target.</summary>
		[Header("Orientation")]
		[SerializeField, Tooltip("The Angle Offset to apply when this Spring Arm Orients towards Target.")]
		MRotator OrientationAngleOffset = new MRotator(15, 0, 0);
		/// <summary>True if this Spring Arm should Orient towards Target.</summary>
		[SerializeField, Tooltip("True if this Spring Arm should Orient towards Target.")] bool bShouldOrientTowardsTarget;

		/// <summary>The Distance this Spring Arm will try to keep from Target.</summary>
		[Space(10)]
		[Tooltip("The Distance this Spring Arm will try to keep from Target.")] public float Distance;

		[Header("Collision Settings")]
		[SerializeField, Tooltip("Definition of settings when a Collision occurs between the Spring Arm and Target.")]
		MSpringArmCollision CollisionSettings;

		/// <summary>The Strength of Lag to be applied when changing positions.</summary>
		[Header("Lag Settings")]
		[SerializeField, Tooltip("The Strength of Lag to be applied when changing positions.")] float PositionalLagStrength = .2f;
		MRotator PreviousRotation;
		MRotator RefVelocities;

		/// <summary>Sets the Rotation and Distance with Collision Checks, and then Orients to Target.</summary>
		/// <decorations decor="protected void"></decorations>
		protected void Update()
		{
			GetRotationAndDistance(out MRotator SpringArmRotation, out float SpringArmDistance);
			SetRotationAndDistance(SpringArmRotation, SpringArmDistance);
			OrientToTarget();
		}

		/// <summary>Sets the Rotation and Distance of this Spring Arm around and from Target, respectively.</summary>
		/// <decorations decor="protected void"></decorations>
		/// <param name="SpringArmRotation">The Rotation of the Spring Arm should take.</param>
		/// <param name="SpringArmDistance">The Distance the Spring Arm should try to maintain from Target.</param>
		protected void SetRotationAndDistance(MRotator SpringArmRotation, float SpringArmDistance)
		{
			if (!Application.isPlaying)
			{
				transform.position = GetRotationEndPosition(SpringArmDistance, SpringArmRotation);
				return;
			}

			MRotator InterpRotation = ArmRotation;

			if (!SpringArmRotation.IsZero(.015f))
			{
				SpringArmRotation.Pitch = FMath.ClosestMultiple(SpringArmRotation.Pitch, .2f);
				SpringArmRotation.Yaw = FMath.ClosestMultiple(SpringArmRotation.Yaw, .2f);
				SpringArmRotation.Roll = FMath.ClosestMultiple(SpringArmRotation.Roll, .2f);

				FMath.IfZeroThenZero(ref SpringArmRotation.Pitch);
				FMath.IfZeroThenZero(ref SpringArmRotation.Yaw);
				FMath.IfZeroThenZero(ref SpringArmRotation.Roll);

				InterpRotation.Pitch = FMath.SmoothDampAngle(PreviousRotation.Pitch, SpringArmRotation.Pitch, ref RefVelocities.Pitch, PositionalLagStrength, Time.deltaTime);
				InterpRotation.Yaw = FMath.SmoothDampAngle(PreviousRotation.Yaw, SpringArmRotation.Yaw, ref RefVelocities.Yaw, PositionalLagStrength, Time.deltaTime);
				InterpRotation.Roll = FMath.SmoothDampAngle(PreviousRotation.Roll, SpringArmRotation.Roll, ref RefVelocities.Roll, PositionalLagStrength, Time.deltaTime);

				FMath.IfZeroThenZero(ref InterpRotation.Pitch);
				FMath.IfZeroThenZero(ref InterpRotation.Yaw);
				FMath.IfZeroThenZero(ref InterpRotation.Roll);
			}

			transform.position = GetRotationEndPosition(SpringArmDistance, InterpRotation);
			PreviousRotation = InterpRotation;
		}

		/// <summary>Rotates the Spring Arm by Pitch and Yaw.</summary>
		/// <decorations decor="public void"></decorations>
		/// <param name="Pitch"></param>
		/// <param name="Yaw"></param>
		/// <param name="Roll"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetDeltaRotation(float Pitch, float Yaw, float Roll = 0F)
		{
			ArmRotation.Pitch += Pitch;
			ArmRotation.Yaw += Yaw;
			ArmRotation.Roll += Roll;
		}

		void OrientToTarget()
		{
			if (!bShouldOrientTowardsTarget)
				return;

			Vector3 LookDirection = (Target.position - transform.position).FNormalise();
			MRotator LookRotation = LookDirection.MV().Rotation();
			transform.rotation = LookRotation + OrientationAngleOffset;

			if (bShowArmLine)
				Arrow.DebugArrow(transform.position, LookRotation.AsOrientationVector().V3() * .1f, Color.grey, .025f);
		}

		void GetRotationAndDistance(out MRotator SpringArmRotation, out float SpringArmDistance)
		{
			SpringArmRotation = ArmRotation;
			SpringArmDistance = Distance;

			CheckCollisions(ref SpringArmDistance, ref SpringArmRotation);

			if (bShowArmLine)
			{
				Arrow.DebugArrow(GetTargetPosition(), FMath.Clamp(Distance * .9f, .1f, 1f) * GetArmDirection(SpringArmRotation), Color.green, .1f);
				Debug.DrawRay(GetTargetPosition(), GetArmDirection(SpringArmRotation) * Distance, Color.black);
				Debug.DrawLine(GetTargetPosition(), GetRotationEndPosition(SpringArmDistance, SpringArmRotation), Color.red);
			}
		}

		bool CheckCollisions(ref float FinalDistance, ref MRotator FinalRotation)
		{
			if (CollisionSettings.CollisionType == 0)
				return false;

			Vector3 TargetPosition = GetTargetPosition();

			bool bHit = Physics.Raycast(TargetPosition, GetArmDirection(), out RaycastHit HitInformation, Distance, CollisionSettings.OnlyCollideWith);

			if (bHit)
			{
				FinalDistance = HitInformation.distance - CollisionSettings.CollisionDistanceGap;
			}
			else
			{
				return false;
			}

			if (CollisionSettings.CollisionType == ECollisionCheckType.SingleRaycast)
				return bHit;

			// If the Ray hit something, but not far enough to activate Advanced Collisions.
			if (FinalDistance >= CollisionSettings.AdvancedCollisionActivationDistance)
				return bHit;

			// Advanced Collision...
			//

			RaycastHit Priority = HitInformation;
			MRotator AdvancedRotation = ArmRotation;
			AdvancedRotation.Roll = -(CollisionSettings.AdvancedColisionSearchAngle * .5f);

			for (float A = 0f; A <= CollisionSettings.AdvancedColisionSearchAngle; A += CollisionSettings.AdvancedCollisionAngleDelta)
			{
				AdvancedRotation.Roll += CollisionSettings.AdvancedCollisionAngleDelta;
				Ray R = new Ray(TargetPosition, GetArmDirection(AdvancedRotation));
				if (Physics.Raycast(R, out RaycastHit AdvancedInformation, Distance, CollisionSettings.OnlyCollideWith))
				{
					if (DetermineRotationPriority(Priority, AdvancedInformation))
					{
						FinalDistance = AdvancedInformation.distance;
						FinalRotation = AdvancedRotation;

						Priority = AdvancedInformation;
					}

					if (CollisionSettings.bShowAdvancedDebug)
						Debug.DrawRay(R.origin, R.direction * AdvancedInformation.distance, Color.cyan);
				}
			}

			return true;
		}

		/// <summary>Determines which Point this Spring Arm should assume when doing MultiRaycast.</summary>
		/// <remarks>By default, the two Points: Previous and Comparison, are based on altitude (Y-Axis).</remarks>
		/// <decorations decor="protected virtual bool"></decorations>
		/// <param name="Previous"></param>
		/// <param name="Comparison"></param>
		/// <returns>True if Comparison takes precedence over Previous.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual bool DetermineRotationPriority(RaycastHit Previous, RaycastHit Comparison)
			=> Previous.point.y < Comparison.point.y;

		/// <summary>Gets the Target Position.</summary>
		/// <decorations decor="public Vector3"></decorations>
		/// <param name="bWithOffsets">True to consider Offset.</param>
		/// <returns>Target's Position, considering Offset.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3 GetTargetPosition(bool bWithOffsets = true)
			=> bWithOffsets ? Target.position + Offset : Target.position;

		/// <summary>Computes the world-space position of Rotation AtDistance.</summary>
		/// <decorations decor="protected virtual Vector3"></decorations>
		/// <param name="AtDistance">The distance away from Target.</param>
		/// <param name="Rotation">The rotation to calculate.</param>
		/// <returns>The Position from Target relative to a Rotation orbit AtDistance.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual Vector3 GetRotationEndPosition(float AtDistance, MRotator Rotation)
			=> (AtDistance * Rotation.Relative(-Target.forward, Target.up).AsOrientationVector()) + GetTargetPosition();

		/// <summary>Computes the position of this Spring Arm's ArmRotation AtDistance.</summary>
		/// <decorations decor="protected virtual Vector3"></decorations>
		/// <param name="AtDistance">The distance away from Target when rotated by ArmRotation.</param>
		/// <returns>The position of ArmRotation around Target AtDistance .</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual Vector3 GetArmEndPosition(float AtDistance)
			=> GetRotationEndPosition(AtDistance, ArmRotation);

		/// <summary>Computes the position of a given Rotation at this Spring Arm's desired Distance.</summary>
		/// <decorations decor="protected virtual Vector3"></decorations>
		/// <param name="Rotation">The rotation to calculate.</param>
		/// <returns>The position of Rotation around Target at Distance.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual Vector3 GetRotationEndPosition(MRotator Rotation)
			=> GetRotationEndPosition(Distance, Rotation);

		/// <summary>An orientation vector of ArmRotation.</summary>
		/// <decorations decor="protected Vector3"></decorations>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected Vector3 GetArmDirection()
			=> GetRelativeRotation(ArmRotation).AsOrientationVector();

		/// <summary>An orientation vector of a given Rotation.</summary>
		/// <decorations decor="protected Vector3"></decorations>
		/// <param name="Rotation"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected Vector3 GetArmDirection(MRotator Rotation)
			=> GetRelativeRotation(Rotation).AsOrientationVector();

		/// <summary>Converts Rotation into a world-space rotation, relative to Target.</summary>
		/// <decorations decor="protected MRotator"></decorations>
		/// <param name="Rotation">The rotation to convert.</param>
		/// <returns>A Rotation relative to Target.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected MRotator GetRelativeRotation(MRotator Rotation)
			=> Rotation.Relative(-Target.forward, Target.up);
	}

	/// <summary>Spring Arm Collision Settings.</summary>
	[Serializable]
	public struct MSpringArmCollision
	{
		/// <summary>The type of Collision to check.</summary>
		public ECollisionCheckType CollisionType;

		/// <summary>The Layer that collisions.</summary>
		public LayerMask OnlyCollideWith;
		/// <summary>The buffer distance between the Spring Arm and the point of Collision.</summary>
		public float CollisionDistanceGap;
		/// <summary>The distance required to activate Advanced Collision Logic.</summary>
		[Min(0f)] public float AdvancedCollisionActivationDistance;
		/// <summary>The step angle at which Advanced Collision Logic should search.</summary>
		[Min(.01f)] public float AdvancedCollisionAngleDelta;
		/// <summary>The angle at which the Advanced Collision Logic should be spread.</summary>
		[Range(0f, 360f)] public float AdvancedColisionSearchAngle;

		/// <summary>True to show Advanced Collision Logic Debug Lines.</summary>
		[Header("Advanced Collision Debug")]
		public bool bShowAdvancedDebug;
	}

	/// <summary>Spring Arm Collision Types.</summary>
	public enum ECollisionCheckType : byte
	{
		/// <summary>Ignore Collisions.</summary>
		NoCollision = 0b0,
		/// <summary>Individual Collision Check Type.</summary>
		SingleRaycast = 0b1,
		/// <summary>Advanced Collision Check Type.</summary>
		MultiRaycast = 0b10
	}
}
#endif // RELEASE