using MW.IO;
using UnityEngine;

namespace MW.CameraUtils
{
	/// <summary>Utilities for an Orthographic camera.</summary>
	public static class Orthographic
	{
		const string kCameraIsNotOrthographicError = " is not orthographic";

		/// <summary>Fires a ray from ReferenceCamera to mouse position.</summary>
		/// <param name="ReferenceCamera">The camera to fire a ray from using ScreenPointToRay.</param>
		/// <returns>OrthographicRaycast</returns>
		public static OrthographicRaycast Raycast(Camera ReferenceCamera)
		{
			if (!ReferenceCamera.orthographic) { Diagnostics.Log.P(ReferenceCamera.name + kCameraIsNotOrthographicError); }

			Ray r = ReferenceCamera.ScreenPointToRay(Input.mousePosition);

			RaycastHit2D hit = Physics2D.Raycast(r.origin, r.direction, 50);

			return new OrthographicRaycast(hit, hit.collider != null);
		}

		/// <summary>The Plane to pan a Camera for an Orthographic world.</summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "PPlane is intended to be used and modified outside of MW.sln; inside a Unity project. A programmer might want to change the UnityEngine.Plane if the 2D, orthographic, game requires panning to be performed in a different manner than otherwise provided.")]
		public static Plane Plane = new Plane(-Vector3.forward, Vector3.zero);
		static Vector3 StartDrag;
		static Vector3 EndDrag;
		static Vector3 DragPos = Vector3.zero;

		/// <summary>Pan ReferenceCamera using ButtonToActivate by linearlly interpolating with InterpolateSpeed.</summary>
		/// <param name="ReferenceCamera">The camera to pan.</param>
		/// <param name="ButtonToActivate">The mouse button to start activate panning.</param>
		/// <param name="InterpolateSpeed">The speed to ease the camera's movement.</param>
		public static void Pan(Camera ReferenceCamera, EButton ButtonToActivate, float InterpolateSpeed)
		{
			if (!ReferenceCamera.orthographic) { Diagnostics.Log.P(ReferenceCamera.name + kCameraIsNotOrthographicError); }

			if (I.Click(ButtonToActivate))
			{
				Ray ray = ReferenceCamera.ScreenPointToRay(Input.mousePosition);

				if (Plane.Raycast(ray, out float intersect))
				{
					StartDrag = ray.GetPoint(intersect);
				}
			}

			if (I.Click(ButtonToActivate, true))
			{
				Ray ray = ReferenceCamera.ScreenPointToRay(Input.mousePosition);

				if (Plane.Raycast(ray, out float intersect))
				{
					EndDrag = ray.GetPoint(intersect);

					DragPos = ReferenceCamera.transform.position + StartDrag - EndDrag;
				}
			}

			if (DragPos != Vector3.zero)
				ReferenceCamera.transform.position = Vector3.Lerp(ReferenceCamera.transform.position, DragPos, InterpolateSpeed * Time.deltaTime);
		}
	}

	/// <summary>RaycastHit2D information about the Raycast.</summary>
	public struct OrthographicRaycast
	{
		/// <summary>The RaycastHit2D information about the ray itself.</summary>
		public RaycastHit2D Raycast;
		/// <summary>If raycast hit something.</summary>
		public bool bHit;

		public OrthographicRaycast(RaycastHit2D Raycast, bool bHit)
		{
			this.Raycast = Raycast;
			this.bHit = bHit;
		}
	}
}
