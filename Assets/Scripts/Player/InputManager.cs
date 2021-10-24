using UnityEngine; // GameObject and Input

public class InputManager {
	private short move = 0;

	//private Land terrain;
	//private Player player;

	public InputManager(){
	//public InputManager(Land terrain , Player player){
		//this.terrain = terrain;
		//this.player = player;
	}

	//Input.GetKeyDown este adevarat doar cand s-a apasat butonul , nu este adevarat daca butonul este in continuare apasat !!!
	//Input.GetKeyUp este adevarat cand butonul respectiv s-a ridicat / i s-a dat drumul , cu conditia ca un buton sa fie apasat in acel timp

	public Vector2Int SetMove(){
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			move = 6;
		}
		if (Input.GetKey (KeyCode.RightArrow)) { // true daca butonul este apasat , 
			if (move == 0)
				move = 6;
		} else {
			if (move == 6)
				move = 0;
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {	
			move = 4;
		}
		if (Input.GetKey (KeyCode.LeftArrow)) { // true daca butonul este apasat , 
			if (move == 0)
				move = 4;			
		} else {
			if (move == 4)
				move = 0;
		}

		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			move = 8;
		}
		if (Input.GetKey (KeyCode.UpArrow)) { // true daca butonul este apasat , 
			if (move == 0)
				move = 8;			
		} else {
			if (move == 8)
				move = 0;
		}

		if (Input.GetKeyDown (KeyCode.DownArrow)) {	
			move = 2;
		}
		if (Input.GetKey (KeyCode.DownArrow)) { // true daca butonul este apasat , 
			if (move == 0)
				move = 2;			
		} else {
			if (move == 2)
				move = 0;
		}


		if (move == 2) {
			return new Vector2Int (0, -1);
		}
		else if (move == 4) {
			return new Vector2Int (-1, 0);
		}
		else if (move == 6) {
			return new Vector2Int (1, 0);
		}
		else if (move == 8) {
			return new Vector2Int (0, 1);
		}
		return new Vector2Int (0, 0);
	}

	public Vector2Int Control(){
		/*
		Vector2Int direction = SetMove ();
		Debug.Log (direction);
		if (direction.x != 0 && direction.y != 0) {			
			terrain.Move (direction, speed);

		}
		*/
		return SetMove ();
		//Move (new Vector2Int (0, -1), speed);
		//player.Move (move,speed);
		//terrain.Move (move,speed);

	}

}