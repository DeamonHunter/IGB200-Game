using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tower_Scripts;
using UnityEngine;

public class LightTower : Tower {
    private GameObject light;
    private float hit;
    private RaycastHit raycasthit;
    private int Reach = 1;

    protected override void Start() {
        base.Start();
        light = transform.GetChild(0).gameObject;
    }

    protected override bool LookAtEnemy(GameObject target) {
        bool result = base.LookAtEnemy(target);
        light.transform.LookAt(new Vector3(curLookDir.x, 0f, curLookDir.y) + transform.position + new Vector3(0, 0.5f, 0));
        return result;
    }

    private void Update() {
        Vector2 Test = new Vector2(transform.rotation.x, transform.rotation.y);
        if (Physics.Raycast(transform.position, Test, hit, Reach)) {
            Debug.Log("Eyyy Boi");
        }
    }
}
