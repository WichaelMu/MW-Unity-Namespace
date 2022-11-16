using System;
using System.Collections.Generic;
using System.Linq;
using MW.CameraUtils;
using UnityEngine;
using static MW.Utils;

namespace MW.Behaviour
{
	/// <summary>Forward and Backward Reaching Inverse Kinematics.</summary>
	/// <remarks>
	/// This class is marked <see langword="partial"/> and can have its functionality extended.
	/// <br></br><br></br>
	/// Alternatively, this class can have its functionality extended.
	/// </remarks>
	/// <docs>
	/// Forward and Backward Reaching Inverse Kinematics.
	/// &lt;br&gt;&lt;br&gt;
	/// This class is marked 'partial' and can have its functionality extended.
	/// &lt;br&gt;&lt;br&gt;
	/// Alternatively, this class can have its functionality extended.
	/// </docs>
	/// <decorations decor="public partial class : MonoBehaviour"></decorations>
	public partial class FABRIK : MonoBehaviour
	{
		/// <summary><see langword="true"/> to show <see cref="FABRIKLeg"/> connections.</summary>
		/// <docs>Should FABRIKLeg connections shown?</docs>
		/// <decorations decor="[SerializeField] protected bool"></decorations>
		[Header("Debug Settings. [EDITOR ONLY]")]
		[SerializeField, Tooltip("Show " + nameof(FABRIKLeg) + " connections?")] protected bool bShowJoints = true;
		/// <summary>How large should <see cref="FABRIKLeg"/> Joints be displayed?</summary>
		/// <docs>How large should FABRIKLeg Joints be displayed?</docs>
		/// <decorations decor="[SerializeField] protected float"></decorations>
		[SerializeField, Tooltip("How large should " + nameof(FABRIKLeg) + " Joints be displayed?")] protected float JointGizmosRadius = .1f;

		/// <summary><see langword="true"/> to show where the FABRIK biases are.</summary>
		/// <docs>Should the FABRIK bias points be shown?</docs>
		/// <decorations decor="[SerializeField] protected bool"></decorations>
		[SerializeField, Tooltip("Show where the FABRIK biases are?")] protected bool bShowBiasPoint = true;
		/// <summary>How large should the Bias Point be displayed?</summary>
		[SerializeField, Tooltip("How large should the Bias Point be displayed?")] protected float BiasGizmosRadius = .1f;

		/// <summary><see langword="true"/> to show rays for determining the FABRIK Target.</summary>
		/// <docs>Should the Rays that determine the FABRIK Target be shown?</docs>
		/// <decorations decor="[SerializeField] protected bool"></decorations>
		[SerializeField, Tooltip("Show rays for determining the ground?")] protected bool bShowRays = true;

		bool bHasBeenShownWarning;

		/// <summary>The maximum number of times FABRIK should run.</summary>
		/// <remarks>
		/// Higher increases accuracy, but is slower to compute.
		/// <br></br>
		/// Lower decreases accuracy, but is faster to compute.
		/// </remarks>
		/// <decorations decor="[SerializeField] protected int"></decorations>
		[Space(10)]

