using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MizJam
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private GameObject projectilePrefab;

        [SerializeField]
        private float shootInterval = 3f;
        private float countdown = 0f;

        void Update()
        {
            countdown += Time.fixedDeltaTime;
            if (countdown >= shootInterval)
            {
                ThrowProjectileAtPlayer();
                countdown = 0;
            }
        }

        void ThrowProjectileAtPlayer()
        {
            Vector3 dir = (Camera.main.transform.position - transform.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = transform.position;
            projectile.GetComponent<Projectile>().SetSource(this);
            projectile.GetComponent<Rigidbody>().AddForceAtPosition(dir * 3000, Vector3.zero);
        }
    }
}
