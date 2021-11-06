using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace TheDarkPath
{
    public class EventSystem : MonoBehaviour
    {
        private static Dictionary<int, Dictionary<EventType, EventCallBack>> callbacks = new Dictionary<int, Dictionary<EventType, EventCallBack>>();

        public delegate void EventCallBack();

        public static void RegisterEvent(int objectID, EventType eventName, EventCallBack newCallBack)
        {
            if (!callbacks.ContainsKey(objectID))
            {
                callbacks.Add(objectID, new Dictionary<EventType, EventCallBack>());
            }
            if (!callbacks[objectID].ContainsKey(eventName))
            {
                callbacks[objectID].Add(eventName, newCallBack);
            }
            else
            {
                callbacks[objectID][eventName] += newCallBack;
            }
        }

        public static void TriggerEvent(int objectID, EventType eventName)
        {
            callbacks[objectID]?[eventName]?.Invoke();
        }

        public static void DeleteEvent(int objectID, EventType eventName, EventCallBack eventCallBack)
        {
            callbacks[objectID][eventName] -= eventCallBack;
        }

        public static void DeleteEventInstance(int objectID)
        {
            Dictionary<EventType, EventCallBack> outCallbacks;
            if (callbacks.TryGetValue(objectID, out outCallbacks))
            {
                outCallbacks.Clear();
            }
        }


        public enum EventType
        {
            Shoot,
            SwitchWeapon,
            PlayerDamaged,
            NextRoom,
            PlayerDeath,
            Count,
        }

    }
}