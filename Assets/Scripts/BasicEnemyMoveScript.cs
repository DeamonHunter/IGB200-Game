using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyMoveScript : MonoBehaviour {

    private Vector3 position;
    public float moveSpeed = 3.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Move();

	}

    void Move() {

        position = transform.position;

        //Right Movement
        if (Input.GetKey("d"))
            position.x += moveSpeed * Time.deltaTime;

        //Left Movement
        if (Input.GetKey("a"))
            position.x -= moveSpeed * Time.deltaTime;

        //Up Movement
        if (Input.GetKey("w"))
            position.z += moveSpeed * Time.deltaTime;

        //Down Movement
        if (Input.GetKey("s"))
            position.z -= moveSpeed * Time.deltaTime;

        transform.position = position;

    }

}
