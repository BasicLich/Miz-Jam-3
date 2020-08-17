using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MizJam
{
    [RequireComponent(typeof(Collider))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float damage = 50f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<Player>().TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
    }
}
