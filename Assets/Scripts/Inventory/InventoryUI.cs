using UnityEngine;

public class InventoryUI : MonoBehaviour {
	public Transform itemsParent;
	public GameObject inventoryUI;
	Inventory inventory;
	InventorySlot[] slots;

	// Use this for initialization
	void Start () {
		inventory = Inventory.instance;
		inventory.onItemChangedCallBack += UpdateUI;
		slots = itemsParent.GetComponentsInChildren<InventorySlot> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.I)) {
			inventoryUI.SetActive (!inventoryUI.activeSelf);
		}
	}

	void UpdateUI(){
		Debug.Log ("Updating InventoryUI");
		for (int i = 0; i < slots.Length; i++) {
			Debug.Log (i);
			if (i < inventory.items.Count) {
				slots [i].AddItem (inventory.items [i]);
				Debug.Log ("item added");
			} else {
				slots [i].ClearSlot ();
				Debug.Log ("item cleared");
			}
		}
	}

}
