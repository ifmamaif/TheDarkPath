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

        private Room scriptRoom;
        private SceneController sceneController;

        // Start is called before the first frame update
        void Start()
        {
            if (enemiesPrefabs == null)
            {
                Debug.LogError("This room does not have enemy spawns assigned. Is this intended?");
            }

            scriptRoom = this.gameObject.GetComponent<Room>();
            var sceneControllerGameObject = GameObject.Find("Scene Controller");
            if(sceneControllerGameObject == null)
            {
                Debug.LogError("There is no Scene Controller");
            }
            sceneController = sceneControllerGameObject.GetComponent<SceneController>();
        }

        public void SpawnEnemies(int count, int multiplier = 1)
        {
            List<GameObject> enemySpawns = scriptRoom.EnemySpawnPoints;
            Transform playerTransform = sceneController.PlayerTransform;

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
                    //enemy.GetComponent<Follower>().target = playerTransform;
                    enemy.GetComponent<Unit>().target = playerTransform;
                    enemy.GetComponent<Unit>().speed = Random.Range(1,3);
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
                        scriptRoom.RoomDefeated();
                    }
                }
            }
        }
    }
}