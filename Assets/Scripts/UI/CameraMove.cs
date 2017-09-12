using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {
    public float CameraSpeed;
    // Use this for initialization
    private void Start() {

    }

    // Update is called once per frame
    private void Update() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        transform.position += new Vector3(horizontal * CameraSpeed * Time.deltaTime, 0, vertical * CameraSpeed * Time.deltaTime);
    }
}
