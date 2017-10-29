using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : BasicEnemy {

    public GameObject barrier;
    public float barrierHealth          = 3.0f;
    private float maxBarrierHealth      = 3.0f;
    public Color colourBlue             = Color.blue;
    public Color colourYellow           = Color.yellow;
    public Color colourRed              = Color.red;

    private float currentRegenWaitTime;
    public float regenWaitTime          = 3.0f;

    public bool canRegen                = false;
    public bool canRegenAfterDestroy    = false;

    public AudioSource shieldHit;

    protected override void Update() {
        if (AllowMove && !Attacking && pathNum < path.Count)
            MoveToNextPath();
        if (Attacking)
            AttackNextPos();
        if (IsInDark) {
            if (!revealing) {
                if (currentRegenWaitTime < regenWaitTime + 1.0f) {
                    currentRegenWaitTime += Time.deltaTime;
                }
                if (barrier != null) {
                    if (canRegenAfterDestroy) {
                        if (canRegen && currentRegenWaitTime >= regenWaitTime && barrierHealth < maxBarrierHealth) {
                            barrierHealth += Time.deltaTime;
                        }
                    } else {
                        if (canRegen && currentRegenWaitTime >= regenWaitTime && barrierHealth > 0 && barrierHealth < maxBarrierHealth) {
                            barrierHealth += Time.deltaTime;
                        }
                    }
                }
            } else {
                currentRegenWaitTime = 0;
                Debug.Log(currentRegenWaitTime);
            }

            if (revealCoolDown > 0)
                revealCoolDown -= Time.deltaTime;
            else {
                revealing = false;
                timeTillReveal = 1f;
                revealCoolDown = 0;
            }             
        }

        if (barrier != null) {
            if (barrierHealth <= 0) { 
                barrier.gameObject.SetActive(false);
                //Destroy(barrier.gameObject);
            } else if (barrierHealth > 0 && revealing) {
                barrierHealth -= Time.deltaTime;
            }

            if (canRegenAfterDestroy && barrierHealth >= 0) {
                barrier.gameObject.SetActive(true);
            }

            Renderer rend = barrier.GetComponent<Renderer>();
            rend.material.shader = Shader.Find("Standard");

            if (barrierHealth > 2.0f) {
                rend.material.SetColor("_Color", colourBlue);
            } else if (barrierHealth <= 2.0f && barrierHealth > 1.0f) {
                rend.material.SetColor("_Color", colourYellow);
            } else {
                rend.material.SetColor("_Color", colourRed);
            }
        }
    }

    public override void TakeDamage(float amount) {

        if (barrierHealth <= 0) {
            curHealth -= amount;
            if (curHealth <= 0) {
                //Gain resources.
                gameController.GetComponent<ResourceScript>().GainMoney(carriedGold);
                Destroy(this.gameObject);
            }
        } else {
            PlayObjectHit();
        }
    }

    void PlayObjectHit() {
        if (shieldHit != null) {
            shieldHit.Play();
        }
    }

}
