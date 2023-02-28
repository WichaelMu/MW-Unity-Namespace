//#define ENABLE_CUSTOM_PROJECTION

using UnityEngine;
using MW.Console;
using MW.Extensions;
using static MW.Math.Mathematics;
using static MW.Utils;
using static MW.Debugger.Arrow;
using static MW.Math.Magic.Fast;

namespace MW.CameraUtils
{
	/// <summary>A Spring Arm Component that allows <see cref="Boom"/> to track a <see cref="Target"/>.</summary>
	/// <docs>A Spring Arm Component that allows a Boom to track a Target.</docs>
	/// <remarks>While commonly used as for Camera tracking, this can be used for any object that needs to track <see cref="Target"/>.</remarks>
	/// <docremarks>While commonly used as for Camera tracking, this can be used for any object that needs to track a Target.</docremarks>
	/// <decorations decor="public class : MonoBehaviour"></decorations>
	public class SpringArm : MonoBehaviour
	{
		[Header("Debug")]
		[SerializeField] bool bDrawRotationalLines;
		[SerializeField] bool bDrawAdvancedCollisionLines;

		/// <summary>The <see cref="Transform"/> that will track <see cref="Boom"/>.</summary>
		/// <docs>The Transform that will track Boom.</docs>
		/// <decorations decor="public Transform"></decorations>
		[Header("Target Settings.")]
		public Transform Boom;
		/// <summary>The Target for <see cref="Boom"/> to track.</summary>
		/// <docs>The Target for Boom to track.</docs>
		/// <decorations decor="public Transform"></decorations>
		public Transform Target;
		/// <summary>The offset at which Boom will track Target.</summary>
		/// <decorations decor="[SerializeField] Vector3"></decorations>
		[SerializeField] Vector3 TargetOffset;

		[HideInInspector] public Camera CameraComponent;

		/// <summary>The distance in which Boom will maintain from Target whilst tracking.</summary>
		/// <remarks>Will not be used when the line between <see cref="Boom"/> and <see cref="Target"/> is interrupted by a <see cref="Collider"/> collision.</remarks>
		/// <docremarks>Will not be used when the line between Boom and Target is interrupted by a collision.</docremarks>
		/// <decorations decor="public float"></decorations>
		[Header("Spring Arm Settings.")]
		public float Distance;
		/// <summary>The world-rotation of <see cref="Boom"/>, about <see cref="Target"/> + <see cref="TargetOffset"/>.</summary>
		/// <docs>The world-rotation of Boom, about Target + TargetOffset.</docs>
		/// <decorations decor="public Vector3"></decorations>
		[SerializeField] Vector3 GimbalRotation;
		/// <summary>The world-rotation of <see cref="Boom "/> as it looks and faces <see cref="Target"/>.</summary>
		/// <docs>The world-rotation of Boom as it looks and faces Target.</docs>
		/// <decorations decor="public Vector3"></decorations>
		[SerializeField] protected Vector3 CameraRotation;
		/// <summary><see langword="true"></see> to inherit <see cref="Target"/>'s rotation.</summary>
		/// <docs>True to inherit Target's rotation.</docs>
		/// <decorations decor="public bool"></decorations>
		public bool bInheritRotation;
		/// <summary><see langword="true"/> to inherit the <see cref="Target"/>'s Pitch rotation only.</summary>
		/// <docs>True to inherit the Target's Pitch rotation only.</docs>
		/// <decorations decor="[SerializeField] bool"></decorations>
		[SerializeField] bool bInheritPitch;
		[HideInInspector, SerializeField] Vector3 DefaultGimbalRotation;
		[HideInInspector, SerializeField] Vector3 DefaultCameraRotation;

		/// <summary><see langword="true"></see> to let the mouse wheel control <see cref="Distance"/>.</summary>
		/// <docs>True to let the mouse wheel control Distance.</docs>
		/// <decorations decor="public bool"></decorations>
		[Header("Scroll Settings")]
		[SerializeField] bool bEnableScrollToDistance;
		/// <summary>How much will <see cref="Distance"/> change when scrolling?</summary>
		/// <docs>How much will Distance change when scrolling?</docs>
		/// <decorations decor="public float"></decorations>
		[SerializeField] float ScrollSensitivity;
		/// <summary>The minimum and maximum Distance of the Spring Arm when Scrolling.</summary>
		/// <decorations decor="[SerializeField] Vector2"></decorations>
		[SerializeField] Vector2 MinMaxDistance = new Vector2(1, 30);

