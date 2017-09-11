using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTower : MonoBehaviour {

    /// <summary>
    /// Code that runs the light tower in Quantambush. The tower will act the exact same as the basic tower but has a spotlight.
    /// 
    /// Important parts to have:
    ///                         .Needs to have a bullet emitter, this can be used to have the bullets fire from a barrel.
    /// </summary>

    private Transform target;

    public Transform Bullet_Emitter;
    public Rigidbody Bullet;
    private Rigidbody Temporary_Bullet_Handler;
    public GameObject Light;

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

    //Aims the spot light in the same directon as the tower and the gun barrel.
    void AimLight() {

        Vector3 lightDirection = target.position - Light.transform.position;
        Quaternion LightRotation = Quaternion.LookRotation(lightDirection);
        Light.transform.rotation = Quaternion.Lerp(Light.transform.rotation, LightRotation, rotateSpeed * Time.deltaTime);

    }

    void ShootAtEnemy() {

        if (Time.time > fireTimer) {

            Temporary_Bullet_Handler = Instantiate(Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as Rigidbody;

            Temporary_Bullet_Handler.transform.Rotate(Vector3.left * 270);

            Temporary_Bullet_Handler.AddForce(Bullet_Emitter.forward * Bullet_Forward_Force);

            fireTimer = Time.time + fireRate;

        }

    }

    void OnTriggerStay(Collider other) {

        if (other.transform.tag == "Enemy") {

            FindEnemy();
            LookAtEnemy();
            AimAtEnemy();
            AimLight();
            ShootAtEnemy();

        }

    }

}
