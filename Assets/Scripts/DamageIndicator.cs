using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MizJam
{
    public class DamageIndicator : MonoBehaviour
    {
        private Enemy enemy;
        private Image indicatorImage;

        void Start()
        {
            indicatorImage = GetComponentInChildren<Image>();
            Destroy(this.gameObject, 2f);
        }

        void Update()
        {
            var dirVector = enemy.transform.position - Camera.main.transform.parent.position;
            dirVector.y = 0;
            var rot = Quaternion.FromToRotation(Camera.main.transform.parent.forward, dirVector);
            float angle = rot.eulerAngles.y;
            indicatorImage.transform.parent.eulerAngles = new Vector3(0, 0, -angle);
        }

        public void SetDamageSource(Enemy enemy)
        {
            this.enemy = enemy;
        }
    }
}