		[Header("FABRIK Settings.")]
		[SerializeField, Tooltip("The maximum number of times FABRIK should run. Higher = Increases accuracy & Slower. Lower = Decreases accuracy & faster.")] protected int MaximumPasses = 100;
		/// <summary>How close should the FABRIK Leg be to be considered reached?</summary>
		/// <remarks>This controls the accuracy for all <see cref="Legs"/>.</remarks>
		/// <docremarks>This controls the accuracy for all Legs.</docremarks>
		/// <decorations decor="[SerializeField] protected float"></decorations>
		[SerializeField, Tooltip("How close should the FABRIK Leg be to considered reached?")] protected float Tolerance = .01f;
		/// <summary>The minimum height for the upward motion of the <see cref="FABRIKLeg"/>.</summary>
		/// <docs>The minimum height for the upward motion of the FABRIKLeg.</docs>
		/// <remarks><b>This functionality was purpose built for procedural Crawling Animations</b>.</remarks>
		/// <docremarks>This functionality was purpose built for procedural Crawling Animations.</docremarks>
		/// <decorations decor="[SerializeField] protected float"></decorations>
		[SerializeField, Tooltip("The minimum height for the upward motion of the " + nameof(FABRIKLeg) + ". (Crawling animation)")] protected float MinLegUpHeight = 0f;
		/// <summary>The maximum height for the upward motion of the <see cref="FABRIKLeg"/>.</summary>
		/// <docs>The maximum height for the upward motion of the FABRIKLeg.</docs>
		/// <remarks><b>This functionality was purpose built for procedural Crawling Animations</b>.</remarks>
		/// <docremarks>This functionality was purpose built for procedural Crawling Animations.</docremarks>
		/// <decorations decor="[SerializeField] protected float"></decorations>
		[SerializeField, Tooltip("The maximum height for the upward motion of the " + nameof(FABRIKLeg) + ". (Crawling animation)")] protected float MaxLegUpHeight;
		/// <summary>How fast should the <see cref="FABRIKLeg"/> go upwards?</summary>
		/// <docs>How fast should the FABRIKLeg go upwards?</docs>
		/// <remarks><b>This functionality was purpose built for procedural Crawling Animations</b>.</remarks>
		/// <docremarks>This functionality was purpose built for procedural Crawling Animations.</docremarks>
		/// <decorations decor="[SerializeField] protected float"></decorations>
		[SerializeField, Tooltip("How fast should the " + nameof(FABRIKLeg) + " go upwards? (Crawling animation)")] protected float LegUpSpeed;
		float RandomHeightAlpha;

		/// <summary>The <see cref="FABRIKLeg"/>s affected and controlled by FABRIK.</summary>
		/// <docs>The FABRIKLegs affected and controlled by FABRIK.</docs>
		/// <decorations decor="public List{FABRIKLeg}"></decorations>
		[Header("Targets.")]
		[Tooltip("The " + nameof(Legs) + " affected and controlled by FABRIK.")] public List<FABRIKLeg> Legs;

		Vector3 LastFramePosition, ThisFramePosition;
		Vector3 LastFrameEulers, ThisFrameEulers;
		// True if this Leg is still.
		bool bHasStopped;


		[Header("Optimisations")]
		static SpringArm MainSpringArmComponent;
		/// <summary>The maximum <see cref="SpringArm.Distance"/> before FABRIK turns off.</summary>
		/// <docs>The maximum SpringArm.Distance before FABRIK turns off.</docs>
		/// <decorations decor="[SerializeField] protected float"></decorations>
		[SerializeField, Tooltip("The Maximum Distance a Spring Arm can be before FABRIK turns off.")] protected float SpringArmThreshold = 15;
		/// <summary>The maximum distance the Camera can be before FABRIK turns off.</summary>
		/// <remarks>
		/// <b>
		/// Assumes that <see cref="SpringArm.Boom"/> is the Camera.
		/// <br></br>
		/// <i>Will not work otherwise!</i>
		/// </b>
		/// </remarks>
		/// <docremarks>
		/// Assumes that SpringArm.Boom is the Camera.
		/// &lt;br&gt;
		/// &lt;b&gt;&lt;i&gt;Will not work otherwise!&lt;/i&gt;&lt;/b&gt;
		/// </docremarks>
		/// <decorations decor="[SerializeField] protected float"></decorations>
		[SerializeField, Tooltip("The maximum distance the camera can be before FABRIK turns off.")] protected float CameraDistanceThreshold = 17f;
		/// <summary><see langword="true"/> to automatically turn FABRIK off when the Frame Rate is too low.</summary>
		/// <docs>Should FABRIK automatically turn off when the Frame Rate is too low?</docs>
		/// <decorations decor="[SerializeField] protected bool"></decorations>
		[SerializeField, Tooltip("Should FABRIK turn off if the frame rate is too low?")] protected bool bLimitWithFramerate;
		/// <summary>The minimum amount of Frames per Second FABRIK needs to run.</summary>
		/// <decorations decor="[SerializeField] protected int"></decorations>
		[SerializeField, Tooltip("The minimum amount of FPS for FABRIK to run.")] protected int FrameCutoff = 30;
		/// <summary><see langword="true"/> to make FABRIK always run and ignore any optimisations.</summary>
		/// <docs>Should FABRIK ignore optimisations?</docs>
		/// <decorations decor="[SerializeField] protected bool"></decorations>
		[SerializeField, Tooltip("Should FABRIK run regardless of optimisations?")] protected bool bAlwaysRunRegardless;
		bool bFrameLimiterActive;
		bool bUpdateOneLastTime;