		/// <summary>How much will the Spring Arm rotate around Target?</summary>
		/// <decorations decor="[SerializeField] float"></decorations>
		[Header("Orbit Settings")]
		[SerializeField] float OrbitSensitivity = 1f;
		/// <summary>The minimum and maximum angle of the Spring Arm when Orbiting.</summary>
		/// <decorations decor="[SerializeField] Vector2"></decorations>
		[SerializeField] Vector2 MinMaxOrbitAngle = new Vector2(1f, 70f);

		/// <summary>True to permanently change the TargetOffset when panning.</summary>
		/// <decorations decor="[SerializeField] bool"></decorations>
		[Header("Pan Settings")]
		[SerializeField] bool bPermanentlyChangePanTargetOffset;
		/// <summary>The maximum distance of the Spring Arm's offset to Target when Panning.</summary>
		/// <decorations decor="[SerializeField] float"></decorations>
		[SerializeField] float MaxPanDistance = 5f;

		Vector3 GimbalRotationInherited;
		Vector3 CameraRotationInherited;
		Vector3 OriginalTargetOffset;
		float PitchDegrees;

		/// <summary><see langword="true"></see> to invert the Controls when Orbiting about the world X-Axis.</summary>
		/// <docs>True to invert the Controls when Orbiting around the world X-Axis.</docs>
		/// <decorations decor="public bool"></decorations>
		[Header("Inverse Settings.")]
		public bool bInvertX; // Inverse LR dragging Orbit Controls.
		/// <summary><see langword="true"></see> to invert the Controls when Orbiting about the world Y-Axis.</summary>
		/// <docs>True to invert the Controls when Orbiting around the world Y-Axis.</docs>
		/// <decorations decor="public bool"></decorations>
		public bool bInvertY; // Inverse UD dragging Orbit Controls.
		/// <summary><see langword="true"></see> to invert the Zooming Distance direction.</summary>
		/// <docs>True to invert the Zooming Distance direction.</docs>
		/// <decorations decor="public bool"></decorations>
		public bool bInvertZ; // Inverse Zoom Controls.

		/// <summary>Should this Spring Arm run Collision Checks against OnlyCollideWith?</summary>
		/// <decorations decor="[SerializeField] bool"></decorations>
		[Header("Collisions")]
		[SerializeField] bool bRunCollisionChecks;
		/// <summary>Use Advanced Collision Behaviour upon Collision?</summary>
		/// <remarks>This automatically rotates and swivels the camera out of the way when the Target has backed up against a wall of OnlyCollideWith.</remarks>
		/// <decorations decor="[SerializeField] bool"></decorations>
		[SerializeField] bool bUseAdvancedCollisionBehaviour;
		/// <summary>Should this Spring Arm force Advanced Collision Behaviour?</summary>
		/// <remarks>Advanced Collision Behaviour will run, regardless of if Target is backed up against a wall of OnlyCollideWith.</remarks>
		/// <decorations decor="[SerializeField] bool"></decorations>
		[SerializeField] bool bForceAdvancedBehaviour;
		/// <summary>The Distance in which to activate Advanced Collision Behaviour.</summary>
		/// <decorations decor="[SerializeField] float"></decorations>
		[SerializeField] float AdvancedCollisionActivationDistance;
		/// <summary>The maximum angle the Advanced Collision Behaviour will rotate and swivel.</summary>
		/// <decorations decor="[SerializeField] float"></decorations>
		[SerializeField] float MaxAdvancedCollisionAngle;
		/// <summary>Interrupt Distance when a collision is made with this layer/s.</summary>
		/// <decorations decor="[SerializeField] LayerMask"></decorations>
		[SerializeField] LayerMask OnlyCollideWith;
		bool bIsAdvancedBehaviourEffective;
		Vector3 AdvancedForwardVector;
		Vector3 AdvancedRightVector;
		bool bHasSetAdvancedVectors;

