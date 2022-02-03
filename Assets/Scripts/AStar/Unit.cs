using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	
	public Transform target;
	public float speed = 20;
	public float clearance = 8;

	Vector2[] drum;
	int tnum;

	void Start() {
		StartCoroutine (CautaDrum());
	}

	IEnumerator CautaDrum() {
		Vector2 tposO = (Vector2)transform.position;
			
		while (true) {

			if (tposO != (Vector2)target.position && 
			new Rect(target.position.x-clearance/2,target.position.y-clearance/2,clearance,clearance).
			Contains(new Vector2(transform.position.x,transform.position.y))==false) {

				drum = Pathfinding.Drum(new Vector2(transform.position.x,transform.position.y), new Vector2(target.position.x,target.position.y));

				StopCoroutine ("Follow");
				StartCoroutine ("Follow");
			}else{
				StopCoroutine ("Follow");
			}

			yield return new WaitForSeconds (0.25f);
		}
	}
		
	IEnumerator Follow() {
		if (drum.Length > 0) {
			tnum = 0;
			Vector2 punctCurr = drum [0];
			while (true) {
				if (new Vector2(transform.position.x,transform.position.y) == punctCurr) {
					tnum++;
					if (tnum >= drum.Length) {
						yield break;
					}
					punctCurr = drum [tnum];
				}
				
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(punctCurr.x,punctCurr.y,transform.position.z), speed * Time.deltaTime);
				yield return null;

			}
		}
	}

	void DrawRect(Rect rect)
	{
		Gizmos.DrawWireCube(new Vector3(rect.center.x, rect.center.y, 0.01f), new Vector3(rect.size.x, rect.size.y, 0.01f));
	}

	public void OnDrawGizmos() {

		if (drum != null && Pathfinding.drawPathGizmo) {
			for (int i = tnum; i < drum.Length; i ++) {
				Gizmos.color = Color.white;
				if (i == tnum) {
					Gizmos.DrawLine(transform.position, drum[i]);
				}
				else {
					Gizmos.DrawLine(drum[i-1],drum[i]);
				}
			}
		}
	}
}
