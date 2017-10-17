using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneEnemy : BasicEnemy {

	public GameObject cloneSpawned;
	private List<Vector3> BasePath;

	public override void TakeDamage(float amount)
	{
		curHealth = curHealth - amount;
		if (curHealth <= 0)
		{
			//Gain resources.
			gameController.GetComponent<ResourceScript>().GainMoney(carriedGold);
			var enemy = Instantiate (cloneSpawned, transform.position, transform.rotation);
			for (int i = 0; i < 4; i++) {
				enemy.transform.GetChild(i).GetComponent<BasicEnemy>().SetPath(path, pathNum);
				enemy.transform.GetChild(i).GetComponent<BasicEnemy>().SetHealth(1);
			}
			Destroy(gameObject);
		}
	}
}