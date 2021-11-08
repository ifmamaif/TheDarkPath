using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheDarkPath
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory instance = null;

        public GameObject InventoryUI;
        public Transform itemsParent;

        private InventorySlot[] inventorySlots;
        private bool pickUpItem = false;
        private Cooldown cooldownPickUp = new Cooldown();

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("There are multiple instance of inventory");
            }


            if (InventoryUI == null)
            {
                Debug.LogError("InventoryUI member is null");
            }

            if (itemsParent == null)
            {
                Debug.LogError("itemsParent member is null");
            }

            inventorySlots = itemsParent.GetComponentsInChildren<InventorySlot>();


            instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown("i"))
            {
                InventoryUI.SetActive(!InventoryUI.activeSelf);
            }
            if (Input.GetKey("e"))
            {
                pickUpItem = true;
            }
            else
            {
                pickUpItem = false;
            }

            if (Input.GetKeyDown("1"))
            {
                inventorySlots[0].item?.Use();
            }
            if (Input.GetKeyDown("2"))
            {
                itemsParent.GetComponentsInChildren<Image>()[3].color = new Color(1, 1, 1, 1f);
                itemsParent.GetComponentsInChildren<Image>()[5].color = new Color(1, 1, 1, 0.5f);
                inventorySlots[1].enabled = true;
                inventorySlots[2].enabled = false;
            }
            else if (Input.GetKeyDown("3"))
            {
                itemsParent.GetComponentsInChildren<Image>()[3].color = new Color(1, 1, 1, 0.5f);
                itemsParent.GetComponentsInChildren<Image>()[5].color = new Color(1, 1, 1, 1f);
                inventorySlots[1].enabled = false;
                inventorySlots[2].enabled = true;
            }

            cooldownPickUp.UpdateCooldowns();
        }

        public bool AddItem(Item item)
        {
            if (cooldownPickUp.IsOnCooldown())
            {
                return false;
            }

            if (pickUpItem == false)
            {
                return false;
            }

            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].AddItem(item, this.transform))
                {
                    cooldownPickUp.RestoreCooldown(0.3f);
                    return true;
                }
            }

            Debug.Log("Not enough slots in inventory");
            return false;
        }

        public void RemoveItem(Item item)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].item == item)
                {
                    inventorySlots[i].RemoveItem();
                    return;
                }
            }

            Debug.Log("Item not found to remove from inventory");
        }

        public void DropItem(Item item)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].item == item)
                {
                    inventorySlots[i].DropItem(this.gameObject.transform);
                    return;
                }
            }

            Debug.Log("Item not found to remove from inventory");
        }
    }
}