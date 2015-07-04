using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	private GameObject target;
	// Use this for initialization
	void Start () {
		this.transform.rotation=Quaternion.Euler(30,45,0);
	}
	// Update is called once per frame
	void Update () {
		if(target==null)
		{
			target = GameObject.FindWithTag("Player");
		}

		this.transform.position = new Vector3(target.transform.position.x-1,target.transform.position.y+1, target.transform.position.z-1);
		//this.transform.rotation = target.transform.rotation;
	}
}
