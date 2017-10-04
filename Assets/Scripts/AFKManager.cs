﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFKManager : MonoBehaviour {
    public CameraMove camera;
    public GameObject Cursor;

    private CameraMove cursor;
    // Use this for initialization
    private void Start() {
        camera.AllowPayerControl = false;
        cursor = Cursor.GetComponent<CameraMove>();
        cursor.AllowPayerControl = false;
        StartCoroutine(AFKScript());
    }


    IEnumerator AFKScript() {
        yield return new WaitForSeconds(3);
        camera.SetNewCameraMovePosition(new Vector3(10, 0, 10), 2);
        cursor.SetNewCameraMovePosition(new Vector3(10, 0, 10), 2);
        yield return new WaitForSeconds(3);
        camera.SetNewCameraMovePosition(new Vector3(-10, 0, 10), 2);
        cursor.SetNewCameraMovePosition(new Vector3(-10, 0, 10), 2);
        Debug.Log("Hello");
    }
    // Update is called once per frame
    private void Update() {

    }
}
