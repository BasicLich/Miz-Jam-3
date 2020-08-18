using System.Collections;
using UnityEngine;

namespace MizJam
{
    public class SelfDestructTimer : MonoBehaviour
    {
        public float timeToDestroy = 10.0f;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(this.timeToDestroy);
            Destroy(this.gameObject);
        }
    }
}