﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour {
    public bool AllowMove;
    public float MinDistance;
    public float Health;
    public float Speed;
    public float Damage;
    public float AttackSpeed;

    private float attackTimer;
    private List<Vector3> path;
    private int pathNum = 0;
    private bool Attacking;
    private float curSpeed;

    // Use this for initialization
    private void Start() {
        curSpeed = Speed;
    }

    // Update is called once per frame
    private void Update() {
        if (AllowMove && !Attacking && pathNum < path.Count)
            MoveToNextPath();
        if (Attacking)
            AttackNextPos();
    }

    private void MoveToNextPath() {
        var pos = transform.position;
        if ((path[pathNum] - pos).sqrMagnitude <= MinDistance) {
            pathNum++;
            if (pathNum >= path.Count)
                return;
            if (GameController.instance.TC.GetTileAtWorldPos(path[pathNum]).HasTower) {
                Attacking = true;
                return;
            }
        }
        transform.position = Vector3.MoveTowards(pos, path[pathNum], curSpeed * Time.deltaTime);
    }

    private void AttackNextPos() {
        if (Time.time < attackTimer)
            return;
        Tile tile = GameController.instance.TC.GetTileAtWorldPos(path[pathNum]);
        if (tile.DamageTower(Damage)) {
            Attacking = false;
        }
    }

    public void ChangeSpeed(float percent) {
        curSpeed = percent * Speed;
    }


    public void SetPath(List<Vector3> positions) {
        AllowMove = true;
        path = positions;
        pathNum = 0;
    }

    public void TakeDamage(float amount) {
        Health -= amount;
        if (Health <= 0) {
            //Need to gain resources here.
            Destroy(gameObject);
        }
    }

    public void UpdatePath() {
        var tile = GameController.instance.TC.WorldToTilePosition(transform.position);
        path = GameController.instance.TC.TileToWorldPosition(GameController.instance.TC.PF.CalculatePath(tile));
        pathNum = 0;
        if (GameController.instance.TC.GetTileAtWorldPos(path[pathNum]).HasTower)
            Attacking = true;
    }
}
