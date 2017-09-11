using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : MonoBehaviour {

    /// <summary>
    /// Code that runs the basic tower in Quantambush. The tower will simply aim at an enemy and fire.
    /// 
    /// Important parts to have:
    ///                         .Needs to have a bullet emitter, this can be used to have the bullets fire from a barrel.
    /// </summary>

    private Transform target;

    public Transform Bullet_Emitter;
    public Rigidbody Bullet;
    private Rigidbody Temporary_Bullet_Handler;

    public float Bullet_Forward_Force;
    public float rotateSpeed;
    private float fireTimer;
    private float fireRate = 0.1f;

    //This will need to be changed and altered for the different types of enemies and may need an overflow so that it can handle 
    //multiple enemies at once
    void FindEnemy() {

        GameObject temp = GameObject.FindGameObjectWithTag("Enemy");
        target = temp.GetComponent<Transform>();

    }

    //This aims the tower at the enemy but does not aim the barrel/bullet emitter. This is so that the tower does not rotate into the ground.
    void LookAtEnemy() {

        Vector3 direction = target.position - transform.position;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);

    }

    //This actually aims the bullet emitter/barrel at the enemy for a more precise aiming system.
    void AimAtEnemy() {

        Vector3 aimDirection = target.position - Bullet_Emitter.transform.position;
        Quaternion aimRotation = Quaternion.LookRotation(aimDirection);
        Bullet_Emitter.transform.rotation = Quaternion.Lerp(Bullet_Emitter.transform.rotation, aimRotation, rotateSpeed * Time.deltaTime);

    }

    //Fires rigidbody projectiles at the enemy every few miliseconds. Bulllets may need to be altered slightly to actually come out of the barrel correctly.
    void ShootAtEnemy() {

        if (Time.time > fireTimer) {

            Temporary_Bullet_Handler = Instantiate(Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as Rigidbody;

            Temporary_Bullet_Handler.transform.Rotate(Vector3.left * 270);

            Temporary_Bullet_Handler.AddForce(Bullet_Emitter.forward * Bullet_Forward_Force);

            fireTimer = Time.time + fireRate;

        }

    }

    //Looks for an enemy within the collider and executes code. Will need to be altered for different enemy types.
    void OnTriggerStay(Collider other) {

        if (other.transform.tag == "Enemy") {

            FindEnemy();
            LookAtEnemy();
            AimAtEnemy();
            ShootAtEnemy();

        }

    }

}
