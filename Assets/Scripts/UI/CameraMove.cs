using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {
	public float CameraSpeed;

	private Vector3 position;
	private float zBoundary = 27.7f;
	private float xBoundary = 14.2f;


	// Use this for initialization
	private void Start() {

	}

	// Update is called once per frame
	private void Update() {

		position = transform.position;

		CameraMovement();
		BoundaryCheck();

		transform.position = position;

	}

	private void CameraMovement() {
		//Right Movement
		if (Input.GetKey("d"))
			position.x += CameraSpeed * Time.deltaTime;

		//Left Movement
		if (Input.GetKey("a"))
			position.x -= CameraSpeed * Time.deltaTime;

		//Up Movement
		if (Input.GetKey("w"))
			position.z += CameraSpeed * Time.deltaTime;

		//Down Movement
		if (Input.GetKey("s"))
			position.z -= CameraSpeed * Time.deltaTime;
	}

	private void BoundaryCheck() {
		//X Boundary Check
		if (position.x <= -xBoundary) {
			position.x = -xBoundary;
		}else if (position.x >= xBoundary) {
			position.x = xBoundary;
		}

		//Z Boundary Check
		if (position.z <= -zBoundary) {
			position.z = -zBoundary;
		} else if (position.z >= zBoundary) {
			position.z = zBoundary;
		}

	}
}
