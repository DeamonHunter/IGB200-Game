using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFKManager : MonoBehaviour {
    public CameraMove camera;

    // Use this for initialization
    private void Start() {
        camera.AllowPayerControl = false;
        StartCoroutine(AFKScript());
    }


    IEnumerator AFKScript() {
        yield return new WaitForSeconds(3);
        camera.SetNewCameraMovePosition(new Vector3(10, 0, 10), 2);
        Debug.Log("Hello");
    }
    // Update is called once per frame
    private void Update() {

    }
}
