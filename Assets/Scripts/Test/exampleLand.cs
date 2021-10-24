using UnityEngine; // GameObject
using System.Collections; // Transform.Translate
using Random = UnityEngine.Random; 		//Tells Random to use the Unity Engine random number generator. //
using System.IO; // file

//using System;
//using UnityEngine.UI;	//Allows us to use UI.
//using UnityEngine.SceneManagement;
//using System.Collections.Generic; 		//Allows us to use Lists.

public class exampleLand{
	private GameObject boardHolder;	//	A variable to store a reference to the transform of our Board object.	// GameObject or Transform
	private GameObject[,] terrain;	//undeva / cumva matricea maxima ar fi 23170 pe 23170

	private int rows;	//	Number of rows in our game board.
	private int columns;	//	Number of columns in our game board.	

	//private int textureSize = 124;	// texturi 124 x 124
	private float textureSize = 1.24f;	// texturi 124 x 124
	private float coordError = 0.5f; // ajustarea pozitiilor in spatiu a elementelor din matrice

	private int toMoveHorizontal=0;
	private int toMoveVertical=0;
	//private int indiceVertical = 0;
	//private IEnumerator coroutine; //coroutine = PlayLoop (x, y);StartCoroutine (coroutine);

	private Vector3 initialPosition;

	public exampleLand(Vector2 backScreen)	{
		rows = (int)(backScreen.y / textureSize/100) +3;	// +1 pentru eventuale spatiu nefolosit , +2 pentru mutarea liniilor
		columns = (int)(backScreen.x / textureSize/100)+ 3;	// +1 pentru eventuale spatiu nefolosit , +2 pentru mutarea coloanelor

		//Debug.Log (rows+" "+columns);

		terrain = new GameObject[rows, columns];
		boardHolder = new GameObject ("Terrain");		//Instantiate Board and set boardHolder to its transform.
		initialPosition = boardHolder.transform.position;

		//string path = "Assets/Resources/test.txt";
		//string path = "Assets/Resources/mat.txt";
		//FileStream f = new FileStream (path, FileMode.Open);
		//int n=f.ReadByte ();



		for(int x = 0; x < rows; x++)		//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
		{			
			for(int y = 0; y < columns; y++)		//Loop along y axis, starting from -1 to place floor or outerwall tiles.
			{	
				terrain[x,y] = new GameObject("[ "+x+" , "+y+" ]");
				terrain [x,y].AddComponent<SpriteRenderer> ();
				SpriteRenderer sprite = terrain[x,y].GetComponent<SpriteRenderer>();

				int i = Random.Range (0, 3);
				//int i = f.ReadByte();

				//Debug.Log (x+" "+y+" "+i.ToString ());

				sprite.sprite = Resources.Load <Sprite> (i.ToString());	// as Sprite;
				terrain[x,y].transform.position = new Vector3((y - (float)columns/2 +coordError) * textureSize , (x - (float)rows/2 +coordError) * textureSize);
				terrain [x,y].transform.SetParent (boardHolder.transform);		//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
			}
		}

		AddBlock (boardHolder.transform,new Vector3(-1,-1,-0.05f),"Item1");
		AddBlock (boardHolder.transform,new Vector3(-3,-1,-0.05f),"Item2");
		AddBlock (boardHolder.transform,new Vector3(-2,-1,-0.05f),"Item3");

		//f.Close ();
	}

	public void ReSizeLand(Vector2 backScreen)	{
		rows = (int)(backScreen.y / textureSize) +3;	// +1 pentru eventuale spatiu nefolosit , +2 pentru mutarea liniilor
		columns = (int)(backScreen.x / textureSize)+ 3;	// +1 pentru eventuale spatiu nefolosit , +2 pentru mutarea coloanelor

		terrain = new GameObject[rows, columns];
		boardHolder = new GameObject ("Terrain");		//Instantiate Board and set boardHolder to its transform.
		for(int x = 0; x < rows; x++)		//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
		{			
			for(int y = 0; y < columns; y++)		//Loop along y axis, starting from -1 to place floor or outerwall tiles.
			{	
				terrain[x,y] = new GameObject(x+","+y);
				terrain [x,y].AddComponent<SpriteRenderer> ();
				SpriteRenderer sprite = terrain[x,y].GetComponent<SpriteRenderer>();
				int i = Random.Range (0, 3);
				sprite.sprite = Resources.Load <Sprite> (i.ToString());	// as Sprite;
				terrain[x,y].transform.position = new Vector3((y - (float)columns/2 +coordError) * textureSize , (x - (float)rows/2 +coordError) * textureSize);
				terrain [x,y].transform.SetParent (boardHolder.transform);		//Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
			}
		}
	}