		/// <summary>The strength Boom will Lag behind tracking Target.</summary>
		/// <remarks>0 is standstill (infinite Lag). 1 is instant (no Lag).</remarks>
		/// <decorations decor="[SerializeField] float"></decorations>
		[Header("Lag Settings")]
		[SerializeField] float PositionalLagStrength = .2f;
		/// <summary>The strength Boom will rotate to face Target.</summary>
		/// <remarks>0 is standstill (infinite Lag). 1 is instant (no Lag).</remarks>
		/// <decorations decor="[SerializeField] float"></decorations>
		[SerializeField] float RotationalLagStrength = .2f;
		Vector3 TargetPosition;
		Quaternion TargetRotation;

#if ENABLE_CUSTOM_PROJECTION
		/// <summary><see langword="true"/> to use a Camera Projection that cuts away close <see cref="Mesh"/>es.</summary>
		/// <docs>True to use a Camera Projection that cuts away close Meshes.</docs>
		/// <decorations decor="public bool"></decorations>
		[Header("Projection Settings.")]
		[SerializeField] bool bUseCustomProjection;
		[SerializeField] Transform Plane;
		/// <summary>The distance when <see cref="Mesh"/>es are clipped.</summary>
		/// <docs>The distance when Meshes are clipped.</docs>
		/// <decorations decor="[SerializeField] float"></decorations>
		[SerializeField] float NearClipDistance;
		/// <summary>How close should the Camera be to stop using this Custom Projection?</summary>
		/// <decorations decor="[SerializeField] float"></decorations>
		[SerializeField] float DistanceLimit;
		Matrix4x4 DefaultProjection;
#endif

		bool bNoClip = false;

		/// <summary></summary>
		/// <decorations decor="protected virtual void"></decorations>
		protected virtual void Start()
		{
			DefaultGimbalRotation = GimbalRotation;
			DefaultCameraRotation = CameraRotation;

			GimbalRotationInherited = DefaultGimbalRotation;
			CameraRotationInherited = DefaultCameraRotation;

			OriginalTargetOffset = TargetOffset;

			CameraComponent = GetComponent<Camera>();

#if ENABLE_CUSTOM_PROJECTION
			DefaultProjection = CameraComponent.projectionMatrix;
#endif
		}

		void Update()
		{
			if (bNoClip)
			{
				const float kNoClipSpeed = 25f;

				if (Input.GetKey(KeyCode.W))
				{
					Boom.position += kNoClipSpeed * Time.deltaTime * Boom.forward;
				}
				else if (Input.GetKey(KeyCode.S))
				{
					Boom.position -= kNoClipSpeed * Time.deltaTime * Boom.forward;
				}

				if (Input.GetKey(KeyCode.D))
				{
					Boom.position += kNoClipSpeed * Time.deltaTime * Boom.right;
				}
				else if (Input.GetKey(KeyCode.A))
				{
					Boom.position -= kNoClipSpeed * Time.deltaTime * Boom.right;
				}

				if (Input.GetKey(KeyCode.Q))
				{
					Boom.position -= kNoClipSpeed * Time.deltaTime * Vector3.up;
				}
				else if (Input.GetKey(KeyCode.E))
				{
					Boom.position += kNoClipSpeed * Time.deltaTime * Vector3.up;
				}
			}
		}

		Vector3 SmoothPositionVelocity;

		void FixedUpdate()
		{
			if (bNoClip)
				return;

			Boom.SetPositionAndRotation(
				Vector3.SmoothDamp(Boom.position, TargetPosition, ref SmoothPositionVelocity, PositionalLagStrength),
				Quaternion.Slerp(Boom.rotation, TargetRotation, RotationalLagStrength)
			);

			PlaceCamera();
		}

#if ENABLE_CUSTOM_PROJECTION
	void OnPreCull()
	{
		ComputeProjection();
	}
#endif

