using UnityEngine;

namespace MizJam
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleSystemAutoDestroy : MonoBehaviour
    {
        private new ParticleSystem particleSystem;

        private void Awake()
        {
            this.particleSystem = this.GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (!this.particleSystem.IsAlive())
                Destroy(this.gameObject);
        }
    }
}