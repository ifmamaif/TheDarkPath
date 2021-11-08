using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheDarkPath
{
    public class Pickable : MonoBehaviour
    {
        public Item item;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (item == null)
            {
                return;
            }

            GameObject other = collision.gameObject;
            if (other.tag == "Player")
            {
                //Pick the item
                if (!other.GetComponent<Inventory>().AddItem(item))
                {
                    return;
                }

                Debug.Log("Picked up the item " + item.name);
                Destroy(this.gameObject);
            }
        }
    }
}