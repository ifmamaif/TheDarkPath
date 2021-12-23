using UnityEngine;

namespace TheDarkPath
{
	[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
	public class Item : ScriptableObject
	{
		new public string name = "New Item";        // Name of the item
		public Sprite icon = null;              // Item icon
		public Type type = Type.Unknown;
		public bool isDefaultItem = false;

		public enum Type
		{
			Unknown,

			Consumable,
			Weapon,
			Active,

			Count,
		}

		// Called when the item is pressed in the inventory
		public virtual void Use()
		{
			// Use the item
			Debug.Log("Used item " + name);
		}
	}
}