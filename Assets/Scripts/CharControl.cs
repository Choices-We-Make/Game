using UnityEngine;
using System.Collections;

public class CharControl : MonoBehaviour {

	private CharacterController controller;
	private MeshRenderer charMesh;

	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	public float pushPower = 2.0f;

	private Vector3 charDirection = Vector3.zero;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 lookAtVector = Vector3.zero;

	void Update() {
		MoveChar ();
	}

	void MoveChar(){
		controller = GetComponent<CharacterController>();
		charMesh = GetComponentInChildren<MeshRenderer>();

		if (Input.GetKey(KeyCode.LeftShift)){
			speed = 12.0f;
			pushPower = 4.0f;
		}
		else{
			speed = 6.0f;
			pushPower = 2.0f;
		}

		charDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		if (controller.isGrounded) {
			moveDirection = charDirection;
			transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			if (Input.GetButton("Jump"))
				moveDirection.y = jumpSpeed;
			
		}
		
		moveDirection.y -= gravity * Time.deltaTime;
		
		lookAtVector = new Vector3(transform.position.x + charDirection.x, transform.position.y, transform.position.z + charDirection.z);
		if (charDirection.sqrMagnitude > .1)
			charMesh.transform.LookAt(lookAtVector, Vector3.up);
		
		controller.Move(moveDirection * Time.deltaTime);
		
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body == null || body.isKinematic)
			return;
		
		if (hit.moveDirection.y < -0.3F)
			return;
		
		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
		body.velocity = pushDir * pushPower;
	}

}
