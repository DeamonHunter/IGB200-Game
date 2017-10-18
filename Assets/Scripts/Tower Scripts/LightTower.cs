using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tower_Scripts;
using UnityEngine;

public class LightTower : Tower {
    private GameObject light;

    protected override void Start() {
        base.Start();
        light = transform.GetChild(0).gameObject;
    }

    protected override bool LookAtEnemy(GameObject target) {
        bool result = base.LookAtEnemy(target);
        light.transform.LookAt(new Vector3(curLookDir.x, 0f, curLookDir.y) + transform.position + new Vector3(0, 0.5f, 0));
        return result;
    }
}
