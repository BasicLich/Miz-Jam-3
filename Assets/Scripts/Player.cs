using MizJam.Utility;
using UnityEngine;

namespace MizJam
{
    public class Player : Singleton<Player>
    {
        [SerializeField]
        private float health = 100f;

        private float currentHealth = 100.0f;

        [SerializeField]
        private float timeToHeal = 5.0f;

        private float healCounter = 0.0f;

        public float MaxHealth => this.health;
        public float CurrentHealth => this.currentHealth;

        private void Update()
        {
            this.healCounter += Time.deltaTime;
            if (this.healCounter >= this.timeToHeal)
            {
                this.currentHealth++;
                this.currentHealth = Mathf.Min(this.currentHealth, this.health);
                this.healCounter = 0.0f;
            }
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if(currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            this.currentHealth = 0.0f;
            HUDManager.Instance.EndGame(false);
        }

        public void Heal()
        {
            this.currentHealth = this.health;
        }
    }
}