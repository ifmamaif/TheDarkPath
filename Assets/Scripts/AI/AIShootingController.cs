using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{
    public class AIShootingController : EventBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            // Very simple for now, just spam the enemy shoot event
            TriggerEvent(EventSystem.EventType.Shoot);
        }
    }
}