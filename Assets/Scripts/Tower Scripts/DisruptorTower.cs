using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tower_Scripts;
using UnityEngine;


/// <summary>
/// Code that runs the disruptor tower in Quantambush. This tower will act the same as the basic tower but slows down the enemies move speed once they enter
/// the collision box.
/// </summary>
public class DisruptorTower : Tower {
    //Code will need to be altered for different enemy types
    private void SlowEnemy(BasicEnemy enemy) {
        enemy.ChangeSpeed(0.5f, true);
    }

    private void ResetEnemy(BasicEnemy enemy) {
        enemy.ChangeSpeed(1, false);
    }

    private void OnTriggerExit(Collider other) {
        if (other.transform.tag == "Enemy")
            ResetEnemy(other.gameObject.GetComponent<BasicEnemy>());
    }

    protected override void OnTriggerEnter(Collider other) {
        base.OnTriggerEnter(other);
        if (other.transform.tag == "Enemy")
            SlowEnemy(other.gameObject.GetComponent<BasicEnemy>());
    }
}