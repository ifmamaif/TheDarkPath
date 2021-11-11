using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{
    public class LevelGeneration : MonoBehaviour
    {
        private GameObject[,] rooms = null;
        private readonly Vector2Int GRID_SIZE = new Vector2Int(14, 14);
        private Vector2Int mainRoomIndex;
        private readonly Vector2Int HARD_CODED = new Vector2Int(31, 32);
        private readonly string ROOM_PREFAB = "Prefabs/Rooms/Main Room";

        public int numberOfRooms = 20;

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
                        InstantiateRoom(new Vector2Int(i, j), false);
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