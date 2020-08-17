using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MizJam
{
    public class LevelGenerator : MonoBehaviour
    {
        [Serializable]
        public struct GroundPrefab
        {
            public GameObject prefab;
            public float spawRate;

            [HideInInspector]
            public float spawnCumulative;
        }

        public Vector2Int size;

        public GroundPrefab[] groundPrefabs;

        private GameObject ground;

        public void Generate()
        {
            this.Clear();
            this.GenerateGround();
        }

        private void Clear()
        {
            DestroyImmediate(this.ground);
            this.ground = new GameObject("Ground");
            this.ground.transform.SetParent(this.transform, false);
        }

        private void GenerateGround()
        {
            float cumulative = 0;
            for (int i = 0; i < this.groundPrefabs.Length; i++)
            {
                cumulative += this.groundPrefabs[i].spawRate;
                this.groundPrefabs[i].spawnCumulative = cumulative;
            }

            for (int x = 0; x < this.size.x; x++)
            {
                for (int z = 0; z < this.size.y; z++)
                {
                    float random = Random.Range(0, cumulative);
                    var prefab = this.groundPrefabs.First(el => random < el.spawnCumulative).prefab;
                    var instance = Instantiate(prefab, this.ground.transform);
                    instance.transform.SetPositionAndRotation(new Vector3(x, 0, z), Quaternion.identity);
                }
            }
        }
    }
}