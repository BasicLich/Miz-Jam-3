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
        private LayerMask enemies;

        private new Rigidbody rigidbody;
        private bool beeingThrown = false;


        private void Update()
        {
            this.beeingThrown = this.rigidbody.velocity.magnitude >= 1.0f;
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
            this.rigidbody.isKinematic = false;
            this.rigidbody.detectCollisions = true;
            this.rigidbody.AddForce(force, ForceMode.Impulse);
            this.beeingThrown = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (this.beeingThrown)
            {
                IEnumerable<Enemy> inRange = Physics.OverlapSphere(this.transform.position, this.explosionRadius, this.enemies).Select(el => el.GetComponent<Enemy>()).Where(el => el != null);
                foreach (Enemy enemy in inRange)
                    enemy.SufferImpact(this.transform.position);
            }
        }
    }
}