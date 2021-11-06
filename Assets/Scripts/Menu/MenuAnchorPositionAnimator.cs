using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{
    public class MenuAnchorPositionAnimator : MonoBehaviour
    {
        public Vector2 initialPos;
        public Vector2 targetPos;
        private float interp = 0f;
        // Start is called before the first frame update
        void Start()
        {
            initialPos = transform.position;
            targetPos.x = transform.position.x;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = Vector2.Lerp(initialPos, targetPos, interp);
            interp += Time.deltaTime;
            if (interp >= 1f)
            {
                interp = 0;
                Vector2 aux = new Vector2(targetPos.x, targetPos.y);
                targetPos = initialPos;
                initialPos = aux;
            }
        }
    }
}