using UnityEngine;

namespace MW.CameraUtils
{
	/// <summary>Utilities for a Camera following or looking at something.</summary>
	/// <decorations decor="public static class"></decorations>
	public static class Tracking
	{
		/// <summary>Have the ReferenceCamera follow Target.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="ReferenceCamera">The camera to move.</param>
		/// <param name="Target">The target's transform component.</param>
		public static void CameraFollow(Camera ReferenceCamera, Transform Target)
		{
			ReferenceCamera.transform.position += Target.position;
		}

		/// <summary>Have the ReferenceCamera to follow Target at an offset.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="ReferenceCamera">The camera to move.</param>
		/// <param name="Target">The target's transform component.</param>
		/// <param name="Offset">The target's position at an offset.</param>
		public static void CameraFollow(Camera ReferenceCamera, Transform Target, Vector3 Offset)
		{
			ReferenceCamera.transform.position += Target.position + Offset;
		}

		/// <summary>Have the ReferenceCamera follow Target.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="ReferenceCamera">The camera to move.</param>
		/// <param name="Target">The target's position to follow.</param>
		public static void CameraFollow(Camera ReferenceCamera, Vector3 Target)
		{
			ReferenceCamera.transform.position += Target;
		}

		/// <summary>Have the ReferenceCamera follow Target at an offset.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="ReferenceCamera">The camera to move.</param>
		/// <param name="Target">The target's position to follow.</param>
		/// <param name="Offset">The target's position at an offset.</param>
		public static void CameraFollow(Camera ReferenceCamera, Vector3 Target, Vector3 Offset)
		{
			ReferenceCamera.transform.position += Target + Offset;
		}

		/// <summary>Have the main camera follow Target.</summary>
		/// <decorations decor="public static void"></decorations>
		public static void CameraFollow(Transform Target)
		{
			Camera.main.transform.position = Target.position;
		}

		/// <summary>Have <see cref="Camera.main"/> follow Target at an offset.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <docs>Have the main camera follow target's transform at an offset.</docs>
		/// <param name="Target">The target's transform component.</param>
		/// <param name="Offset">The target's position at an offset.</param>
		public static void CameraFollow(Transform Target, Vector3 Offset)
		{
			Camera.main.transform.position = Target.position + Offset;
		}

		/// <summary>Have <see cref="Camera.main"/> follow Target.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <docs>Have the main camera follow Target.</docs>
		public static void CameraFollow(Vector3 Target)
		{
			Camera.main.transform.position = Target;
		}

		/// <summary>Have the <see cref="Camera.main"/> follow Target at an offset.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <docs>Have the main camera follow Target at an offset.</docs>
		/// <param name="Target">The target's position.</param>
		/// <param name="Offset">The target's position at an offset.</param>
		public static void CameraFollow(Vector3 Target, Vector3 Offset)
		{
			Camera.main.transform.position = Target + Offset;
		}

		/// <summary>Ensures the Self always faces the <see cref="Camera.main"/>.</summary>
		/// <docs>Ensures the Self always faces the main camera.</docs>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Self">The transform to look towards the main camera.</param>
		public static void Billboard(Transform Self)
		{
			Self.LookAt(Self.position + Camera.main.transform.rotation * Vector3.forward, Vector3.up);
		}

		/// <summary>Ensures the Self always faces ReferenceCamera.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Self">The transform to look towards the camera.</param>
		/// <param name="ReferenceCamera">The camera to look at.</param>
		public static void Billboard(Transform Self, Camera ReferenceCamera)
		{
			Self.LookAt(Self.position + ReferenceCamera.transform.rotation * Vector3.forward, Vector3.up);
		}

		/// <summary>Ensures the Self always faces Point.</summary>
		/// <decorations decor="[Obsolete] public static void"></decorations>
		/// <param name="Self">The transform to look towards the point.</param>
		/// <param name="Point">The transform of where self needs to look towards</param>
		[System.Obsolete("This is obsolete, just use Self.LookAt(Point)")]
		public static void Billboard(Transform Self, Transform Point)
		{
			Self.LookAt(Point);
		}

		/// <summary>Ensures the Self always faces Point.</summary>
		/// <decorations decor="[Obsolete] public static void"></decorations>
		/// <param name="Self">The transform to look towards the point.</param>
		/// <param name="Point">The point in world coordinates.</param>
		[System.Obsolete("This is obsolete, just use Self.LookAt(Point)")]
		public static void Billboard(Transform Self, Vector3 Point)
		{
			Self.LookAt(Point);
		}
	}
}
