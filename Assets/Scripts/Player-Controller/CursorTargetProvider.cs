using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{
    public class CursorTargetProvider : TargetProvider
    {
        public override Vector2 GetTarget()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}