//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

	private Animator animator;
	//private int direction;
	private float horizontal;
	private short move = 0;

	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponent<Animator> ();
		//direction = animator.parameters [0];
	}
	
	// Update is called once per frame
	void Update () {
		/*
		horizontal = Input.GetAxis ("Horizontal");
		if (horizontal > 0)
			animator.SetInteger ("direction", 6);
		else if (horizontal < 0)
			animator.SetInteger ("direction", 4);
		else
			animator.SetInteger ("direction", 0);
		Debug.Log (horizontal);
		*/
		if (Input.GetKeyDown (KeyCode.RightArrow)) {	//	true daca butonul a fost apasast , one time respond!
			move=6;
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {	//	true daca butonul a fost apasast , one time respond!
			move=4;
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)) {	//	true daca butonul a fost apasast , one time respond!
			move=8;
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {	//	true daca butonul a fost apasast , one time respond!	
			move=2;
		}

		if (Input.GetKey (KeyCode.RightArrow)) {	//	true daca butonul este apasat , while holding down
			if(move == 0)
				move = 6;
		}
		else {
			if (move == 6)
				move = 0;
		}

		if (Input.GetKey (KeyCode.LeftArrow)) { // true daca butonul este apasat , 
			if(move == 0)
				move = 4;			
		}
		else {
			if (move == 4)
				move = 0;
		}

		if (Input.GetKey (KeyCode.UpArrow)) { // true daca butonul este apasat , 
			if(move == 0)
				move = 8;			
		}
		else {
			if (move == 8)
				move = 0;
		}

		if (Input.GetKey (KeyCode.DownArrow)) { // true daca butonul este apasat , 
			if(move == 0)
				move = 2;			
		}
		else {
			if (move == 2)
				move = 0;
		}

		animator.SetInteger ("direction", move);
		//Debug.Log (move);
	}
}
