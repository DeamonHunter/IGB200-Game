using System.Collections;
using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;

public class TileNode : FastPriorityQueueNode {
    private TileNode parent;
    private Tile tile;
    public Vector2I Location;
    public bool IsWalkable;

    /// <summary>
    /// Current Pathfinding cost to node.
    /// </summary>
    public float G;

    /// <summary>
    /// Hueristic cost to the end.
    /// </summary>
    public float H;

    /// <summary>
    /// Current State of the node. <seealso cref="NodeState"/>
    /// </summary>
    public NodeState State;

    /// <summary>
    /// Total Pathfinding Cost. Adds <see cref="G"/> + <see cref="H"/>;
    /// </summary>
    public float F {
        get { return G + H; }
    }

    public TileNode Parent {
        get { return parent; }
        set {
            parent = value;
            if (parent != null)
                G = parent.G + tile.PathFindingPenalty;
        }
    }

    public TileNode(int x, int y, bool isWalkable, Vector2 endLocation, Tile tile) {
        this.Location = new Vector2I(x, y);
        this.State = NodeState.Untested;
        this.IsWalkable = isWalkable;
        this.H = GetGTraversalCost(Location, endLocation);
        this.G = 0;
        this.tile = tile;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="location"></param>
    /// <param name="otherLocation"></param>
    /// <returns></returns>
    internal static float GetGTraversalCost(Vector2I location, Vector2I otherLocation) {
        return GetGTraversalCost(location, new Vector2(otherLocation.x, otherLocation.y));
    }

    internal static float GetGTraversalCost(Vector2I location, Vector2 otherLocation) {
        float deltaX = otherLocation.x - location.x;
        float deltaY = otherLocation.y - location.y;
        return (Mathf.Abs(deltaX) + Mathf.Abs(deltaY)) * 2.5f;
    }

    public void ResetNode() {
        this.State = NodeState.Untested;
        this.Parent = null;
        this.G = 0;
    }

    public enum NodeState {
        Untested,
        Open,
        Closed
    }
}