using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace TheDarkPath
{
    public class CameraController : MonoBehaviour
    {
        public Transform target;

        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }
    }
}