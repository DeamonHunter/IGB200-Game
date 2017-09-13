using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour {

	public int damagePerEnemy = 1;
	private GameObject gameController;

	// Use this for initialization
	private void Start() {
		gameController = GameObject.FindGameObjectWithTag ("GameController");
	}

    protected virtual void OnTriggerEnter(Collider other) {
        if (!other.transform.CompareTag("Enemy"))
            return;
		gameController.GetComponent<ResourceScript>().LoseLife(damagePerEnemy);
        Destroy(other.gameObject);
    }
}