	public GameObject GetboardHolder(){
		return boardHolder;
	}



	public void Move(short move,int speed){		

		if (move == 2) {
			boardHolder.transform.Translate (0, textureSize / speed, 0);
		}
		if (move == 4) {
			boardHolder.transform.Translate (textureSize / speed, 0, 0);
		}
		if (move == 6) {
			boardHolder.transform.Translate (-textureSize / speed, 0, 0);
		}
		if (move == 8) {
			boardHolder.transform.Translate (0, -textureSize / speed, 0);
		}

		if (move == 2) {
			if (	(int)(boardHolder.transform.position.y/ textureSize )	!=	(int)(initialPosition.y/ textureSize )	) {				
				int deMutat = rows - 1 - toMoveVertical;
				int inainteDe = (rows - toMoveVertical) % rows;
				for (int i = 0; i < columns; i++) {					
					terrain [deMutat, i].transform.position = new Vector3 (
						terrain [inainteDe, i].transform.position.x ,
						terrain [inainteDe, i].transform.position.y - textureSize, 
						0);
				}
				toMoveVertical++;
				if (toMoveVertical == rows) {
					toMoveVertical = 0;
				}
				initialPosition = boardHolder.transform.position;
			}
		}

		if (move == 4) {
			if (	(int)(boardHolder.transform.position.x/ textureSize )	!=	(int)(initialPosition.x/ textureSize )	) {				
				int deMutat = columns - 1 - toMoveHorizontal;
				int inainteDe = (columns - toMoveHorizontal) % columns;
				for (int i = 0; i < rows; i++) {					
					terrain [i, deMutat].transform.position = new Vector3 (
						terrain [i, inainteDe].transform.position.x - textureSize,
						terrain [i, inainteDe].transform.position.y, 
						0);
				}
				toMoveHorizontal++;
				if (toMoveHorizontal == columns) {
					toMoveHorizontal = 0;
				}
				initialPosition = boardHolder.transform.position;
			}
		}

		if (move == 6) {
			if (	(int)(boardHolder.transform.position.x/ textureSize )	!=	(int)(initialPosition.x/ textureSize )	) {						
				int deMutat = (columns - toMoveHorizontal) % columns;
				int inainteDe = columns - 1 - toMoveHorizontal;
				for (int i = 0; i < rows; i++) {					
					terrain [i, deMutat].transform.position = new Vector3 (
						terrain [i, inainteDe].transform.position.x + textureSize,
						terrain [i, inainteDe].transform.position.y, 
						0);
				}
				toMoveHorizontal--;
				if (toMoveHorizontal == -1) {
					toMoveHorizontal = columns - 1;
				}
				initialPosition = boardHolder.transform.position;
			}
		}

		if (move == 8) {
			if (	(int)(boardHolder.transform.position.y/ textureSize )	!=	(int)(initialPosition.y/ textureSize )	) {				
				int deMutat = (rows - toMoveVertical) % rows;
				int inainteDe = rows - 1 - toMoveVertical;
				for (int i = 0; i < columns; i++) {					
					terrain [deMutat, i].transform.position = new Vector3 (
						terrain [inainteDe, i].transform.position.x ,
						terrain [inainteDe, i].transform.position.y + textureSize, 
						0);
				}
				toMoveVertical--;
				if (toMoveVertical == -1) {
					toMoveVertical = rows-1;
				}
				initialPosition = boardHolder.transform.position;
			}
		}



	}


	void AddBlock(Transform boardHolder,Vector3 pos,string name){
		
		GameObject obj = new GameObject(name);
		//obj.transform.position = new Vector3 (-1, -1, -0.05f);
		obj.transform.position = pos;
		obj.AddComponent<SpriteRenderer> ();
		obj.GetComponent<SpriteRenderer>().sprite = Resources.Load <Sprite> ("cheie");	// as Sprite;
		obj.AddComponent<BoxCollider2D>();
		obj.GetComponent<BoxCollider2D> ().isTrigger = true;
		obj.AddComponent<ItemPickUp> ();
		obj.GetComponent<ItemPickUp> ().item = Resources.Load<Item> ("Items/Helmet");
		//obj.AddComponent<Interactable> ();
		//obj.AddComponent<TestCollision>();
		obj.transform.SetParent (boardHolder);	
	}
}