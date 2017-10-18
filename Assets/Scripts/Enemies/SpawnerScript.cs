using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour {
    public GameObject[] Enemies;
    public bool Spawning = false;

    private Vector2I startPos;
    private LineRenderer lr;
    private List<Vector3> BasePath;
    private List<Vector3> BaseIgnorePath;
    private List<int> enemiesToSpawn;
    private float delayEnemies;
    public float[] EnemyHealthMultipliers;

    // Use this for initialization
    public void Setup(Vector2I start) {
        lr = GetComponent<LineRenderer>();
        startPos = start;
    }

    // Update is called once per frame
    private void Update() {

    }

    public void NewWave(List<int> enemies, float initialDelay, float delayBetweenEnemies) {
        enemiesToSpawn = new List<int>();
        enemiesToSpawn.AddRange(enemies); //Needed to be done. Copies pointer otherwise leading to null refs later.
        delayEnemies = delayBetweenEnemies;
        Spawning = true;
        Invoke("Spawn", initialDelay);
    }

    private void Spawn() {
        int enemyID = enemiesToSpawn[0];
        enemiesToSpawn.RemoveAt(0);
        var enemy = Instantiate(Enemies[enemyID], transform.position, transform.rotation, GameController.instance.EnemyParent.transform).GetComponent<BasicEnemy>();
        enemy.SetHealth(EnemyHealthMultipliers[enemyID]);
        if (enemy.IgnoresWalls)
            enemy.SetPath(BaseIgnorePath, 0);
        else
            enemy.SetPath(BasePath, 0);
        if (enemiesToSpawn.Count > 0)
            Invoke("Spawn", delayEnemies);
        else
            Spawning = false;
    }

    public void UpdatePath() {
        BasePath = GameController.instance.TC.TileToWorldPosition(GameController.instance.TC.PF.CalculatePath(startPos));
        BaseIgnorePath = GameController.instance.TC.TileToWorldPosition(GameController.instance.TC.PF.CalculatePath(startPos, true));
        var linePos = BasePath;
        for (int i = 0; i < linePos.Count; i++) {
            linePos[i] += new Vector3(0, 0.5f, 0);
        }
        lr.positionCount = linePos.Count;
        lr.SetPositions(linePos.ToArray());
    }

}
