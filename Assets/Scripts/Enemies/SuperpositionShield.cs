using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : BasicEnemy {

    public GameObject barrier;
    public float barrierHealth = 3.0f;
    private float maxBarrierHealth = 3.0f;
    public bool isObserved;
    private float totalObserveTime;

    protected override void Update() {
        if (AllowMove && !Attacking && pathNum < path.Count)
            MoveToNextPath();
        if (Attacking)
            AttackNextPos();
        if (IsInDark) {
            if (!revealing)
                return;
            if (revealCoolDown > 0)
                revealCoolDown -= Time.deltaTime;
            else {
                revealing = false;
                timeTillReveal = 1f;
                revealCoolDown = 0;
            }
        }
    }

    public override void TakeDamage(float amount) {

        if (barrierHealth <= 0) {
            Destroy(barrier.gameObject);
        } else if (barrierHealth > 0 && isObserved) {
            barrierHealth -= Time.deltaTime;
        } else if (barrierHealth > 0 && !isObserved && barrierHealth < maxBarrierHealth) {
            barrierHealth += Time.deltaTime;
        }

        if (barrierHealth <= 0) {
            curHealth -= amount;
            if (curHealth <= 0) {
                //Gain resources.
                gameController.GetComponent<ResourceScript>().GainMoney(carriedGold);
                Destroy(this.gameObject);
            }
        }
    }
}
