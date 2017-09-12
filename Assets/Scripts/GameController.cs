using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    //Singleton Setup
    public static GameController instance = null;

    //Global References
    public GameObject TileControlPreFab;
    [HideInInspector]
    public TileController TC;

    public GameObject SpawnerPreFab;
    [HideInInspector]
    public SpawnerScript[] Spawners;
    public Vector2I[] StartPos;

    [HideInInspector]
    public GameObject EnemyParent;


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
    }

    // Use this for initialization
    private void Start() {
        TC = Instantiate(TileControlPreFab).GetComponent<TileController>();
        TC.Setup();
        EnemyParent = new GameObject();

        Spawners = new SpawnerScript[StartPos.Length];
        for (int i = 0; i < StartPos.Length; i++) {
            Spawners[i] = Instantiate(SpawnerPreFab, TC.TileToWorldPosition(StartPos[i]), transform.rotation).GetComponent<SpawnerScript>();
            Spawners[i].Setup(StartPos[i]);
            Spawners[i].UpdatePath();
        }
    }

    // Update is called once per frame
    private void Update() {
        if (EnemyParent.transform.childCount <= 0) {
            bool spawning = false;
            foreach (var spawner in Spawners) {
                spawning = spawning || spawner.Spawning;
            }
            if (!spawning)
                CreateNewWave();
        }
    }

    private void CreateNewWave() {
        List<int> enemies = new List<int>();
        enemies.AddRange(new[] { 0, 0, 0, 0, 0, 0, 0 });
        foreach (var spawner in Spawners) {
            spawner.NewWave(enemies, 1, 1);
        }
    }
}
