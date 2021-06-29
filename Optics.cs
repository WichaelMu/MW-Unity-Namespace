using UnityEngine;
using MW.IO;

namespace MW.Optics {


    public static class Tracking {
        /// <summary>Have the camera follow target's transform.</summary>
        /// <param name="CCamera">The camera to move.</param>
        /// <param name="ATarget">The target's transform component.</param>
        public static void CameraFollow(Camera CCamera, Transform ATarget) {
            CCamera.transform.position += ATarget.position;
        }

        /// <summary>Have the camera to follow target's transform at an offset.</summary>
        /// <param name="CCamera">The camera to move.</param>
        /// <param name="ATarget">The target's transform component.</param>
        /// <param name="vOffset">The target's position at an offset.</param>
        public static void CameraFollow(Camera CCamera, Transform ATarget, Vector3 vOffset) {
            CCamera.transform.position += ATarget.position + vOffset;
        }

        /// <summary>Have the camera follow target's position.</summary>
        /// <param name="CCamera">The camera to move.</param>
        /// <param name="vTarget">The target's position to follow.</param>
        public static void CameraFollow(Camera CCamera, Vector3 vTarget) {
            CCamera.transform.position += vTarget;
        }

        /// <summary>Have the camera follow target's position at an offset.</summary>
        /// <param name="CCamera">The camera to move.</param>
        /// <param name="vTarget">The target's position to follow.</param>
        /// <param name="vOffset">The target's position at an offset.</param>
        public static void CameraFollow(Camera CCamera, Vector3 vTarget, Vector3 vOffset) {
            CCamera.transform.position += vTarget + vOffset;
        }

        /// <summary>Have the main camera follow target's transform.</summary>
        public static void CameraFollow(Transform ATarget) {
            Camera.main.transform.position = ATarget.position;
        }

        /// <summary>Have the main camera follow target's transform at an offset.</summary>
        /// <param name="ATarget">The target's transform component.</param>
        /// <param name="vOffset">The target's position at an offset.</param>
        public static void CameraFollow(Transform ATarget, Vector3 vOffset) {
            Camera.main.transform.position = ATarget.position + vOffset;
        }

        /// <summary>Have the main camera follow target's position.</summary>
        public static void CameraFollow(Vector3 vTarget) {
            Camera.main.transform.position = vTarget;
        }

        /// <summary>Have the main camera follow target's position at an offset.</summary>
        /// <param name="vTarget">The target's position.</param>
        /// <param name="vOffset">The target's position at an offset.</param>
        public static void CameraFollow(Vector3 vTarget, Vector3 vOffset) {
            Camera.main.transform.position = vTarget + vOffset;
        }

        /// <summary>Ensures the transform always faces the main camera.</summary>
        /// <param name="ASelf">The transform to look towards the main camera.</param>
        public static void Billboard(Transform ASelf) {
            ASelf.LookAt(ASelf.position + Camera.main.transform.rotation * Vector3.forward, Vector3.up);
        }

        /// <summary>Ensures the transform always faces camera.</summary>
        /// <param name="ASelf">The transform to look towards the camera.</param>
        /// <param name="CCamera">The camera to look at.</param>
        public static void Billboard(Transform ASelf, Camera CCamera) {
            ASelf.LookAt(ASelf.position + CCamera.transform.rotation * Vector3.forward, Vector3.up);
        }

        /// <summary>Ensures the transform always faces point.</summary>
        /// <param name="ASelf">The transform to look towards the point.</param>
        /// <param name="APoint">The transform of where self needs to look towards</param>
        public static void Billboard(Transform ASelf, Transform APoint) {
            ASelf.LookAt(APoint);
        }

        /// <summary>Ensures the transform always faces point.</summary>
        /// <param name="ASelf">The transform to look towards the point.</param>
        /// <param name="vPoint">The point in world coordinates.</param>
        public static void Billboard(Transform ASelf, Vector3 vPoint) {
            ASelf.LookAt(vPoint);
        }
    }

    public static class Orthographic {
        /// <summary>Fires a ray from CCamera to mouse position.</summary>
        /// <returns>Pair::First RaycastHit2D information about the Raycast. Pair::Second if the ray hit something.</returns>
        public static TPair<RaycastHit2D, bool> Raycast(Camera CCamera) {
            Ray r = CCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(r.origin, r.direction, 50);

            return new TPair<RaycastHit2D, bool>(hit, hit.collider != null);
        }

        public static Plane PPlane = new Plane(-Vector3.forward, Vector3.zero);
        static Vector3 vStartDrag;
        static Vector3 vEndDrag;
        static Vector3 vDragPos = Vector3.zero;

        public static void Pan(Camera CCamera, EButton BButtonToActivate, float fInterpolateSpeed) {
            if (Mouse.Click(BButtonToActivate)) {
                Ray ray = CCamera.ScreenPointToRay(Input.mousePosition);

                if (PPlane.Raycast(ray, out float intersect)) {
                    vStartDrag = ray.GetPoint(intersect);
                }
            }

            if (Mouse.Click(BButtonToActivate, true)) {
                Ray ray = CCamera.ScreenPointToRay(Input.mousePosition);

                if (PPlane.Raycast(ray, out float intersect)) {
                    vEndDrag = ray.GetPoint(intersect);

                    vDragPos = CCamera.transform.position + vStartDrag - vEndDrag;
                }
            }

            if (vDragPos != Vector3.zero)
                CCamera.transform.position = Vector3.Lerp(CCamera.transform.position, vDragPos, fInterpolateSpeed * Time.deltaTime);
        }
    }

}
