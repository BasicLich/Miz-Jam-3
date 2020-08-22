using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MizJam
{
    public class Boss : MonoBehaviour
    {
        [SerializeField]
        private Slider healthBar;

        public float health;
        private float initialHealth;
        private Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            initialHealth = health;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
            healthBar.value = health / initialHealth;

            if (health < initialHealth / 2)
            {
                animator.SetBool("isEnraged", true);
            }

            if(health <= 0)
            {
                Die();
            }
        }

        public void SufferImpact(Vector3 impactPoint, float dmg)
        {
            TakeDamage(dmg);
        }

        private void Die()
        {
            animator.SetTrigger("DidDie");
        }
    }
}
