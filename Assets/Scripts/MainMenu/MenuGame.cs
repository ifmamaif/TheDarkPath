using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Text
using System.IO;	//	File, FileStream , GetFiles;

public class MenuGame : MonoBehaviour {
	public GameObject mainMenu;
	public GameObject savedGames;
	public GameObject settings;

	private bool isVisibleSavedGames = false;
	private Vector2 scrollViewVector = Vector2.zero;
	private Vector2 guiDimension = new Vector2(410,340);
	private float longlist = 0;

	public GameObject soundButton;
	private bool isSoundMuted = false;

	private string[] listSavedGames;
	private string whatSavedGame="";
	public Text textLoadButton;


	// Use this for initialization
	void Start () {
		mainMenu.SetActive (true);
		savedGames.SetActive (false);
		settings.SetActive (false);
		isVisibleSavedGames = false;
	}

	void OnGUI(){
		if (isVisibleSavedGames == true) {
			// Begin the ScrollView
			scrollViewVector = GUI.BeginScrollView (new Rect (Screen.width / 2 - guiDimension.x / 2, Screen.height / 2 - guiDimension.y / 2, guiDimension.x, guiDimension.y), scrollViewVector, new Rect (0, 0, 0, longlist));
			GUI.BeginGroup (new Rect (0, 0, guiDimension.x, longlist));	//	Bottom right group of buttons
			int k=0;
			for (int i = 0; i < listSavedGames.Length; i++) {	
				string textSavedGame =  IMFile.Filter (listSavedGames[i]);					
				if (textSavedGame == ".") {	
					k++;
				} else if (textSavedGame == "..") {
					k++;
				} else {
					if (GUI.Button (new Rect (0, ((i - k) * 60) + ((i - k) * 10), guiDimension.x - 90, 60),	textSavedGame)) {
						//Debug.Log ("You pressed " + textSavedGame);
						whatSavedGame = textSavedGame;
						textLoadButton.text = "Load " + whatSavedGame;

					}
					if (GUI.Button (new Rect (guiDimension.x - 80, ((i - k) * 60) + ((i - k) * 10), 50, 60),	"X")) {						
						//Debug.Log ("You delete " + textSavedGame);
						File.Delete(listSavedGames[i]);
						File.Delete(listSavedGames[i]+".meta");
						textLoadButton.text = "Load";
						whatSavedGame = "";
						UpdateSavedGames ();
					}
				}
			}
			GUI.EndGroup ();
			GUI.EndScrollView ();	//	End the ScrollView
		}
	}

	public void UpdateSavedGames(){
		listSavedGames = Directory.GetFiles (Application.dataPath + "/Resources/Saved", "*.data");
		int dimensiune = listSavedGames.Length * 70;
		if (dimensiune > guiDimension.y) {
			longlist = dimensiune + 10;
		} else {
			longlist = guiDimension.y;
		}
	}

	public void BackToMainMenuFromSavedGames(){		
		isVisibleSavedGames = false;
		savedGames.SetActive (false);
		mainMenu.SetActive (true);
	}

	public void BackToMainMenuFromSettings(){	
		settings.SetActive (false);
		mainMenu.SetActive (true);
	}

	public void ToSavedGames(){
		savedGames.transform.localPosition = new Vector3 (0, 0, 0);
		mainMenu.SetActive (false);
		savedGames.SetActive (true);

		listSavedGames = Directory.GetFiles(Application.dataPath+ "/Resources/Saved","*.data");

		int dimensiune = listSavedGames.Length *70;
		if (dimensiune > guiDimension.y) {
			longlist = dimensiune + 10;
		} else {
			longlist = guiDimension.y;
		}

		isVisibleSavedGames = true;
	}

	public void ToSettings(){		
		settings.transform.localPosition= new Vector3 (0, 0, 0);
		mainMenu.SetActive (false);
		settings.SetActive (true);
	}

	public void SaveGame(){
		//aici salvezi characterul nivelul
	}

	public void ExitLevel(){
		SaveGame ();
		GameSystem.ChangeLevelOfGame (GameSystem.levelSceneOfGame.startUpMenu);
	}

	public void SoundVolume(){
		if (isSoundMuted == false) {
			isSoundMuted = true;
			soundButton.GetComponent<Image>().overrideSprite = Resources.Load<Sprite> ("Sprites/UI/1");
		} else {
			isSoundMuted = false;
			soundButton.GetComponent<Image>().overrideSprite = Resources.Load<Sprite> ("Sprites/UI/0");
		}
	}

	public void LoadSavedGame(){
		if (whatSavedGame != "") {
			string path = "Assets/Resources/Config/whatToPlay.cfg";
			FileStream f = new FileStream (path, FileMode.Create);

			byte[] data = new byte[whatSavedGame.Length];
			for (int i = 0; i < whatSavedGame.Length; i++) {
				data [i] = (byte)whatSavedGame [i];
			}
			f.Write (data, 0, data.Length);
			f.Close ();
			SaveGame ();
			GameSystem.ChangeLevelOfGame (GameSystem.levelSceneOfGame.newgame);
		}
	}
}
