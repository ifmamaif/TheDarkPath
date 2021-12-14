using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{
    public class LevelGeneration : MonoBehaviour
    {
        private GameObject[,] rooms;
        private readonly Vector2Int GRID_SIZE = new Vector2Int(14, 14);
        private Vector2Int mainRoomIndex;
        private readonly Vector2Int HARD_CODED = new Vector2Int(31, 32);
        private readonly string ROOM_PREFAB = "Prefabs/Rooms/Main Room";
        private readonly string terenPathString = "Sprites/freepath";//"Tiles/Dungeon/Misc 2_89"; // "Sprites/1"
        private readonly string pereteNormalPathString = "Sprites/wall";


        public int numberOfRooms = 20;

        //private const float textureScaleSize = 0.4f;
        private const float textureSizeX = 0.81f;
        private const float textureSizeY = 0.93f;


        public void Start()
        {
            mainRoomIndex = GRID_SIZE / 2;       // IMPORTANT : mainRoomIndex must be half of gridSize
            Generate();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                foreach (var room in rooms)
                {
                    if (room)
                    {
                        Destroy(room);
                    }
                }
                Generate();
            }
        }

        public void Generate()
        {
            rooms = new GameObject[GRID_SIZE.x, GRID_SIZE.y];
            DungeonGenerator generator = new DungeonGenerator();
            int[,] terrain = generator.GenerateDungeon(GRID_SIZE, mainRoomIndex);

            for (int i = 0; i < GRID_SIZE.x; i++)
            {
                for (int j = 0; j < GRID_SIZE.y; j++)
                {
                    if (terrain[i, j] != 0)
                    {
                        //InstantiateRoom(new Vector2Int(i, j), false);
                        InstantiateNewRoom(new Vector2Int(i, j), false);
                    }
                }
            }

            rooms[mainRoomIndex.x, mainRoomIndex.y].GetComponent<Room>().isDefeated = true;
            SetRoomDoors();


            GameObject mainRoom = rooms[mainRoomIndex.x, mainRoomIndex.y];

            mainRoom.SetActive(true);
            Room room = mainRoom.GetComponent<Room>();
            GameObject.Find("Scene Controller").GetComponent<SceneController>().TeleportPlayer(room.playerSpawn);
        }

        private void InstanteCell(int i, int j, GameObject parent, string texture, bool collider = false)
        {
            GameObject cell = new GameObject(i + " " + j);
            cell.transform.parent = parent.transform;
            cell.transform.localPosition = new Vector3(j, i, 0);
            cell.transform.localScale = new Vector3(textureSizeX, textureSizeY, 1);
            var renderer = cell.AddComponent<SpriteRenderer>();
            renderer.sprite = Resources.Load<Sprite>(texture);
            if (collider)
            {
                cell.AddComponent<BoxCollider2D>();
            }
        }

        private GameObject CreatePortal(PortalPoint.Position position, Transform parent, ref Room roomScript)
        {
            string name = "GameObject";
            Vector3 localPos = Vector3.zero;
            var playerPos = Vector3.zero;
            float offsetSpawn = 1.3f;

            switch (position)
            {
                case PortalPoint.Position.East:
                    name = "Portal East";
                    localPos = new Vector3(30f, 15f, -1);
                    playerPos = new Vector3(-offsetSpawn, 0, 0);
                    break;
                case PortalPoint.Position.West:
                    localPos = new Vector3(1f, 15f, -1);
                    name = "Portal West";
                    playerPos = new Vector3(offsetSpawn, 0, 0);
                    break;
                case PortalPoint.Position.South:
                    name = "Portal South";
                    localPos = new Vector3(15f, 1f, -1);
                    playerPos = new Vector3(0, offsetSpawn, 0);
                    break;
                case PortalPoint.Position.North:
                    name = "Portal North";
                    localPos = new Vector3(15f, 29f, -1);
                    playerPos = new Vector3(0, -offsetSpawn, 0);
                    break;
            }

            var gameObject = new GameObject(name);
            gameObject.transform.parent = parent;
            gameObject.transform.localScale = new Vector3(textureSizeX, textureSizeY, 1);
            gameObject.transform.localPosition = localPos;

            var boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
            boxCollider2D.isTrigger = true;

            var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/portal");
            spriteRenderer.color = new Color(1, 92 / 255f, 1, 1);

            var portalScript = gameObject.AddComponent<PortalPoint>();
            portalScript.position = position;

            var playerSpawn = new GameObject();
            playerSpawn.transform.parent = gameObject.transform;
            playerSpawn.transform.localPosition = playerPos;

            portalScript.playerSpawnPosition = playerSpawn.transform;
            roomScript.portalPoints.Add(portalScript);

            return gameObject;
        }

        private void InstantiateNewRoom(Vector2Int matrixPosition, bool defeated)
        {
            GameObject dynamicRoom = new GameObject("Room " + matrixPosition.x + " " + matrixPosition.y);
            //dynamicRoom.transform.parent = rooms[matrixPosition.x, matrixPosition.y].transform;
            //dynamicRoom.transform.localPosition = Vector3.zero;
            dynamicRoom.transform.position = new Vector3((HARD_CODED.x + 2) * matrixPosition.x, HARD_CODED.y * matrixPosition.y, 1);

            GameObject portals = new GameObject("Cells");
            portals.transform.parent = dynamicRoom.transform;
            portals.transform.localPosition = Vector3.zero;

            int width = 31;
            int height = 32;

            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    InstanteCell(i, j, portals, terenPathString);
                }
            }

            // left side
            for (int i = 0; i < width; i++)
            {
                InstanteCell(i, 0, portals, pereteNormalPathString, true);
            }

            // right side
            for (int i = 0; i < width; i++)
            {
                InstanteCell(i, (height - 1), portals, pereteNormalPathString, true);
            }

            // bottom side
            for (int j = 1; j < height - 1; j++)
            {
                InstanteCell(0, j, portals, pereteNormalPathString, true);
            }

            // top side
            for (int j = 1; j < height - 1; j++)
            {
                InstanteCell((width - 1), j, portals, pereteNormalPathString, true);
            }

            var roomScript = dynamicRoom.AddComponent<Room>();
            var roomUnitManager = dynamicRoom.AddComponent<RoomUnitManager>();

            var playerSpawn = new GameObject("PlayerSpawn");
            playerSpawn.transform.parent = dynamicRoom.transform;
            playerSpawn.transform.localPosition = new Vector3(15, 15, 0);
            roomScript.playerSpawn = playerSpawn.transform;

            roomScript.portalPoints = new List<PortalPoint>();

            CreatePortal(PortalPoint.Position.North, dynamicRoom.transform, ref roomScript);
            CreatePortal(PortalPoint.Position.South, dynamicRoom.transform, ref roomScript);
            CreatePortal(PortalPoint.Position.East, dynamicRoom.transform, ref roomScript);
            CreatePortal(PortalPoint.Position.West, dynamicRoom.transform, ref roomScript);

            roomScript.enemySpawnPoints = new List<GameObject>();
            var gam1 = new GameObject("Enemy spawn 1");
            gam1.transform.parent = dynamicRoom.transform;
            gam1.transform.localPosition = new Vector3(4, 2);
            roomScript.enemySpawnPoints.Add(gam1);

            roomUnitManager.enemiesPrefabs = new List<GameObject>();
            roomUnitManager.enemiesPrefabs.Add(Resources.Load<GameObject>("Prefabs/Enemy"));

            rooms[matrixPosition.x, matrixPosition.y] = dynamicRoom;
        }

        private void InstantiateRoom(Vector2Int matrixPosition, bool defeated)
        {
            rooms[matrixPosition.x, matrixPosition.y] = Instantiate(Resources.Load<GameObject>(ROOM_PREFAB));
            rooms[matrixPosition.x, matrixPosition.y].SetActive(true);
            rooms[matrixPosition.x, matrixPosition.y].transform.position = new Vector3(HARD_CODED.x * matrixPosition.x, HARD_CODED.y * matrixPosition.y, 1);
            rooms[matrixPosition.x, matrixPosition.y].GetComponent<Room>().isDefeated = defeated;
        }


        private void SetRoomDoors()
        {
            for (int x = 0; x < GRID_SIZE.x; x++)
            {
                for (int y = 0; y < GRID_SIZE.y; y++)
                {
                    if (rooms[x, y] == null)
                    {
                        continue;
                    }

                    Room room = rooms[x, y].GetComponent<Room>();

                    if (y - 1 >= 0 && rooms[x, y - 1] != null)
                    {
                        room.typeRoom |= (int)Room.TypeRoom.South;
                        room.portalPoints[1].linkedRoom = rooms[x, y - 1];
                    }
                    else
                    {
                        room.portalPoints[1].gameObject.SetActive(false);
                    }

                    if (y + 1 < GRID_SIZE.y && rooms[x, y + 1] != null)
                    {
                        room.typeRoom |= (int)Room.TypeRoom.North;
                        room.portalPoints[0].linkedRoom = rooms[x, y + 1];
                    }
                    else
                    {
                        room.portalPoints[0].gameObject.SetActive(false);
                    }

                    if (x - 1 >= 0 && rooms[x - 1, y] != null)
                    {
                        room.typeRoom |= (int)Room.TypeRoom.West;
                        room.portalPoints[3].linkedRoom = rooms[x - 1, y];
                    }
                    else
                    {
                        room.portalPoints[3].gameObject.SetActive(false);
                    }

                    if (x + 1 < GRID_SIZE.x && rooms[x + 1, y] != null)
                    {
                        room.typeRoom |= (int)Room.TypeRoom.East;
                        room.portalPoints[2].linkedRoom = rooms[x + 1, y];
                    }
                    else
                    {
                        room.portalPoints[2].gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}