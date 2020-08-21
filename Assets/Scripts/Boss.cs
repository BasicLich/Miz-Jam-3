using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MizJam
{
    public class Boss : MonoBehaviour
    {
        public float health;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void TakeDamage(float damage)
        {
            health -= damage;

            if(health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {

        }
    }
}