		/// <summary>Initialises the <see cref="MainSpringArmComponent"/>.</summary>
		/// <docs>Initialises MainSpringArmComponent.</docs>
		/// <remarks>Assumes that <see cref="Camera.main"/> has a <see cref="SpringArm"/> attached to it.</remarks>
		/// <docremarks>Assumes that Camera.main has a SpringArm attached to it.</docremarks>
		/// <decorations decor="protected virtual void"></decorations>
		protected virtual void Start()
		{
			if (!MainSpringArmComponent)
				MainSpringArmComponent = Camera.main.GetComponent<SpringArm>();

			if (!MainSpringArmComponent)
				Debug.LogWarning("The Camera has no Spring Arm Component. Optimisations made to FABRIK will be switched off.");

			InvokeRepeating(nameof(AssignLegHeights), 0, Mathf.PI / LegUpSpeed);
		}

		/// <summary>Runs FABRIK logic every frame, accounting for optimisations.</summary>
		/// <decorations decor="protected virtual void"></decorations>
		protected virtual void Update()
		{
			if (bAlwaysRunRegardless)
			{
				ExecuteFABRIKLogic(true);

				if (!bHasBeenShownWarning)
				{
					Debug.LogWarning(nameof(bAlwaysRunRegardless) + " is switched on. Remember to turn it off if you're not testing <color=#FEFE00>:)</color>");
					bHasBeenShownWarning = true;
				}

				return;
			}

			bFrameLimiterActive = Utils.FPS() < FrameCutoff;

			if (bFrameLimiterActive)
			{
				if (!bUpdateOneLastTime)
				{
					ExecuteFABRIKLogic(true);
					bUpdateOneLastTime = true;
				}

				return;
			}

			// FABRIK will not execute if this Leg is too far from the Camera.
			if (IsTooFarFromCamera())
				return;

			// FABRIK will not execute on *any* Leg if the Spring Arm Component is too far.
			if (MainSpringArmComponent)
				if (MainSpringArmComponent.Distance >= SpringArmThreshold)
					return;

			ThisFramePosition = transform.position;
			ThisFrameEulers = transform.eulerAngles;

			bUpdateOneLastTime = false;

			if (HasMovedSinceLastFrame())
			{
				ExecuteFABRIKLogic(false);
				bHasStopped = false;
			}
			else
			{
				// Only run FABRIK once more to ensure Legs are on the ground.
				// But do not continue FABRIK when not needed - when this transform is not moving.
				if (!bHasStopped || HasRotatedSinceLastFrame())
					ExecuteFABRIKLogic(true);

				bHasStopped = true;
			}

			LastFramePosition = ThisFramePosition;
			LastFrameEulers = ThisFrameEulers;
		}

		/// <summary>Checks whether this transform has rotated since the last frame.</summary>
		/// <docreturns>True if this transform has rotated by at least .1 on any axis since the last frame.</docreturns>
		/// <returns><see langword="true"/> if this transform has rotated by at least .1 on any axis since the last frame.</returns>
		/// <decorations decor="protected bool"></decorations>
		protected bool HasRotatedSinceLastFrame()
		{
			Vector3 DeltaRot = Abs(ThisFrameEulers - LastFrameEulers);

			const float kThreshold = .1f;
			bool bHasRotatedSinceLastFrame = DeltaRot.x >= kThreshold || DeltaRot.y >= kThreshold || DeltaRot.z >= kThreshold;
			return bHasRotatedSinceLastFrame;
		}

		/// <summary>Checks whether this transform has rotated since the last frame.</summary>
		/// <docreturns>True if this transform has moved kEpsilon since the last frame.</docreturns>
		/// <returns><see langword="true"/> if this transform has moved <see cref="MVector.kEpsilon"/> since the last frame.</returns>
		/// <decorations decor="protected bool"></decorations>
		protected bool HasMovedSinceLastFrame()
		{
			Vector3 DeltaPosition = Abs(ThisFramePosition - LastFramePosition);

			bool bHasMovedSinceLastFrame = DeltaPosition.x >= Vector3.kEpsilon || DeltaPosition.y >= Vector3.kEpsilon || DeltaPosition.z >= Vector3.kEpsilon;
			return bHasMovedSinceLastFrame;
		}

