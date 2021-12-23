using UnityEngine;

namespace TheDarkPath
{
    // TODO: Add heal and death
    public class HealthObserver : EventBehaviour
    {
        [SerializeField] private int initialHealth = 0;
        [SerializeField] private HeartsHealthVisual healthVisual;
        HeartsHealthSystem healthSystem;

        public SceneController sceneControllerScript;

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
                sceneControllerScript.TriggerEvent(EventSystem.EventType.PlayerDeath);
            }
        }

        public void NotifyHeal(int healAmount)
        {
            healthSystem.Heal(healAmount);
        }
    }
}