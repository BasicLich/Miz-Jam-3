﻿using DG.Tweening;
using MizJam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MizJam
{
    [RequireComponent(typeof(Animator))]
    public class Enemy_Kamikaze : MonoBehaviour
    {
        private Animator animator;
        private bool isChasingPlayer;
        private bool isExploding;
        private NavMeshAgent navMeshAgent;
        private Rigidbody rb;

        [SerializeField]
        private float explosionRadius = 2f, explosionDamage = 10f, seeRange = 20f;
        [SerializeField]
        private LayerMask explodeOn;
        [SerializeField]
        private GameObject explosionPrefab;


        void Start()
        {
            animator = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (isExploding)
                return;

            if (!isChasingPlayer)
            {
                if (SeesPlayer())
                {
                    isChasingPlayer = true;
                    animator.speed = 3;
                    BalloonsManager.Instance.SpawnBalloonAt(BalloonEmotionsEnum.ALERT, transform);
                }
            }
            else
            {
                ChasePlayer();
                CheckIfInExplodeRange();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, seeRange);
        }

        private void Explode(Player player)
        {
            player.TakeDamage(explosionDamage);

            isExploding = true;
            animator.enabled = false;
            navMeshAgent.enabled = false;
            rb.useGravity = true;

            //Explode FX
            Instantiate(explosionPrefab).transform.position = transform.position;

            //Death animations
            Destroy(transform.Find("Body").gameObject);
            rb.AddExplosionForce(800f, transform.position + transform.forward, 5f, 0.5f, ForceMode.Force);
            transform.DOScaleY(-0.8f, 0.3f).OnComplete(() => {
                transform.DOShakeScale(2, new Vector3(0.5f, 0.5f, 0), 10, 50, true).OnComplete(() =>
                {
                    Destroy(this.gameObject);
                });
            });
        }

        private void CheckIfInExplodeRange()
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, explodeOn);
            foreach(var col in cols)
            {
                if (col.transform.root.tag == "Player")
                    Explode(col.transform.root.GetComponent<Player>());
            }
        }

        private bool SeesPlayer()
        {
            if ((Camera.main.transform.position - transform.position).magnitude > seeRange)
                return false;

            RaycastHit raycastHit;
            Physics.Linecast(transform.position, Camera.main.transform.position, out raycastHit);
            return (raycastHit.transform.root.tag == "Player");
        }

        private void ChasePlayer()
        {
            Vector3 target = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
            navMeshAgent.SetDestination(target);
        }
    }
}

