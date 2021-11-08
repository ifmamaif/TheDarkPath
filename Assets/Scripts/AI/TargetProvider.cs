using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{
    [Serializable]
    public abstract class TargetProvider : MonoBehaviour
    {
        public abstract Vector2 GetTarget();
    }
}