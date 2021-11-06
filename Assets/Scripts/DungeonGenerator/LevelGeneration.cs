using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{
    public class LevelGeneration : MonoBehaviour
    {
        private GameObject[,] rooms = null;
        private List<Vector2Int> takenPositions = null;
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
            // Make sure we dont try to make more rooms than can fit in our grid
            int value = GRID_SIZE.x * GRID_SIZE.y;
            if (numberOfRooms >= value)
            {
                numberOfRooms = value;
                Debug.LogWarning("Warning: numberOfRooms was invalid => new value is : " + numberOfRooms);
            }

            // lays out the actual map
            CreateRooms();
            // assigns the doors where rooms would connect
            SetRoomDoors();

            GameObject mainRoom = rooms[mainRoomIndex.x, mainRoomIndex.y];

            mainRoom.SetActive(true);
            Room room = mainRoom.GetComponent<Room>();
            GameObject.Find("Scene Controller").GetComponent<SceneController>().TeleportPlayer(room.playerSpawn);
        }

        private void CreateRooms()
        {
            // Set the main room
            rooms = new GameObject[GRID_SIZE.x, GRID_SIZE.y];
            // main room, always defeated
            InstantiateRoom(new Vector2Int(mainRoomIndex.x, mainRoomIndex.y), true);

            takenPositions = new List<Vector2Int>();
            takenPositions.Insert(0, Vector2Int.zero);

            //magic numbers
            float randomCompareStart = 0.2f;
            float randomCompareEnd = 0.01f;
            //add rooms
            for (int i = 0; i < numberOfRooms - 1; i++)
            {
                float randomPerc = i / ((float)numberOfRooms - 1);
                float randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
                //grab new position
                Vector2Int checkPos = NewPosition();
                //test new position
                if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare)
                {
                    int iterations = 0;
                    do
                    {
                        checkPos = SelectiveNewPosition();
                        iterations++;
                    } while (NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);
                    if (iterations >= 50)
                    {
                        Debug.LogWarning("Warning: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, takenPositions));
                    }
                }
                //finalize position
                Vector2Int roomIndex = new Vector2Int((int)checkPos.x + mainRoomIndex.x, (int)checkPos.y + mainRoomIndex.y);
                // regular room
                InstantiateRoom(roomIndex, false);

                takenPositions.Insert(0, checkPos);
            }
        }

        private void InstantiateRoom(Vector2Int matrixPosition, bool defeated)
        {
            rooms[matrixPosition.x, matrixPosition.y] = Instantiate(Resources.Load<GameObject>(ROOM_PREFAB));
            rooms[matrixPosition.x, matrixPosition.y].SetActive(true);
            rooms[matrixPosition.x, matrixPosition.y].transform.position = new Vector3(HARD_CODED.x * matrixPosition.x, HARD_CODED.y * matrixPosition.y, 1);
            rooms[matrixPosition.x, matrixPosition.y].GetComponent<Room>().isDefeated = defeated;
        }

        private Vector2Int NewPosition()
        {
            Vector2Int checkingPos;
            int index;
            do
            {
                index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1)); // pick a random room
                checkingPos = GetNewPosition(index);

            } while (takenPositions.Contains(checkingPos)
                || checkingPos.x >= mainRoomIndex.x
                || checkingPos.x < -mainRoomIndex.x
                || checkingPos.y >= mainRoomIndex.y
                || checkingPos.y < -mainRoomIndex.y
            ); //make sure the position is valid
            return checkingPos;
        }

        // method differs from the above in the two commented ways
        private Vector2Int SelectiveNewPosition()
        {
            int inc;
            Vector2Int checkingPos;
            do
            {
                inc = 0;
                int index;
                do
                {
                    //instead of getting a room to find an adject empty space, we start with one that only 
                    //as one neighbor. This will make it more likely that it returns a room that branches out
                    index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
                    inc++;
                } while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);

                checkingPos = GetNewPosition(index);

            } while (takenPositions.Contains(checkingPos)
                || checkingPos.x >= mainRoomIndex.x
                || checkingPos.x < -mainRoomIndex.x
                || checkingPos.y >= mainRoomIndex.y
                || checkingPos.y < -mainRoomIndex.y
            );

            if (inc >= 100)
            { // break loop if it takes too long: this loop isnt garuanteed to find solution, which is fine for this
                Debug.Log("Error: could not find position with only one neighbor");
            }
            return checkingPos;
        }

        private Vector2Int GetNewPosition(int index)
        {
            //capture its x, y position
            Vector2Int newPosition = new Vector2Int(takenPositions[index].x, takenPositions[index].y);
            bool UpDown = Random.value < 0.5f;                //randomly pick wether to look on hor or vert axis
            int positive = (Random.value < 0.5f) ? 1 : -1;    //pick whether to be positive or negative on that axis

            //find the position based on the above bools
            if (UpDown)
            {
                newPosition.y += positive;
            }
            else
            {
                newPosition.x += positive;
            }

            return newPosition;
        }

        private int NumberOfNeighbors(Vector2Int checkingPos, List<Vector2Int> usedPositions)
        {
            int ret = 0; // start at zero, add 1 for each side there is already a room
            ret += usedPositions.Contains(checkingPos + Vector2Int.right) ? 1 : 0;
            ret += usedPositions.Contains(checkingPos + Vector2Int.left) ? 1 : 0;
            ret += usedPositions.Contains(checkingPos + Vector2Int.up) ? 1 : 0;
            ret += usedPositions.Contains(checkingPos + Vector2Int.down) ? 1 : 0;

            return ret;
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