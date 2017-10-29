using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enemies;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private float newIntensity;

    public AFKManager AFK;
    private bool afkMode;

    private ResourceScript rs;
    private bool waveActive;

    public Wave[] Waves;

    // UI Elements
    public GameObject enemyMenu;
    public GameObject crawlerInfo;
    public GameObject tunnelerInfo;
    public GameObject superposInfo;
    public GameObject swarmerInfo;

    public GameObject openButton;
    public GameObject closeButton;

    public AudioSource waveIsInactive;
    public AudioSource waveIsActive;

    private bool isFadingOut = false;
    private bool isFadingIn = false;
    private bool oneSet = false;


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

        //Screen.SetResolution(Screen.height * 9 / 16, Screen.height, false);
    }

    // Use this for initialization
    private void Start() {
        TC = Instantiate(TileControlPreFab).GetComponent<TileController>();
        rs = GetComponent<ResourceScript>();
        TC.Setup();
        TC.Cursor.GetComponent<ParticleSystem>().Stop();
        Minimap.Setup(TC.NumTiles);
        EnemyParent = new GameObject();

        waveText.text = currentWave.ToString();

        Spawners = new SpawnerScript[StartPos.Length];
        for (int i = 0; i < StartPos.Length; i++) {
            Spawners[i] = Instantiate(SpawnerPreFab, TC.TileToWorldPosition(StartPos[i]), transform.rotation).GetComponent<SpawnerScript>();
            Spawners[i].Setup(StartPos[i]);
            Spawners[i].UpdatePath();
        }

        if (AFK != null) {
            afkMode = true;
            TC.AllowPlayerMovement = false;
            StartWaveText.GetComponent<Text>().text = "Press G to Start the game!";
        }
    }

    // Update is called once per frame
    private void Update() {

        ControlLight();

        SetWaveActiveSound();
        SetWaveInactiveSound();

        waveText.text = currentWave.ToString();

        if (afkMode && Input.GetKeyDown(KeyCode.G)) {
            SceneManager.LoadScene("Main");
        }

        if (EnemyParent.transform.childCount <= 0) {
            spawning = false;
            foreach (var spawner in Spawners) {
                spawning = spawning || spawner.Spawning;
            }
            if (!spawning && Input.GetKeyDown(KeyCode.G)) {
                isFadingOut = true;
                oneSet = true;
                // Enemy Information
                if (currentWave == 0) {
                    Debug.Log ("Wave 1 start");
                    closeButton.SetActive (true);
                    enemyMenu.SetActive(true);
                    crawlerInfo.SetActive(true);
                } else if (currentWave == 2) {
                    closeButton.SetActive (true);
                    enemyMenu.SetActive(true);
                    tunnelerInfo.SetActive(true);
                } else if (currentWave == 4) {
                    closeButton.SetActive (true);
                    enemyMenu.SetActive(true);
                    superposInfo.SetActive(true);
                } else if (currentWave == 7) {
                    closeButton.SetActive (true);
                    enemyMenu.SetActive(true);
                    swarmerInfo.SetActive(true);
                }

                CreateNewWave();
                spawning = true;
                waveActive = true;
            }
            else if (!spawning && waveActive) {
                StartWaveText.SetActive(true);
                rs.GainMoney(Waves[currentWave].WaveBonus);
                waveActive = false;
                isFadingIn = true;
                oneSet = true;
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

    public void CreateNewWave() {
        if (currentWave < Waves.Length) {
            List<int>[] enemies = new List<int>[Spawners.Length];
            for (int i = 0; i < Spawners.Length; i++)
                enemies[i] = new List<int>();
            for (int i = 0; i < Waves[currentWave].Enemies.Length; i++) {
                enemies[Waves[currentWave].Spawner[i]].Add(Waves[currentWave].Enemies[i]);
            }
            for (int i = 0; i < enemies.Length; i++) {
                if (enemies[i].Count <= 0)
                    continue;
                Spawners[i].NewWave(enemies[i], 1, Waves[currentWave].SecondsBetweenEnemies, Waves[currentWave].IsWaveDark);
                Spawners[i].EnemyHealthMultipliers = Waves[currentWave].HealthMultipliers;
            }
        }
        else {
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
                    Spawners[num].NewWave(enemies, 1, 1f / (4 - numActiveSpawners), true);
                }
            }
        }
        currentWave++;
        if (currentWave <= 5 && currentWave > 1) {
            if (currentWave == 5)
                newIntensity = 0;
            else
                newIntensity = mainLight.intensity / 2;
            dimLight = true;
        }
        if (!afkMode)
            StartWaveText.SetActive(false);
    }

    public bool IsSpawning() {
        return spawning;
    }

    private void ControlLight() {
        if (!dimLight)
            return;
        mainLight.intensity = Mathf.MoveTowards(mainLight.intensity, newIntensity, Time.deltaTime / 10);
        if (Mathf.Abs(newIntensity - mainLight.intensity) <= 0.01)
            dimLight = false;
    }

    public void FlipLineRenderers() {
        foreach (var spawner in Spawners) {
            spawner.ToggleLR();
        }
    }

    public void FlipCursorRangeSystem() {
        var ps = TC.Cursor.GetComponent<ParticleSystem>();
        
        if (ps.isPlaying)
            ps.Stop();
        else 
            ps.Play();
    }

    void SetWaveActiveSound() {
        if (isFadingOut) {
            FadeOut(waveIsInactive);
            FadeIn(waveIsActive);
        }
    }

    void SetWaveInactiveSound() {
        if (isFadingIn) {
            FadeOut(waveIsActive);
            FadeIn(waveIsInactive);
        }
    }

    void FadeIn(AudioSource IN) {
        if (oneSet) {
            IN.volume = 0.1f;
            IN.Play();
            oneSet = false;
        }
        if (IN.volume < 0.7) {
            IN.volume += 0.15f * Time.deltaTime;
        }else {
            isFadingIn = false;
        }
    }

    void FadeOut(AudioSource OUT) {
        if(OUT.volume > 0.1) {
            OUT.volume -= 0.15f * Time.deltaTime;
        }
        if (OUT.volume <= 0.1) {
            OUT.Stop();
            isFadingOut = false;
        }
    }
}
