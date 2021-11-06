using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{
    public class PortalPoint : MonoBehaviour
    {
        [SerializeField]
        public GameObject linkedRoom = null;
        [SerializeField]
        public Position position = Position.Unknown;
        [SerializeField]
        public Transform playerSpawnPosition = null;

        public enum Position
        {
            Unknown = -1,
            North = 0,
            South,
            East,
            West,
            Count,
        }

        // Start is called before the first frame update
        void Start()
        {
            if (position == Position.Unknown)
            {
                Debug.LogError("Invalid position");
            }

            if (playerSpawnPosition == null)
            {
                Debug.LogError("player spawn position is not assigned");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player") ||
                linkedRoom == null)
            {
                return;
            }

            TeleportPlayerToNewRoom(collision.transform);
        }

        void TeleportPlayerToNewRoom(Transform playerTransform)
        {
            // TODO: do not let this commented into final version you dumbfuck!!!!!!
            //this.transform.parent.gameObject.SetActive(false);
            //linkedRoom.SetActive(true);

            PortalPoint portalPoint = linkedRoom.GetComponent<Room>().portalPoints[FindBackPortalIndex(position)];
            Vector3 newPlayerPosition = portalPoint.playerSpawnPosition.position;
            playerTransform.position = newPlayerPosition;
            linkedRoom.GetComponent<Room>().OnRoomEnter();
        }

        int FindBackPortalIndex(Position previousPosition)
        {
            int i = 0;
            switch (previousPosition)
            {
                case Position.North:
                    i = (int)Position.South;
                    break;
                case Position.South:
                    i = (int)Position.North;
                    break;
                case Position.East:
                    i = (int)Position.West;
                    break;
                case Position.West:
                    i = (int)Position.East;
                    break;
                default:
                    Debug.Log("Esti prost");
                    break;
            }

            return i;
        }

    }
}