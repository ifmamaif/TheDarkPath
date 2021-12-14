using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{
    public class LevelGeneration : MonoBehaviour
    {
        private GameObject parentRooms;
        private GameObject[,] rooms;
        private Vector2Int mainRoomIndex;

        private readonly DungeonGenerator DUNGEON_GENERATOR = new DungeonGenerator();
        private readonly Vector2Int HARD_CODED = new Vector2Int(32, 31);
        private readonly Vector2Int GRID_SIZE = new Vector2Int(14, 14);

        private readonly string TERAIN_TEXTURE_PATH = "Sprites/freepath";
        private const string WALL_TEXTURE_PATH = "Sprites/wall";
        private const float TEXTURE_SIZE_X = 0.81f;
        private const float TEXTURE_SIZE_Y = 0.93f;
        private const string NAME_PARENT_ROOMS = "Rooms";



        public void Start()
        {
            parentRooms = new GameObject(NAME_PARENT_ROOMS);
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
            int[,] terrain = DUNGEON_GENERATOR.GenerateDungeon(GRID_SIZE, mainRoomIndex);

            for (int i = 0; i < GRID_SIZE.x; i++)
            {
                for (int j = 0; j < GRID_SIZE.y; j++)
                {
                    if (terrain[i, j] != 0)
                    {
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

        private void InstantiateCell(int i, int j, GameObject parent, string texture, bool collider = false)
        {
            GameObject cell = new GameObject(i + " " + j);
            cell.transform.parent = parent.transform;
            cell.transform.localPosition = new Vector3(j, i, 0);
            cell.transform.localScale = new Vector3(TEXTURE_SIZE_X, TEXTURE_SIZE_Y, 1);
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
            float offsetSpawn = 1.5f;

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
            gameObject.transform.localScale = new Vector3(TEXTURE_SIZE_X, TEXTURE_SIZE_Y, 1);
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
            dynamicRoom.transform.parent = parentRooms.transform;
            dynamicRoom.transform.position = new Vector3(HARD_CODED.x * matrixPosition.x, HARD_CODED.y * matrixPosition.y, 1);

            GameObject portals = new GameObject("Cells");
            portals.transform.parent = dynamicRoom.transform;
            portals.transform.localPosition = Vector3.zero;

            const int WIDTH = 31;
            const int HEIGHT = 32;

            InstantiateArrayCells(1        , 1         , WIDTH - 1, HEIGHT - 1, portals, TERAIN_TEXTURE_PATH, false);    // center
            InstantiateArrayCells(0        , 0         , WIDTH    , 1         , portals, WALL_TEXTURE_PATH  , true);     // left side
            InstantiateArrayCells(0        , HEIGHT - 1, WIDTH    , HEIGHT    , portals, WALL_TEXTURE_PATH  , true);     // right side
            InstantiateArrayCells(0        , 1         , 1        , HEIGHT - 1, portals, WALL_TEXTURE_PATH  , true);     // bottom side
            InstantiateArrayCells(WIDTH - 1, 1         , WIDTH    , HEIGHT - 1, portals, WALL_TEXTURE_PATH  , true);     // top side

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

            roomUnitManager.enemiesPrefabs = new List<GameObject>
            {
                Resources.Load<GameObject>("Prefabs/Enemy")
            };

            rooms[matrixPosition.x, matrixPosition.y] = dynamicRoom;
        }

        private void InstantiateArrayCells(int i, int j, int width, int height, GameObject portals, string texturePath, bool tuta )
        {
            for (int column = i; column < width; column++)
            {
                for (int row = j; row < height; row++)
                {
                    InstantiateCell(column, row, portals, texturePath, tuta);
                }
            }
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

                    CheckDoor(x, y, Room.TypeRoom.South);
                    CheckDoor(x, y, Room.TypeRoom.West);
                    CheckDoor(x, y, Room.TypeRoom.North);
                    CheckDoor(x, y, Room.TypeRoom.East);
                }
            }
        }

        void CheckDoor(int x,int y, Room.TypeRoom typeRoom)
        {
            GameObject neightboorRoomIndex;
            Func<bool> condition;
            int portalIndex;

            switch (typeRoom)
            {
                case Room.TypeRoom.East:
                    condition = () => x + 1 < GRID_SIZE.x;
                    neightboorRoomIndex = rooms[x + 1, y];
                    portalIndex = 2;
                    break;

                case Room.TypeRoom.North:
                    condition = () => y + 1 < GRID_SIZE.y;
                    neightboorRoomIndex = rooms[x, y + 1];
                    portalIndex = 0;
                    break;

                case Room.TypeRoom.South:
                    condition = () => y - 1 >= 0;
                    neightboorRoomIndex = rooms[x, y - 1];
                    portalIndex = 1;
                    break;


                case Room.TypeRoom.West:
                    condition = () => x - 1 >= 0;
                    neightboorRoomIndex = rooms[x - 1, y];
                    portalIndex = 3;
                    break;
                default:
                    // error
                    return;
            }



            Room room = rooms[x, y].GetComponent<Room>();

            if (condition() && neightboorRoomIndex != null)
            {
                room.typeRoom |= (int)typeRoom;
                room.portalPoints[portalIndex].linkedRoom = neightboorRoomIndex;
            }
            else
            {
                room.portalPoints[portalIndex].gameObject.SetActive(false);
            }
        }
    }
}