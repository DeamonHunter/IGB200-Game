using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpotter : MonoBehaviour {

    public List<GameObject> targets = new List<GameObject>();

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (targets.Any()) {
            foreach (GameObject enemy in targets) {
                for (int i = targets.Count - 1; i >= 0; i--) {
                    if (enemy == null) {
                        targets.Remove(enemy);
                    } else if (enemy.tag == "Enemy" /*&& wave > 3*/) {
                        enemy.GetComponent<BasicEnemy>().IsInDark = true;
                        enemy.GetComponent<Tunneller>().IsInDark = true;
                    }
                }

            }

        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Enemy" && !targets.Contains(other.gameObject)) {
            targets.Add(other.gameObject);
        }
    }

}
