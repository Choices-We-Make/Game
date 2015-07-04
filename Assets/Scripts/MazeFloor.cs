using UnityEngine;
using System.Collections;

public class MazeFloor : MonoBehaviour {

	public Material litMaterial;
	private Material unlitMaterial;
	public Material FloorEWMaterial;
	public Material FloorSEMaterial;
	public Material FloorSWEMaterial;
	public Material FloorNSEWMaterial;
	public Mesh Floor_EW;
	public Mesh Floor_SE;
	public Mesh Floor_SWE;
	public Mesh Floor_NSEW;
	private MeshRenderer mr;
	private MeshFilter mf;
	private Transform t;
	private MeshCollider mc;

	void Awake()
	{
		mr = this.GetComponent<MeshRenderer>();
		mf = this.GetComponent<MeshFilter>();
		t = this.GetComponent<Transform>();
		mc = this.GetComponent<MeshCollider>();
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void lightFloor()
	{
		mr.material = litMaterial;
	}

	public void unlightFloor()
	{
		mr.material = unlitMaterial;
	}

	public void SetFloor(bool north, bool south, bool east, bool west)
	{
		int tile=0;
		if(north) tile +=1000;
		if(south) tile +=100;
		if(east) tile +=10;
		if(west) tile +=1;
		switch(tile)
		{
		case 1111:
			mf.mesh = Floor_NSEW;
			t.rotation = Quaternion.Euler(new Vector3(0,0,0));
			unlitMaterial = FloorNSEWMaterial;
			unlightFloor();
			mc.sharedMesh = Floor_NSEW;
			break;
		case 1:
		case 10:
		case 11:
			mf.mesh = Floor_EW;
			t.rotation = Quaternion.Euler(new Vector3(0,0,0));
			unlitMaterial = FloorEWMaterial;
			unlightFloor();
			mc.sharedMesh = Floor_EW;
			break;
		case 100:
		case 1000:
		case 1100:
			mf.mesh = Floor_EW;
			t.rotation = Quaternion.Euler(new Vector3(0,90,0));
			unlitMaterial = FloorEWMaterial;
			unlightFloor();
			mc.sharedMesh = Floor_EW;
			break;
		case 110:
			mf.mesh = Floor_SE;
			t.rotation = Quaternion.Euler(new Vector3(0,0,0));
			unlitMaterial = FloorSEMaterial;
			unlightFloor();
			mc.sharedMesh = Floor_SE;
			break;
		case 101:
			mf.mesh = Floor_SE;
			t.rotation = Quaternion.Euler(new Vector3(0,90,0));
			unlitMaterial = FloorSEMaterial;
			unlightFloor();
			mc.sharedMesh = Floor_SE;
			break;
		case 1001:
			mf.mesh = Floor_SE;
			t.rotation = Quaternion.Euler(new Vector3(0,180,0));
			unlitMaterial = FloorSEMaterial;
			unlightFloor();
			mc.sharedMesh = Floor_SE;
			break;
		case 1010:
			mf.mesh = Floor_SE;
			t.rotation = Quaternion.Euler(new Vector3(0,-90,0));
			unlitMaterial = FloorSEMaterial;
			unlightFloor();
			mc.sharedMesh = Floor_SE;
			break;
		case 111:
			mf.mesh = Floor_SWE;
			t.rotation = Quaternion.Euler(new Vector3(0,0,0));
			unlitMaterial = FloorSWEMaterial;
			unlightFloor();
			mc.sharedMesh = Floor_SWE;
			break;
		case 1101:
			mf.mesh = Floor_SWE;
			t.rotation = Quaternion.Euler(new Vector3(0,90,0));
			unlitMaterial = FloorSWEMaterial;
			unlightFloor();
			mc.sharedMesh = Floor_SWE;
			break;
		case 1011:
			mf.mesh = Floor_SWE;
			t.rotation = Quaternion.Euler(new Vector3(0,180,0));
			unlitMaterial = FloorSWEMaterial;
			unlightFloor();
			mc.sharedMesh = Floor_SWE;
			break;
		case 1110:
			mf.mesh = Floor_SWE;
			t.rotation = Quaternion.Euler(new Vector3(0,-90,0));
			unlitMaterial = FloorSWEMaterial;
			unlightFloor();
			mc.sharedMesh = Floor_SWE;
			break;
		default:
			//error
			break;
		}
	}
}
