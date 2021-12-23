﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{
    public class PlayerHandController : MonoBehaviour
    {
        [SerializeField]
        private GameObject player;
        public new Camera camera;

        private void Start()
        {
            if (camera == null)
                Debug.LogError("camera is missing");
        }
        private void FixedUpdate()
        {
            Rotate();
        }

        void Rotate()
        {
            Vector2 difference = (camera.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

            RotateIf(rotationZ);
        }

        void RotateIf(float rotationZ)
        {
            if (rotationZ < -90f || rotationZ > 90f)
            {
                if (player.transform.eulerAngles.y == 0)
                {
                    transform.localRotation = Quaternion.Euler(180, 0, -rotationZ);
                }
                else if (player.transform.eulerAngles.y == 180)
                {
                    transform.localRotation = Quaternion.Euler(180, 180, -rotationZ);
                }
            }
        }
    }
}