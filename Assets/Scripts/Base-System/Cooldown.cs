using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{
    [Serializable]
    public class Cooldown
    {
        [SerializeField]
        private float cooldown;

        public Cooldown()
        {
            cooldown = 0f;
        }

        public void UpdateCooldowns()
        {
            if (cooldown < 0f)
            {
                cooldown = 0f;
            }
            else if (cooldown > 0f)
            {
                cooldown -= Time.deltaTime;
            }
        }

        public bool IsOnCooldown()
        {
            return cooldown > 0f;
        }

        public void RestoreCooldown(float newValue)
        {
            cooldown = newValue;
        }
    }
}