using System.Collections;
using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;

public class TileNode : FastPriorityQueueNode {
    private TileNode parent;
    public Vector2I Location;
    public bool IsWalkable;
    public float G;
    public float H;
    public NodeState State;

    public float F {
        get { return G + H; }
    }

    public TileNode Parent {
        get { return parent; }
        set {
            parent = value;
            G = parent.G + GetGTraversalCost(Location, parent.Location);
        }
    }

    public TileNode(int x, int y, bool isWalkable, Vector2 endLocation) {
        this.Location = new Vector2I(x, y);
        this.State = NodeState.Untested;
        this.IsWalkable = isWalkable;
        this.H = GetGTraversalCost(Location, endLocation);
        this.G = 0;
    }

    internal static float GetGTraversalCost(Vector2I location, Vector2I otherLocation) {
        float deltaX = otherLocation.x - location.x;
        float deltaY = otherLocation.y - location.y;
        return (Mathf.Abs(deltaX) + Mathf.Abs(deltaY)) * 2.5f;
    }

    internal static float GetGTraversalCost(Vector2I location, Vector2 otherLocation) {
        float deltaX = otherLocation.x - location.x;
        float deltaY = otherLocation.y - location.y;
        return (Mathf.Abs(deltaX) + Mathf.Abs(deltaY)) * 2.5f;
    }

    public void ResetNode() {
        this.State = NodeState.Untested;
        this.G = 0;
    }

    public enum NodeState {
        Untested,
        Open,
        Closed
    }
}