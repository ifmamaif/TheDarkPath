﻿using UnityEngine;

namespace TheDarkPath
{
	public class ItemPickUp : Interactable
	{

		public Item item;

		public override void Interact()
		{
			base.Interact();
			PickUp();
		}

		void PickUp()
		{
			Debug.Log("Picking up " + item.name);
			// Add to inventory
			bool wasPickedUp = Inventory.instance.AddItem(item);
			if (wasPickedUp)
				Destroy(gameObject);
		}
	}
}