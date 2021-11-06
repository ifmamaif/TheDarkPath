using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{

    [RequireComponent(typeof(Rigidbody2D))]
    public class Follower : MonoBehaviour
    {
        public Transform target;
        public float speed = 1f;
        public float clearance = 1f;

        private Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        void FixedUpdate()
        {
            var dir = target.position - transform.position;
            if (dir.magnitude > clearance)
            {
                rb.velocity = dir.normalized * speed;
                //transform.rotation = Quaternion.Euler(transform.rotation.x,
                //    transform.rotation.y,
                //    Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }
}