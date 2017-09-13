using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Tower_Scripts {
    public abstract class Tower : MonoBehaviour {
        //Tower Setup
        public Transform BulletEmitter;
        public GameObject Bullet;
        public GameObject TowerObject;

        //Tower Properties
        public float BulletsPerSecond;
        public float BulletSpeed; //Shouldn't this be in the bullet?
        public float RotateSpeed;
        public int Cost;
        public int PathFindingCost; //For Enemy Pathfinding purposes
        public float Health;
        //public int Level // If we need it

        //private variables
        private List<GameObject> targets = new List<GameObject>();
        private SphereCollider collider; //Will we ever have a non sphere collider?
        protected Vector3 curLookDir = Vector3.forward;
        private float fireTimer;
        private float fireRate;

        /// <summary>
        /// Set a couple variables.
        /// </summary>
        protected virtual void Start() {
            fireRate = 1 / BulletsPerSecond;
            collider = GetComponent<SphereCollider>();
        }

        /// <summary>
        /// Update loop. Fixed update to ensure that x amount of updates per in-game second is run.
        /// </summary>
        protected virtual void FixedUpdate() {
            GameObject target = FindTarget();
            if (target == null)
                return;
            if (!LookAtEnemy(target))
                return;
            ShootAtEnemy(target);

        }

        /// <summary>
        /// Find a target out of all avaliable enemies.
        /// </summary>
        /// <returns>The oldest targetable enemy that is still alive/in range. Null if no enemy found.</returns>
        private GameObject FindTarget() {
            //Used to make sure that we remove old references.
            List<int> removePositions = new List<int>();

            GameObject target = null;
            for (int i = 0; i < targets.Count; i++) {
                if (targets[i] != null) {
                    //Is the enemy still in range
                    var distance = (targets[i].transform.position - transform.position - collider.center).magnitude;
                    if (distance < collider.radius + 1f) {
                        target = targets[i];
                        break;
                    }
                }
                removePositions.Add(i);
            }
            //Remove all enemies outside of range.
            if (removePositions.Count > 0) {
                removePositions.Reverse(); // We want to make sure positions don't change. So remove from last to first.
                foreach (var pos in removePositions)
                    targets.RemoveAt(pos);
            }

            return target;
        }

        /// <summary>
        /// Attempts to aim target at the enemy, but may be too far.
        /// </summary>
        /// <param name="target">Gameobject to aim tower at.</param>
        /// <returns>Is tower successfully alligned to target.</returns>
        protected virtual bool LookAtEnemy(GameObject target) {
            Vector3 targetDir = target.transform.position - transform.position;

            curLookDir = Vector3.RotateTowards(curLookDir, targetDir, RotateSpeed * Time.fixedDeltaTime * Mathf.Deg2Rad, 0.0F);
            targetDir.y = 0;
            //TODO: Need logic for turning actual tower here

            Vector3 targetLocation = curLookDir + transform.position;
            TowerObject.transform.LookAt(targetLocation);
            TowerObject.transform.eulerAngles = new Vector3(-90, TowerObject.transform.eulerAngles.y, 0);

            return Vector3.Angle(curLookDir, targetDir) < 5;
        }


        /// <summary>
        /// Shoot a bullet at the selected target.
        /// </summary>
        /// <param name="target">The gameobject that will be the bullet's target.</param>
        protected virtual void ShootAtEnemy(GameObject target) {
            if (Time.time > fireTimer) {
                var bullet = Instantiate(Bullet, BulletEmitter.transform.position, Quaternion.identity, transform).GetComponent<BasicBullet>();
                bullet.Target = target;
                //TODO: Maybe set bullet damage if adding upgrade system.
                fireTimer = Time.time + fireRate;
            }
        }

        protected virtual void OnTriggerEnter(Collider other) {
            if (!other.transform.CompareTag("Enemy"))
                return;
            targets.Add(other.gameObject);
        }

        public bool TakeDamage(float damage) {
            Health -= damage;
            if (Health <= 0) {
                Vector2I pos = GameController.instance.TC.WorldToTilePosition(transform.position);
                GameController.instance.TC.Tiles[pos.x, pos.y].DeleteTower();
                return true;
            }
            return false;
        }
    }
}
