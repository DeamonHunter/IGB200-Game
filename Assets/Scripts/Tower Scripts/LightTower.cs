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
        Debug.Log(curLookDir);
        light.transform.LookAt(curLookDir);
        return result;
    }
}
