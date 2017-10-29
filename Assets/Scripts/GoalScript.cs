using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalScript : MonoBehaviour {

	public int damagePerEnemy;
	private GameObject gameController;

    public AudioSource objectHit;
    public Image damageUI;

    private int startFade = 120;
    public int fade;

    private float fadeCount;
    private bool tookDamage = false;
    private int fadeScale;
    private int startFadeScale = 5;
    private int lastFade;
    private int minFade = 20;
    private int fadeIncrease = 8;
    private int fadeIncrement = 5;


    // Use this for initialization
    private void Start() {
		gameController = GameObject.FindGameObjectWithTag ("GameController");
        fade = startFade;
        fadeCount = startFade;
        lastFade = startFade;
        fadeScale = startFadeScale;
    }

    private void Update() {

        if (tookDamage) {
            damageUI.gameObject.SetActive(true);

            fadeCount -= Time.unscaledDeltaTime*fadeScale;

            if (fade >= minFade) {
                fade = Mathf.FloorToInt(fadeCount);
                Debug.Log(fade);
                damageUI.GetComponent<Image>().color = new Color32(255, 0, 0, (byte)fade);

                if (fade <= lastFade - fadeIncrement) {
                    lastFade = fade;
                    fadeScale += fadeIncrease;
                }
            } else {
                damageUI.gameObject.SetActive(false);
                fade = startFade;
                damageUI.GetComponent<Image>().color = new Color32(255, 0, 0, (byte)fade);
                fadeCount = startFade;
                tookDamage = false;
            }
        }

    }

    protected virtual void OnTriggerEnter(Collider other) {
        if (!other.transform.CompareTag("Enemy"))
            return;
        if (!tookDamage) {
            tookDamage = true;
        } else {
            fade = startFade;
            fadeCount = startFade;
            lastFade = startFade;
            fadeScale = startFadeScale;
        }
		damagePerEnemy = other.gameObject.GetComponent<BasicEnemy> ().BaseDamage;
		gameController.GetComponent<ResourceScript>().LoseLife(damagePerEnemy);
        PlayObjectHit();
        Destroy(other.gameObject);
    }

    void PlayObjectHit() {
        if(objectHit != null) {
            objectHit.Play();
        }
    }
}
