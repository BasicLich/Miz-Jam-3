using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace MizJam
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Balloon : MonoBehaviour
    {
        [SerializeField]
        private Sprite alertBalloon, skullBalloon;

        private SpriteRenderer sr;

        private BalloonEmotionsEnum emotion;

        void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            Load(emotion);
            Appear();
        }

        private void Load(BalloonEmotionsEnum emotion)
        {
            switch (emotion)
            {
                case BalloonEmotionsEnum.ALERT:
                    sr.sprite = alertBalloon;
                    break;
                case BalloonEmotionsEnum.SKULL:
                    sr.sprite = skullBalloon;
                    break;
            }
        }

        public void SetEmotion(BalloonEmotionsEnum emotion)
        {
            this.emotion = emotion;
        }

        private void Appear()
        {
            transform.parent.localScale = Vector3.zero;
            transform.parent.DOScale(1, 0.5f).OnComplete(() =>
            {
                StartCoroutine(Disappear());
            });
        }

        private IEnumerator Disappear()
        {
            yield return new WaitForSeconds(1);
            transform.parent.DOScale(0, 0.5f).OnComplete(() => Destroy(this.gameObject));
        }
    }
}

public enum BalloonEmotionsEnum
{
    ALERT,
    SKULL
}
