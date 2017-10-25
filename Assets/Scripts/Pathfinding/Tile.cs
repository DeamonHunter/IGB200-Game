using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tower_Scripts;
using UnityEngine;

public class Tile
{
    public bool IsWall;
    public bool HasTower;
    public int PathFindingPenalty;
    public bool IsGoal;
    public TileNode Node;
    public Vector2I Position;

    private GameObject tower;

    public Tile(Vector2I pos, bool isWall, int pathPenalty, Vector2 endLocation)
    {
        Position = pos;
        IsWall = isWall;
        PathFindingPenalty = pathPenalty;
        Node = new TileNode(Position.x, Position.y, !isWall, endLocation, this);
    }

    public void SetTower(GameObject newTower)
    {
        if (HasTower)
            throw new Exception("Tried to create a tower on a tile with a tower.");
        tower = newTower;
        HasTower = true;
        PathFindingPenalty = tower.GetComponent<Tower>().PathFindingCost;
        GameController.instance.UpdatePathFinding();
    }

    public void DeleteTower()
    {
        UnityEngine.Object.Destroy(tower);
        tower = null;
        HasTower = false;
        PathFindingPenalty = 1;
        GameController.instance.UpdatePathFinding();
    }

    public void ActivateTowerIndicator() {
        var ps = tower.GetComponent<ParticleSystem>();
        if (ps != null) 
            ps.Play();
    }

    public void DeactivateTowerIndicator() {
        var ps = tower.GetComponent<ParticleSystem>();
        if (ps != null)
            ps.Stop();
    }
    public bool DamageTower(float damage)
    {
        if (tower == null)
            return true;
        return tower.GetComponent<Tower>().TakeDamage(damage);
    }
}

