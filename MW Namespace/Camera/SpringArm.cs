using UnityEngine;

namespace MW.CameraUtils
{
	/// <summary>A Spring Arm Component that allows <see cref="Boom"/> to track a <see cref="Target"/>.</summary>
	/// <docs>A Spring Arm Component that allows a Boom to track a Target.</docs>
	/// <remarks>While commonly used as for Camera tracking, this can be used for any object that needs to track <see cref="Target"/>.</remarks>
	public class SpringArm : MonoBehaviour
	{
		[Header("Debug Options.")]
		[SerializeField] bool bDrawRotationalLines;

		/// <summary>The <see cref="Transform"/> that will track <see cref="Boom"/>.</summary>
		/// <docs>The Transform that will track Boom.</docs>
		[Space(10), Header("Target Settings.")]
		public Transform Boom;
		/// <summary>The Target for <see cref="Boom"/> to track.</summary>
		/// <docs>The Target for Boom to track.</docs>
		public Transform Target;
		/// <summary>The offset at which Boom will track Target.</summary>
		[SerializeField, Tooltip("The offset at which Boom will track Target.")] Vector3 TargetOffset;
		Camera MainCamera;

		/// <summary>The distance in which Boom will maintain from Target whilst tracking.</summary>
		/// <remarks>Will not be used when the line between <see cref="Boom"/> and <see cref="Target"/> is interrupted by a <see cref="Collider"/> collision.</remarks>
		/// <docremarks>Will not be used when the line between Boom and Target is interrupted by a collision.</docremarks>
		[Header("Spring Arm Settings.")]
		public float Distance;
		/// <summary>The world-rotation of <see cref="Boom"/>, about <see cref="Target"/> + <see cref="TargetOffset"/>.</summary>
		/// <docs>The world-rotation of Boom, about Target + TargetOffset.</docs>
		public Vector3 GimbalRotation;
		/// <summary>The world-rotation of <see cref="Boom "/> as it looks and faces <see cref="Target"/>.</summary>
		/// <docs>The world-rotation of Boom as it looks and faces Target.</docs>
		public Vector3 CameraRotation;
		/// <summary><see langword="true"></see> to inherit <see cref="Target"/>'s rotation.</summary>
		/// <docs>True to inherit Target's rotation.</docs>
		public bool bInheritRotation;
		/// <summary><see langword="true"></see> to let the mouse wheel control <see cref="Distance"/>.</summary>
		[Space(5)] public bool bEnableScrollToDistance;
		/// <summary>How much will <see cref="Distance"/> change when scrolling?</summary>
		/// <docs>How much will Distance change when scrolling?</docs>
		public float ScrollSensitivity;
		[HideInInspector, SerializeField] Vector3 DefaultGimbalRotation;
		[HideInInspector, SerializeField] Vector3 DefaultCameraRotation;
		float OrbitSensitivity = 1f;
		Vector2 PreviousMouseDragPosition;
		Vector3 GimbalRotationInherited;
		Vector3 CameraRotationInherited;
		Vector2 PreviousMousePanPosition;
		Vector3 OriginalTargetOffset;

		/// <summary><see langword="true"></see> to invert the Controls when Orbiting about the world X-Axis.</summary>
		[Header("Inverse Settings.")]
		public bool bInvertX; // Inverse LR dragging Orbit Controls.
		/// <summary><see langword="true"></see> to invert the Controls when Orbiting about the world Y-Axis.</summary>
		public bool bInvertY; // Inverse UD dragging Orbit Controls.
		/// <summary><see langword="true"></see> to invert the Zooming Distance direction.</summary>
		public bool bInvertZ; // Inverse Zoom Controls.

		/// <summary>Interrupt Distance when a collision is made with this layer/s.</summary>
		[Header("Collisions")]
		[SerializeField, Tooltip("Interrupt the Spring Arm's Distance when a collision is made with this layer/s.")] LayerMask OnlyCollideWith;

		/// <summary>The strength Boom will Lag behind tracking Target.</summary>
		/// <remarks>0 is standstill (infinite Lag). 1 is instant (no Lag).</remarks>
		[Header("Lag Settings")]
		[SerializeField] float PositionalLagStrength = .2f;
		/// <summary>The strength Boom will rotate to face Target.</summary>
		/// <remarks>0 is standstill (infinite Lag). 1 is instant (no Lag).</remarks>
		[SerializeField] float RotationalLagStrength = .2f;
		Vector3 TargetPosition;
		Quaternion TargetRotation;

