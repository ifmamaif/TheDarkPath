using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private const string NAME_PARENT_ROOMS = "Rooms";

        public void Start()
        {
            parentRooms = UnityWrapper.InstantiateGameObject(NAME_PARENT_ROOMS);
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

            rooms = UnityWrapper.InstantiateGameObject(GRID_SIZE.x, GRID_SIZE.y);
            int[,] terrain = null;

            Utils.MessureTime(() =>
            {
                terrain = DUNGEON_GENERATOR.GenerateDungeon(GRID_SIZE, mainRoomIndex);
            }, "Dungeon generator");

            Utils.MessureTime(() =>
            {
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
            }, "InstantiateRooms");

            SetRoomDoors();

            GameObject mainRoom = rooms[mainRoomIndex.x, mainRoomIndex.y];
            mainRoom.SetActive(true);

            Room room = mainRoom.GetComponent<Room>();
            room.IsDefeated = true;

            GameObject.Find("Scene Controller").GetComponent<SceneController>().TeleportPlayer(room.PlayerSpawn);
        }

        private void InstantiateCell(int i, int j, GameObject parent, string texture, bool collider = false)
        {
            GameObject cell = UnityWrapper.InstantiateGameObject(i + " " + j);
            cell.transform.parent = parent.transform;
            cell.transform.localPosition = new Vector3(j, i, 0);
            cell.transform.localScale = new Vector3(Constant.TEXTURE_SIZE_X, Constant.TEXTURE_SIZE_Y, 1);
            var renderer = cell.AddComponent<SpriteRenderer>();
            renderer.sprite = Resources.Load<Sprite>(texture);
            if (collider)
            {
                cell.AddComponent<BoxCollider2D>();
                cell.tag = "Wall";
            }
        }

        private void InstantiateNewRoom(Vector2Int matrixPosition, bool defeated)
        {
            GameObject dynamicRoom = UnityWrapper.InstantiateGameObject("Room " + matrixPosition.x + " " + matrixPosition.y);
            dynamicRoom.transform.parent = parentRooms.transform;
            dynamicRoom.transform.position = new Vector3(HARD_CODED.x * matrixPosition.x, HARD_CODED.y * matrixPosition.y, 1);

            GameObject portals = UnityWrapper.InstantiateGameObject("Cells");
            portals.transform.parent = dynamicRoom.transform;
            portals.transform.localPosition = Vector3.zero;

            const int WIDTH = 31;
            const int HEIGHT = 32;

            InstantiateArrayCells(1        , 1         , WIDTH - 1, HEIGHT - 1, portals, Constant.TERAIN_TEXTURE_PATH, false);    // center
            InstantiateArrayCells(0        , 0         , WIDTH    , 1         , portals, Constant.WALL_TEXTURE_PATH  , true);     // left side
            InstantiateArrayCells(0        , HEIGHT - 1, WIDTH    , HEIGHT    , portals, Constant.WALL_TEXTURE_PATH  , true);     // right side
            InstantiateArrayCells(0        , 1         , 1        , HEIGHT - 1, portals, Constant.WALL_TEXTURE_PATH  , true);     // bottom side
            InstantiateArrayCells(WIDTH - 1, 1         , WIDTH    , HEIGHT - 1, portals, Constant.WALL_TEXTURE_PATH  , true);     // top side

            var roomScript = dynamicRoom.AddComponent<Room>();
            roomScript.ConstructRoom();

            var roomUnitManager = dynamicRoom.AddComponent<RoomUnitManager>();

            roomUnitManager.enemiesPrefabs = new List<GameObject>
            {
                Resources.Load<GameObject>("Prefabs/Enemy")
            };

            rooms[matrixPosition.x, matrixPosition.y] = dynamicRoom;
        }

        private void InstantiateArrayCells(int i, int j, int width, int height, GameObject portals, string texturePath, bool collider)
        {
            for (int column = i; column < width; column++)
            {
                for (int row = j; row < height; row++)
                {
                    InstantiateCell(column, row, portals, texturePath, collider);
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

                    CheckDoor(x, y, Room.RoomPosition.South);
                    CheckDoor(x, y, Room.RoomPosition.West);
                    CheckDoor(x, y, Room.RoomPosition.North);
                    CheckDoor(x, y, Room.RoomPosition.East);
                }
            }
        }

        void CheckDoor(int x, int y, Room.RoomPosition typeRoom)
        {
            GameObject neightboorRoomIndex;
            Func<bool> condition;
            int portalIndex;
            Vector2Int offset;

            switch (typeRoom)
            {
                case Room.RoomPosition.East:
                    condition = () => x + 1 < GRID_SIZE.x;
                    offset = new Vector2Int(1, 0);
                    portalIndex = 2;
                    break;

                case Room.RoomPosition.North:
                    condition = () => y + 1 < GRID_SIZE.y;
                    offset = new Vector2Int(0, 1);
                    portalIndex = 0;
                    break;

                case Room.RoomPosition.South:
                    condition = () => y - 1 >= 0;
                    offset = new Vector2Int(0, -1);
                    portalIndex = 1;
                    break;

                case Room.RoomPosition.West:
                    condition = () => x - 1 >= 0;
                    offset = new Vector2Int(-1, 0);
                    portalIndex = 3;
                    break;
                default:
                    // error
                    return;
            }

            Room room = rooms[x, y].GetComponent<Room>();

            if (condition() && ((neightboorRoomIndex = rooms[x + offset.x, y + offset.y]) != null))
            {
                room.TypeRoom |= (int)typeRoom;
                room.portalPoints[portalIndex].linkedRoom = neightboorRoomIndex;
            }
            else
            {
                room.portalPoints[portalIndex].gameObject.SetActive(false);
            }
        }
    }
}