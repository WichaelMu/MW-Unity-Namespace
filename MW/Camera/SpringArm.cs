#if RELEASE
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using MW.Diagnostics;

namespace MW.CameraUtils
{

	[ExecuteAlways]
	public class MSpringArm : MonoBehaviour
	{
		[Header("Debug")]
		[SerializeField] bool bShowArmLine;

		[Header("SpringArm Targets")]
		public Transform Target;
		[SerializeField, Space(10)] MRotator ArmRotation;
		[SerializeField] MVector Offset;

		[Space(10)]
		public float Distance;

		[Header("Collision Settings")]
		[SerializeField] MSpringArmCollision CollisionSettings;

		[Header("Lag Settings")]
		[SerializeField] float PositionalLagStrength = .2f;
		MRotator PreviousRotation;
		MRotator RefVelocities;

		protected void Update()
		{
			GetRotationAndDistance(out MRotator SpringArmRotation, out float SpringArmDistance);
			SetRotationAndDistance(SpringArmRotation, SpringArmDistance);
		}

		protected void SetRotationAndDistance(MRotator SpringArmRotation, float SpringArmDistance)
		{
			if (!Application.isPlaying)
			{
				transform.position = GetRotationEndPosition(SpringArmDistance, SpringArmRotation);
				return;
			}

			MRotator InterpRotation;
			InterpRotation.Pitch = FMath.SmoothDampAngle(PreviousRotation.Pitch, SpringArmRotation.Pitch, ref RefVelocities.Pitch, PositionalLagStrength, Time.deltaTime);
			InterpRotation.Yaw = FMath.SmoothDampAngle(PreviousRotation.Yaw, SpringArmRotation.Yaw, ref RefVelocities.Yaw, PositionalLagStrength, Time.deltaTime);
			InterpRotation.Roll = FMath.SmoothDampAngle(PreviousRotation.Roll, SpringArmRotation.Roll, ref RefVelocities.Roll, PositionalLagStrength, Time.deltaTime);

			transform.position = GetRotationEndPosition(SpringArmDistance, InterpRotation);
			PreviousRotation = InterpRotation;
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

		protected virtual bool DetermineRotationPriority(RaycastHit Previous, RaycastHit Comparison)
		{
			return Previous.point.y < Comparison.point.y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3 GetTargetPosition(bool bWithOffsets = true)
			=> bWithOffsets ? Target.position + Offset : Target.position;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual Vector3 GetRotationEndPosition(float AtDistance, MRotator Rotation)
			=> (AtDistance * Rotation.Relative(-Target.forward, Target.up).AsOrientationVector()) + GetTargetPosition();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual Vector3 GetArmEndPosition(float AtDistance)
			=> GetRotationEndPosition(AtDistance, ArmRotation);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual Vector3 GetRotationEndPosition(MRotator Rotation)
			=> GetRotationEndPosition(Distance, Rotation);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		Vector3 GetArmDirection()
			=> GetRelativeRotation(ArmRotation).AsOrientationVector();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		Vector3 GetArmDirection(MRotator Rotation)
			=> GetRelativeRotation(Rotation).AsOrientationVector();

		MRotator GetRelativeRotation(MRotator Rotation)
			=> Rotation.Relative(-Target.forward, Target.up);
	}

	[Serializable]
	public struct MSpringArmCollision
	{
		public ECollisionCheckType CollisionType;

		public LayerMask OnlyCollideWith;
		public float CollisionDistanceGap;
		[Min(0f)] public float AdvancedCollisionActivationDistance;
		[Min(.01f)] public float AdvancedCollisionAngleDelta;
		[Range(0f, 360f)] public float AdvancedColisionSearchAngle;

		public bool bShowAdvancedDebug;
	}

	public enum ECollisionCheckType : byte
	{
		NoCollision = 0b0,
		SingleRaycast = 0b1,
		MultiRaycast = 0b10
	}

}
#endif // RELEASE