		/// <summary><see langword="true"/> to use a Camera Projection that cuts away close <see cref="Mesh"/>es.</summary>
		/// <docs>True to use a Camera Projection that cuts away close Meshes.</docs>
		[Header("Projection Settings. Where Boom is a Camera")]
		public bool bUseCustomProjection;
		/// <summary>The distance when <see cref="Mesh"/>es are clipped.</summary>
		/// <docs>The distance when Meshes are clipped.</docs>
		[SerializeField, Tooltip("The distance when Meshes are clipped.")] float NearClipDistance;
		/// <summary>How close should the Camera be to stop using this Custom Projection?</summary>
		[SerializeField, Tooltip("How close should the Camera be to stop using this Custom Projection?")] float DistanceLimit;
		Matrix4x4 DefaultProjection;

		void Start()
		{

			DefaultGimbalRotation = GimbalRotation;
			DefaultCameraRotation = CameraRotation;

			GimbalRotationInherited = DefaultGimbalRotation;
			CameraRotationInherited = DefaultCameraRotation;

			OriginalTargetOffset = TargetOffset;

			MainCamera = UnityEngine.Camera.main;
			DefaultProjection = MainCamera.projectionMatrix;
		}

		void Update()
		{
			UpdateRotationOnMouse();
			PanCameraOnMouse();

			ScrollDistance();
		}

		void FixedUpdate()
		{
			Boom.position = Vector3.Lerp(Boom.position, TargetPosition, PositionalLagStrength);
			Boom.rotation = Quaternion.Slerp(Boom.rotation, TargetRotation, RotationalLagStrength);

			PlaceCamera();
		}

		void OnPreCull()
		{
			ComputeProjection();
		}

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
				// Rotates the Camera around Target, given the Gimbal Rotation's Pitch (Y).
				// As a side-effect, this also inherits the Yaw.
				Quaternion InheritRotation = Quaternion.AngleAxis(GimbalRotationInherited.y, Target.right);
				ArmDirection = (InheritRotation * Target.forward).normalized;

				FinalRotation = GetInheritedRotation();
			}

			// If the Spring Arm will collider with something:
			if (RunCollisionsCheck(ref ArmDirection))
				return;

			// Make the Position and Rotation for Lag.
			FinalPosition = TargetPos() - (Distance * ArmDirection);

