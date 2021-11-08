using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheDarkPath
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(HealthObserver))]
    public class MovementController : MonoBehaviour
    {
        [SerializeField]
        public float speed = 10.0f;
        public float rotationSpeed = 10.0f;
        public float rotationAngle = 0f;

        private Rigidbody2D rb;
        private HealthObserver healthObserver;
        public float horz = 0.0f;
        public float vert = 0.0f;
        private bool facingLeft = false;


        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            GetMovement();
            FlipPlayer();
            var dir = Vector2.ClampMagnitude(new Vector2(horz, vert), 1f);
            rb.velocity = speed * dir;
        }

        private void Update()
        {
            ////////////////////////////////
            //var sceneController = GameObject.Find("Scene Controller");
            //if (Input.GetKeyDown(KeyCode.T))
            //{
            //    var currentRoom = sceneController.GetComponent<SceneController>().currentRoom;
            //    currentRoom.GetComponent<Room>().enemyCount--;
            //    if (currentRoom.GetComponent<Room>().enemyCount == 0)
            //    {
            //        foreach (PortalPoint point in currentRoom.GetComponent<Room>().portalPoints)
            //        {
            //            if (point.linkedRoom != null)
            //            {
            //                point.gameObject.SetActive(true);
            //            }
            //        }
            //        currentRoom.GetComponent<Room>().isDefeated = true;
            //    }
            //}
            ////////////////////////////////
        }

        void GetMovement()
        {
            horz = Input.GetAxisRaw("Horizontal");
            vert = Input.GetAxisRaw("Vertical");
            if (horz > 0)
            {
                facingLeft = false;
            }
            else if (horz < 0)
            {
                facingLeft = true;
            }
        }

        void FlipPlayer()
        {
            if (facingLeft)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Obstacle")
            {
                // TODO: add damage to enemies
                GetComponent<HealthObserver>().NotifyDamage(2);
            }
        }
    }
}