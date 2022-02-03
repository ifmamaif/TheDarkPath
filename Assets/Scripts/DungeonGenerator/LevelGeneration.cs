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
        const int WIDTH_ROOM = 31;
        const int HEIGHT_ROOM = 32;

        public void Start()
        {
            parentRooms = UnityWrapper.InstantiateGameObject(NAME_PARENT_ROOMS);
            mainRoomIndex = GRID_SIZE / 2;       // IMPORTANT : mainRoomIndex must be half of gridSize
            Generate();
            LevelScript.levelValue = 1;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R) || RemainingRoomsScript.remainingRoomsValue == 0)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    ScoreScript.scoreValue = 0;
                    LevelScript.levelValue = 1;
                }
                if (RemainingRoomsScript.remainingRoomsValue == 0)
                {
                    LevelScript.levelValue += 1;
                }
                foreach (var room in rooms)
                {
                    if (room)
                    {
                        Destroy(room);
                    }
                }
                GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (var enemyObject in enemyObjects)
                {
                    if (enemyObject)
                    {
                        Destroy(enemyObject);
                    }
                }
                Generate();
            }
        }

        public void Generate()
        {
            int totalRooms = 0;
            int[,] terrain = null;

            var task = Task.Run(() => {
                Utils.MessureTime(() =>
                {
                    terrain = DUNGEON_GENERATOR.GenerateDungeon(GRID_SIZE, mainRoomIndex);
                }, "Dungeon generator");
            });

            Utils.MessureTime(() =>
            {
                InstantiateRootRooms();

                }, "Instantiate root rooms");

            task.Wait();

            Color[,][,] colors = null;
            task = Task.Run(() =>
            {
                Utils.MessureTime(() =>
                {
                    colors = GenerateColors(ref terrain);
                }, "Color generator");
            });

            Utils.MessureTime(() =>
            {
                for (int i = 0; i < GRID_SIZE.x; i++)
                {
                    for (int j = 0; j < GRID_SIZE.y; j++)
                    {
                        if (terrain[i, j] != 0)
                        {
                            var room = rooms[i, j].gameObject;
                            InstantiateNewRoomDetails(room, false);
                            totalRooms++;
                        }
                    }
                }
            }, "InstantiateRooms");

            RemainingRoomsScript.allRoomsValue = totalRooms - 1;
            RemainingRoomsScript.remainingRoomsValue = totalRooms - 1;

            SetRoomDoors();

            task.Wait();

            ColoringRooms(terrain, colors);

            GameObject mainRoom = rooms[mainRoomIndex.x, mainRoomIndex.y];
            mainRoom.SetActive(true);

            Room room = mainRoom.GetComponent<Room>();
            room.IsDefeated = true;

            GameObject.Find("Scene Controller").GetComponent<SceneController>().TeleportPlayer(room.PlayerSpawn);
        }

        private void ColoringRooms(int[,] terrain, Color[,][,] colors)
        {
            for (int i = 0; i < GRID_SIZE.x; i++)
            {
                for (int j = 0; j < GRID_SIZE.y; j++)
                {
                    if (terrain[i, j] != 0)
                    {
                        var cells = rooms[i, j].transform.Find("Cells");

                        for (int x = 0; x < WIDTH_ROOM; x++)
                        {
                            for (int y = 0; y < HEIGHT_ROOM; y++)
                            {
                                var cell = cells.transform.Find(x + " " + y);
                                var renderer = cell.GetComponent<SpriteRenderer>();
                                renderer.color = colors[i,j][x,y];
                            }
                        }
                    }
                }
            }
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

        private Color[,][,] GenerateColors(ref int[,] terrain)
        {
            float[,] entireMapNoise = PerlinNoise.PerlinNoise.PerlinNoiseImproved(WIDTH_ROOM * GRID_SIZE.x, HEIGHT_ROOM * GRID_SIZE.y);
            float[,] whichRGBNoise = PerlinNoise.PerlinNoise.PerlinNoiseImproved(GRID_SIZE.x, GRID_SIZE.y);


            Color[,][,] result = new Color[GRID_SIZE.x, GRID_SIZE.y][,];
            const float MIN_VALUE = 0.01f;
            const float MAX_VALUE = 0.98f;

            for (int i = 0; i < GRID_SIZE.x; i++)
            {
                for (int j = 0; j < GRID_SIZE.y; j++)
                {
                    if (terrain[i, j] != 0)
                    {
                        result[i, j] = new Color[WIDTH_ROOM, HEIGHT_ROOM];

                        for (int x = 0; x < WIDTH_ROOM; x++)
                        {
                            for (int y = 0; y < HEIGHT_ROOM; y++)
                            {
                                var valueNoise = entireMapNoise[x + GRID_SIZE.x * i, y + GRID_SIZE.y * j];

                                Color color;
                                if (whichRGBNoise[i, j] < 0.16f)
                                {
                                    color = new Color(MAX_VALUE, MIN_VALUE, MIN_VALUE);
                                    color.g = Utils.Lerp(MIN_VALUE, MAX_VALUE, valueNoise);
                                }
                                else if(whichRGBNoise[i, j] < 0.32f)
                                {
                                    color = new Color(MAX_VALUE, MAX_VALUE, MIN_VALUE);
                                    color.r = Utils.Lerp(MAX_VALUE, MIN_VALUE, valueNoise);
                                }
                                else if (whichRGBNoise[i, j] < 0.48f)
                                {
                                    color = new Color(MIN_VALUE, MAX_VALUE, MIN_VALUE);
                                    color.b = Utils.Lerp(MIN_VALUE, MAX_VALUE, valueNoise);
                                }
                                else if (whichRGBNoise[i, j] < 0.64f)
                                {
                                    color = new Color(MIN_VALUE, MAX_VALUE, MAX_VALUE);
                                    color.g = Utils.Lerp(MAX_VALUE, MIN_VALUE, valueNoise);
                                }
                                else if (whichRGBNoise[i, j] < 0.80f)
                                {
                                    color = new Color(MIN_VALUE, MIN_VALUE, MAX_VALUE);
                                    color.r = Utils.Lerp(MIN_VALUE, MAX_VALUE, valueNoise);
                                }
                                else
                                {
                                    color = new Color(MAX_VALUE, MIN_VALUE, MAX_VALUE);
                                    color.b = Utils.Lerp(MAX_VALUE, MIN_VALUE, valueNoise);
                                }
                                

                                result[i, j][x, y] = color;
                            }
                        }
                    }
                }
            }
            return result;
        }

        private void InstantiateRootRooms()
        {
            rooms = UnityWrapper.InstantiateGameObject(GRID_SIZE.x, GRID_SIZE.y);

            for (int i = 0; i < GRID_SIZE.x; i++)
            {
                for (int j = 0; j < GRID_SIZE.y; j++)
                {
                    GameObject dynamicRoom = UnityWrapper.InstantiateGameObject("Room " + i + " " + j);
                    dynamicRoom.transform.parent = parentRooms.transform;
                    dynamicRoom.transform.position = new Vector3(HARD_CODED.x * i, HARD_CODED.y * j, 1);
                    dynamicRoom.SetActive(false);
                    rooms[i, j] = dynamicRoom;
                }
            }
        }

        private void InstantiateNewRoomDetails(GameObject dynamicRoom, bool defeated)
        {
            dynamicRoom.SetActive(true);

            GameObject portals = UnityWrapper.InstantiateGameObject("Cells");
            portals.transform.parent = dynamicRoom.transform;
            portals.transform.localPosition = Vector3.zero;

            InstantiateArrayCells(1        , 1         , WIDTH_ROOM - 1, HEIGHT_ROOM - 1, portals, Constant.TERAIN_TEXTURE_PATH, false);    // center
            InstantiateArrayCells(0        , 0         , WIDTH_ROOM    , 1         , portals, Constant.WALL_TEXTURE_PATH  , true);     // left side
            InstantiateArrayCells(0        , HEIGHT_ROOM - 1, WIDTH_ROOM    , HEIGHT_ROOM    , portals, Constant.WALL_TEXTURE_PATH  , true);     // right side
            InstantiateArrayCells(0        , 1         , 1        , HEIGHT_ROOM - 1, portals, Constant.WALL_TEXTURE_PATH  , true);     // bottom side
            InstantiateArrayCells(WIDTH_ROOM - 1, 1         , WIDTH_ROOM    , HEIGHT_ROOM - 1, portals, Constant.WALL_TEXTURE_PATH  , true);     // top side

            var roomScript = dynamicRoom.AddComponent<Room>();
            roomScript.ConstructRoom();

            var roomUnitManager = dynamicRoom.AddComponent<RoomUnitManager>();

            roomUnitManager.enemiesPrefabs = new List<GameObject>
            {
                Resources.Load<GameObject>("Prefabs/Enemy")
            };

           
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
                    if (rooms[x, y] == null || rooms[x,y].activeSelf == false)
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

            if (condition() && ((neightboorRoomIndex = rooms[x + offset.x, y + offset.y]) != null && neightboorRoomIndex.activeSelf == true))
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