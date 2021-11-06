using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{
    public class EventBehaviour : MonoBehaviour
    {
        // Important : related events should be on the same gameObject

        public void RegisterEvent(EventSystem.EventType eventName, EventSystem.EventCallBack callBack)
        {
            EventSystem.RegisterEvent(this.gameObject.GetInstanceID(), eventName, callBack);
        }

        public void TriggerEvent(EventSystem.EventType eventName)
        {
            EventSystem.TriggerEvent(this.gameObject.GetInstanceID(), eventName);
        }

        private void OnDestroy()
        {
            EventSystem.DeleteEventInstance(this.gameObject.GetInstanceID());
        }

    }
}