		void PlaceCamera()
		{
			// Where the Spring Arm will point towards.
			Vector3 ArmDirection = Vector3.one;
			Vector3 FinalPosition;
			Quaternion FinalRotation = Quaternion.Euler(CameraRotation);

			if (!bInheritRotation)
			{
				float VerticalOrbit = GimbalRotation.x;
				float HorizontalOrbit = -GimbalRotation.y;

				VerticalOrbit *= Mathf.Deg2Rad;
				HorizontalOrbit *= Mathf.Deg2Rad;

				// Convert Angles to Vectors.
				Vector3 Ground = new Vector3(Mathf.Sin(VerticalOrbit), 0, Mathf.Cos(VerticalOrbit)); // XZ.
				Vector3 Up = new Vector3(0, Mathf.Sin(HorizontalOrbit), Mathf.Cos(HorizontalOrbit)); // XYZ.

				// Ground's XZ and Up's Y will be used to define the direction of the Spring Arm.
				ArmDirection = new Vector3(Ground.x, Up.y, Ground.z).normalized;

				if (bDrawRotationalLines)
				{
					Debug.DrawLine(Target.position, Target.position + -Ground * Distance, Color.red);
					Debug.DrawLine(Target.position, Target.position + -Up * Distance, Color.green);
					Debug.DrawLine(Target.position, Target.position + -ArmDirection * Distance, Color.yellow);
				}
			}
			else
			{
				if (bInheritPitch && Physics.Raycast(Target.position + Vector3.up, Vector3.down, out RaycastHit Ground, 5f, OnlyCollideWith))
				{
					PitchDegrees = 90f - V2PYR(Ground.normal).x;
				}
				else
				{
					PitchDegrees = 0f;
				}

				// Rotates the Spring Arm around to face the Target's forward vector.
				// Ignores the Target's Y-Axis, replacing it with the Yaw rotation,
				// relative to the Target, after inheritance.
				ArmDirection = Target.forward;
				ArmDirection.y = Mathf.Sin((-GimbalRotationInherited.y - PitchDegrees) * Mathf.Deg2Rad);
				ArmDirection.Normalize();

				FinalRotation = GetInheritedRotation();
			}

			// If the Spring Arm will collider with something:
			if (bRunCollisionChecks && RunCollisionsCheck(ref ArmDirection))
				return;

			// Make the Position and Rotation for Lag.
			FinalPosition = GetTargetPosition() - (Distance * ArmDirection);

			SetPositionAndRotation(FinalPosition, FinalRotation);
		}

		/// <summary>Runs Collision Checks.</summary>
		/// <decorations decor="protected bool"></decorations>
		/// <param name="Direction">A reference to the desired Direction of the Spring Arm.</param>
		/// <returns>True if a Collision with OnlyCollideWith was detected.</returns>
		protected bool RunCollisionsCheck(ref Vector3 Direction)
		{
#if ENABLE_CUSTOM_PROJECTION
			if (bUseCustomProjection)
				return false;
#endif

			Vector3 TP = GetTargetPosition();
			Ray FOV = new Ray(TP, -Direction);
			bool bViewToTargetBlocked = Physics.Raycast(FOV, out RaycastHit Hit, Distance, OnlyCollideWith);

			Quaternion Rotation = !bInheritRotation
						? Quaternion.Euler(CameraRotation)
						: GetInheritedRotation();

			CollisionLogic(Direction, TP, FOV, bViewToTargetBlocked, Hit, Rotation);

			return bViewToTargetBlocked;
		}

		/// <summary>Handles the Collision Logic and determines either basic or Advanced Behaviour should be used.</summary>
		/// <decorations decor="protected virtual void"></decorations>
		/// <param name="Direction">The normalised Direction towards the Collision point from the Spring Arm.</param>
		/// <param name="TargetPosition">The Target's Position.</param>
		/// <param name="FOV">The Ray that was fired and made a Collision.</param>
		/// <param name="bViewToTargetBlocked">True if the Spring Arm's view to Target was blocked.</param>
		/// <param name="Hit">The RaycastHit information of the Collision.</param>
		/// <param name="Rotation">The desired Rotation of the Spring Arm.</param>
		protected virtual void CollisionLogic(Vector3 Direction, Vector3 TargetPosition, Ray FOV, bool bViewToTargetBlocked, RaycastHit Hit, Quaternion Rotation)
		{
			if (bViewToTargetBlocked)
			{
				// Run Collision Checks NORMALLY if:
				// The Ground Normal Pitch Degrees is greater than 10 degrees. OR                                  // If greater than 10 degrees, use Advanced.
				// We're not Forcing Advanced Collision Behaviour AND EITHER ANY OF:                               // If Forcing Advanced Collision, use Advanced.
				// We're not even using the Advanced Collision Behaviour OR                                        // If we are using Advanced Collision, continue evaluating...
				// We're not Inheriting Rotation OR                                                                // If we are using Advanced Collision, we *must* also be Inheriting Rotation, continue evaluating if so...
				// The distance between the wall and the Target is greater than the Advanced Distance Threshold.   // If the wall is 'close enough' to the Target, finally use Advanced Collision.
				if (PitchDegrees > 10f || (!bForceAdvancedBehaviour && !bUseAdvancedCollisionBehaviour ||
					!bInheritRotation || Hit.distance > AdvancedCollisionActivationDistance))
				{
					Vector3 Point = Hit.point - FOV.direction;
					SetPositionAndRotation(Point, Rotation);
				}
				// Fail-safe checks. Here we sort of redefine what Force Advanced Behaviour means...
				// If the above fails, but we're still forcing the use of Advanced, we can still do it regardless if we're Inheriting Rotations.
				// Otherwise, if we're not using Force, the Advanced Behaviour must use Inherit Rotation.
				else if (bForceAdvancedBehaviour || bInheritRotation)
				{
					RunAdvancedCollision(TargetPosition, Hit, Rotation);
				}
				// Otherwise, skip the frame.
			}

			// bIsAdvancedBehaviourEffective = bViewToTargetBlocked;

			if (!bIsAdvancedBehaviourEffective)
				bHasSetAdvancedVectors = false;
		}

