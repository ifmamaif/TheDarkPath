using System.Collections.Generic;	//	List
using UnityEngine;	// core , Random
using UnityEngine.SceneManagement; //SceneManager
using UnityEngine.UI; // Text
using System.IO;	//	File, FileStream , GetFiles;
using System;
public class MenuSystem : MonoBehaviour {

	public GameObject mainMenu;
	public GameObject newGame;
	public GameObject savedGames;
	public GameObject exitMessage;
	public GameObject newCharacter;
	public GameObject alertMessage;
	public GameObject settings;

	private bool isHardMode = false;
	public Button normalDifficultyButton;
	public Button hardDifficultyButton;
	public GameObject nameCharacterInputField;

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
		exitMessage.SetActive (false);
		newGame.SetActive (false);
		newCharacter.SetActive (false);
		alertMessage.SetActive (false);
		settings.SetActive (false);
		savedGames.SetActive (false);
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

	public void BackToMainMenuFromExit(){
		mainMenu.transform.localPosition= new Vector3 (0, 0, 0);
		mainMenu.SetActive (true);
		exitMessage.SetActive (false);
	}

	public void ToExitGame(){
		exitMessage.transform.localPosition= new Vector3 (0, 0, 0);
		mainMenu.SetActive (false);
		exitMessage.SetActive (true);
	}

	public void GooGoodBye(){
		Application.Quit ();
	}

	public void BackToMainMenuFromNewGame(){
		mainMenu.transform.localPosition= new Vector3 (0, 0, 0);
		mainMenu.SetActive (true);
		newGame.SetActive (false);
	}

	public void ToNewGame(){
		newGame.transform.localPosition= new Vector3 (0, 0, 0);
		mainMenu.SetActive (false);
		newGame.SetActive (true);
	}

	public void BackToMainMenuFromNewCharacter(){
		mainMenu.transform.localPosition= new Vector3 (0, 0, 0);
		mainMenu.SetActive (true);
		newCharacter.SetActive (false);
	}

	public void ToNewCharacter(){
		newCharacter.transform.localPosition= new Vector3 (0, 0, 0);
		newGame.SetActive (false);
		newCharacter.SetActive (true);
	}

	public void BackToNewCharacter(){		
		alertMessage.SetActive (false);
		newCharacter.SetActive (true);
	}

	public void NewStart(){
		string name = nameCharacterInputField.GetComponent<InputField> ().text;

		if (IsValidNameCharacter (name) == true) {

			string path = "Assets/Resources/Saved/" + name + ".data";
			if (File.Exists (path) == false) {
				FileStream f = new FileStream (path, FileMode.CreateNew);
				byte[] byteArray;
				/*
				int offSetX = UnityEngine.Random.Range (-99999, 99999);
				Debug.Log ("Random X : " + offSetX);
				byteArray = BitConverter.GetBytes (offSetX);
				f.Write (byteArray, 0, byteArray.Length);

				int offSetY = UnityEngine.Random.Range (-99999, 99999);
				Debug.Log ("Random Y : " + offSetY);
				byteArray = BitConverter.GetBytes (offSetY);
				f.Write (byteArray, 0, byteArray.Length);
				*/

				byteArray = BitConverter.GetBytes ((int)0);
				f.Write (byteArray, 0, byteArray.Length);
				byteArray = BitConverter.GetBytes ((int)0);
				f.Write (byteArray, 0, byteArray.Length);

				byteArray = System.Text.Encoding.UTF8.GetBytes (name);
				f.Write (byteArray, 0, byteArray.Length);

				byteArray [0] = (byte)(isHardMode == true ? 1 : 0);
				f.WriteByte (byteArray [0]);
				f.Close ();
				//Debug.Log (name + " Character Saved!");

				path = "Assets/Resources/Config/whatToPlay.cfg";
				f = new FileStream (path, FileMode.Create);
				byte[] data = new byte[name.Length];
				for (int i = 0; i < name.Length; i++) {
					data [i] = (byte)name [i];
					//Debug.Log (name [i]+" "+data[i]);
				}
				f.Write (data, 0, data.Length);
				f.Close ();
				BackToMainMenuFromNewCharacter ();
				GameSystem.ChangeLevelOfGame (GameSystem.levelSceneOfGame.game);

			} else {
				alertMessage.transform.localPosition = new Vector3 (0, 0, 0);
				newCharacter.SetActive (false);
				alertMessage.SetActive (true);
			}
		}
	}

	public static bool IsValidNameCharacter(string text){
		for (int i = 0; i < text.Length; i++) {			
			if (text [i] < '0' || text [i] > '9')
			if (text [i] < 'a' || text [i] > 'z')
			if (text [i] < 'A' || text [i] > 'Z')
				return false;
		}
		return true;
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



	public void SoundVolume(){
		if (isSoundMuted == false) {
			isSoundMuted = true;
			soundButton.GetComponent<Image>().overrideSprite = Resources.Load<Sprite> ("Sprites/UI/1");
		} else {
			isSoundMuted = false;
			soundButton.GetComponent<Image>().overrideSprite = Resources.Load<Sprite> ("Sprites/UI/0");
		}
	}


	public void BackToMainMenuFromSettings(){
		mainMenu.SetActive (true);
		settings.SetActive (false);
	}

	public void ToSettings(){		
		settings.transform.localPosition= new Vector3 (0, 0, 0);
		mainMenu.SetActive (false);
		settings.SetActive (true);
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

			GameSystem.ChangeLevelOfGame (GameSystem.levelSceneOfGame.game);

		}
	}

	public void SetToNormalDifficulty(){
		isHardMode = false;
		normalDifficultyButton.interactable = false;
		hardDifficultyButton.interactable = true;
	}

	public void SetToHardDifficulty(){		
		isHardMode = true;
		normalDifficultyButton.interactable = true;
		hardDifficultyButton.interactable = false;
	}
}
	