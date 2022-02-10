using UnityEngine;
using MW.IO;

namespace MW.CameraUtils
{
	public static class Orthographic
	{
		const string kCameraIsNotOrthographicError = " is not orthographic";

		/// <summary>Fires a ray from CCamera to mouse position.</summary>
		/// <returns><see cref="OrthographicRaycast"/></returns>
		public static OrthographicRaycast Raycast(Camera CCamera)
		{
			if (!CCamera.orthographic) { Diagnostics.Log.Print(CCamera.name + kCameraIsNotOrthographicError); }

			Ray r = CCamera.ScreenPointToRay(Input.mousePosition);

			RaycastHit2D hit = Physics2D.Raycast(r.origin, r.direction, 50);

			return new OrthographicRaycast(hit, hit.collider != null);
		}

		/// <summary>RaycastHit2D information about the Raycast.</summary>
		public struct OrthographicRaycast
		{
			/// <summary>The <see cref="RaycastHit2D"/> information about the ray itself.</summary>
			public RaycastHit2D raycast;
			/// <summary>If <see cref="raycast"/> hit something.</summary>
			public bool bHit;

			public OrthographicRaycast(RaycastHit2D raycast, bool bHit)
			{
				this.raycast = raycast;
				this.bHit = bHit;
			}
		}


		/// <summary>The Plane to pan a Camera for an Orthographic world.</summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "PPlane is intended to be used and modified outside of MW.sln; inside a Unity project. A programmer might want to change the UnityEngine.Plane if the 2D, orthographic, game requires panning to be performed in a different manner than otherwise provided.")]
		public static Plane PPlane = new Plane(-Vector3.forward, Vector3.zero);
		static Vector3 vStartDrag;
		static Vector3 vEndDrag;
		static Vector3 vDragPos = Vector3.zero;

		/// <summary>Pan CCamera using BButtonToActivate by linearlly interpolating with fInterpolateSpeed.</summary>
		/// <param name="CCamera">The camera to pan.</param>
		/// <param name="BButtonToActivate">The mouse button to start activate panning.</param>
		/// <param name="fInterpolateSpeed">The speed to ease the camera's movement.</param>
		public static void Pan(Camera CCamera, EButton BButtonToActivate, float fInterpolateSpeed)
		{
			if (!CCamera.orthographic) { Diagnostics.Log.Print(CCamera.name + kCameraIsNotOrthographicError); }

			if (I.Click(BButtonToActivate))
			{
				Ray ray = CCamera.ScreenPointToRay(Input.mousePosition);

				if (PPlane.Raycast(ray, out float intersect))
				{
					vStartDrag = ray.GetPoint(intersect);
				}
			}

			if (I.Click(BButtonToActivate, true))
			{
				Ray ray = CCamera.ScreenPointToRay(Input.mousePosition);

				if (PPlane.Raycast(ray, out float intersect))
				{
					vEndDrag = ray.GetPoint(intersect);

					vDragPos = CCamera.transform.position + vStartDrag - vEndDrag;
				}
			}

			if (vDragPos != Vector3.zero)
				CCamera.transform.position = Vector3.Lerp(CCamera.transform.position, vDragPos, fInterpolateSpeed * Time.deltaTime);
		}
	}

}
