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

        private Enemy source;

        private void Start()
        {
            Destroy(this.gameObject, 4f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<Player>().TakeDamage(damage);
                HUDManager.instance.DisplayHitDirection(source);
                Destroy(this.gameObject);
            }
        }

        public void SetSource(Enemy enemy)
        {
            this.source = enemy;
        }
    }
}
