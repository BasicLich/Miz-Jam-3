using System.Collections;
using UnityEngine;

namespace MizJam
{
    public class SelfDestructTimer : MonoBehaviour
    {
        public float timeToDestroy = 10.0f;

        private void Start()
        {
            Destroy(this.gameObject, this.timeToDestroy);
        }
    }
}