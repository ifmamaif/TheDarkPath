using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{
    public class HeartsHealthSystem
    {
        private List<Heart> heartList;
        public const int MAX_FRAGMENT_AMOUNT = 4;

        public HeartsHealthSystem(int heartAmount)
        {
            heartList = new List<Heart>();
            for (int i = 0; i < heartAmount; i++)
            {
                Heart heart = new Heart(4);
                heartList.Add(heart);
            }
        }

        public List<Heart> GetHeartList()
        {
            return heartList;
        }

        public void Damage(int damageAmount)
        {
            // Cycle hearts and apply damage to one or more hearts depending on damage
            for (int i = heartList.Count - 1; i >= 0; i--)
            {
                Heart heart = heartList[i];
                if (damageAmount > heart.GetFragmentsAmount())
                {
                    // Heart is depleted and damage carries over.
                    damageAmount -= heart.GetFragmentsAmount();
                    heart.Damage(heart.GetFragmentsAmount());
                }
                else
                {
                    // Heart takes all damage
                    heart.Damage(damageAmount);
                    break;
                }
            }
        }

        public void Heal(int healAmount)
        {
            for (int i = 0; i < heartList.Count; i++)
            {
                Heart heart = heartList[i];
                int missingFragments = MAX_FRAGMENT_AMOUNT - heart.GetFragmentsAmount();
                if (healAmount > missingFragments)
                {
                    healAmount -= missingFragments;
                    heart.Heal(missingFragments);
                }
                else
                {
                    heart.Heal(healAmount);
                    break;
                }
            }
        }

        public bool IsDead()
        {
            return heartList[0].GetFragmentsAmount() == 0;
        }

        // A single heart
        public class Heart
        {
            private int fragments;

            public Heart(int fragments)
            {
                this.fragments = fragments;
            }

            public void SetFragments(int fragments)
            {
                if (fragments > 4 || fragments < 0)
                {
                    Debug.LogWarning("Invalid Amount of fragments.");
                    return;
                }
                this.fragments = fragments;
            }

            public int GetFragmentsAmount()
            {
                return fragments;
            }

            public void Damage(int damageAmount)
            {
                if (damageAmount >= fragments)
                {
                    fragments = 0;
                }
                else
                {
                    fragments -= damageAmount;
                }
            }

            public void Heal(int healAmount)
            {
                if (fragments + healAmount > MAX_FRAGMENT_AMOUNT)
                {
                    fragments = MAX_FRAGMENT_AMOUNT;
                }
                else
                {
                    fragments += healAmount;
                }
            }
        }

    }
}