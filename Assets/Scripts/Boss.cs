using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MizJam
{
    public class Boss : MonoBehaviour
    {
        [SerializeField]
        private Slider healthBar;
        [SerializeField]
        private float triggerIntroDistance;
        private bool didIntroduce;

        public float health;
        private float initialHealth;
        private Animator animator;
        private Rigidbody rb;

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

        [Header("Mellee Attack")]
        [SerializeField]
        private float melleeRange, melleeDamage;
        [SerializeField]
        private LayerMask attackLayers;
        private Vector3 chargePos;

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
            if (!didIntroduce)
            {
                if((Camera.main.transform.position - transform.position).magnitude <= triggerIntroDistance)
                {
                    TriggerIntro();
                }
            }
        }

        private void TriggerIntro()
        {
            didIntroduce = true;
            healthBar.transform.parent.gameObject.SetActive(true);
            animator.SetTrigger("IntroduceTrigger");
            animator.SetBool("DidIntroduce", true);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, triggerIntroDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, melleeRange);
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

            Vector3 bowPos = transform.Find("Body").Find("Bow").position;
            Vector3 dir = (Camera.main.transform.position - bowPos).normalized;
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.transform.position = bowPos;
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

        public void GetChargePosition()
        {
            chargePos = Camera.main.transform.position;
            chargePos.y = transform.position.y;
        }

        public void ChargeAtPlayer()
        {
            if(chargePos != Vector3.zero)
            {
                Vector3 finalPos = (chargePos - transform.position) * 0.9f;
                transform.DOMove(transform.position + finalPos, 0.5f).OnComplete(() => CheckIfMelleedPlayer());

                chargePos = Vector3.zero;
            }
        }

        private void CheckIfMelleedPlayer()
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, melleeRange, attackLayers);
            foreach(var col in cols)
            {
                Player player = col.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(melleeDamage);
                    break;
                }
            }
        }
    }
}
