using UnityEngine;

namespace MizJam
{
    public class FPSCamera : MonoBehaviour
    {
        public float sensitivity = 100.0f;
        public Camera camera;


        private float xRotation = 0.0f;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            Vector2 mouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * this.sensitivity * Time.deltaTime;

            this.xRotation -= mouse.y;
            this.xRotation = Mathf.Clamp(this.xRotation, -90.0f, 90.0f);

            this.transform.Rotate(Vector3.up, mouse.x);
            this.camera.transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
        }
    }
}