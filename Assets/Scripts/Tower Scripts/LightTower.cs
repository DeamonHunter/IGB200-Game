using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tower_Scripts;
using UnityEngine;

public class LightTower : Tower {
    public GameObject Light;
    public GameObject LightTarget;
    private int Reach = 1;
    private Plane pl = new Plane(Vector3.up, Vector3.zero);

    protected override bool LookAtEnemy(GameObject target) {
        TowerObject.transform.LookAt(target.transform.position);
        TowerObject.transform.eulerAngles = new Vector3(-90, TowerObject.transform.eulerAngles.y, 0);
        Light.transform.LookAt(target.transform);

        //Light.transform.LookAt(new Vector3(curLookDir.x, 0, curLookDir.y) * (target.transform.position - transform.position).magnitude + new Vector3(transform.position.x, 0, transform.position.y));
       
        //Something broke this
        //Ray ray = new Ray(transform.position, Light.transform.forward);
        //float distance;
        //if (pl.Raycast(ray, out distance)) {
        //    LightTarget.transform.position = ray.GetPoint(distance);
        //}

        LightTarget.transform.position = target.transform.position;

        return true;
    }

    private void Update() {
    }
}
