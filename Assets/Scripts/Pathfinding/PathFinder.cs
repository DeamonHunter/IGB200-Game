using System;
using System.Collections.Generic;
using System.Diagnostics;
using Priority_Queue;

public class PathFinder
{
    private Tile[,] tiles;
    // Use this for initialization

    public PathFinder(Tile[,] tiles)
    {
        this.tiles = tiles;
    }

    public List<Vector2I> CalculatePath(Vector2I position, bool ignoreNaturalWalls = false, bool ignoreUnnaturalWalls = false)
    {
        //Set all nodes to unvisited
        ResetNodes();

        TileNode finalNode = Search(tiles[position.x, position.y].Node, ignoreNaturalWalls, ignoreUnnaturalWalls);
        Debug.Assert(tiles[position.x, position.y].Node.Parent == null);
        if (finalNode == null || finalNode.Parent == null)
            throw new Exception("Path was not found.");

        //Get the path from node to start and reverse it
        List<Vector2I> path = new List<Vector2I>();
        while (finalNode.Parent != null)
        {
            path.Add(finalNode.Location);
            finalNode = finalNode.Parent;
        }
        path.Reverse();

        return path;
    }

    /// <summary>
    /// Handles the Searching of Tiles. Using something that should be similar to A*
    /// </summary>
    /// <param name="startNode">The starting node from which search starts.</param>
    /// <returns>The goal node. This node will have a parent unless no path is found.</returns>
    private TileNode Search(TileNode startNode, bool ignoreNaturalWalls, bool ignoreUnnaturalWalls)
    {
        startNode.State = TileNode.NodeState.Closed;
        SimplePriorityQueue<TileNode> nextNodes = new SimplePriorityQueue<TileNode>();
        nextNodes.Enqueue(startNode, startNode.F);

        while (nextNodes.Count != 0)
        {
            var nextNode = nextNodes.Dequeue();
            if (tiles[nextNode.Location.x, nextNode.Location.y].IsGoal)
                return nextNode;
            var nodes = GetAdjacentWalkableNodes(nextNode, ignoreNaturalWalls, ignoreUnnaturalWalls);
            foreach (var node in nodes)
            {
                nextNodes.Enqueue(node, node.F);
            }
        }

        return null;
    }

    private List<TileNode> GetAdjacentWalkableNodes(TileNode fromNode, bool ignoreNaturalWalls, bool ignoreUnnaturalWalls)
    {
        List<TileNode> walkableNodes = new List<TileNode>();

        //Grab all locations that we could move to.
        IEnumerable<Vector2I> nextLocations = GetAdjacentLocations(fromNode.Location);

        foreach (var location in nextLocations)
        {
            //Ensure we can move to this tile physically. 
            if (!CanMoveToTile(location, ignoreNaturalWalls))
                continue;

            //Does the node allow us to continue?
            TileNode node = tiles[location.x, location.y].Node;
            if (!node.IsWalkable || node.State == TileNode.NodeState.Closed)
                continue;

            //Has it already been traversed at least once
            if (node.State == TileNode.NodeState.Open)
            {
                float traversalCost = ignoreUnnaturalWalls ? 1 : tiles[location.x, location.y].PathFindingPenalty;
                float gTemp = fromNode.G + traversalCost;
                if (gTemp < node.G)
                {
                    node.Parent = fromNode;
                    walkableNodes.Add(node);
                }
            }
            else
            {
                //Tile has not been visited
                node.Parent = fromNode;
                node.State = TileNode.NodeState.Open;
                walkableNodes.Add(node);
            }
        }
        return walkableNodes;
    }

    private bool CanMoveToTile(Vector2I pos, bool ignoreNaturalWalls)
    {
        if (pos.x <= 0 && pos.x > tiles.GetLength(0))
            return false;
        if (pos.y <= 0 && pos.y > tiles.GetLength(1))
            return false;

        return ignoreNaturalWalls || !tiles[pos.x, pos.y].IsWall;
    }

    private static IEnumerable<Vector2I> GetAdjacentLocations(Vector2I fromLocation)
    {
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

    private void ResetNodes()
    {
        foreach (var tile in tiles)
        {
            tile.Node.ResetNode();
        }
    }
}