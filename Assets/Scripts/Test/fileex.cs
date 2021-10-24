using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
public class fileex : MonoBehaviour {
	// 15 mb in 3sc scriere , 2sc citire ,,, maxim 20 mb (standarde hamil)

	//public static int gb2tobytes = 2147395600;
    //  int_max     2147483647
    // sqrt_and_^2  2147395600
	// little 536870911 >= 23170 ^ 23170
	//private int byte1 = 1;
	// Use this for initialization
	void Start () {
		//float timp;
		//mat();

		//Debug.Log (Time.time + "Start write");
		WriteString ();
		//Debug.Log (Time.time + "Finish write");
		ReadString ();
	}

	void mat(){
		string path = "Assets/Resources/mat.txt";

		FileStream f = new FileStream (path, FileMode.Open);
		//int a;
		//int n;
		byte[] d=new byte[4];
		f.Read (d, 0, 4);
		//Debug.Log (d[0]+" "+d[1]+" "+d[2]+" "+d[3]);
		/*
		n = f.ReadByte ();
		//f.Read (n);
		//f.Read (n, f.Position, 4);
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
				a=f.ReadByte ();
				Debug.Log(a);
			}
			//Debug.Log()
		}
		*/
		f.Close ();
	}

	static void WriteString()
	{
		string path = "Assets/Resources/test.txt";

		/*
		//Write some text to the test.txt file
		StreamWriter writer = new StreamWriter(path,false,Encoding.ASCII);
		char a='a';

		for (int i = 0; i < 536870911; i++) {
			
			writer.Write (a);
		}
		writer.Close();
		*/

		FileStream f = new FileStream (path, FileMode.Create);
		byte a = 10;
		f.WriteByte (a);
		for (int i = 0; i < a; i++) {
			for (int j = 0; j < a; j++) {
				f.WriteByte (0);
			}
		}
		f.Close ();
	}

	static void ReadString()
	{
		string path = "Assets/Resources/test.txt";
		/*
		//Read the text from directly from the test.txt file
		StreamReader reader = new StreamReader(path,Encoding.ASCII); 
		Debug.Log(reader.ReadToEnd());
		Debug.Log(reader.BaseStream);
		reader.Close();
		*/
		//int a;
		FileStream f = new FileStream (path, FileMode.Open);
		//while(f.CanRead == true) {
		//for (int i = 0; i < 1000; i++) {			
		//	a=f.ReadByte();
		//	Debug.Log (a);
		//}
		//f.Position = 1000;
		int a=f.ReadByte ();
		Debug.Log (a);
		f.Close ();
	}
}
