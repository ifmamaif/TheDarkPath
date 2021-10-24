using UnityEngine;

public class PerlinNoise : MonoBehaviour {


	public int width = 256;
	public int height = 256;

	public float scale = 20f;

	public float offsetX = 100f;
	public float offsetY = 100f;

	[System.Serializable]
	public struct TerrainType {
		public string name;
		public float height;
		public Color colour;
	}
	public TerrainType[] regions;

	private Renderer thisrenderer;

	public bool autoUpdate =false;

	void Start (){
		
		/*
		regions = new TerrainType[8];

		regions [0].name = "Water";
		regions [0].height = 0.3f;
		regions [0].colour = HexToColor("#3263C3");

		regions [1].name = "Water 2.0";
		regions [1].height = 0.4f;
		regions [1].colour = HexToColor("#3667C7");

		regions [2].name = "Sand";
		regions [2].height = 0.45f;
		regions [2].colour = HexToColor("#CFDD3E");

		regions [3].name = "Grass";
		regions [3].height = 0.55f;
		regions [3].colour = HexToColor("#569817");

		regions [4].name = "Grass 2.0";
		regions [4].height = 0.6f;
		regions [4].colour = HexToColor("#3E6B12");

		regions [5].name = "Rock";
		regions [5].height = 0.7f;
		regions [5].colour = HexToColor("#5A453C");

		regions [6].name = "Rock 2.0";
		regions [6].height = 0.9f;
		regions [6].colour = HexToColor("#4B3C35");

		regions [7].name = "Snow";
		regions [7].height = 1f;
		regions [7].colour = HexToColor ("#FFFFFF");
		*/

		// offsetX  = Random.Range();
		thisrenderer = GetComponent<Renderer>();

		//renderer.material.mainTexture = TextureFromcolorMap();

	}

	Color HexToColor(string hex){
		Color myColor = new Color ();
		ColorUtility.TryParseHtmlString (hex, out myColor);
		return myColor;
	}

	void Update(){
		if(autoUpdate)
			thisrenderer.material.mainTexture = TextureFromcolorMap();
	}   

	public Texture2D TextureFromcolorMap() {
		Color[] colorMap = GeneratecolorMap ();
		Texture2D texture = new Texture2D (width, height);
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels (colorMap);
		texture.Apply ();
		return texture;
	}

	public Color[] GeneratecolorMap(){	
		float[,] noiseMap = GenerateNoiseMap ();
		Color[] colorMap = new Color[width * height];

		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {

				for (int i = 0; i < regions.Length; i++) {
					
					if (noiseMap [x, y] <= regions [i].height) {
						colorMap [y * width + x] = regions [i].colour;
						break;
					}

				}

			}
		}

		return colorMap;
	}

	public float[,] GenerateNoiseMap(){
		float[,] noiseMap = new float[width,height];

		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {

				float xCoord = (float)x / width * scale + offsetX;
				float yCoord = (float)y / height * scale + offsetY;

				noiseMap [x, y] = Mathf.PerlinNoise(xCoord,yCoord);
			}
		}

		return noiseMap;
	}

}
