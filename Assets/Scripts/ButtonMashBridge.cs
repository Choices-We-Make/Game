using UnityEngine;
using System.Collections;

public class ButtonMashBridge : MonoBehaviour {

	private Vector3 initVector;

	public float heightToMove = 3f;
	public float vertMoveSpeed = 1f;
	public float fallSpeed = .5f;

	// Use this for initialization
	void Start () {
		initVector = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.E)){ //&& (transform.position.y + initVector.y < heightToMove)){
			Vector3 newPos = new Vector3(transform.position.x, Mathf.Min(transform.position.y + vertMoveSpeed * Time.deltaTime, initVector.y + heightToMove), transform.position.z);
			transform.position = newPos;
			Quaternion newRot = Quaternion.Euler(new Vector3(270, 90*(initVector.y-transform.position.y/heightToMove), 0));
			transform.rotation = newRot;
		}else if (transform.position.y > initVector.y){
			transform.position = new Vector3(transform.position.x, transform.position.y - fallSpeed * Time.deltaTime, transform.position.z);
			transform.rotation = Quaternion.Euler(new Vector3(270, -90*(initVector.y-transform.position.y/heightToMove), 0));
		}
	
	}
}
