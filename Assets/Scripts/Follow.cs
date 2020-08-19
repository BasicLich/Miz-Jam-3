using UnityEngine;

namespace MizJam
{
    public class Follow : MonoBehaviour
    {
        [SerializeField]
        private Transform follow;

        [SerializeField]
        bool x, y, z;

        private void LateUpdate()
        {
            Vector3 n = this.follow.position;
            Vector3 o = this.transform.position;
            this.transform.position = new Vector3(this.x ? n.x : o.x, this.y ? n.y : o.y, this.z ? n.z : o.z);
        }
    }
}