using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColourScript : MonoBehaviour {

    public Color shape;

	// Use this for initialization
	void Start () {

        GetComponent<Renderer>().material.color = shape;


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
