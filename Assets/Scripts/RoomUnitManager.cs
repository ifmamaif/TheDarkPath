using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

namespace TheDarkPath
{
    [RequireComponent(typeof(Room))]
    public class RoomUnitManager : MonoBehaviour
    {
        public List<GameObject> enemiesPrefabs = null;
        public List<GameObject> enemies = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            if (enemiesPrefabs == null)
            {
                Debug.LogError("This room does not have enemy spawns assigned. Is this intended?");
            }
        }

        public void SpawnEnemies(int count, int multiplier = 1)
        {
            List<GameObject> enemySpawns = this.gameObject.GetComponent<Room>().enemySpawnPoints;
            Transform playerTransform = GameObject.Find("Scene Controller").GetComponent<SceneController>().playerTransform; ;

            for (int i = multiplier; i > 0; i--)
            {
                int spawnPosition = Random.Range(0, enemySpawns.Count - 1);

                for (int j = 0; j < count; j++)
                {
                    // TODO: Find a way to spawn multiple enemies when we have them
                    GameObject enemy = Instantiate(enemiesPrefabs[0]);
                    enemies.Add(enemy);
                    // TODO: Maybe add dynamic spawn positions?
                    enemy.transform.position = enemySpawns[spawnPosition].transform.position;
                    enemy.SetActive(true);
                    enemy.GetComponent<Follower>().target = playerTransform;
                    enemy.GetComponent<SimpleTargetProvider>().targetTransform = playerTransform;
                }
            }
        }

        private void Update()
        {
            // TODO: Register an event that fires when the enemy dies
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    if (enemies.Count == 0)
                    {
                        gameObject.GetComponent<Room>().RoomDefeated();
                    }
                }
            }
        }
    }
}