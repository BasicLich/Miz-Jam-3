using MizJam.Utility;
using UnityEngine;

namespace MizJam
{
    public class Player : Singleton<Player>
    {
        private float health = 100f;

        public void TakeDamage(float damage)
        {
            Debug.Log("took damage!");

            health -= damage;
            if(health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            //TODO: die
            Debug.Log("died");
        }
    }
}