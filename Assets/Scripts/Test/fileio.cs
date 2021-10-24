using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class fileio : MonoBehaviour {

	// Use this for initialization
	void Start () {

		string path = "Assets/Resources/Config/whatToPlay.cfg";
		FileStream f = new FileStream (path, FileMode.Open);
		Debug.Log (f.Length);
		for (int i = 0; i < f.Length; i++) {
			int n = f.ReadByte ();
			Debug.Log (n + " " + (byte)n + " " + (char)n);

		}
		f.Close ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
