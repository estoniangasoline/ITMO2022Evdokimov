using UnityEngine;

namespace Ubavar.core.Utils
{
    public class Billboard : MonoBehaviour
    {
        private Camera camera;

        private void Awake()
        {
            camera = Camera.main;
        }

        private void LateUpdate()
        {
            Quaternion cameraRotation = camera.transform.rotation;
            transform.LookAt(transform.position + cameraRotation * Vector3.forward,
                cameraRotation * Vector3.up);
        }
    }
}