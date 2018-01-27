using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	public bool mouseLook;
	public GameObject mainCamera;
	Vector3 movementGoal;
	public float mouseSensitivity;
	Rigidbody ownRigidBody;
	public float maxSpeed;
	public bool LockMouse;


	// Use this for initialization
	void Start () {
		ownRigidBody = this.GetComponent<Rigidbody> ();
		if (LockMouse) {
			Cursor.lockState = CursorLockMode.Locked;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (mouseLook) {
			this.transform.Rotate(new Vector3 (0f, Input.GetAxisRaw ("Mouse X") * mouseSensitivity * Time.deltaTime));
			float xRot = mainCamera.transform.rotation.eulerAngles.x;
			if (xRot > 180f) {
				xRot -= 360;
			}
			float clampedView = Mathf.Clamp (xRot - Input.GetAxisRaw ("Mouse Y") * mouseSensitivity * Time.deltaTime, -90, 90);
			mainCamera.transform.rotation = Quaternion.Euler (clampedView, this.transform.rotation.eulerAngles.y, 0f);
		}

	}

	void FixedUpdate(){
		movementGoal = (new Vector3 (Input.GetAxis ("Horizontal"),0f, Input.GetAxis ("Vertical"))).normalized;
		movementGoal = Quaternion.Euler (new Vector3 (0f, this.transform.rotation.eulerAngles.y)) * movementGoal;
		Vector3 tempVelocity = ownRigidBody.velocity;
		tempVelocity.x = movementGoal.x * maxSpeed;
		tempVelocity.z = movementGoal.z * maxSpeed; 
		ownRigidBody.velocity = tempVelocity;
	}
}
