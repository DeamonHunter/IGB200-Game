using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {
    public float CameraSpeed;
    public Vector2 LowerLeftCorner;
    public Vector2 UpperRightCorner;
    [HideInInspector]
    public bool AllowPayerControl = true;

    private float zBoundary = 27.7f;
    private float xBoundary = 14.2f;

    //Afk mode only
    private bool moving;
    private Vector3 movePos;
    private float speed;

    // Use this for initialization
    private void Start() {

    }

    // Update is called once per frame
    private void Update() {
        if (AllowPayerControl) {
            CameraMovement();
        }
        else if (moving)
            MoveTowards();

    }

    public void SetNewCameraMovePosition(Vector3 pos, float time) {
        movePos = new Vector3(pos.x, transform.position.y, pos.z);
        speed = Vector3.Magnitude(pos - transform.position) / time;
        moving = true;
    }

    private void MoveTowards() {
        transform.position = Vector3.MoveTowards(transform.position, movePos, Time.deltaTime * speed);
        if (Vector3.Magnitude(movePos - transform.position) < 0.01f)
            moving = false;
    }

    private void CameraMovement() {
        Vector3 position = transform.position;
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

        BoundaryCheck(position);
        transform.position = position;
    }

    private void BoundaryCheck(Vector3 position) {
        //X Boundary Check
        if (position.x <= LowerLeftCorner.x) {
            position.x = LowerLeftCorner.x;
        }
        else if (position.x >= UpperRightCorner.x) {
            position.x = UpperRightCorner.x;
        }

        //Z Boundary Check
        if (position.z <= LowerLeftCorner.y) {
            position.z = LowerLeftCorner.y;
        }
        else if (position.z >= UpperRightCorner.y) {
            position.z = UpperRightCorner.y;
        }

    }
}
