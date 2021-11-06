using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{
    public class EnvironmentDamage : MonoBehaviour
    {
        // Start is called before the first frame update
        public int damageAmount;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                collider.gameObject.GetComponent<HealthObserver>().NotifyDamage(damageAmount);
            }
        }
    }
}