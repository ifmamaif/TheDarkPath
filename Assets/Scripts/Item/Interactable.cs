using UnityEngine;
using System.Collections; // collision

public class Interactable : MonoBehaviour {
	/*
	public float radius = 3f;
	public Transform interactionTransform;

	void OnDrawGizmosSelected(){
		if (interactionTransform == null) {
			interactionTransform = transform;
		}

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (transform.position, radius);
	}	
	*/

	public virtual void Interact(){
		Debug.Log ("Interacting with " + transform.name);
	}

	//void OnCollisionEnter2D(Collision2D coll){
	//	Debug.Log (gameObject.name+" collid with "+coll.collider.name);
	///	Destroy (gameObject);
	//}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("2D :" + gameObject.name + " was trigger by " + other.gameObject.name);
		Interact ();
		//Destroy (gameObject);
	}
}
