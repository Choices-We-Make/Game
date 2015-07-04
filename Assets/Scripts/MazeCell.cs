using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeCell : MonoBehaviour {

	private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];
	private int initializedEdgeCount;
	public IntVector2 coordinates;
	public bool reset;
	public MazeFloor FloorPrefab;
	public BoxCollider boxCollider;

	void Awake()
	{
		boxCollider = this.GetComponent<BoxCollider>();
	}

	public bool IsFullyInitialized {
		get {
			return initializedEdgeCount == MazeDirections.Count;
		}
	}
	
	public void SetEdge (MazeDirection direction, MazeCellEdge edge) {
		edges[(int)direction] = edge;
		initializedEdgeCount += 1;
	}
	
	public MazeCellEdge GetEdge (MazeDirection direction) {
		return edges[(int)direction];
	}

	public MazeDirection RandomUninitializedDirection {
		get {
			int skips = Random.Range(0, MazeDirections.Count - initializedEdgeCount);
			for (int i = 0; i < MazeDirections.Count; i++) {
				if (edges[i] == null) {
					if (skips == 0) {
						return (MazeDirection)i;
					}
					skips -= 1;
				}
			}
			throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
		}
	}

	public void lightFloor(bool on){
		if(on)
		{
			FloorPrefab.lightFloor();
		}
		else
		{
			FloorPrefab.unlightFloor();
		}
	}

	public void SetFloor()
	{
		FloorPrefab.SetFloor(GetEdge(MazeDirection.North).IsPassable(), GetEdge (MazeDirection.South).IsPassable(),GetEdge(MazeDirection.East).IsPassable(),GetEdge(MazeDirection.West).IsPassable());
	}

	void OnTriggerEnter(Collider other) {
		reset = true;
		//Destroy(other.gameObject);
	}
	// Use this for initialization
	void Start () {
		reset = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
