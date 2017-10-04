using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFollow : MonoBehaviour {

    public GameObject Light;

    float camRayLength = 100f;

    void Update() {

        LookAtMouseRay();

    }

    void LookAtMouseRay() {

        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength)) {

            Vector3 lightToMouse = floorHit.point - Light.transform.position;
            lightToMouse.y = -Light.transform.position.y;
            Quaternion rotation = Quaternion.LookRotation(lightToMouse);
            Light.transform.rotation = rotation;

        }

    }
}
