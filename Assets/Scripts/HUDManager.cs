using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MizJam
{
    public class HUDManager : MonoBehaviour
    {
        public static HUDManager instance;

        [SerializeField]
        private GameObject dmgIndicationCanvas, dmgIndicatorPrefab;

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        public void DisplayHitDirection(Enemy enemy)
        {
            DamageIndicator di = Instantiate(dmgIndicatorPrefab, dmgIndicationCanvas.transform).GetComponent<DamageIndicator>();
            di.SetDamageSource(enemy);
        }
    }
}
