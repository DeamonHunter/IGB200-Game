using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tower_Scripts;
using UnityEngine;

public class RocketTurret : Tower {

	/// <summary>
	/// Shoot a bullet at the selected target.
	/// </summary>
	/// <param name="target">The gameobject that will be the bullet's target.</param>
	protected override void ShootAtEnemy(GameObject target) {
		if (Time.time > fireTimer) {
			var bullet = Instantiate(Bullet, BulletEmitter.transform.position, Quaternion.identity, transform).GetComponent<RocketBullet>();
			bullet.Target = target;
			fireTimer = Time.time + fireRate;
		}
	}

}
