using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float speed = 6.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;
	public IntVector2 location;
	void Update() {
		CharacterController controller = GetComponent<CharacterController>();
		if (controller.isGrounded) {
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			//if (Input.GetButton("Jump"))
				//moveDirection.y = jumpSpeed;
			
		}
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	}

	public IntVector2 GetLocation(MazeCell[,] cells)
	{
		MazeCell tMin=null;
		float minDist = Mathf.Infinity;
		Vector3 currentPos = transform.position;
		foreach(MazeCell c in cells)
		{
			float dist = Vector3.Distance(c.gameObject.transform.position, currentPos);
			if(dist<minDist)
			{
				tMin = c;
				minDist = dist;
			}
		}
		return tMin.coordinates;
	}
}
