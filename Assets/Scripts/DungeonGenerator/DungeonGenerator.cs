using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{
    public class DungeonGenerator
    {
        private List<Vector2Int> takenPositions;
        private Vector2Int mainRoomIndex;
        private int numberOfRooms = 20;
        private int[,] rooms;
        private Vector2Int gridSize;

        private const int MAX_ITERATIONS = 100;
        private const int HALF_ITERATIONS = MAX_ITERATIONS / 2;

        public enum TypeRoom : byte // [0,255] , 8 bits
        {
            NoRoom = 0,            // 1
            Isolated = 1,          // 1
            HasLeftNeighbour = 2,  // 2
            HasRightNeighbour = 4, // 3
            HasTopNeighbour = 8,   // 4
            HasDownNeighbour = 16, // 5
                                   // 6
                                   // 7
                                   // 8
        }

        public int[,] GenerateDungeon(Vector2Int size, Vector2Int mainIndex, int seed = 0)
        {
            gridSize = size;
            mainRoomIndex = mainIndex != Vector2Int.zero ? mainIndex: Vector2Int.zero;

            Initialize();
            // lays out the actual map
            CreateRooms();
            // assigns the doors where rooms would connect
            SetRoomDoors();

            return rooms;
        }

        private void Initialize()
        {
            // Validate 
            // Make sure we don't try to make more rooms than can fit in our grid
            int value = gridSize.x * gridSize.y;
            if (numberOfRooms >= value)
            {
                numberOfRooms = value;
                Debug.LogWarning("Warning: numberOfRooms was invalid => new value is : " + numberOfRooms);
            }

            // Set the main room
            rooms = new int[gridSize.x, gridSize.y];
            takenPositions = new List<Vector2Int>();
            AddRoom(mainRoomIndex);
        }

        private void CreateRooms()
        {
            //magic numbers
            const float randomCompareStart = 0.2f;
            const float randomCompareEnd = 0.01f;

            float diveToRooms = ((float)numberOfRooms - 1);

            //add rooms
            for (int i = 0; i < numberOfRooms - 1; i++)
            {
                float randomPerc = i / diveToRooms;
                float randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
                //grab new position
                Vector2Int checkPos = NewPosition();

                //test new position
                if (HasMultipleNeighbors(checkPos) && PerlinNoise.Random.Rand01_s() > randomCompare)
                {
                    int iterations = 0;
                    do
                    {
                        checkPos = SelectiveNewPosition();
                        iterations++;
                    } while (HasMultipleNeighbors(checkPos) && iterations < MAX_ITERATIONS);
                    if (iterations >= HALF_ITERATIONS)
                    {
                        Debug.LogWarning("Warning: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos));
                    }
                }

                AddRoom(checkPos);
            }
        }

        private void AddRoom(Vector2Int position)
        {
            takenPositions.Insert(0, position);
            rooms[position.x, position.y] = 1;
        }

        private Vector2Int NewPositionWrapper(Func<int> calcIndex)
        {
            Vector2Int checkingPos;
            do
            {
                checkingPos = GetNewPosition(calcIndex());

            } while (takenPositions.Contains(checkingPos)
                || checkingPos.x >= gridSize.x
                || checkingPos.x < 0
                || checkingPos.y >= gridSize.y
                || checkingPos.y < 0
            ); //make sure the position is valid
            return checkingPos;
        }

        private Vector2Int NewPosition()
        {
            return NewPositionWrapper(() =>
            {
                return PickARandomRoom();
            });
        }

        // method differs from the above in the two commented ways
        private Vector2Int SelectiveNewPosition()
        {
            int inc = 0;
            Vector2Int checkingPos = NewPositionWrapper(() =>
            {
                inc = 0;
                int index;
                do
                {
                    //instead of getting a room to find an abject empty space, we start with one that only 
                    //as one neighbor. This will make it more likely that it returns a room that branches out
                    index = PickARandomRoom();
                    inc++;
                } while (HasMultipleNeighbors(takenPositions[index]) && inc < MAX_ITERATIONS);
                return index;
            });

            if (inc >= MAX_ITERATIONS)
            { // break loop if it takes too long: this loop isn't guaranteed to find solution, which is fine for this
                Debug.Log("Error: could not find position with only one neighbor");
            }
            return checkingPos;
        }

        private Vector2Int GetNewPosition(int index)
        {
            //capture its x, y position
            Vector2Int newPosition = new Vector2Int(takenPositions[index].x, takenPositions[index].y);
            bool UpDown = PerlinNoise.Random.Rand01_s() < 0.5f;                //randomly pick wether to look on hor or vert axis
            int positive = (PerlinNoise.Random.Rand01_s() < 0.5f) ? 1 : -1;    //pick whether to be positive or negative on that axis

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

        private int PickARandomRoom()
        {
            return Mathf.RoundToInt(PerlinNoise.Random.Rand01_s() * (takenPositions.Count - 1)); // pick a random room
        }

        private bool HasMultipleNeighbors(Vector2Int checkingPos)
        {
            return NumberOfNeighbors(checkingPos) > 1;
        }

        private int NumberOfNeighbors(Vector2Int checkingPos)
        {
            int ret = 0; // start at zero, add 1 for each side there is already a room
            ret += takenPositions.Contains(checkingPos + Vector2Int.right) ? 1 : 0;
            ret += takenPositions.Contains(checkingPos + Vector2Int.left) ? 1 : 0;
            ret += takenPositions.Contains(checkingPos + Vector2Int.up) ? 1 : 0;
            ret += takenPositions.Contains(checkingPos + Vector2Int.down) ? 1 : 0;

            return ret;
        }

        private void SetRoomDoors()
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    int room = rooms[x, y];
                    if (room == 0)
                    {
                        continue;
                    }
                    if (y - 1 >= 0 && rooms[x, y - 1] != 0)
                    {
                        room |= (int)Room.TypeRoom.South;
                    }

                    if (y + 1 < gridSize.y && rooms[x, y + 1] != 0)
                    {
                        room |= (int)Room.TypeRoom.North;
                    }

                    if (x - 1 >= 0 && rooms[x - 1, y] != 0)
                    {
                        room |= (int)Room.TypeRoom.West;
                    }

                    if (x + 1 < gridSize.x && rooms[x + 1, y] != 0)
                    {
                        room |= (int)Room.TypeRoom.East;
                    }

                    rooms[x, y] = room;
                }
            }
        }
    }
}