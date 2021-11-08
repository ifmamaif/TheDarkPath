using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TheDarkPath
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ShootingSystem : EventBehaviour
    {
        [SerializeField]
        private List<Weapon> weapons = null;
        [SerializeField]
        protected Weapon currentWeapon = null;
        [SerializeField]
        private int idCurrentWeapon = 0;
        [SerializeField]
        private Cooldown cooldown;
        [SerializeField]
        private GameObject WeaponArm = null;
        [SerializeField]
        private TargetProvider targetProvider = null;

        // Start is called before the first frame update
        void Start()
        {
            if (weapons == null)
            {
                Debug.LogError("weapons is null");
            }

            if (targetProvider == null)
            {
                Debug.LogError("targetProvider is null");
            }

            RegisterEvent(EventSystem.EventType.Shoot, () => { Shoot(); });

            if (weapons.Count > 0)
            {
                currentWeapon = weapons[0];
            }

            cooldown = new Cooldown();

            RegisterEvent(EventSystem.EventType.SwitchWeapon, () =>
            {
                idCurrentWeapon = idCurrentWeapon == 0 ? (weapons.Count - 1) : (idCurrentWeapon - 1);
                currentWeapon = weapons[idCurrentWeapon];
            });
        }

        // Update is called once per frame
        void Update()
        {
            cooldown.UpdateCooldowns();
        }

        void Shoot()
        {
            if (cooldown.IsOnCooldown())
            {
                return;
            }
            // create bullet and let if fly
            cooldown.RestoreCooldown(currentWeapon.GetRateOfFire());
            if (WeaponArm != null)
            {
                currentWeapon.Shoot(WeaponArm.transform, this.gameObject, targetProvider);
            }
            else
            {
                currentWeapon.Shoot(transform, this.gameObject, targetProvider);
            }
        }


    }
}