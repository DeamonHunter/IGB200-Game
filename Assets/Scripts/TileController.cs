﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {
    public GameObject TileMarker;
    public Vector2 TileSize;
    public Vector2 BottomLeftPosition;
    public Vector2 EndLocation;

    public Vector2I NumTiles;
    public Tile[,] Tiles;
    public GameObject[,] Markers;
    public Vector2I[] StartPos;

    public PathFinder PF;

    // Use this for initialization
    private void Start() {
        Tiles = new Tile[NumTiles.x, NumTiles.y];
        Markers = new GameObject[NumTiles.x, NumTiles.y];
        for (int i = 0; i < NumTiles.x; i++) {
            for (int j = 0; j < NumTiles.y; j++) {
                Tiles[i, j] = new Tile(new Vector2I(i, j), false, 1, EndLocation - BottomLeftPosition);
                //Markers[i, j] = Instantiate(TileMarker, new Vector3(BottomLeftPosition.x + i * TileSize.x, 0, BottomLeftPosition.y + j * TileSize.y), transform.rotation);
            }
        }
        //Temporary until we get level loading
        Tiles[44, 40].IsGoal = true;
        Tiles[44, 39].IsGoal = true;
        Tiles[43, 40].IsGoal = true;
        Tiles[43, 39].IsGoal = true;


        PF = new PathFinder(Tiles);

        foreach (var start in StartPos) {
            var path = PF.CalculatePath(start);
            foreach (var pos in path) {
                Instantiate(TileMarker, new Vector3(BottomLeftPosition.x + pos.x * TileSize.x, 0, BottomLeftPosition.y + pos.y * TileSize.y), transform.rotation);
            }
        }
    }

    // Update is called once per frame
    private void Update() {

    }
}
