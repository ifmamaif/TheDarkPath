using UnityEngine;

public class CameraManager {

	private GameObject objectCamera;
	public Camera cam;
	public bool isZoomChanged = false;

	public CameraManager(){

		objectCamera = new GameObject("MainCamera");
		objectCamera.transform.position = new Vector3(0, 0, -1);
		objectCamera.tag = "MainCamera";
		objectCamera.AddComponent<Camera> ();

		cam = objectCamera.GetComponent<Camera> ();
		cam.orthographic = true;

		cam.clearFlags = CameraClearFlags.SolidColor;
		cam.backgroundColor = Color.black;

		cam.nearClipPlane = -0.1f;
		cam.farClipPlane = 2;

	}

	public void ChangeOrthographicSize(float value){
		cam.orthographicSize += value;
		isZoomChanged = true;
	}

	public void SetOrthographicSize(float size){
		cam.orthographicSize = size;
	}

	public Vector2Int GetScreen(){		
		float y = cam.orthographicSize * 2;
		float x = y * cam.aspect;
		return new Vector2Int((int)x,(int)y);
	}

}
