using UnityEngine;

public class TestCollision : MonoBehaviour {

	void OnCollisionEnter(Collision col){
		Debug.Log ("3D " + gameObject.name + " has collided with " + col.collider.name);
	}

	void OnTriggerEnter(Collider other){
		Debug.Log ("3D :" + gameObject.name + " was trigger by " + other.gameObject.name);
	}


	void OnCollisionEnter2D(Collision2D col){
		Debug.Log ("2D " + gameObject.name + " has collided with " + col.collider.name);
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("2D :" + gameObject.name + " was trigger by " + other.gameObject.name);
	}
}
