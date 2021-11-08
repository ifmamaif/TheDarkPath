using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{

    // Expand on this for other enemies that have special behaviours
    public class EnemyHealthController : MonoBehaviour
    {
        [SerializeField]
        private float healthAmount;
        // maybe we add shields later on
        [SerializeField]
        private float shieldAmount;

        private void OnDeath()
        {
            Destroy(this.gameObject);
        }

        public void NotifyDamage(float damageAmount)
        {
            healthAmount -= damageAmount;
            if (healthAmount <= 0f)
            {
                OnDeath();
            }
        }
    }
}