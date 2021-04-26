using UnityEngine;

namespace MW.MWCamera {


    public static class MWCamera {
        /// <summary>Have the camera follow target's transform.</summary>
        /// <param name="camera">The camera to move.</param>
        /// <param name="target">The target's transform component.</param>
        public static void CameraFollow(Camera camera, Transform target) {
            camera.transform.position += target.position;
        }

        /// <summary>Have the camera to follow target's transform at an offset.</summary>
        /// <param name="camera">The camera to move.</param>
        /// <param name="target">The target's transform component.</param>
        /// <param name="offset">The target's position at an offset.</param>
        public static void CameraFollow(Camera camera, Transform target, Vector3 offset) {
            camera.transform.position += target.position + offset;
        }

        /// <summary>Have the camera follow target's position.</summary>
        /// <param name="camera">The camera to move.</param>
        /// <param name="target">The target's position to follow.</param>
        public static void CameraFollow(Camera camera, Vector3 target) {
            camera.transform.position += target;
        }

        /// <summary>Have the camera follow target's position at an offset.</summary>
        /// <param name="camera">The camera to move.</param>
        /// <param name="target">The target's position to follow.</param>
        /// <param name="offset">The target's position at an offset.</param>
        public static void CameraFollow(Camera camera, Vector3 target, Vector3 offset) {
            camera.transform.position += target + offset;
        }

        /// <summary>Have the main camera follow target's transform.</summary>
        public static void CameraFollow(Transform target) {
            Camera.main.transform.position = target.position;
        }

        /// <summary>Have the main camera follow target's transform at an offset.</summary>
        /// <param name="target">The target's transform component.</param>
        /// <param name="offset">The target's position at an offset.</param>
        public static void CameraFollow(Transform target, Vector3 offset) {
            Camera.main.transform.position = target.position + offset;
        }

        /// <summary>Have the main camera follow target's position.</summary>
        public static void CameraFollow(Vector3 target) {
            Camera.main.transform.position = target;
        }

        /// <summary>Have the main camera follow target's position at an offset.</summary>
        /// <param name="target">The target's position.</param>
        /// <param name="offset">The target's position at an offset.</param>
        public static void CameraFollow(Vector3 target, Vector3 offset) {
            Camera.main.transform.position = target + offset;
        }

        /// <summary>Ensures the transform always faces the main camera.</summary>
        /// <param name="self">The transform to look towards the main camera.</param>
        public static void Billboard(Transform self) {
            self.LookAt(self.position + Camera.main.transform.rotation * Vector3.forward, Vector3.up);
        }

        /// <summary>Ensures the transform always faces camera.</summary>
        /// <param name="self">The transform to look towards the camera.</param>
        /// <param name="camera">The camera to look at.</param>
        public static void Billboard(Transform self, Camera camera) {
            self.LookAt(self.position + camera.transform.rotation * Vector3.forward, Vector3.up);
        }

        /// <summary>Ensures the transform always faces point.</summary>
        /// <param name="self">The transform to look towards the point.</param>
        /// <param name="point">The transform of where self needs to look towards</param>
        public static void Billboard(Transform self, Transform point) {
            self.LookAt(point);
        }

        /// <summary>Ensures the transform always faces point.</summary>
        /// <param name="self">The transform to look towards the point.</param>
        /// <param name="point">The point in world coordinates.</param>
        public static void Billboard(Transform self, Vector3 point) {
            self.LookAt(point);
        }
    }
}
