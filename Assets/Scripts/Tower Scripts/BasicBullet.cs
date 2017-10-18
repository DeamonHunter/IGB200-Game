using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : MonoBehaviour {
    //Setup Bullet
    public GameObject Target;
    private Vector3 lastKnownLocation;

    //Bullet Variables
    public float Damage;
    public float Speed;
    private bool hitEnemy;

    // Update is called once per frame
    void Update() {
        if (Target != null)
            lastKnownLocation = Target.transform.position;
        else if (lastKnownLocation == Vector3.zero)
            Destroy(gameObject);

        MoveTowardsTarget();
        if ((lastKnownLocation - transform.position).magnitude < 0.1f)
            Destroy(gameObject);
    }

    private void MoveTowardsTarget() {
        var pos = Vector3.MoveTowards(transform.position, lastKnownLocation, Speed * Time.deltaTime);
        transform.position = pos;
        transform.LookAt(lastKnownLocation);
        transform.eulerAngles += new Vector3(0, 0, 90);
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if (other.tag == "Enemy" && !hitEnemy) {
            hitEnemy = true;
            var enemy = other.gameObject.GetComponent<BasicEnemy>();
            if (enemy == null)
                return;
            enemy.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}
