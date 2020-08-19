using MizJam.Utility;
using UnityEngine;

namespace MizJam
{
    public class HUDManager : Singleton<HUDManager>
    {
        [SerializeField]
        private GameObject dmgIndicationCanvas, dmgIndicatorPrefab;

        public void DisplayHitDirection(Enemy enemy)
        {
            DamageIndicator di = Instantiate(dmgIndicatorPrefab, dmgIndicationCanvas.transform).GetComponent<DamageIndicator>();
            di.SetDamageSource(enemy);
        }
    }
}