		/// <summary>Calculates how high <see cref="FABRIKLeg.Limbs"/> or <see cref="FABRIKLeg.KneeOrToe"/> should go.</summary>
		/// <docs>Calculates how high Limbs, Knees or Toes should go.</docs>
		/// <decorations decor="protected virtual void"></decorations>
		/// <param name="Height">The maximum height Limbs, Knee or Toes can go.</param>
		/// <param name="ClampedSine">[Default] Height from Sine function.</param>
		/// <param name="ClampedCosine">[Default] Height from Cosine function.</param>
		protected virtual void GetCrawlingHeights(ref float Height, out float ClampedSine, out float ClampedCosine)
		{
			float Degrees = Time.time * LegUpSpeed;

			float UnclampedSine = Mathf.Sin(Degrees);
			ClampedSine = (UnclampedSine + 1) * .5f * Height;

			float UnclampedCosine = Mathf.Cos(Degrees);
			ClampedCosine = (UnclampedCosine + 1) * .5f * Height;

			if (UnclampedSine < -.9f || UnclampedCosine < -.9f)
				RandomHeightAlpha = UnityEngine.Random.Range(.2f, 1f);
		}

		/// <summary>Assigns the height of the maximum height of the legs randomly.</summary>
		/// <remarks>
		/// <b>This functionality was purpose built for procedural Crawling Animations</b>.
		/// <br></br><br></br>
		/// It controls the random heights in <see cref="GetCrawlingHeights(ref float, out float, out float)"/> to mimic
		/// crawling legs.
		/// </remarks>
		/// <docremarks>
		/// &lt;b&gt;This functionality was purpose built for procedural Crawling Animations.&lt;/b&gt;
		/// &lt;br&gt;&lt;br&gt;
		/// It controls the random heights in GetCrawlingHeights() to mimic crawling legs.
		/// </docremarks>
		/// <decorations decor="protected virtual void"></decorations>
		protected virtual void AssignLegHeights()
		{
			if (Legs.Count == 2) // For anything with only two legs.
			{
				float Random = UnityEngine.Random.Range(MinLegUpHeight, MaxLegUpHeight);

				// Leg at index 0.
				FABRIKLeg L0 = Legs[0];
				L0.Height = Random;
				Legs[0] = L0;

				// Leg at index 1.
				FABRIKLeg L1 = Legs[1];
				// Mirror Random with Min and Max.
				L1.Height = MinLegUpHeight + MaxLegUpHeight - Random;
				Legs[1] = L1;
			}
			else // Everything else.
			{
				for (int i = 0; i < Legs.Count; ++i)
				{
					FABRIKLeg L = Legs[i];
					L.Height = UnityEngine.Random.Range(MinLegUpHeight, MaxLegUpHeight);
					Legs[i] = L;
				}
			}
		}

