using UnityEngine;

namespace MizJam
{
    public class Billboard : MonoBehaviour
    {
        public bool fixedY = true;

        // Update is called once per frame
        void LateUpdate()
        {
            Vector3 lookAt = Camera.main.transform.position;
            
            if (this.fixedY)
                lookAt.y = this.transform.position.y;
            
            this.transform.LookAt(lookAt);
        }
    }
}