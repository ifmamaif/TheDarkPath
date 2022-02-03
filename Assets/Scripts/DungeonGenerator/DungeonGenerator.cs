using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TheDarkPath
{
    public class DungeonGenerator
    {
        private List<Vector2Int> m_TakenPositions;
        private Vector2Int m_MainRoomIndex;
        private int m_NumberOfRooms = 20;
        private int m_NumberOfRooms_MinusOne;
        private int[,] m_Rooms;
        private Vector2Int m_GridSize;

        private const int MAX_ITERATIONS = 100;
        private const int HALF_ITERATIONS = MAX_ITERATIONS / 2;
        const float RANDOM_COMPARE_START = 0.2f;
        const float RANDOM_COMPARE_END = 0.01f;

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
            m_GridSize = size;
            m_MainRoomIndex = mainIndex != Vector2Int.zero ? mainIndex : Vector2Int.zero;

            Initialize();
            // lays out the actual map
            CreateRooms();
            // assigns the doors where rooms would connect
            SetRoomDoors();

            return m_Rooms;
        }

        private void Initialize()
        {
            // Validate 
            // Make sure we don't try to make more rooms than can fit in our grid
            int value = m_GridSize.x * m_GridSize.y;
            if (m_NumberOfRooms >= value)
            {
                m_NumberOfRooms = value;
                Debug.LogWarning("Warning: numberOfRooms was invalid => new value is : " + m_NumberOfRooms);
            }

            // Set the main room
            m_Rooms = new int[m_GridSize.x, m_GridSize.y];
            m_TakenPositions = new List<Vector2Int>();
            m_NumberOfRooms_MinusOne = m_NumberOfRooms - 1;
            AddRoom(m_MainRoomIndex);
        }

        private void CreateRooms()
        {
            //add rooms
            for (int i = 0; i < m_NumberOfRooms_MinusOne; i++)
            {
                CreateAndAddRoom(i);
            }
        }

        private void CreateAndAddRoom(int index)
        {
            float diveToRooms = ((float)m_NumberOfRooms - 1);
            float randomPerc = index / diveToRooms;
            float randomCompare = Mathf.Lerp(RANDOM_COMPARE_START, RANDOM_COMPARE_END, randomPerc);
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

        private void AddRoom(Vector2Int position)
        {
            m_TakenPositions.Insert(0, position);
            m_Rooms[position.x, position.y] = 1;
        }

        private Vector2Int NewPositionWrapper(Func<int> calcIndex)
        {
            Vector2Int checkingPos;
            do
            {
                checkingPos = GetNewPosition(calcIndex());

            } while (m_TakenPositions.Contains(checkingPos)
                || checkingPos.x >= m_GridSize.x
                || checkingPos.x < 0
                || checkingPos.y >= m_GridSize.y
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
            int index;
            Vector2Int checkingPos = NewPositionWrapper(() =>
            {
                inc = 0;
                do
                {
                    //instead of getting a room to find an abject empty space, we start with one that only 
                    //as one neighbor. This will make it more likely that it returns a room that branches out
                    index = PickARandomRoom();
                    inc++;
                } while (HasMultipleNeighbors(m_TakenPositions[index]) && inc < MAX_ITERATIONS);
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
            Vector2Int newPosition;

            //capture its x, y position
            newPosition = new Vector2Int(m_TakenPositions[index].x, m_TakenPositions[index].y);

            bool UpDown = PerlinNoise.Random.Rand01_s() < 0.5f;                //randomly pick whether to look on horizontal or vertical axis
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
            return Mathf.RoundToInt(PerlinNoise.Random.Rand01_s() * (m_TakenPositions.Count - 1)); // pick a random room
        }

        private bool HasMultipleNeighbors(Vector2Int checkingPos)
        {
            return NumberOfNeighbors(checkingPos) > 1;
        }

        private int NumberOfNeighbors(Vector2Int checkingPos)
        {
            int ret = 0; // start at zero, add 1 for each side there is already a room
            ret += m_TakenPositions.Contains(checkingPos + Vector2Int.right) ? 1 : 0;
            ret += m_TakenPositions.Contains(checkingPos + Vector2Int.left) ? 1 : 0;
            ret += m_TakenPositions.Contains(checkingPos + Vector2Int.up) ? 1 : 0;
            ret += m_TakenPositions.Contains(checkingPos + Vector2Int.down) ? 1 : 0;

            return ret;
        }

        private void SetRoomDoors()
        {
            for (int x = 0; x < m_GridSize.x; x++)
            {
                for (int y = 0; y < m_GridSize.y; y++)
                {
                    int room = m_Rooms[x, y];
                    if (room == 0)
                    {
                        continue;
                    }
                    if (y - 1 >= 0 && m_Rooms[x, y - 1] != 0)
                    {
                        room |= (int)Room.RoomPosition.South;
                    }

                    if (y + 1 < m_GridSize.y && m_Rooms[x, y + 1] != 0)
                    {
                        room |= (int)Room.RoomPosition.North;
                    }

                    if (x - 1 >= 0 && m_Rooms[x - 1, y] != 0)
                    {
                        room |= (int)Room.RoomPosition.West;
                    }

                    if (x + 1 < m_GridSize.x && m_Rooms[x + 1, y] != 0)
                    {
                        room |= (int)Room.RoomPosition.East;
                    }

                    m_Rooms[x, y] = room;
                }
            }
        }
    }
}