		/// <summary>
		/// Computes FABRIK logic according to predefined settings and assigns <see cref="Legs"/>
		/// their FABRIK-solved positions and rotations.
		/// </summary>
		/// <docs>
		/// Computes FABRIK logic according to predefined settings and assigns Legs to
		/// their FABRIK-solved positions and rotations.
		/// </docs>
		/// <decorations decor="protected void"></decorations>
		/// <param name="bSnapToTarget">Should the FABRIK solver snap to the final position?</param>
		protected void ExecuteFABRIKLogic(bool bSnapToTarget = false)
		{
			for (int L = 0; L < Legs.Count; ++L)
			{
				FABRIKLeg Leg = Legs[L];

				RaySettings(Leg, out Vector3 Origin, out Vector3 Direction);

				if (Physics.Raycast(Origin, Direction, out RaycastHit Hit, 50, 256))
				{
					Vector3[] Joints = Leg.GetJointPositions();

					RunFABRIK(Joints, Leg.KneeOrToe[0].position, GetTarget(Hit, Leg, L), GetRelativeXYZ(Leg.Bias));

					for (int i = 0; i < Joints.Length; ++i)
					{
						Leg.KneeOrToe[i].position = Joints[i];

						if (i > 0)
						{
							Transform Limb = Leg.Limbs[i];

							Limb.position = Joints[i - 1];

							if (i < Joints.Length - 1)
							{
								Limb.LookAt(Leg.KneeOrToe[i]);
								Limb.RotateAround(Limb.position, Limb.up, 90);
							}
							else
							{
								Limb.LookAt(Leg.KneeOrToe[Joints.Length - 1]);
								Limb.RotateAround(Limb.position, Limb.up, 90);
							}
						}
					}
				}
			}

			Vector3 GetTarget(RaycastHit Hit, FABRIKLeg Leg, int L)
			{
				GetCrawlingHeights(ref Leg.Height, out float Sine, out float Cos);

				float Alpha = L % 2 == 0 ? Sine : Cos;
				float HeightAlternator = Leg.Height * Alpha;

				return bSnapToTarget ? Hit.point : Hit.point + transform.up * HeightAlternator;
			}

			if (bShowJoints)
			{
				foreach (FABRIKLeg Leg in Legs)
				{
					for (int i = 0; i < Leg.KneeOrToe.Count - 1; ++i)
					{
						Debug.DrawLine(Leg.KneeOrToe[i].position, Leg.KneeOrToe[i + 1].position, Color.cyan);
					}
				}
			}
		}

		#region FABRIK Utils

		/// <summary>The implementation of the FABRIK Solver.</summary>
		/// <decorations decor="protected void"></decorations>
		/// <param name="Joints">The Joints to consider when adjusting positions and rotations.</param>
		/// <param name="Root">The position of the Joint that is directly conneected to the rest of the body.</param>
		/// <param name="Target">The positions of Joints that FABRIK should solve towards.</param>
		/// <param name="RelativeBias">THe Joints' Bias point. (Vector3.zero for no bias)</param>
		protected void RunFABRIK(Vector3[] Joints, Vector3 Root, Vector3 Target, Vector3 RelativeBias)
		{
			float[] Lengths = new float[Joints.Length - 1];
			for (int i = 0; i < Joints.Length - 1; i++)
			{
				Lengths[i] = (Joints[i + 1] - Joints[i]).magnitude;
				Joints[i] += RelativeBias;
			}

			float ToleranceSquared = Tolerance * Tolerance;

			for (int Pass = 0; Pass < MaximumPasses; ++Pass)
			{
				bool bGoingBackward = Pass % 2 == 0;

				// Reverse arrays to alternate between forward and backward passes
				Array.Reverse(Joints);
				Array.Reverse(Lengths);

				Joints[0] = (bGoingBackward) ? Target : Root;

				// Constrain lengths
				for (int i = 1; i < Joints.Length; i++)
				{
					Vector3 Direction = (Joints[i] - Joints[i - 1]).normalized;
					Joints[i] = Joints[i - 1] + Direction * Lengths[i - 1];
				}

				// Terminate if close enough to target
				float Distance = (Joints[Joints.Length - 1] - Target).sqrMagnitude;
				if (!bGoingBackward && Distance <= ToleranceSquared)
					return;
			}
		}

		/// <summary>Calculates how rays should be shot to determine FABRIK Targets.</summary>
		/// <decorations decor="protected virtual void"></decorations>
		/// <param name="Leg">The relative position to calculate the Origin of the Ray.</param>
		/// <param name="Origin">The Origin of the ray to determine the FABRIK Target.</param>
		/// <param name="Direction">The Direction the ray will travel to determine the FABRIK Target.</param>
		protected virtual void RaySettings(FABRIKLeg Leg, out Vector3 Origin, out Vector3 Direction)
		{
			Origin = transform.position + GetRelativeXYZ(Leg.TargetRayOrigin.x, Leg.TargetRayOrigin.y, Leg.TargetRayOrigin.z);
			Direction = -transform.up;
		}

		/// <summary>Gets the relative position of V whilst considering rotations.</summary>
		/// <decorations decor="protected Vector3"></decorations>
		/// <param name="V">The vector to convert to local space.</param>
		/// <returns>The coordinates of V, relative to this transform.</returns>
		protected Vector3 GetRelativeXYZ(Vector3 V) => GetRelativeXYZ(V.x, V.y, V.z);

