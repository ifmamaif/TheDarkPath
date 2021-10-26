using UnityEngine;	//	GameObject, Color
using System.IO;	//	files, IO;
using System;		//	BitConverter, Char.GetNumericValue, Convert.ToDouble

public class Land  {
	
	private Camera cam;
	private GameObject boardHolder;
	private GameObject terrain;

	public int perlinWidth;
	public int perlinHeight;

	public float perlinScale = 1f;

	public float perlinOffsetX = 0f;
	public float perlinOffsetY = 0f;

	[System.Serializable]
	public struct TerrainType {
		public string name;
		public double height;
		public Color colour;
	}
	public TerrainType[] regions;

	private Renderer thisrenderer;

	public bool autoUpdate = false;

	private float textureSize = 1.24f;	// texturi 124 x 124
	private Vector3 initialPosition;

	// Use this for initialization
	public Land (Vector2Int screen) {
		perlinWidth = screen.x + 2;
		perlinHeight = screen.y + 2;
		if (perlinWidth < perlinHeight)
			perlinWidth = perlinHeight;
		else
			perlinHeight = perlinWidth;


		string path = "Assets/Resources/Config/whatToPlay.cfg";
		FileStream f = new FileStream (path, FileMode.Open);
		int n = f.ReadByte ();
		string name = ((char)n).ToString ();
		for (int i = 1; i < f.Length; i++) {
			n = f.ReadByte ();
			name += ((char)n).ToString ();
		}
		f.Close ();

		path = "Assets/Resources/Saved/" + name + ".data";
		f = new FileStream (path, FileMode.Open);
		byte[] byteArray = new byte[sizeof(int)];

		f.Read (byteArray, 0, byteArray.Length);
		perlinOffsetX =  (float)BitConverter.ToInt32(byteArray, 0);
		f.Read (byteArray, 0, byteArray.Length);
		perlinOffsetY = (float) BitConverter.ToInt32(byteArray, 0);

		f.Close();

		boardHolder = new GameObject ("Terrain");					//Instantiate Board and set boardHolder to its transform.

		initialPosition = boardHolder.transform.position;

		terrain = GameObject.CreatePrimitive(PrimitiveType.Quad);	//	the land based on procedural generate by perlinnoise
		terrain.GetComponent<MeshCollider> ().enabled = false;
		Material landMaterial = new Material(Shader.Find ("Unlit/Texture"));
		landMaterial.name = "landMaterial";
		terrain.GetComponent<MeshRenderer> ().material = landMaterial;
		terrain.name = "Land";
		terrain.transform.SetParent(boardHolder.transform);

		thisrenderer = terrain.GetComponent<Renderer>();

		SetRegions ();

		UpdateMap ();
	}

	void Update(){
		UpdateMap ();
	}

	public void Move(Vector2Int move,int speed){
		if (move.x != 0) {
			boardHolder.transform.Translate (-move.x * textureSize / speed, 0, 0);

			if (	(int)(boardHolder.transform.position.x/ textureSize )	!=	(int)(initialPosition.x/ textureSize )	) {				
				terrain.transform.Translate (move.x * textureSize, 0, 0);
				perlinOffsetX += perlinScale/perlinWidth * move.x;
				UpdateMap ();
				initialPosition = boardHolder.transform.position;
			}

		}
		else if (move.y != 0) {
			boardHolder.transform.Translate (0, -move.y * textureSize / speed, 0);
			if (	(int)(boardHolder.transform.position.y/ textureSize )	!=	(int)(initialPosition.y/ textureSize )	) {				
				terrain.transform.Translate (0,move.y * textureSize, 0);
				perlinOffsetY += perlinScale/perlinHeight * move.y;
				UpdateMap ();
				initialPosition = boardHolder.transform.position;
			}
		}
	}

	void SetRegions(){
		string path = "Assets/Resources/regions.data";
		if (File.Exists (path)) {
			FileStream f = new FileStream (path, FileMode.Open);
			int n = Int32.Parse (IMFile.ReadLine (f));
			regions = new TerrainType[n];			
			for (int i = 0; i < n; i++) {				
				regions [i].name = IMFile.ReadLine (f);
				regions [i].height = Convert.ToDouble (IMFile.ReadLine (f));
				regions [i].colour = HexToColor ("#" + IMFile.ReadLine (f));
			}
			f.Close ();
		} else {
			Debug.LogError ("regions.data doesn't exist!");
		}
	}



	void UpdateMap(){
		if (terrain.transform.localScale != new Vector3(perlinWidth*textureSize,perlinHeight*textureSize,1)) {
			terrain.transform.localScale = new Vector3(perlinWidth*textureSize,perlinHeight*textureSize,1);
		}
		thisrenderer.material.mainTexture = TextureFromcolorMap();
	}

	public Texture2D TextureFromcolorMap() {
		Color[] colorMap = GeneratecolorMap ();
		Texture2D texture = new Texture2D(perlinWidth, perlinHeight)
		{
			filterMode = FilterMode.Point,
			wrapMode = TextureWrapMode.Clamp,
		};
		texture.SetPixels (colorMap);
		texture.Apply ();
		return texture;
	}

	public Color[] GeneratecolorMap(){	
		float[,] noiseMap = GenerateNoiseMap ();
		Color[] colorMap = new Color[perlinWidth * perlinHeight];
		for (int y = 0; y < perlinHeight; y++) {
			for (int x = 0; x < perlinWidth; x++) {
				for (int i = 0; i < regions.Length; i++) {
					if (noiseMap [x, y] <= regions [i].height) {
						colorMap [y * perlinWidth + x] = regions [i].colour;
						break;
					}
				}
			}
		}
		return colorMap;
	}

	public float[,] GenerateNoiseMap(){
		float[,] noiseMap = new float[perlinWidth,perlinHeight];
		for (int y = 0; y < perlinHeight; y++) {
			for (int x = 0; x < perlinWidth; x++) {
				float xCoord = (float)x / perlinWidth * perlinScale + perlinOffsetX;
				float yCoord = (float)y / perlinHeight * perlinScale + perlinOffsetY;
				noiseMap [x, y] = Mathf.PerlinNoise(xCoord,yCoord);
			}
		}
		return noiseMap;
	}

	public static Color HexToColor(string hex){
		ColorUtility.TryParseHtmlString (hex, out Color myColor);
		return myColor;
	}

	public GameObject DestroyGameObject(){		
		return boardHolder;
	}
}
