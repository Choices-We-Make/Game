using UnityEngine;
using System.Collections;

public class SlidingDoor : MonoBehaviour {

	public bool doorLocked = false;
	public bool doorOpen = false;

	public float openSpeed = 10;

	private Vector3 closedPosition;
	private Vector3 openPosition;

	private Transform doorMesh;

	void Start(){
		doorMesh = transform.GetChild(0);
		closedPosition = doorMesh.position;
		openPosition = new Vector3(doorMesh.position.x + doorMesh.localScale.x, doorMesh.position.y, doorMesh.position.z);

	}

	void Update(){
		SlideEvent();
	}

	void SlideEvent(){
		if (!doorLocked && doorOpen){
			//doorMesh.Translate(doorMesh.localScale.x, 0, 0, Space.Self);
			doorMesh.position = Vector3.Lerp(doorMesh.position, openPosition, Time.deltaTime * openSpeed);
			if (doorMesh.localPosition.x > doorMesh.localScale.x)
				doorMesh.localPosition.Set(doorMesh.localScale.x, 0, 0);
		}else if (!doorOpen){
			doorMesh.position = Vector3.Lerp(doorMesh.position, closedPosition, Time.deltaTime * openSpeed);
		}
	}

	void OnTriggerEnter(Collider otherCollider){
		if (otherCollider.CompareTag("Player"))
			doorOpen = true;
	}

	void OnTriggerExit(Collider otherCollider){
		if (otherCollider.CompareTag("Player"))
			doorOpen = false;
	}
}
