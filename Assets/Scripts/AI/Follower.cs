using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(EnemySFX))]
    public class Follower : MonoBehaviour
    {
        void Start()
        {
            gameObject.GetComponent<EnemySFX>().playHover();
        }

    }
}