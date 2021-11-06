using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheDarkPath
{
    public class Room : MonoBehaviour
    {
        public List<PortalPoint> portalPoints = null;
        public List<GameObject> enemySpawnPoints = null;
        public int typeRoom = 0;
        public Transform playerSpawn = null;
        public bool isDefeated = false;

        public enum TypeRoom
        {
            Empty = 0,
            North = 1,
            South = 2,
            East = 4,
            West = 8,
        }

        private void Start()
        {
            if (portalPoints == null)
            {
                Debug.LogError("Portal Points list is NULL!");
            }
            if (typeRoom == 0)
            {
                Debug.LogError("Type room is invalid");
            }
            if (enemySpawnPoints == null)
            {
                Debug.LogError("Enemy Spawn Points list is NULL!");
            }
        }

        public void OnRoomEnter()
        {
            int enemiesToSpawn = 0;
            int spawnsMultiplier = Random.Range(2, 5);
            foreach (PortalPoint point in portalPoints)
            {
                if (point.gameObject.activeSelf)
                {
                    enemiesToSpawn++;
                }
            }

            if (!isDefeated)
            {
                GetComponent<RoomUnitManager>().SpawnEnemies(enemiesToSpawn, spawnsMultiplier);

                foreach (PortalPoint portal in portalPoints)
                {
                    portal.gameObject.SetActive(false);
                }
            }
        }

        public void RoomDefeated()
        {
            isDefeated = true;
            foreach (PortalPoint point in portalPoints)
            {
                if (point.linkedRoom != null)
                {
                    point.gameObject.SetActive(true);
                }
            }
        }
    }
}