using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour {
	private byte thisType;
	private byte thisId;
	private GameObject thisObject;

	//floor:player can walk
	//item:food,equipment,..
	//block:player can't move forward : tree,rock,...
	//water?
	//player/character/npc/enemy?
	//extrafloor:grass,flowers

	//char = 1 byte = 8 bits ... 128 or 256 values


	public MapObject(byte type,byte id){
		this.thisType = type;
		this.thisId = id;
	}

	public MapObject(MapObject other){
		this.thisType = other.thisType;
		this.thisId = other.thisId;
		this.thisObject = other.thisObject; 
	}

	public void Render(Object prefab,Vector3 position ){	//Instantiate the GameObject instance
		thisObject = Instantiate (prefab, position , Quaternion.identity) as GameObject;		
	}

	public void ChangeObject(MapObject toChange){	// Change data Object but no render;
		this.thisType = toChange.thisType;
		this.thisId = toChange.thisId;
	} 

	public void ChangeRender(MapObject toChange,Vector3 position){ // Change Object and render it
		this.thisType = toChange.thisType;
		this.thisId = toChange.thisId;
		Render (toChange, position);
	}
}
