using System.Collections.Generic;
using UnityEngine;

public class Inventory {
	#region 
	public static Inventory instance;
	void Awake(){	// derivated from MonoBehaviour script;
		if (instance != null) {
			Debug.LogWarning ("More instance of inventory!");
			return;
		}
		Debug.Log ("daaaaa");
		instance = this;
	}
	#endregion
	public Inventory(){
		if (instance != null) {
			Debug.LogWarning ("More instance of inventory!");
			return;
		}
		instance = this;
	}
	public delegate void OnItemChanged ();
	public OnItemChanged onItemChangedCallBack;
	public int Space = 20;
	public List<Item> items = new List<Item>();

	public bool Add(Item item){
		if(!item.isDefaultItem){
			if (items.Count >= Space) {
				Debug.Log ("Not enough space in inventory!");
				return false;
			}
			items.Add(item);
			if(onItemChangedCallBack != null)
				onItemChangedCallBack.Invoke ();
			//return true;
		}
		return true;
	}

	public void Remove(Item item){
		items.Remove(item);
		if(onItemChangedCallBack != null)
			onItemChangedCallBack.Invoke ();
	}

	public void DestroyGameObject(){
		items.Clear ();

	}
}
