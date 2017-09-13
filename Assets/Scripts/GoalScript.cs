using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour {
    protected virtual void OnTriggerEnter(Collider other) {
        if (!other.transform.CompareTag("Enemy"))
            return;
        Destroy(other.gameObject);
        //TODO: Take life
    }
}
