using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MizJam
{
    public class DamageIndicator : MonoBehaviour
    {
        private Enemy enemy;
        private Image indicatorImage;

        void Start()
        {
            indicatorImage = GetComponentInChildren<Image>();
            Color transparentImageColor = new Color(indicatorImage.color.r, indicatorImage.color.g, indicatorImage.color.b, 0);
            indicatorImage.DOColor(transparentImageColor, 3f).OnComplete(() => Destroy(this.gameObject));
        }

        void Update()
        {
            if (this.enemy)
            {
                var dirVector = enemy.transform.position - Camera.main.transform.parent.position;
                dirVector.y = 0;
                var rot = Quaternion.FromToRotation(Camera.main.transform.parent.forward, dirVector);
                float angle = rot.eulerAngles.y;
                indicatorImage.transform.parent.eulerAngles = new Vector3(0, 0, -angle);
            }
        }

        public void SetDamageSource(Enemy enemy)
        {
            this.enemy = enemy;
        }
    }
}
