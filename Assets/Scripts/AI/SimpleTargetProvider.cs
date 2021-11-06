using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{
    public class SimpleTargetProvider : TargetProvider
    {
        public Transform targetTransform;

        public override Vector2 GetTarget()
        {
            return targetTransform.position;
        }
    }

}