using UnityEngine;

namespace MW.MWCamera {


    public static class MWCamera {
        /// <summary>Have the camera follow target's transform.</summary>
        /// <param name="CCamera">The camera to move.</param>
        /// <param name="TTarget">The target's transform component.</param>
        public static void CameraFollow(Camera CCamera, Transform TTarget) {
            CCamera.transform.position += TTarget.position;
        }

        /// <summary>Have the camera to follow target's transform at an offset.</summary>
        /// <param name="CCamera">The camera to move.</param>
        /// <param name="TTarget">The target's transform component.</param>
        /// <param name="vOffset">The target's position at an offset.</param>
        public static void CameraFollow(Camera CCamera, Transform TTarget, Vector3 vOffset) {
            CCamera.transform.position += TTarget.position + vOffset;
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
        public static void CameraFollow(Transform TTarget) {
            Camera.main.transform.position = TTarget.position;
        }

        /// <summary>Have the main camera follow target's transform at an offset.</summary>
        /// <param name="TTarget">The target's transform component.</param>
        /// <param name="vOffset">The target's position at an offset.</param>
        public static void CameraFollow(Transform TTarget, Vector3 vOffset) {
            Camera.main.transform.position = TTarget.position + vOffset;
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
        /// <param name="TSelf">The transform to look towards the main camera.</param>
        public static void Billboard(Transform TSelf) {
            TSelf.LookAt(TSelf.position + Camera.main.transform.rotation * Vector3.forward, Vector3.up);
        }

        /// <summary>Ensures the transform always faces camera.</summary>
        /// <param name="TSelf">The transform to look towards the camera.</param>
        /// <param name="CCamera">The camera to look at.</param>
        public static void Billboard(Transform TSelf, Camera CCamera) {
            TSelf.LookAt(TSelf.position + CCamera.transform.rotation * Vector3.forward, Vector3.up);
        }

        /// <summary>Ensures the transform always faces point.</summary>
        /// <param name="TSelf">The transform to look towards the point.</param>
        /// <param name="TPoint">The transform of where self needs to look towards</param>
        public static void Billboard(Transform TSelf, Transform TPoint) {
            TSelf.LookAt(TPoint);
        }

        /// <summary>Ensures the transform always faces point.</summary>
        /// <param name="TSelf">The transform to look towards the point.</param>
        /// <param name="vPoint">The point in world coordinates.</param>
        public static void Billboard(Transform TSelf, Vector3 vPoint) {
            TSelf.LookAt(vPoint);
        }
    }
}
