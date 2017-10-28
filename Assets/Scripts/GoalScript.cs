using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour {

	public int damagePerEnemy;
	private GameObject gameController;

    public AudioSource objectHit;

	// Use this for initialization
	private void Start() {
		gameController = GameObject.FindGameObjectWithTag ("GameController");
	}

    protected virtual void OnTriggerEnter(Collider other) {
        if (!other.transform.CompareTag("Enemy"))
            return;
		damagePerEnemy = other.gameObject.GetComponent<BasicEnemy> ().BaseDamage;
		gameController.GetComponent<ResourceScript>().LoseLife(damagePerEnemy);
        PlayObjectHit();
        Destroy(other.gameObject);
    }

    void PlayObjectHit() {
        if(objectHit != null) {
            objectHit.Play();
        }
    }

}
