using UnityEngine;
using System.Collections;

public abstract class MazeCellEdge : MonoBehaviour {

	public MazeCell cell, otherCell;
	public MazeDirection direction;
	public abstract bool IsPassable();

	public void Initialize (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		this.cell = cell;
		this.otherCell = otherCell;
		this.direction = direction;
		cell.SetEdge(direction, this);
		transform.parent = cell.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = direction.ToRotation();
	}

	private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];
	
	public MazeCellEdge GetEdge (MazeDirection direction) {
		return edges[(int)direction];
	}
	
	public void SetEdge (MazeDirection direction, MazeCellEdge edge) {
		edges[(int)direction] = edge;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
