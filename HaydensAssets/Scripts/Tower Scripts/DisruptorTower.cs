using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisruptorTower : MonoBehaviour {

    /// <summary>
    /// Code that runs the disruptor tower in Quantambush. This tower will act the same as the basic tower but slows down the enemies move speed once they enter
    /// the collision box.
    /// </summary>

    private Transform target;

    public Transform Bullet_Emitter;
    public Rigidbody Bullet;
    private Rigidbody Temporary_Bullet_Handler;

    public float Bullet_Forward_Force;
    public float rotateSpeed;
    private float fireTimer;
    private float fireRate = 0.1f;

    void FindEnemy() {

        GameObject temp = GameObject.FindGameObjectWithTag("Enemy");
        target = temp.GetComponent<Transform>();

    }

    void LookAtEnemy() {

        Vector3 direction = target.position - transform.position;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

    }

    void AimAtEnemy() {

        Vector3 aimDirection = target.position - Bullet_Emitter.transform.position;
        Quaternion aimRotation = Quaternion.LookRotation(aimDirection);
        Bullet_Emitter.transform.rotation = Quaternion.Lerp(Bullet_Emitter.transform.rotation, aimRotation, rotateSpeed * Time.deltaTime);

    }

    void ShootAtEnemy() {

        if (Time.time > fireTimer) {

            Temporary_Bullet_Handler = Instantiate(Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as Rigidbody;

            Temporary_Bullet_Handler.transform.Rotate(Vector3.left * 270);

            Temporary_Bullet_Handler.AddForce(Bullet_Emitter.forward * Bullet_Forward_Force);

            fireTimer = Time.time + fireRate;

        }

    }

    //Code will need to be altered for different enemy types
    void SlowEnemy() {

        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        BasicEnemyMoveScript moveScript = enemy.GetComponent<BasicEnemyMoveScript>();
        moveScript.moveSpeed = 1.5f;

    }

    void SpeedUpEnemy() {

        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        BasicEnemyMoveScript moveScript = enemy.GetComponent<BasicEnemyMoveScript>();
        moveScript.moveSpeed = 3f;

    }

    void OnTriggerStay(Collider other) {

        if (other.transform.tag == "Enemy") {

            FindEnemy();
            LookAtEnemy();
            SlowEnemy();
            AimAtEnemy();
            ShootAtEnemy();

        }

    }

    void OnTriggerExit(Collider other) {

        if (other.transform.tag == "Enemy") {

            SpeedUpEnemy();

        }

    }

}