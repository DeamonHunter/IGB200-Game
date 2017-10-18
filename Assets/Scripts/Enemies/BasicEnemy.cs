using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour {
    public bool AllowMove;
    public float MinDistance;
    public float BaseHealth;
    public float LightSpeed;
    public float DarkSpeed;
    public float Damage;
    public int BaseDamage = 1;
    public float AttackSpeed;
    public bool IgnoresWalls;

    public bool IsInDark = false;
    private float timeTillReveal = 1f;
    private bool revealing = false;
    private float revealCoolDown = 0;

    public int carriedGold = 5;

    private float attackTimer;
    protected List<Vector3> path;
    protected int pathNum = 0;
    protected bool Attacking;
    public float curSpeed;
    protected float curHealth;

    protected bool slowed = false;

    protected float baseSpeed;

    protected GameObject gameController;
    protected bool CalculatePathOnNextMovement;

    // Use this for initialization
    private void Start() {
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    public void SetSpeed() {
        if (IsInDark)
            curSpeed = DarkSpeed;
        else
            curSpeed = LightSpeed;
        baseSpeed = curSpeed;
    }

    // Update is called once per frame
    protected virtual void Update() {
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

    protected virtual void MoveToNextPath() {
        var pos = transform.position;
        if ((path[pathNum] - pos).sqrMagnitude <= MinDistance) {
            pathNum++;
            if (CalculatePathOnNextMovement)
                UpdatePath();
            if (pathNum >= path.Count)
                return;
            if (GameController.instance.TC.GetTileAtWorldPos(path[pathNum]).HasTower) {
                Attacking = true;
                return;
            }
        }
        transform.position = Vector3.MoveTowards(pos, path[pathNum], curSpeed * Time.deltaTime);
        transform.LookAt(path[pathNum]);
    }

    private void AttackNextPos() {
        if (Time.time < attackTimer)
            return;
        Tile tile = GameController.instance.TC.GetTileAtWorldPos(path[pathNum]);
        if (tile.DamageTower(Damage)) {
            Attacking = false;
        }
    }

    public void ChangeSpeed(float percent, bool isSlowed) {
        curSpeed = percent * baseSpeed;
        slowed = isSlowed;
    }

    public void SetHealth(float healthMultiplier) {
        curHealth = BaseHealth * healthMultiplier;
    }

    public void SetPath(List<Vector3> positions, int curPathNum) {
        AllowMove = true;
        path = positions;
        pathNum = curPathNum;
    }

    public virtual void TakeDamage(float amount) {
        curHealth -= amount;
        if (curHealth <= 0) {
            //Gain resources.
            gameController.GetComponent<ResourceScript>().GainMoney(carriedGold);
            Destroy(this.gameObject);
        }
    }

    public virtual void UpdatePath() {
        var tile = GameController.instance.TC.WorldToTilePosition(transform.position);
        try {
            path = GameController.instance.TC.TileToWorldPosition(GameController.instance.TC.PF.CalculatePath(tile));
        }
        catch (Exception) {
            path = GameController.instance.TC.TileToWorldPosition(GameController.instance.TC.PF.CalculatePath(tile, true, true));
        }
        for (int i = 0; i < path.Count; i++) {
            path[i] += new Vector3(0, 0.1f, 0);
        }
        pathNum = 0;
        if (GameController.instance.TC.GetTileAtWorldPos(path[pathNum]).HasTower)
            Attacking = true;
    }

    public int GetBaseDamage() {
        return BaseDamage;
    }

    public void RevealEnemy(float time) {
        timeTillReveal -= time;
        if (timeTillReveal < 0)
            EnemyIsRevealed();
        revealing = true;
        revealCoolDown = 0.5f;
    }

    protected void EnemyIsRevealed() {
        IsInDark = true;
        if (!slowed)
            curSpeed = LightSpeed;
        else
            curSpeed *= LightSpeed / DarkSpeed;
        baseSpeed = LightSpeed;
        //Do other stuff
    }
}