		/// <summary>Gets the relative position of X, Y, and Z whilst considering rotations.</summary>
		/// <decorations decor="protected Vector3"></decorations>
		/// <param name="X">The X-Axis value to convert to local space.</param>
		/// <param name="Y">The Y-Axis value to convert to local space.</param>
		/// <param name="Z">The Z-Axis value to convert to local space.</param>
		/// <returns>The coordinates of X, Y, and Z, relative to this transform.</returns>
		protected Vector3 GetRelativeXYZ(float X, float Y, float Z) => transform.right * X + transform.up * Y + transform.forward * Z;

		#endregion

		/// <summary>Determines how far is too far for the Camera.</summary>
		/// <decorations decor="protected virtual void"></decorations>
		/// <docreturns>True if the distance between this and the Camera exceeds CameraDistanceThreshold.</docreturns>
		/// <returns><see langword="true"/> if the distance between this and the Camera exceeds <see cref="CameraDistanceThreshold"/>.</returns>
		protected virtual bool IsTooFarFromCamera()
		{
			if (MainSpringArmComponent)
			{
				Vector3 FABRIK = transform.position;
				Vector3 Camera = MainSpringArmComponent.Boom.transform.position;

				float A = FABRIK.x - Camera.x;
				float B = FABRIK.y - Camera.y;
				float C = FABRIK.z - Camera.z;

				return (A * A + B * B + C * C) > (CameraDistanceThreshold * CameraDistanceThreshold);
			}

			return false;
		}

		protected virtual void OnDrawGizmos()
		{
			if (Legs != null && (bShowRays || bShowJoints || bShowBiasPoint))
			{
				foreach (FABRIKLeg Leg in Legs)
				{
					Gizmos.color = Color.cyan;

					if (bShowRays)
					{
						RaySettings(Leg, out Vector3 Origin, out Vector3 Direction);

						Gizmos.DrawRay(Origin, Direction);
					}

					if (bShowJoints)
					{
						foreach (Transform T in Leg.KneeOrToe)
							Gizmos.DrawSphere(T.position, JointGizmosRadius);
					}

					if (bShowBiasPoint)
					{
						Gizmos.color = Color.red;
						Gizmos.DrawSphere(transform.position + GetRelativeXYZ(Leg.Bias.x, Leg.Bias.y, Leg.Bias.z), BiasGizmosRadius);
					}
				}
			}
		}
	}


	/// <summary>Relevant information referencing how and what will be affected by FABRIK.</summary>
	/// <decorations decor="[Serializable] public struct"></decorations>
	[Serializable]
	public struct FABRIKLeg
	{
		/// <summary>The position FABRIK should begin to searching for a FABRIK Target.</summary>
		/// <decorations decor="public Vector3"></decorations>
		[Header("Origin and Targets.")]
		public Vector3 TargetRayOrigin;
		/// <summary>Straight and unbendable transforms affected by FABRIK.</summary>
		/// <decorations decor="public List{Transform}"></decorations>
		public List<Transform> Limbs;
		/// <summary>The Joints/positions between the Limbs for FABRIK Target angle corrections.</summary>
		/// <decorations decor="public List{Transform}"></decorations>
		public List<Transform> KneeOrToe;
		/// <summary>The random Height assigned by <see cref="FABRIK.AssignLegHeights"/>.</summary>
		/// <docs>The random Height assigned by AssignLegHeights().</docs>
		/// <decorations decor="public float"></decorations>
		[HideInInspector] public float Height;
		/// <summary>The position where <see cref="Limbs"/> and <see cref="KneeOrToe"/> should aim towards while adjusting to FABRIK.</summary>
		/// <docs>The positoin where Limbs, Knees, Toes should aim towards while adjusting to FABRIK.</docs>
		/// <decorations decor="public Vector3"></decorations>
		public Vector3 Bias;

		/// <summary>Gets the positions of all the Joints.</summary>
		/// <decorations decor="public Vector3[]"></decorations>
		/// <returns>The positions of all the Joints.</returns>
		public Vector3[] GetJointPositions() => KneeOrToe.Select(KT => KT.position).ToArray();
	}
}