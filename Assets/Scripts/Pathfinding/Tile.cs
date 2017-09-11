using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
    public bool IsWall;
    public bool HasTower;
    public int PathFindingPenalty;
    public bool IsGoal;
    public TileNode Node;
    public Vector2I Position;

    public Tile(Vector2I pos, bool isWall, int pathPenalty, Vector2 endLocation) {
        Position = pos;
        IsWall = isWall;
        PathFindingPenalty = pathPenalty;
        Node = new TileNode(Position.x, Position.y, !isWall, endLocation);
    }
}

