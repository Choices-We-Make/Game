using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {

	public IntVector2 size;
	public MazeCell cellPrefab;
	private MazeCell[,] cells;
	private PlayerController playerInstance;
	public float generationStepDelay;
	public MazePassage passagePrefab;
	public MazeWall wallPrefab;
	public MazeWall outerWallPrefab;
	public PlayerController playerPrefab;
	private int cellCount;
	public IntVector2 exit;
	public int difficulty;

	// Use this for initialization
	void Start () {
		difficulty = 0;
	}
	
	// Update is called once per frame
	void Update () {
		cellPrefab.reset = cells[size.x-1, size.z-1].reset;
	}

	public void killPlayer()
	{
		Destroy(playerInstance.gameObject);
	}

	public IntVector2 PlayerLocation()
	{
		return playerInstance.GetLocation(cells);
	}

	//public IEnumerator Generate () {
	public void Generate () {
		//WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
		cells = new MazeCell[size.x, size.z];
		exit = new IntVector2(size.x-1, size.z-1);
		List<MazeCell> activeCells = new List<MazeCell>();
		DoFirstGenerationStep(activeCells);
		while (activeCells.Count > 0) {
			//yield return delay;
			difficulty = Mathf.Max(activeCells.Count, difficulty);
			DoNextGenerationStep(activeCells);
		}
		FindShortestPath(new IntVector2(0,0), exit, ref difficulty);
		foreach(MazeCell cell in cells)
		{
			cell.SetFloor();
		}
	}

	private void DoFirstGenerationStep (List<MazeCell> activeCells) {
		//adds the first maze cell
		IntVector2 firstCoord = RandomCoordinates;
		activeCells.Add(CreateCell(firstCoord, cellPrefab));
		playerInstance = Instantiate(playerPrefab) as PlayerController;
	}
	
	private void DoNextGenerationStep (List<MazeCell> activeCells) {
		int currentIndex = activeCells.Count - 1;
		MazeCell currentCell = activeCells[currentIndex];
		if (currentCell.IsFullyInitialized) {
			activeCells.RemoveAt(currentIndex);
			return;
		}
		MazeDirection direction = currentCell.RandomUninitializedDirection;
		IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
		if (ContainsCoordinates(coordinates)) {
			MazeCell neighbor = GetCell(coordinates);
			if (neighbor == null) {
				if(coordinates.x==exit.x && coordinates.z==exit.z){
					neighbor = CreateCell(coordinates,cellPrefab);
					neighbor.lightFloor(true);
					neighbor.boxCollider.enabled = true;
				}
				else{
					neighbor = CreateCell(coordinates,cellPrefab);
					neighbor.lightFloor(false);
					neighbor.boxCollider.enabled = false;
				}
				CreatePassage(currentCell, neighbor, direction);
				activeCells.Add(neighbor);
			}
			else {
				CreateWall(currentCell, neighbor, direction);
			}
		}
		else {
			CreateWall(currentCell, null, direction);
		}
	}

	private void CreatePassage (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazePassage passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(otherCell, cell, direction.GetOpposite());
	}
	
	private void CreateWall (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazeWall wall = Instantiate(outerWallPrefab) as MazeWall;
		wall.Initialize(cell, otherCell, direction);
		if (otherCell != null) {
			wall = Instantiate(wallPrefab) as MazeWall;
			wall.Initialize(otherCell, cell, direction.GetOpposite());
		}
	}

	private MazeCell CreateCell (IntVector2 coordinates, MazeCell cellType) {
		MazeCell newCell = Instantiate(cellType) as MazeCell;
		cells[coordinates.x, coordinates.z] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
		newCell.transform.parent = transform;
		newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
		return newCell;
	}

	public IntVector2 RandomCoordinates {
		get {
			return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
		}
	}
	
	public bool ContainsCoordinates (IntVector2 coordinate) {
		//checks whether the coordinate falls within the boundaries of the maze
		return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
	}

	public MazeCell GetCell (IntVector2 coordinates) {
		return cells[coordinates.x, coordinates.z];
	}

	//Using A* Pathfinding Algorithm
	//References:
	//https://www.youtube.com/watch?v=KNXfSOx4eEE
	//https://en.wikipedia.org/wiki/A*_search_algorithm
	private class PathData{
		public PathData Parent;
		public IntVector2 Cell;
		public int GCost;
		public int HCost;

		public PathData(IntVector2 CurrentCell, int InitialGCost, int InitialHCost, PathData Prior)
		{
			Parent = Prior;
			Cell = CurrentCell;
			HCost = InitialHCost;
			GCost = InitialGCost;
		}

		public int FCost{
			get{
				return GCost + HCost;
			}
		}
	}

	public List<IntVector2> FindShortestPath(IntVector2 start, IntVector2 goal, ref int closedCount)
	{

		/* We want to enable sorting a list by FCost and then by HCost, however, the SortedList type only allows 1 key.
		 * To get around this, we will encode FCost and HCost into one key so that sorting the single key will 
		 * result in an equivalent result as sorting on two keys. Suppose HCost can only have values from 0-9.
		 * Then we would simply multiply FCost by 10 and add HCost to the result.
		 * Example:
		 * Item 1: FCost = 2, HCost = 3, Key = 2x10 + 3 = 23
		 * Item 2: FCost = 2, HCost = 1, Key = 2x10 + 1 = 21
		 * 
		 * The order of this list should be: {Item 2, Item 1} because their FCosts are tied, but when you look at
		 * HCost, Item 2 has higher priority (sorted in ascending order). So to get this result we use their calculated keys.
		*/
		int MaximumPossibleHCost = size.x+size.z;
		float FCostMultiplier = Mathf.Pow(10,Mathf.Floor(Mathf.Log10(MaximumPossibleHCost)+1)); 

		PathData startPath = new PathData(start,1,HCostEstimate(start,goal),null);
		PathData goalPath = new PathData(goal,1,0,null);

		SortedList<float, PathData> OpenCells = new SortedList<float, PathData>();
		HashSet<IntVector2> ClosedCells = new HashSet<IntVector2>();

		OpenCells.Add(startPath.FCost*FCostMultiplier+startPath.HCost, startPath);

		while(OpenCells.Count>0){
			PathData currentPath = OpenCells.Values[0];
			OpenCells.RemoveAt(0);
			//cells[currentPath.Cell.x, currentPath.Cell.z].lightFloor(false);
			ClosedCells.Add(currentPath.Cell);
			if(currentPath.Cell.x==goalPath.Cell.x && currentPath.Cell.z==goalPath.Cell.z)
			{
				OpenCells.Clear();
				PathData priorNode = currentPath.Parent;
				List<IntVector2> result = new List<IntVector2>();
				while(currentPath.Cell.x!=startPath.Cell.x||currentPath.Cell.z!=startPath.Cell.z){
					result.Add(currentPath.Cell);
					currentPath = priorNode;
					priorNode = currentPath.Parent;
				}
				closedCount = ClosedCells.Count;
				return result;
			}

			//Get neighboring cells and add to OpenCells
			foreach(MazeDirection direction in System.Enum.GetValues(typeof(MazeDirection)))
			{
				MazeCellEdge temp = cells[currentPath.Cell.x, currentPath.Cell.z].GetEdge(direction);
				if(temp.IsPassable())
				{
					if(ClosedCells.Contains(temp.otherCell.coordinates))
					{
						continue;
					}
					else
					{
						PathData Node = new PathData(temp.otherCell.coordinates, 1, HCostEstimate(temp.otherCell.coordinates, goal), null);
						float NodeKey = Node.FCost*FCostMultiplier+Node.HCost;
						while(OpenCells.ContainsKey(NodeKey))
						{
							//cannot add a duplicate key so make unique
							NodeKey += 0.001f;
						}
						Node.GCost += currentPath.GCost;
						Node.Parent = currentPath;
						OpenCells.Add(NodeKey, Node);
						//cells[Node.Cell.x, Node.Cell.z].lightFloor(true);
					}
				}
			}
		}
		throw new System.InvalidOperationException("Maze cannot calculate a path.");
	}

	private int HCostEstimate(IntVector2 start, IntVector2 goal)
	{
		return Mathf.Abs(start.x-goal.x) + Mathf.Abs(start.z-goal.z);
	}
}
