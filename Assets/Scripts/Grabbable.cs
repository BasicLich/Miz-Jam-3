using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MizJam
{
    [RequireComponent(typeof(Rigidbody))]
    public class Grabbable : MonoBehaviour
    {
        [SerializeField]
        private float explosionRadius = 5.0f;

        [SerializeField]
        private float minImpactVelocity = 1.0f;

        [SerializeField]
        private float maxImpactVelocity = 5.0f;

        [SerializeField]
        private LayerMask enemies;

        [SerializeField]
        private GameObject explosionEffectPrefab;

        private new Rigidbody rigidbody;
        private bool beeingThrown = false;


        private void Update()
        {
            if (this.rigidbody.IsSleeping())
                this.beeingThrown = false;
        }

        private void Awake()
        {
            this.rigidbody = this.GetComponent<Rigidbody>(); 
        }

        public void GetGrabbed()
        {
            this.rigidbody.detectCollisions = false;
            this.rigidbody.isKinematic = true;
        }

        public void GetThrown(Vector3 force)
        {
            this.beeingThrown = true;
            this.rigidbody.isKinematic = false;
            this.rigidbody.detectCollisions = true;
            this.rigidbody.AddForce(force, ForceMode.Impulse);
        }

        private void Explode()
        {
            IEnumerable<Enemy> inRange = Physics.OverlapSphere(this.transform.position, this.explosionRadius, this.enemies).Select(el => el.GetComponent<Enemy>()).Where(el => el != null);
            foreach (Enemy enemy in inRange)
                enemy.SufferImpact(this.transform.position);

            Boss bossInRange = Physics.OverlapSphere(this.transform.position, this.explosionRadius, this.enemies)?.Select(el => el.GetComponent<Boss>()).FirstOrDefault();
            bossInRange?.SufferImpact(this.transform.position, 10f);

            Destroy(this);
            this.GetComponentInChildren<Renderer>().material.DOFade(0.0f, "_BaseColor", 1.0f);
            Instantiate(this.explosionEffectPrefab, this.transform.position, this.transform.rotation);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (this.beeingThrown)
            {
                Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
                enemy?.SufferImpact(this.transform.position);

                Boss boss = collision.gameObject.GetComponentInParent<Boss>();
                boss?.SufferImpact(this.transform.position, 10f);
            }

            if (collision.relativeVelocity.magnitude > this.maxImpactVelocity)
                this.Explode();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(this.transform.position, this.explosionRadius);
        }
#endif
    }
}