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
    private int baseMoney = 150;
    private int moneyIncrease = 1;
    private float timeCount;
    private float lastUpdate;
    private float moneyTime = 1;
    private float timer;
    private bool spawning;

    // UI Objects
    public Text livesText;
    public Text moneyText;
    public GameObject GameOverImage;

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
            timer += Time.unscaledDeltaTime;
            if (timer > 3.0f) {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        // If game isn't between waves, increase currency every second

        spawning = gameController.GetComponent<GameController>().IsSpawning();

    }

    public int GetTotalLives() {
        return lives;
    }

    public int GetTotalMoney() {
        return money;
    }

    public void LoseLife(int lostLives) {
        lives -= lostLives;
        if (lives <= 0)
            GameOver();
    }

    private void GameOver() {
        GameOverImage.SetActive(true);
        timer = 0;
        Time.timeScale = 0.1f;
    }

    public void GainMoney(int gainedMoney) {
        money += gainedMoney;
    }

    public void PurchaseItem(int cost) {
        money -= cost;
    }

}
