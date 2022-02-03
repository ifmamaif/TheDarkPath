using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour {

	Grid grid;
	static Pathfinding instance;
	public static bool drawPathGizmo;
	public bool setdrawPathGizmo;
	
	void Awake() {
		grid = GetComponent<Grid>();
		instance = this;
	}

	void Update(){
		drawPathGizmo=setdrawPathGizmo;
	}

	public static Vector2[] Drum(Vector2 from, Vector2 to) {
		return instance.FindPath (from, to);
	}
	
	Vector2[] FindPath(Vector2 from, Vector2 to) {
		
		Stopwatch sw = new Stopwatch();
		sw.Start();
		
		Vector2[] waypoints = new Vector2[0];
		bool pathSuccess = false;
		
		Node startNode = grid.getNodDinWorld(from);
		Node targetNode = grid.getNodDinWorld(to);

		//UnityEngine.Debug.Log("sn "+startNode.worldPosition.ToString());
		//UnityEngine.Debug.Log("tn "+targetNode.worldPosition.ToString());

		startNode.parent = startNode;

		if (!startNode.traversabil) {
			startNode = grid.CelMaiApropiatNodOK (startNode);
		}
		if (!targetNode.traversabil) {
			targetNode = grid.CelMaiApropiatNodOK (targetNode);
		}
		
		if (startNode.traversabil && targetNode.traversabil) {
			
			Heap<Node> openSet = new Heap<Node>(grid.MaxSize());
			HashSet<Node> closedSet = new HashSet<Node>();
			openSet.Add(startNode);
			
			while (openSet.Count > 0) {
				Node currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);

				if (currentNode == targetNode) {
					sw.Stop();
					//print ("Path found: " + sw.ElapsedMilliseconds + " ms");
					pathSuccess = true;
					break;
				}
				
				foreach (Node neighbour in grid.GetVecini(currentNode)) {
					if (!neighbour.traversabil || closedSet.Contains(neighbour)) {
						continue;
					}
					
					int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						neighbour.parent = currentNode;
						
						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
						else 
							openSet.UpdateItem(neighbour);
					}
				}
			}
		}

		if (pathSuccess) {
			waypoints = RetracePath(startNode,targetNode);
		}

		return waypoints;
		
	}

	Vector2[] RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;
		
		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		Vector2[] waypoints = SimplifyPath(path);
		Array.Reverse(waypoints);
		return waypoints;
		
	}
	
	Vector2[] SimplifyPath(List<Node> path) {
		List<Vector2> waypoints = new List<Vector2>();
		Vector2 directionOld = Vector2.zero;
		
		for (int i = 1; i < path.Count; i ++) {
			Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX,path[i-1].gridY - path[i].gridY);
			if (directionNew != directionOld) {
				waypoints.Add(path[i].worldPosition);
			}
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}
	
	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
		
		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}
	
	
}
