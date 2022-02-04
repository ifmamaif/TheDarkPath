using UnityEngine;

namespace TheDarkPath
{
    // TODO: Add heal and death
    [RequireComponent(typeof(PlayerSFX))]
    public class HealthObserver : EventBehaviour
    {
        [SerializeField] private int initialHealth;
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
            gameObject.GetComponent<PlayerSFX>().playHurt();
            healthSystem.Damage(damageAmount);
            healthVisual.RefreshAllHearts();
            if (healthSystem.IsDead())
            {
                sceneControllerScript.TriggerEvent(EventSystem.EventType.PlayerDeath);
            }
        }

        public void RestartHeal()
        {
            healthSystem.Heal((initialHealth * HeartsHealthSystem.MAX_FRAGMENT_AMOUNT) - healthSystem.GetAllFragments());
            healthVisual.RefreshAllHearts();
        }
    }
}