using UnityEngine;
using System.Collections;

public class PhysCharControl : MonoBehaviour {

	public float moveSpeed = 100.0f;
	public float jumpVelocity = 200.0f;
	public float gravity = 1.0f;

	private Rigidbody rbody;
	private new CapsuleCollider collider;
	private MeshRenderer charMesh;
	private Vector3 vel = Vector3.zero;
	private Vector3 charDirection = Vector3.zero;
	private float distToGround;
	private Vector3 lastPos;

	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody>();
		collider = GetComponent<CapsuleCollider>();
		charMesh = GetComponentInChildren<MeshRenderer>();

		lastPos = transform.position;

		distToGround = collider.bounds.extents.y;
	}
	
	// Update is called once per frame
	void Update () {
		MoveChar();
	
	}

	void MoveChar(){
		vel = rbody.velocity;
		//charDirection = new Vector3(Input.GetAxis("Horizontal")*moveSpeed*Time.deltaTime, 0, Input.GetAxis("Vertical")*moveSpeed*Time.deltaTime);
		charDirection = transform.position - lastPos;

		if(isGrounded())
			rbody.velocity = new Vector3(Input.GetAxis("Horizontal")*moveSpeed*Time.deltaTime, vel.y, Input.GetAxis("Vertical")*moveSpeed*Time.deltaTime);

		if (Input.GetKeyDown(KeyCode.Space) && isGrounded()){
			rbody.AddForce(new Vector3(0, jumpVelocity, 0));
		}
		if (charDirection.sqrMagnitude > .0003)
			charMesh.transform.LookAt(new Vector3(transform.position.x + charDirection.x, transform.position.y, transform.position.z + charDirection.z), Vector3.up);

		lastPos = transform.position;
	}

	bool isGrounded(){
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}
}
