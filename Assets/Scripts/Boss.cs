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

        [Header("RangedAttack")]
        [SerializeField]
        private GameObject normalProjectilePrefab;
        [SerializeField]
        private GameObject enragedProjectilePrefab;

        [Header("Summon")]
        [SerializeField]
        private GameObject[] minionsPrefabs;
        [SerializeField]
        private GameObject summonFXPrefab;

        public float health;
        private float initialHealth;
        private Animator animator;
        private Rigidbody rb;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
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

            if (health <= initialHealth / 2)
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
            Knockback(impactPoint);
            TakeDamage(dmg);
        }

        public void Knockback(Vector3 origin)
        {
            rb.AddForce((transform.position - origin).normalized * 500);
            //rb.AddExplosionForce(1000f, origin, 5f, 2f, ForceMode.Force);
        }

        private void Die()
        {
            animator.SetTrigger("DidDie");
        }

        public void ThrowProjectileAtPlayer()
        {
            GameObject projectilePrefab = animator.GetBool("isEnraged") ? enragedProjectilePrefab : normalProjectilePrefab;

            Vector3 dir = (Camera.main.transform.position - transform.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = transform.Find("Body").Find("Bow").position;
            //projectile.GetComponent<Projectile>().SetSource(this);
            projectile.GetComponent<Rigidbody>().AddForceAtPosition(dir * 3000, Vector3.zero);
        }

        public void SpawnMinion()
        {
            int rand = Random.Range(0, minionsPrefabs.Length);
            GameObject minion = Instantiate(minionsPrefabs[rand]);

            Vector3 summonPos = animator.transform.position + animator.transform.Find("Body").forward * 6;
            minion.transform.position = summonPos;
            Instantiate(summonFXPrefab).transform.position = summonPos;
        }
    }
}
