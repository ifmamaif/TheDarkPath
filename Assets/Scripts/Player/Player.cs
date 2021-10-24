using UnityEngine;
//using System.Collections;
using UnityEditor;

public class Player	{
	private GameObject player;	 
	private SpriteRenderer sprite;
	private Animator animator;


	public Player(){
		player = new GameObject ("Player");
		player.tag= "Player";
		player.transform.position = new Vector3 (0, 0, -0.1f);

		player.AddComponent<SpriteRenderer> ();
		sprite = player.GetComponent<SpriteRenderer>();
		//sprite.sprite = Resources.Load < Sprite	> ("Player/bd02");

		Material playerMaterial = new Material(Shader.Find ("Sprites/Default"));
		playerMaterial.name = "playerMaterial";
		sprite.material = playerMaterial;

		player.AddComponent<AudioListener> ();

		player.AddComponent<BoxCollider2D> ();
		player.GetComponent<BoxCollider2D> ().size = new Vector2 (1.02f, 1.24f);

		player.AddComponent<Rigidbody2D> ();
		player.GetComponent<Rigidbody2D> ().isKinematic = true;

		player.AddComponent<TestCollision> ();

		player.AddComponent<Animator> ();
		animator = player.GetComponent<Animator> ();
		animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("Animations/Player");
	}

	public void Move(Vector2Int move,int speed){
		int direction = 0;
		if(move.x == 1)
			direction = 6;
		else if(move.x == -1)
			direction = 4;
		else if(move.y == 1)
			direction = 8;
		else if(move.y == -1)
			direction = 2;
		else 
			direction =0;
		animator.SetInteger ("direction", direction);
	}

	public GameObject DestroyGameObject(){
		return player;
	}
}