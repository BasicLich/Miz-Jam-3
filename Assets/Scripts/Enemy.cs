using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace MizJam
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private GameObject projectilePrefab;

        [SerializeField]
        private float shootInterval = 3f;
        private float countdown = 0f;

        private bool isDead = false;

        private Rigidbody rb;
        private Animator animator;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (!isDead)
            {
                countdown += Time.fixedDeltaTime;
                if (countdown >= shootInterval)
                {
                    animator.SetTrigger("ShootAtPlayer");
                    //ThrowProjectileAtPlayer();
                    countdown = 0;
                }
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

        public void SufferImpact(Vector3 point)
        {
            //TODO: Death sound
            animator.enabled = false;
            isDead = true;
            rb.useGravity = true;
            rb.AddExplosionForce(800f, point, 5f, 2f, ForceMode.Force);

            //deformations to animate death
            transform.DOScaleY(-0.8f, 0.3f).OnComplete(() => {
                transform.DOShakeScale(2, new Vector3(0.5f, 0.5f, 0), 10, 50, true).OnComplete(() =>
                {
                    Destroy(this.gameObject);
                });
            });
        }
    }
}
