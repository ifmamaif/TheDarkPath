using UnityEngine;

namespace TheDarkPath
{
	[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable")]
	public class Consumable : Item
	{
		// Called when the item is pressed in the inventory
		public override void Use()
		{
			Inventory.instance.RemoveItem(this);
		}

	}
}