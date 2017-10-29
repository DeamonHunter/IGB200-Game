using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpotter : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Enemy") {
            var enemy = other.GetComponent < BasicEnemy>();
            if (enemy.IsInDark)
                enemy.RevealEnemy(Time.fixedDeltaTime);
        }
    }

}
