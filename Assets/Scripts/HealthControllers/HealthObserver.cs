using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheDarkPath
{
    // TODO: Add heal and death
    public class HealthObserver : EventBehaviour
    {
        [SerializeField] private int initialHealth = 0;
        [SerializeField] private HeartsHealthVisual healthVisual = null;
        HeartsHealthSystem healthSystem;

        // Start is called before the first frame update
        void Start()
        {
            healthSystem = new HeartsHealthSystem(initialHealth);
            healthVisual.SetHeartsHealthSystem(healthSystem);
        }

        public void NotifyDamage(int damageAmount)
        {
            healthSystem.Damage(damageAmount);
            healthVisual.RefreshAllHearts();
            if (healthSystem.IsDead())
            {
                GameObject.Find("Scene Controller").GetComponent<SceneController>().TriggerEvent(EventSystem.EventType.PlayerDeath);
            }
        }

        public void NotifyHeal(int healAmount)
        {
            healthSystem.Heal(healAmount);
        }
    }
}