using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{
    [Serializable]
    public class Weapon
    {
        [SerializeField]
        private float rateOfFire;
        //[SerializeField]
        //private float damage;
        [SerializeField]
        private float range;
        [SerializeField]
        private float speed;
        [SerializeField]
        private GameObject BulletPrefab;
        [SerializeField]
        private float weaponDamage;

        public Weapon(Transform parent, string type = "Bullet")
        {
            rateOfFire = 1f;
            //damage = 5f;
            range = 20f;
            speed = 10f;

            BulletPrefab = Resources.Load("Prefabs/" + type) as GameObject;
        }

        public void Shoot(Transform ownerTransform, GameObject unit, TargetProvider targetProvider)
        {
            var bullet = UnityEngine.Object.Instantiate(BulletPrefab, ownerTransform.position, Quaternion.identity);
            var bulletLogic = bullet.GetComponent<ProjectileLogic>();
            bulletLogic.unit = unit;
            bulletLogic.range = range;
            bulletLogic.speed = speed;
            bulletLogic.damage = weaponDamage;
            bulletLogic.ownerTransform = ownerTransform;
            bulletLogic.targetProvider = targetProvider;
        }

        public float GetRateOfFire()
        {
            return rateOfFire;
        }
    }
}