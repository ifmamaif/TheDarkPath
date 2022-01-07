using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ProjectileLogic : MonoBehaviour
    {
        public Vector2 StartingPosition { get; set; }
        private Vector2 direction;
        private Vector2 currentPosition;
        private Rigidbody2D rb;
        [HideInInspector]
        public float range;
        [HideInInspector]
        public float speed;
        [HideInInspector]
        public Transform ownerTransform;
        [SerializeField]
        public GameObject unit;
        [HideInInspector]
        public TargetProvider targetProvider;
        [SerializeField]
        public float damage;

        private string ownerTag;
        private int frameNr = 0;

        protected virtual void IHitSomething() { }

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Renderer>().enabled = false;
            gameObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            rb = GetComponent<Rigidbody2D>();
            direction = (Vector3)targetProvider.GetTarget() - ownerTransform.position;
            rb.rotation = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90f;
            rb.position += direction.normalized * 0.75f;
            StartingPosition = new Vector2(transform.position.x, transform.position.y);
            ownerTag = unit.tag;
        }

        // Update is called once per frame
        void Update()
        {
            frameNr++;
            if (frameNr > 5)
            {
                GetComponent<Renderer>().enabled = true;
            }
            rb.velocity = direction.normalized * speed;
            currentPosition = new Vector2(transform.position.x, transform.position.y);
            if (Vector2.Distance(StartingPosition, currentPosition) > range)
            {
                OnExpire();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject other = collision.gameObject;

            if (other.tag == "Player" && ownerTag == "Enemy")
            {
                other.GetComponent<HealthObserver>().NotifyDamage(1);
                Destroy(this.gameObject);
            }
            // TODO: Rewrite this to be more OOP / not dependant on the bullet            
            if (other.tag == "Enemy" && ownerTag == "Player")
            {
                EnemyHealthController healthController = other.GetComponent<EnemyHealthController>();
                if (healthController)
                {
                    healthController.NotifyDamage(damage);
                }
                Destroy(this.gameObject);
            }
            
            if (other.tag == "Wall")
            {
                IHitSomething();
                Destroy(this.gameObject);
            }

        }

        protected virtual void OnExpire()
        {
            Destroy(this.gameObject);
        }
        protected virtual void OnTargetHit()
        {
            Destroy(this.gameObject);
        }
    }
}