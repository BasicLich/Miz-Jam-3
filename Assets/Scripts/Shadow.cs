using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MizJam
{
    public class Shadow : MonoBehaviour
    {
        [SerializeField]
        private Transform shadowCaster;

        [SerializeField]
        private LayerMask layerMask;

        void LateUpdate()
        {
            Ray ray = new Ray(this.shadowCaster.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                this.transform.position = hit.point + 0.01f * hit.normal;
                this.transform.up = hit.normal;
            }
        }
    }
}