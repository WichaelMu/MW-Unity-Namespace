using UnityEngine;

namespace MW.CameraUtils
{
	/// <summary>Utilities for a Camera following or looking at something.</summary>
	public static class Tracking
	{
		/// <summary>Have the camera follow target's transform.</summary>
		/// <param name="ReferenceCamera">The camera to move.</param>
		/// <param name="Target">The target's transform component.</param>
		public static void CameraFollow(Camera ReferenceCamera, Transform Target)
		{
			ReferenceCamera.transform.position += Target.position;
		}

		/// <summary>Have the camera to follow target's transform at an offset.</summary>
		/// <param name="ReferenceCamera">The camera to move.</param>
		/// <param name="Target">The target's transform component.</param>
		/// <param name="Offset">The target's position at an offset.</param>
		public static void CameraFollow(Camera ReferenceCamera, Transform Target, Vector3 Offset)
		{
			ReferenceCamera.transform.position += Target.position + Offset;
		}

		/// <summary>Have the camera follow target's position.</summary>
		/// <param name="ReferenceCamera">The camera to move.</param>
		/// <param name="Target">The target's position to follow.</param>
		public static void CameraFollow(Camera ReferenceCamera, Vector3 Target)
		{
			ReferenceCamera.transform.position += Target;
		}

		/// <summary>Have the camera follow target's position at an offset.</summary>
		/// <param name="ReferenceCamera">The camera to move.</param>
		/// <param name="Target">The target's position to follow.</param>
		/// <param name="Offset">The target's position at an offset.</param>
		public static void CameraFollow(Camera ReferenceCamera, Vector3 Target, Vector3 Offset)
		{
			ReferenceCamera.transform.position += Target + Offset;
		}

		/// <summary>Have the main camera follow target's transform.</summary>
		public static void CameraFollow(Transform Target)
		{
			Camera.main.transform.position = Target.position;
		}

		/// <summary>Have the main camera follow target's transform at an offset.</summary>
		/// <param name="Target">The target's transform component.</param>
		/// <param name="Offset">The target's position at an offset.</param>
		public static void CameraFollow(Transform Target, Vector3 Offset)
		{
			Camera.main.transform.position = Target.position + Offset;
		}

		/// <summary>Have the main camera follow target's position.</summary>
		public static void CameraFollow(Vector3 Target)
		{
			Camera.main.transform.position = Target;
		}

		/// <summary>Have the main camera follow target's position at an offset.</summary>
		/// <param name="Target">The target's position.</param>
		/// <param name="Offset">The target's position at an offset.</param>
		public static void CameraFollow(Vector3 Target, Vector3 Offset)
		{
			Camera.main.transform.position = Target + Offset;
		}

		/// <summary>Ensures the transform always faces the main camera.</summary>
		/// <param name="Self">The transform to look towards the main camera.</param>
		public static void Billboard(Transform Self)
		{
			Self.LookAt(Self.position + Camera.main.transform.rotation * Vector3.forward, Vector3.up);
		}

		/// <summary>Ensures the transform always faces camera.</summary>
		/// <param name="Self">The transform to look towards the camera.</param>
		/// <param name="ReferenceCamera">The camera to look at.</param>
		public static void Billboard(Transform Self, Camera ReferenceCamera)
		{
			Self.LookAt(Self.position + ReferenceCamera.transform.rotation * Vector3.forward, Vector3.up);
		}

		/// <summary>Ensures the transform always faces point.</summary>
		/// <param name="Self">The transform to look towards the point.</param>
		/// <param name="Point">The transform of where self needs to look towards</param>
		public static void Billboard(Transform Self, Transform Point)
		{
			Self.LookAt(Point);
		}

		/// <summary>Ensures the transform always faces point.</summary>
		/// <param name="Self">The transform to look towards the point.</param>
		/// <param name="Point">The point in world coordinates.</param>
		public static void Billboard(Transform Self, Vector3 Point)
		{
			Self.LookAt(Point);
		}
	}
}