		void RunAdvancedCollision(MVector TP, RaycastHit Hit, Quaternion Rotation)
		{
			if (!bHasSetAdvancedVectors)
			{
				AdvancedForwardVector = Target.forward;
				AdvancedRightVector = Target.right;

				bHasSetAdvancedVectors = true;
			}

			bIsAdvancedBehaviourEffective = true;

			if (bDrawAdvancedCollisionLines)
			{
				DebugArrow(transform.position, AdvancedForwardVector, Color.blue);
				DebugArrow(transform.position, AdvancedRightVector, Color.red);
			}

			// Dynamically adjust the angle to be inversely proportional to the distance with the Wall and the Max Spring Arm Distance.
			float DeltaAngle = 90f * ((Distance - Hit.distance) / Distance);
			ClampMax(ref DeltaAngle, MaxAdvancedCollisionAngle);

			// Make and Rotate L and R Vectors Rotated by the above Angle.
			MVector TargetDirectionToCamera = (Hit.point.MV() - TP).Normalised;
			Vector3 Right = TargetDirectionToCamera.RotateVector(-DeltaAngle, MVector.Up);
			Vector3 Left = TargetDirectionToCamera.RotateVector(DeltaAngle, MVector.Up);

			Ray RR = new Ray(TP, Right);
			Ray RL = new Ray(TP, Left);

			// If the above Rays don't hit anything, set default positions for L and R at Distance units away from Target.
			Vector3 PointRight = TP + Right * Distance;
			Vector3 PointLeft = TP + Left * Distance;

			if (bDrawAdvancedCollisionLines)
				DebugArrow(TP, (Vector3)TargetDirectionToCamera * Hit.distance, Color.yellow);

			bool bRHit = Physics.Raycast(RR, out RaycastHit RHit, Distance, OnlyCollideWith);
			bool bLHit = Physics.Raycast(RL, out RaycastHit LHit, Distance, OnlyCollideWith);

			if (bRHit)
			{
				PointRight = RHit.point;

				if (bDrawAdvancedCollisionLines)
					DebugArrow(TP, Right * RHit.distance, Color.red);
			}

			if (bLHit)
			{
				PointLeft = LHit.point;

				if (bDrawAdvancedCollisionLines)
					DebugArrow(TP, Left * LHit.distance, Color.green);
			}

			// Look at TargetPos() while still maintaining the inherited Pitch rotation.
			Boom.LookAt(TP);
			Boom.localEulerAngles = new Vector3(CameraRotationInherited.x, Boom.localEulerAngles.y, Boom.localEulerAngles.z);

			// Choose the Ray furthest away from Target.
			SetPositionAndRotation(
				MVector.SqrDistance(TP, PointRight) < MVector.SqrDistance(TP, PointLeft)
					? PointLeft
					: PointRight,
				Rotation
			);
		}

		/// <summary>Sets the Spring Arm's Position and Rotation.</summary>
		/// <remarks>Used for lag (if any) during runtime and placement within the Editor.</remarks>
		/// <decorations decor="protected void"></decorations>
		/// <param name="FinalPosition"></param>
		/// <param name="FinalRotation"></param>
		protected void SetPositionAndRotation(Vector3 FinalPosition, Quaternion FinalRotation)
		{
			if (!Application.isPlaying)
			{
				Boom.position = FinalPosition;
				Boom.rotation = FinalRotation;
				return;
			}

			if (TargetPosition != FinalPosition || TargetRotation != FinalRotation)
			{
				TargetPosition = FinalPosition;
				TargetRotation = FinalRotation;
			}
		}

