using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(EnemySFX))]
    public class Follower : MonoBehaviour
    {
        public Transform target;
        public float speed = 1f;
        public float clearance = 1f;

        private Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            gameObject.GetComponent<EnemySFX>().playHover();
        }

        void FixedUpdate()
        {
            var dir = target.position - transform.position;
            if (dir.magnitude > clearance)
            {
                rb.velocity = dir.normalized * speed;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }
}