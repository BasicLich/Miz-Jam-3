using UnityEngine;

namespace MizJam
{
    public class Billboard : MonoBehaviour
    {
        public bool fixedY = true;

        private Camera cam;

        private void Awake()
        {
            this.cam = Camera.main;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            Vector3 lookAt = this.cam.transform.position;
            
            if (this.fixedY)
                lookAt.y = this.transform.position.y;
            
            this.transform.LookAt(lookAt);
        }
    }
}