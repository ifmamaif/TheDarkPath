using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{
    public class PlayerHandController : MonoBehaviour
    {
        //private float rotationAngle = 0f;
        //private float rotationSpeed = 10.0f;
        [SerializeField]
        private GameObject player = null;

        private void FixedUpdate()
        {
            Rotate();
        }

        void Rotate()
        {
            //Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            //rotationAngle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            //Quaternion rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
            //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            difference.Normalize();
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

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