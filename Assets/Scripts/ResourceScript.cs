using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	// UI Objects
	public Text livesText;
	public Text moneyText;

	// Use this for initialization
	void Start () {
		lives = maxLives;
		money = baseMoney;
	}
	
	// Update is called once per frame
	void Update () {

		livesText.text = lives.ToString();
		moneyText.text = money.ToString();

		timeCount = Time.time;
		if (timeCount - lastUpdate >= moneyTime) {
			money += moneyIncrease;
			lastUpdate = Time.time;
		}
	}

	public int GetTotalLives() {
		return lives;
	}

	public int GetTotalMoney() {
		return money;
	}

	public void LoseLife(int lostLives) {
		if (lives < 0) {
			lives -= lostLives;
		}
	}

	public void GainMoney(int gainedMoney){
		money += gainedMoney;
	}

	public void PurchaseItem(int cost) {
		money -= cost;
	}

}
