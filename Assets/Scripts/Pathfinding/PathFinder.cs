using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using Priority_Queue;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.VR;

public class PathFinder {
    private Tile[,] tiles;
    // Use this for initialization

    public PathFinder(Tile[,] tiles) {
        this.tiles = tiles;
    }

    public List<Vector2I> CalculatePath(Vector2I position) {
        ResetNodes();

        TileNode finalNode = Search(tiles[position.x, position.y].Node);
        if (finalNode == null || finalNode.Parent == null)
            throw new Exception("Path was not found.");
        List<Vector2I> path = new List<Vector2I>();
        while (finalNode.Parent != null) {
            path.Add(finalNode.Location);
            finalNode = finalNode.Parent;
        }

        path.Reverse();
        return path;
    }

    private TileNode Search(TileNode startNode) {
        startNode.State = TileNode.NodeState.Closed;
        SimplePriorityQueue<TileNode> nextNodes = new SimplePriorityQueue<TileNode>();
        nextNodes.Enqueue(startNode, startNode.F);

        while (nextNodes.Count != 0) {
            var nextNode = nextNodes.Dequeue();
            if (tiles[nextNode.Location.x, nextNode.Location.y].IsGoal)
                return nextNode;
            var nodes = GetAdjacentWalkableNodes(nextNode);
            foreach (var node in nodes) {
                nextNodes.Enqueue(node, node.F);
            }
        }

        return null;
    }

    private List<TileNode> GetAdjacentWalkableNodes(TileNode fromNode) {
        List<TileNode> walkableNodes = new List<TileNode>();

        //Grab all locations that we could move to.
        IEnumerable<Vector2I> nextLocations = GetAdjacentLocations(fromNode.Location);

        foreach (var location in nextLocations) {
            //Ensure we can move to this tile physically. 
            if (!CanMoveToTile(location))
                continue;

            //Does the node allow us to continue?
            TileNode node = tiles[location.x, location.y].Node;
            if (!node.IsWalkable || node.State == TileNode.NodeState.Closed)
                continue;

            //Has it already been traversed at least once
            if (node.State == TileNode.NodeState.Open) {
                float traversalCost = TileNode.GetGTraversalCost(node.Location, node.Parent.Location);
                float gTemp = fromNode.G + traversalCost;
                if (gTemp < node.G) {
                    node.Parent = fromNode;
                    walkableNodes.Add(node);
                }
            }
            else {
                //Tile has not been visited
                node.Parent = fromNode;
                node.State = TileNode.NodeState.Open;
                walkableNodes.Add(node);
            }
        }
        return walkableNodes;
    }

    private bool CanMoveToTile(Vector2I pos) {
        if (pos.x >= 0 && pos.x < tiles.GetLength(0))
            if (pos.y >= 0 && pos.y < tiles.GetLength(1))
                return !tiles[pos.x, pos.y].IsWall;
        return false;
    }

    private static IEnumerable<Vector2I> GetAdjacentLocations(Vector2I fromLocation) {
        return new Vector2I[] {
            //new Vector2I(fromLocation.x - 1, fromLocation.y - 1),
            new Vector2I(fromLocation.x, fromLocation.y - 1),
           // new Vector2I(fromLocation.x + 1, fromLocation.y - 1),
            new Vector2I(fromLocation.x - 1, fromLocation.y),
            new Vector2I(fromLocation.x + 1, fromLocation.y),
            //new Vector2I(fromLocation.x - 1, fromLocation.y + 1),
            new Vector2I(fromLocation.x, fromLocation.y + 1),
            //new Vector2I(fromLocation.x + 1, fromLocation.y + 1)
        };
    }

    private void ResetNodes() {
        foreach (var tile in tiles) {
            tile.Node.ResetNode();
        }
    }
}