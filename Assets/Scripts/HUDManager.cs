using MizJam.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MizJam
{
    public class HUDManager : Singleton<HUDManager>
    {
        [SerializeField]
        private GameObject dmgIndicationCanvas, dmgIndicatorPrefab, healthBar, deadScreen, winScreen;

        private void Update()
        {
            if (Player.Instance)
                this.healthBar.GetComponent<RectTransform>().localScale = new Vector3(Player.Instance.CurrentHealth / Player.Instance.MaxHealth, 1.0f, 1.0f);
        }

        public void DisplayHitDirection(Enemy enemy)
        {
            DamageIndicator di = Instantiate(dmgIndicatorPrefab, dmgIndicationCanvas.transform).GetComponent<DamageIndicator>();
            di.SetDamageSource(enemy);
        }

        public void EndGame(bool win)
        {
            foreach (MonoBehaviour b in Player.Instance.GetComponentsInChildren<MonoBehaviour>())
                b.enabled = false;

            Cursor.lockState = CursorLockMode.None;

            if (win)
                winScreen.SetActive(true);
            else
                deadScreen.SetActive(true);
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
