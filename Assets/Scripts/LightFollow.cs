using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFollow : MonoBehaviour {

    public GameObject GO;
    private Light light;
    private Plane plane;

    float camRayLength = 100f;

    private void Start() {
        light = GO.GetComponent<Light>();
        plane = new Plane(Vector3.up, Vector3.zero);
    }

    void Update() {

        LookAtMouseRay();

    }

    void LookAtMouseRay() {

        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        float distance;
        if (plane.Raycast(camRay, out distance)) {

            Vector3 lightToMouse = camRay.GetPoint(distance) - GO.transform.position;
            light.spotAngle = Mathf.Min(50, 90 / Mathf.Log(lightToMouse.magnitude));
            lightToMouse.y = -GO.transform.position.y;
            Quaternion rotation = Quaternion.LookRotation(lightToMouse);
            GO.transform.rotation = rotation;

        }

    }
}
