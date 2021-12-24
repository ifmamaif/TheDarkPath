using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheDarkPath
{
    public class Room : MonoBehaviour
    {
        public List<PortalPoint> portalPoints = null;
        public bool IsDefeated { get; set; } = false;

        public List<GameObject> EnemySpawnPoints { get; set; }  = null;
        public int TypeRoom { get; set; } = 0;
        public Transform PlayerSpawn { get; private set; } = null;

        public enum RoomPosition
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
            if (TypeRoom == 0)
            {
                Debug.LogError("Type room is invalid");
            }
            if (EnemySpawnPoints == null)
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

            if (!IsDefeated)
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
            IsDefeated = true;
            foreach (PortalPoint point in portalPoints)
            {
                if (point.linkedRoom != null)
                {
                    point.gameObject.SetActive(true);
                }
            }
        }

        public void ConstructRoom()
        {
            var dynamicRoom = this.gameObject;


            var playerSpawn = new GameObject("PlayerSpawn");
            playerSpawn.transform.parent = dynamicRoom.transform;
            playerSpawn.transform.localPosition = new Vector3(15, 15, 0);
            PlayerSpawn = playerSpawn.transform;

            portalPoints = new List<PortalPoint>();

            CreatePortal(PortalPoint.Position.North, dynamicRoom.transform);
            CreatePortal(PortalPoint.Position.South, dynamicRoom.transform);
            CreatePortal(PortalPoint.Position.East, dynamicRoom.transform);
            CreatePortal(PortalPoint.Position.West, dynamicRoom.transform);

            var gam1 = new GameObject("Enemy spawn 1");
            gam1.transform.parent = dynamicRoom.transform;
            gam1.transform.localPosition = new Vector3(4, 2);
            EnemySpawnPoints = new List<GameObject>()
            {
                gam1
            };
        }

        private GameObject CreatePortal(PortalPoint.Position position, Transform parent)
        {
            string name = "GameObject";
            Vector3 localPos = Vector3.zero;
            var playerPos = Vector3.zero;
            const float OFFSET_SPAWN = 1.5f;

            switch (position)
            {
                case PortalPoint.Position.East:
                    name = "Portal East";
                    localPos = new Vector3(30f, 15f, -1);
                    playerPos = new Vector3(-OFFSET_SPAWN, 0, 0);
                    break;
                case PortalPoint.Position.West:
                    localPos = new Vector3(1f, 15f, -1);
                    name = "Portal West";
                    playerPos = new Vector3(OFFSET_SPAWN, 0, 0);
                    break;
                case PortalPoint.Position.South:
                    name = "Portal South";
                    localPos = new Vector3(15f, 1f, -1);
                    playerPos = new Vector3(0, OFFSET_SPAWN, 0);
                    break;
                case PortalPoint.Position.North:
                    name = "Portal North";
                    localPos = new Vector3(15f, 29f, -1);
                    playerPos = new Vector3(0, -OFFSET_SPAWN, 0);
                    break;
            }

            var gameObject = new GameObject(name);
            gameObject.transform.parent = parent;
            gameObject.transform.localScale = new Vector3(Constant.TEXTURE_SIZE_X, Constant.TEXTURE_SIZE_Y, 1);
            gameObject.transform.localPosition = localPos;

            var boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
            boxCollider2D.isTrigger = true;

            var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>(Constant.PORTAL_TEXTURE_PATH);
            spriteRenderer.color = new Color(1, 92 / 255f, 1, 1);

            var portalScript = gameObject.AddComponent<PortalPoint>();
            portalScript.position = position;

            var soundSource = gameObject.AddComponent<AudioSource>();
            soundSource.clip = Resources.Load("Audio/Teleport SFX") as AudioClip;
            soundSource.volume = .35f;

            var playerSpawn = new GameObject();
            playerSpawn.transform.parent = gameObject.transform;
            playerSpawn.transform.localPosition = playerPos;

            portalScript.playerSpawnPosition = playerSpawn.transform;
            portalPoints.Add(portalScript);

            return gameObject;
        }
    }
}