		/// <summary>The Target's position including any offsets and rotations.</summary>
		/// <decorations decor="protected virtual Vector3"></decorations>
		/// <returns>The Target's Position + TargetOffset + Target.up.y.</returns>
		protected virtual Vector3 GetTargetPosition() => Target.position + TargetOffset * Target.up.y;

		/// <summary>Gets the Inherited Rotation of the Target.</summary>
		/// <decorations decor="protected virtual Quaternion"></decorations>
		/// <returns>A Quaternion representing the Target's rotation, as well as this Spring Arm's rotation settings.</returns>
		protected virtual Quaternion GetInheritedRotation()
		{
			return Quaternion.Euler(new Vector3(CameraRotationInherited.x + PitchDegrees * .5f, CameraRotationInherited.y + GetInheritedAxis(Target.localEulerAngles.y)));
		}

		float GetInheritedAxis(float AxisAngle)
		{
			float TargetAxis = AxisAngle;
			if (TargetAxis < 0f)
				TargetAxis = 360f + TargetAxis; // TargetAxis is negative.
			return TargetAxis;
		}

		/// <summary>Changes the <see cref="Distance"/> of the Spring Arm.</summary>
		/// <docs>Sets the Distance of the Spring Arm.</docs>
		/// <decorations decor="public void"></decorations>
		/// <param name="DistanceDelta">The amount to increase or decrease.</param>
		public void SetDeltaDistance(float DistanceDelta)
		{
			if (bEnableScrollToDistance && FAbs(DistanceDelta) > MVector.kEpsilon)
			{
				Distance += DistanceDelta * (bInvertZ ? -1f : 1f) * -ScrollSensitivity;

				Clamp(ref Distance, MinMaxDistance.x, MinMaxDistance.y);
			}
		}

		/// <summary>Rotates the Spring Arm around <see cref="Target"/>.</summary>
		/// <docs>Rotates the Spring Arm around Target.</docs>
		/// <decorations decor="public void"></decorations>
		/// <param name="RotationDelta">The delta rotation in degrees where X = Yaw, Y = Pitch.</param>
		public void SetDeltaRotation(Vector2 RotationDelta)
		{
			if (RotationDelta.sqrMagnitude > Vector2.kEpsilon)
			{
				float DeltaX = RotationDelta.x * OrbitSensitivity;
				float DeltaY = RotationDelta.y * OrbitSensitivity;

				DetermineInverse(ref DeltaX, ref DeltaY);

				if (!bInheritRotation)
				{
					GimbalRotation.x += DeltaX;
					CameraRotation.y += DeltaX;

					if (GimbalRotation.y - DeltaY < MinMaxOrbitAngle.y && GimbalRotation.y - DeltaY >= MinMaxOrbitAngle.x)
					{
						GimbalRotation.y -= DeltaY;
						CameraRotation.x -= DeltaY;
					}
				}
				else
				{
					CameraRotationInherited.x -= DeltaY;
					CameraRotationInherited.y += DeltaX;

					if (GimbalRotationInherited.y - DeltaY < MinMaxOrbitAngle.y && GimbalRotationInherited.y - DeltaY >= MinMaxOrbitAngle.x)
					{
						GimbalRotationInherited.y -= DeltaY;
					}
				}

			}
			else
			{
				GimbalRotationInherited = DefaultGimbalRotation;
				CameraRotationInherited = DefaultCameraRotation;
			}
		}

		void DetermineInverse(ref float DeltaX, ref float DeltaY)
		{
			if (bInvertX)
				Inverse(ref DeltaX);
			if (bInvertY)
				Inverse(ref DeltaY);

			static void Inverse(ref float F) => F *= -1f;
		}

		/// <summary>Pans the Spring Arm about <see cref="GetTargetPosition"/>.</summary>
		/// <docs>Pans the Spring Arm about GetTargetPosition().</docs>
		/// <decorations decor="public void"></decorations>
		/// <param name="PanDelta">The amount to pan where X = Horizontal, Y = Vertical.</param>
		public void SetDeltaCameraPan(Vector2 PanDelta)
		{
			if (PanDelta.sqrMagnitude > Vector2.kEpsilon)
			{
				float DeltaX = PanDelta.x * OrbitSensitivity * Time.deltaTime;
				float DeltaY = PanDelta.y * OrbitSensitivity * Time.deltaTime;

				// Ensure 'Right' and 'Up' is relative to the Camera.
				TargetOffset -= DeltaX * Time.deltaTime * Boom.right + DeltaY * Time.deltaTime * Boom.up;
				TargetOffset = Vector3.ClampMagnitude(TargetOffset, MaxPanDistance);
			}
			else
			{
				if (!bPermanentlyChangePanTargetOffset)
					TargetOffset = Vector3.Lerp(TargetOffset, OriginalTargetOffset, .2f);
			}
		}

