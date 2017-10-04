using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	//Singleton Setup
	public static GameController instance = null;

	//Global References
	public GameObject TileControlPreFab;
	public MinimapController Minimap;
	[HideInInspector]
	public TileController TC;

	public GameObject SpawnerPreFab;
	[HideInInspector]
	public SpawnerScript[] Spawners;
	public Vector2I[] StartPos;

	[HideInInspector]
	public GameObject EnemyParent;

	public GameObject StartWaveText;
	public Text waveText;
	private int currentWave = 0;
	private bool spawning = false;

	public Light mainLight;
	private bool dimLight = false;

	// Awake Checks - Singleton setup
	void Awake() {

		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);

		Screen.SetResolution(Screen.height * 9 / 16, Screen.height, false);
	}

	// Use this for initialization
	private void Start() {
		TC = Instantiate(TileControlPreFab).GetComponent<TileController>();
		TC.Setup();
		Minimap.Setup(TC.NumTiles);
		EnemyParent = new GameObject();

		waveText.text = currentWave.ToString();

		Spawners = new SpawnerScript[StartPos.Length];
		for (int i = 0; i < StartPos.Length; i++) {
			Spawners[i] = Instantiate(SpawnerPreFab, TC.TileToWorldPosition(StartPos[i]), transform.rotation).GetComponent<SpawnerScript>();
			Spawners[i].Setup(StartPos[i]);
			Spawners[i].UpdatePath();
		}
	}

	// Update is called once per frame
	private void Update() {

		ControlLight();

		waveText.text = currentWave.ToString();

		if (EnemyParent.transform.childCount <= 0) {
			spawning = false;
			foreach (var spawner in Spawners) {
				spawning = spawning || spawner.Spawning;
			}
			if (!spawning && Input.GetKeyDown(KeyCode.G)) {
				CreateNewWave();
				spawning = true;
				if (currentWave < 5) {
					dimLight = true;
				}
			}
			else if (!spawning) {
				StartWaveText.SetActive(true);
			}
		}
	}

	public void UpdatePathFinding() {
		foreach (var spawner in Spawners) {
			spawner.UpdatePath();
		}
		for (int i = 0; i < EnemyParent.transform.childCount; i++) {
			EnemyParent.transform.GetChild(i).GetComponent<BasicEnemy>().UpdatePath();
		}
	}

	public void ChangeTower(int towerNum) {
		Debug.Log(towerNum);
		TC.SelectedTower = towerNum;
	}

	private void CreateNewWave() {
		currentWave++;
		int numEnemies = currentWave * 4 + 4;
		int numActiveSpawners = Random.Range(1, 4);
		int spawnersSet = 0;
		while (spawnersSet < numActiveSpawners) {
			int num = Random.Range(0, 4);
			if (!Spawners[num].Spawning) {
				List<int> enemies = new List<int>();
				spawnersSet++;
				for (int i = 0; i < (numEnemies + 1) / numActiveSpawners; i++)
					enemies.Add(0);
				Spawners[num].NewWave(enemies, 1, 1f / (4 - numActiveSpawners), currentWave);
			}
		}

		StartWaveText.SetActive(false);
	}

	public bool IsSpawning() {
		return spawning;
	}

	void ControlLight() {
		if (dimLight) {
			if (mainLight.intensity > 0 && currentWave < 5 && currentWave > 1) {
				mainLight.intensity = mainLight.intensity / 2;
			} else if (currentWave >= 5) {
				mainLight.intensity = 0;
			}

			dimLight = false;
		}

	}

}
