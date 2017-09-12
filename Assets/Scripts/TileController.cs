﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileController : MonoBehaviour {
    public GameObject CursorPrefab;
    public GameObject TestTower;

    public Vector2 TileSize;
    public Vector2 BottomLeftPosition;
    public Vector2 EndLocation;

    public Vector2I NumTiles;
    public Tile[,] Tiles;

    public PathFinder PF;

    private Plane mapFloor;

    private GameObject markerParent;
    private GameObject cursor;
    private Renderer cursorRenderer;

    //Debug for walls
    //public GameObject TileMarkerPrefab;
    //public GameObject WallMarkerPreFab;
    //public GameObject[,] Markers;

    // Use this for initialization
    public void Setup() {
        markerParent = new GameObject();

        Tiles = new Tile[NumTiles.x, NumTiles.y];
        //Markers = new GameObject[NumTiles.x, NumTiles.y];
        for (int i = 0; i < NumTiles.x; i++) {
            for (int j = 0; j < NumTiles.y; j++) {
                Tiles[i, j] = new Tile(new Vector2I(i, j), false, 1, EndLocation - BottomLeftPosition);
                //Markers[i, j] = Instantiate(TileMarkerPrefab, new Vector3(BottomLeftPosition.x + i * TileSize.x, 0, BottomLeftPosition.y + j * TileSize.y), transform.rotation);
            }
        }
        LoadWallFromFile();

        //Temporary until we get level loading
        Tiles[44, 40].IsGoal = true;
        Tiles[44, 39].IsGoal = true;
        Tiles[43, 40].IsGoal = true;
        Tiles[43, 39].IsGoal = true;

        PF = new PathFinder(Tiles);

        mapFloor = new Plane(Vector3.up, Vector3.zero);

        cursor = Instantiate(CursorPrefab);
        cursorRenderer = cursor.GetComponent<Renderer>();
    }

    // Update is called once per frame
    private void Update() {
        var hoveredPos = GetHoveredTilePosition();
        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0)) {
            //Enable to allow editing of walls.
            //SetToWall(tile);
            PlaceTower(hoveredPos);
        }
        cursor.transform.position = TileToWorldPosition(hoveredPos) + new Vector3(0, 0.01f, 0);
        if (hoveredPos.x < 0 || hoveredPos.x >= NumTiles.x || hoveredPos.y < 0 || hoveredPos.y >= NumTiles.y || Tiles[hoveredPos.x, hoveredPos.y].IsWall || Tiles[hoveredPos.x, hoveredPos.y].HasTower) {
            cursorRenderer.material.color = Color.red;
        }
        else
            cursorRenderer.material.color = Color.white;
        //Enable to allow saving of walls
        //if (Input.GetKeyDown(KeyCode.P))
        //    SaveWallsToFile();
    }

    private Vector2I GetHoveredTilePosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (mapFloor.Raycast(ray, out distance)) {
            Vector3 tilePos = ray.GetPoint(distance);
            tilePos -= new Vector3(BottomLeftPosition.x, 0, BottomLeftPosition.y);

            Vector2I tile = new Vector2I(Mathf.FloorToInt(tilePos.x + TileSize.x / 2), Mathf.FloorToInt(tilePos.z + TileSize.y / 2));
            return tile;
        }
        return new Vector2I(-1, -1);
    }

    public Vector3 TileToWorldPosition(Vector2I pos) {
        var worldPos = new Vector3(BottomLeftPosition.x + pos.x * TileSize.x, 0, BottomLeftPosition.y + pos.y * TileSize.y);
        return worldPos;
    }

    public List<Vector3> TileToWorldPosition(List<Vector2I> pos) {
        var worldPos = new List<Vector3>();
        foreach (var tile in pos) {
            worldPos.Add(TileToWorldPosition(tile));
        }
        return worldPos;
    }

    //private void SetToWall(Vector2I pos) {
    //    Tiles[pos.x, pos.y].IsWall = !Tiles[pos.x, pos.y].IsWall;
    //    if (Markers[pos.x, pos.y] == null)
    //        Markers[pos.x, pos.y] = Instantiate(WallMarkerPreFab, new Vector3(BottomLeftPosition.x + pos.x * TileSize.x, 0, BottomLeftPosition.y + pos.y * TileSize.y), transform.rotation);
    //    else
    //        Destroy(Markers[pos.x, pos.y]);
    //    foreach (Transform child in markerParent.transform) {
    //        Destroy(child.gameObject);
    //    }
    //}

    private void PlaceTower(Vector2I pos) {
        if (Tiles[pos.x, pos.y].HasTower)
            return;
        //Need to check resources/money
        Instantiate(TestTower, TileToWorldPosition(pos), Quaternion.identity);
        //Need to update pathfinding
    }

    private void SaveWallsToFile() {
        string Walls = "";
        for (int i = 0; i < NumTiles.x; i++)
            for (int j = 0; j < NumTiles.x; j++)
                Walls += Tiles[i, j].IsWall ? "1" : "0";
        using (var sw = new System.IO.StreamWriter(Application.dataPath + @"\SaveData\Walls.txt", false)) {
            sw.Write(Walls);
        }
    }

    private void LoadWallFromFile() {
        //Need to double check this works when building
        using (var sr = new System.IO.StreamReader(Application.dataPath + @"\SaveData\Walls.txt", false)) {
            for (int i = 0; i < NumTiles.x; i++)
                for (int j = 0; j < NumTiles.x; j++) {
                    Tiles[i, j].IsWall = sr.Read() == char.Parse("1");
                    //Enable to see walls when loading
                    //if (Tiles[i, j].IsWall)
                    //    Markers[i, j] = Instantiate(WallMarkerPreFab, new Vector3(BottomLeftPosition.x + i * TileSize.x, 0, BottomLeftPosition.y + j * TileSize.y), transform.rotation);
                }
        }
    }
}
