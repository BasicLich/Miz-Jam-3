using MizJam.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MizJam
{
    public class BalloonsManager : Singleton<BalloonsManager>
    {
        [SerializeField]
        private GameObject balloonPrefab;

        public void SpawnBalloonAt(BalloonEmotionsEnum emotion, Transform transform)
        {
            GameObject balloon = Instantiate(balloonPrefab, transform);
            balloon.transform.localPosition = Vector3.zero + transform.up * 2;
            balloon.GetComponentInChildren<Balloon>().SetEmotion(emotion);
        }
    }
}
