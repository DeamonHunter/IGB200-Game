using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResourceScript : MonoBehaviour {

    // Resource Variables
    private int lives;
    private int maxLives = 20;
    private int money;
    private int baseMoney = 250;
    private int moneyIncrease = 1;
    private float timeCount;
    private float lastUpdate;
    private float moneyTime = 1;
    private float timer;
    private bool spawning;

    // UI Objects
    public Text livesText;
    public Text moneyText;
    public Text gameOverText;

    private GameObject gameController;

    // Use this for initialization
    void Start() {
        lives = maxLives;
        money = baseMoney;
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    // Update is called once per frame
    void Update() {

        livesText.text = lives.ToString();
        moneyText.text = money.ToString();

        if (lives <= 0) {
            gameOverText.enabled = true;
            timer += Time.deltaTime;
            Debug.Log(timer);
            if (timer > 3.0f) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Debug.Log("Restarted");
            }
        }

        // If game isn't between waves, increase currency every second

        spawning = gameController.GetComponent<GameController>().IsSpawning();

        if (spawning) {
            timeCount = Time.time;
            if (timeCount - lastUpdate >= moneyTime) {
                money += moneyIncrease;
                lastUpdate = Time.time;
            }
        }
    }

    public int GetTotalLives() {
        return lives;
    }

    public int GetTotalMoney() {
        return money;
    }

    public void LoseLife(int lostLives) {
        if (lives > 0) {
            lives -= lostLives;
        }
    }

    public void GainMoney(int gainedMoney) {
        money += gainedMoney;
    }

    public void PurchaseItem(int cost) {
        money -= cost;
    }

}
