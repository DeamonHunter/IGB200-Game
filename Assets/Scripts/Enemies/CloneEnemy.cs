using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneEnemy : BasicEnemy {

	public GameObject cloneSpawned1;
	public GameObject cloneSpawned2;
	public GameObject cloneSpawned3;
	public GameObject cloneSpawned4;

	private List<Vector3> BasePath;

	public override void TakeDamage(float amount)
	{
		curHealth = curHealth - amount;
		if (curHealth <= 0)
		{
			//Gain resources.
			gameController.GetComponent<ResourceScript>().GainMoney(carriedGold);

			// Spawn cloned enemies
			var enemy1 = Instantiate (cloneSpawned1, transform.position, transform.rotation, GameController.instance.EnemyParent.transform);
			var enemy2 = Instantiate (cloneSpawned2, transform.position, transform.rotation, GameController.instance.EnemyParent.transform);
			var enemy3 = Instantiate (cloneSpawned3, transform.position, transform.rotation, GameController.instance.EnemyParent.transform);
			var enemy4 = Instantiate (cloneSpawned4, transform.position, transform.rotation, GameController.instance.EnemyParent.transform);

			enemy1.transform.GetComponent<BasicEnemy>().SetPath(path, pathNum);
			enemy1.transform.GetComponent<BasicEnemy>().SetHealth(1);

			enemy2.transform.GetComponent<BasicEnemy>().SetPath(path, pathNum);
			enemy2.transform.GetComponent<BasicEnemy>().SetHealth(1);

			enemy3.transform.GetComponent<BasicEnemy>().SetPath(path, pathNum);
			enemy3.transform.GetComponent<BasicEnemy>().SetHealth(1);

			enemy4.transform.GetComponent<BasicEnemy>().SetPath(path, pathNum);
			enemy4.transform.GetComponent<BasicEnemy>().SetHealth(1);

			Destroy(gameObject);
		}
	}
}