			SetPositionAndRotation(FinalPosition, FinalRotation);
		}

		bool RunCollisionsCheck(ref Vector3 Direction)
		{
			if (bUseCustomProjection)
				return false;

			Vector3 TP = TargetPos();
			Ray FOV = new Ray(TP, -Direction);
			bool bViewToTargetBlocked = Physics.Raycast(FOV, out RaycastHit Hit, Distance, OnlyCollideWith);

			if (bViewToTargetBlocked)
			{
				Vector3 Point = Hit.point - FOV.direction;
				SetPositionAndRotation(Point, bInheritRotation
					? GetInheritedRotation()
					: Quaternion.Euler(CameraRotation));
			}

			return bViewToTargetBlocked;
		}

		void SetPositionAndRotation(Vector3 FinalPosition, Quaternion FinalRotation)
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

		Vector3 TargetPos() => Target.position + TargetOffset * Target.up.y;

		Quaternion GetInheritedRotation()
		{
			return Quaternion.Euler(new Vector3(GetInheritedAxis(Target.localEulerAngles.x) + GimbalRotationInherited.y - CameraRotationInherited.x, CameraRotationInherited.y + GetInheritedAxis(Target.localEulerAngles.y)));
		}

		float GetInheritedAxis(float AxisAngle)
		{
			float TargetAxis = AxisAngle;
			if (TargetAxis < 0f)
				TargetAxis = 360f - TargetAxis;
			return TargetAxis;
		}

		void ScrollDistance()
		{
			if (bEnableScrollToDistance)
			{
				Distance += Input.mouseScrollDelta.y * (bInvertZ ? -1f : 1f) * -ScrollSensitivity;

				Distance = Mathf.Clamp(Distance, 1, 30);
			}
		}

		void UpdateRotationOnMouse()
		{
			Vector3 MousePosition = Input.mousePosition;

			if (Input.GetMouseButton(1))
			{
				float DeltaX = (MousePosition.x - PreviousMouseDragPosition.x) * OrbitSensitivity;
				float DeltaY = (MousePosition.y - PreviousMouseDragPosition.y) * OrbitSensitivity;

				DetermineInverse(ref DeltaX, ref DeltaY);

				if (!bInheritRotation)
				{
					GimbalRotation.x += DeltaX;
					CameraRotation.y += DeltaX;

					if (GimbalRotation.y - DeltaY < 70 && GimbalRotation.y - DeltaY >= -70)
					{
						GimbalRotation.y -= DeltaY;
						CameraRotation.x -= DeltaY;
					}
				}
				else
				{
					CameraRotationInherited.y += DeltaX;

					if (GimbalRotationInherited.y - DeltaY < 70 && GimbalRotationInherited.y - DeltaY >= -70)
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

			PreviousMouseDragPosition = MousePosition;
		}

		void DetermineInverse(ref float DeltaX, ref float DeltaY)
		{
			if (bInvertX)
				Inverse(ref DeltaX);
			else if (bInvertY)
				Inverse(ref DeltaY);

			static void Inverse(ref float F) => F *= -1f;
		}


		void PanCameraOnMouse()
		{
			Vector3 MousePosition = Input.mousePosition;

			if (Input.GetMouseButton(2))
			{
				float DeltaX = (MousePosition.x - PreviousMousePanPosition.x) * OrbitSensitivity;
				float DeltaY = (MousePosition.y - PreviousMousePanPosition.y) * OrbitSensitivity;

				// Ensure 'Right' and 'Up' is relative to the Camera.
				TargetOffset -= DeltaX * Time.deltaTime * Boom.right + DeltaY * Time.deltaTime * Boom.up;
				TargetOffset = Vector3.ClampMagnitude(TargetOffset, 5f);
			}
			else
			{
				TargetOffset = Vector3.Lerp(TargetOffset, OriginalTargetOffset, .2f);
			}

			PreviousMousePanPosition = MousePosition;
		}

		void ComputeProjection()
		{
			if (bUseCustomProjection && Distance > 3)
			{
				if (Physics.Linecast(Target.position, Boom.position, out RaycastHit Intercept, 256))
				{
					NearClipDistance = Intercept.distance;
				}
				else
				{
					NearClipDistance = Distance * .5f;
				}

				int Dot = System.Math.Sign(Vector3.Dot(Boom.forward, Target.position - Boom.position));
				Vector3 CameraWorldPosition = MainCamera.worldToCameraMatrix.MultiplyPoint(Target.position);
				Vector3 CameraNormal = MainCamera.worldToCameraMatrix.MultiplyVector(Boom.forward) * Dot;

				float CameraDistance = -Vector3.Dot(CameraWorldPosition, CameraNormal) + NearClipDistance;

				// If the Camera is too close to the Target, don't use oblique projection.
				if (Mathf.Abs(CameraDistance) > DistanceLimit)
				{
					Vector4 clipPlaneCameraSpace = new Vector4(CameraNormal.x, CameraNormal.y, CameraNormal.z, CameraDistance);

					MainCamera.projectionMatrix = MainCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
				}
				else
				{
					MainCamera.projectionMatrix = DefaultProjection;
				}
			}
			else
			{
				MainCamera.projectionMatrix = DefaultProjection;
			}
		}

		void OnValidate()
		{
			if (Boom && Target)
				PlaceCamera();

			GimbalRotation.y = Mathf.Clamp(GimbalRotation.y, -90f, 90f);

			PositionalLagStrength = Mathf.Clamp(PositionalLagStrength, Vector3.kEpsilon, 1f);
			RotationalLagStrength = Mathf.Clamp(RotationalLagStrength, Vector3.kEpsilon, 1f);
		}

		void OnDrawGizmosSelected()
		{
			if (Boom && Target)
				Debug.DrawLine(TargetPos(), Boom.position, Color.red);
		}
	}
}
