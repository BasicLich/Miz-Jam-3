using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

namespace MizJam
{
    public class Enemy : MonoBehaviour, IImpactable
    {
        [SerializeField]
        private GameObject projectilePrefab;

        [SerializeField]
        private float shootInterval = 3f;
        private float countdown = 0f;
        [SerializeField]
        private float runRange = 10f;

        private bool isDead = false;

        private Rigidbody rb;
        private Animator animator;
        private NavMeshAgent navMeshAgent;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (!isDead)
            {
                countdown += Time.fixedDeltaTime;
                if (countdown >= shootInterval && this.IsInRange())
                {
                    animator.SetTrigger("ShootAtPlayer");
                    countdown = 0;
                }

                KeepDistanceFromPlayer();
            }
        }

        private bool IsInRange()
        {
            return (transform.position - Player.Instance.transform.position).magnitude <= 50.0f;
        }

        private void KeepDistanceFromPlayer()
        {
            Vector3 playerPos = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
            if ((playerPos - transform.position).magnitude < runRange)
            {
                Vector3 dir = (transform.position - Camera.main.transform.position).normalized;
                dir.y = transform.position.y;
                navMeshAgent.destination = transform.position + dir;
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
            navMeshAgent.enabled = false;
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
