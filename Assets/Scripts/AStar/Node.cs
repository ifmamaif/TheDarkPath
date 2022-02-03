using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node> {
	
	public bool traversabil;
	public Vector2 worldPosition;
	public int gridX;
	public int gridY;

	public int gCost;
	public int hCost;
	public Node parent;
	int heapIndex;
	
	public Node(bool trav, Vector2 wpos, int gx, int gy) {
		traversabil = trav;
		worldPosition = wpos;
		gridX = gx;
		gridY = gy;
	}

	public int fCost() {
		return gCost + hCost;
	}

	public int HeapIndex {
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

	public int CompareTo(Node nodeToCompare) {
		int compare = fCost().CompareTo(nodeToCompare.fCost());
		if (compare == 0) {
			compare = hCost.CompareTo(nodeToCompare.hCost);
		}
		return -compare;
	}
}
