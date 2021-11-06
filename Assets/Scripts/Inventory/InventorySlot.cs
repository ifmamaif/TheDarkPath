using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheDarkPath
{
    public class InventorySlot : MonoBehaviour
    {
        public Image icon;
        public Item item = null;
        public Item.Type type = Item.Type.Unknown;
        public Text uiButton = null;

        public void Start()
        {
            item = null;

            if (uiButton == null)
            {
                Debug.LogError("uiButton is null");
            }
        }

        public bool AddItem(Item newItem, Transform parentTransform)
        {
            if (!enabled ||
                newItem == null ||
                (type != Item.Type.Unknown && type != newItem.type))
            {
                return false;
            }

            if (item != null)
            {
                Inventory.instance.DropItem(item);
            }

            item = newItem;

            icon.sprite = item.icon;
            icon.enabled = true;
            uiButton.enabled = true;
            Debug.Log("Add item " + item.name);
            return true;
        }

        public void RemoveItem()
        {
            Debug.Log("Remove item " + item.name);

            item = null;
            icon.sprite = null;
            icon.enabled = false;
            uiButton.enabled = false;
        }

        public void DropItem(Transform parent)
        {
            Debug.Log("Drop item " + item.name);

            GameObject newGameObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Items/GenericItem"));
            newGameObject.transform.position = parent.position;
            newGameObject.name = item.name;

            var pickable = newGameObject.GetComponent<Pickable>();
            pickable.item = item;

            newGameObject.GetComponent<SpriteRenderer>().sprite = item.icon;

            RemoveItem();
        }
    }
}