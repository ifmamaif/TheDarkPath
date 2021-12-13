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

        public Rigidbody2D rb;
        public float horz = 0.0f;
        public float vert = 0.0f;

        public SpriteRenderer imgSprite;

        void FixedUpdate()
        {
            GetMovement();
            var dir = Vector2.ClampMagnitude(new Vector2(horz, vert), 1f);
            rb.velocity = speed * dir;
        }

        void GetMovement()
        {
            horz = Input.GetAxisRaw("Horizontal");
            vert = Input.GetAxisRaw("Vertical");

            _ = horz < 0 ? imgSprite.flipX = true : horz > 0 ? imgSprite.flipX = false : true;
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