		/// <summary>Gets the Forward and Right vectors of this Spring Arm.</summary>
		/// <decorations decor="public void"></decorations>
		/// <param name="Forward">Out Forward Vector.</param>
		/// <param name="Right">Out Right Vector.</param>
		public void GetForwardRight(out Vector3 Forward, out Vector3 Right)
		{
			Forward = transform.forward;
			Right = transform.right;

#if false // Kept for Options.
		bool bCheck = bIsAdvancedBehaviourEffective && bInheritRotation;

		Forward = bCheck
			? transform.forward
			: transform.forward;

		Right = bCheck
			? transform.right
			: transform.right;
#endif
		}

		/// <summary>Toggles No Clip.</summary>
		/// <decorations decor="[Exec] public void"></decorations>
		[Exec("Toggles No Clip on the first Spring Arm.")]
		public void NoClip()
		{
			bNoClip = !bNoClip;

			if (!bNoClip)
				TargetOffset = OriginalTargetOffset;
		}

		/// <summary>Teleports Target to the Spring Arm's position.</summary>
		/// <decorations decor="[Exec] public void"></decorations>
		[Exec("Teleports this Spring Arm's Target to the current position.")]
		public void TP_Pos()
		{
			if (bNoClip)
				NoClip();

			Target.position = transform.position;
		}

#if ENABLE_CUSTOM_PROJECTION
	void ComputeProjection()
	{
		if (bUseCustomProjection && Distance > 3)
		{
			Plane = Camera;

			if (Physics.Linecast(Target.position, Camera.position, out RaycastHit Intercept, OnlyCollideWith))
			{
				NearClipDistance = Intercept.distance;
			}
			else
			{
				CameraComponent.projectionMatrix = DefaultProjection;
				return;
			}

			int Dot = Math.Sign(Vector3.Dot(Plane.forward, (Target.position - Camera.position).normalized));
			Vector3 CameraWorldPosition = CameraComponent.worldToCameraMatrix.MultiplyPoint(Target.position);
			Vector3 CameraNormal = CameraComponent.worldToCameraMatrix.MultiplyVector((Target.position - Camera.position).normalized) * 1;

			float CameraDistance = -Vector3.Dot(CameraWorldPosition, CameraNormal) + NearClipDistance;

			// If the Camera is too close to the Target, don't use oblique projection.
			if (Mathf.Abs(CameraDistance) > DistanceLimit)
			{
				Vector4 ClipPlaneCameraSpace = new Vector4(CameraNormal.x, CameraNormal.y, CameraNormal.z, CameraDistance);

				CameraComponent.projectionMatrix = CameraComponent.CalculateObliqueMatrix(ClipPlaneCameraSpace);
			}
			else
			{
				CameraComponent.projectionMatrix = DefaultProjection;
			}
		}
		else
		{
			CameraComponent.projectionMatrix = DefaultProjection;
		}
	}
#endif

		/// <summary></summary>
		/// <decorations decor="protected virtual void"></decorations>
		protected virtual void OnValidate()
		{
			if (Boom && Target)
				PlaceCamera();

			GimbalRotation.y = Mathf.Clamp(GimbalRotation.y, -90f, 90f);

			PositionalLagStrength = Mathf.Clamp(PositionalLagStrength, Vector3.kEpsilon, 1f);
			RotationalLagStrength = Mathf.Clamp(RotationalLagStrength, Vector3.kEpsilon, 1f);

			AdvancedCollisionActivationDistance = Mathf.Clamp(AdvancedCollisionActivationDistance, MinMaxDistance.x, MinMaxDistance.y);
		}

		/// <summary></summary>
		/// <decorations decor="protected virtual void"></decorations>
		protected virtual void OnDrawGizmosSelected()
		{
			if (Boom && Target)
				Debug.DrawLine(GetTargetPosition(), Boom.position, Color.red);
		}
	}
}