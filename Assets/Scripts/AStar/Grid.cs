using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public bool displayGridGizmos;

	public LayerMask mascaObstacol;
	public Vector2 gridTotalSize;
	public float razaNod;

	Node[,] grid;
	float diametruNod;
	int gridSizeX, gridSizeY;

	void Awake() {
		diametruNod = razaNod*2;
		gridSizeX = Mathf.RoundToInt(gridTotalSize.x/diametruNod);
		gridSizeY = Mathf.RoundToInt(gridTotalSize.y/diametruNod);

		CreazaGrid();
	}

	public int MaxSize() {
		return gridSizeX * gridSizeY;
	}

	public void CreazaGrid() {
		grid = new Node[gridSizeX,gridSizeY];
		Vector2 worldBL = (Vector2)transform.position - Vector2.right * gridTotalSize.x/2 - Vector2.up * gridTotalSize.y/2;

		for (int x = 0; x < gridSizeX; x ++) {
			for (int y = 0; y < gridSizeY; y ++) {
				Vector2 worldPoint = worldBL + Vector2.right * (x * diametruNod + razaNod) + Vector2.up * (y * diametruNod + razaNod);
				//daca nu returneaza colliderul atunci nodul este traversabil
				bool traversabil = (Physics2D.OverlapCircle(worldPoint,razaNod,mascaObstacol) == null);

				grid[x,y] = new Node(traversabil,worldPoint, x,y);
			}
		}
	}
	

	public List<Node> GetVecini(Node nod, int adancime = 1) {
		List<Node> vecini = new List<Node>();

		for (int x = -adancime; x <= adancime; x++) {
			for (int y = -adancime; y <= adancime; y++) {
				if (x == 0 && y == 0)
					continue;

				int checkX = nod.gridX + x;
				int checkY = nod.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
					vecini.Add(grid[checkX,checkY]);
				}
			}
		}

		return vecini;
	}
	

	public Node getNodDinWorld(Vector2 wpos) {

		wpos.x-=transform.position.x;
		wpos.y-=transform.position.y;

		float percentX = (wpos.x + gridTotalSize.x/2) / gridTotalSize.x;
		float percentY = (wpos.y + gridTotalSize.y/2) / gridTotalSize.y;

		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);

		return grid[x,y];
	}

	public Node CelMaiApropiatNodOK(Node node) {
		int maxRadius = Mathf.Max (gridSizeX, gridSizeY) / 2;
		for (int i = 1; i < maxRadius; i++) {
			Node n = FindInRaza (node.gridX, node.gridY, i);
			if (n != null) {
				return n;

			}
		}
		return null;
	}
	Node FindInRaza(int centruX, int centruY, int raza) {
		for (int i = -raza; i <= raza; i ++) {
			int vertX = i + centruX;
			int horY = i + centruY;
			//sus
			if (InBounds(vertX, centruY + raza)) {
				if (grid[vertX, centruY + raza].traversabil) {
					return grid [vertX, centruY + raza];
				}
			}
			//jos
			if (InBounds(vertX, centruY - raza)) {
				if (grid[vertX, centruY - raza].traversabil) {
					return grid [vertX, centruY - raza];
				}
			}
			//dr
			if (InBounds(centruY + raza, horY)) {
				if (grid[centruX + raza, horY].traversabil) {
					return grid [centruX + raza, horY];
				}
			}
			//st
			if (InBounds(centruY - raza, horY)) {
				if (grid[centruX - raza, horY].traversabil) {
					return grid [centruX - raza, horY];
				}
			}

		}
		return null;
	}

	bool InBounds(int x, int y) {
		return x>=0 && x<gridSizeX && y>= 0 && y<gridSizeY;
	}
	
	void OnDrawGizmos() {
		//return;
		Gizmos.DrawWireCube(transform.position,new Vector2(gridTotalSize.x,gridTotalSize.y));
		if (grid != null && displayGridGizmos) {
			foreach (Node n in grid) {
				Gizmos.color = Color.red;
				if (n.traversabil)
					Gizmos.color = Color.white;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (diametruNod-.1f));
			}
		}
	}

}