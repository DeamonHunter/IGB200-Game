﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Tower_Scripts;

public class TileController : MonoBehaviour {
    //public Inspector variables
    public GameObject CursorPrefab;
    public GameObject[] Towers;
    public Vector2 TileSize;
    public Vector2 BottomLeftPosition;
    public Vector2 EndLocation;
    public Vector2I NumTiles;


    //public non inspector variables
    [HideInInspector]
    public int SelectedTower;

    [HideInInspector]
    public Tile[,] Tiles;

    [HideInInspector]
    public PathFinder PF;

    [HideInInspector]
    public bool AllowPlayerMovement = true;

    public float[] CursorSizes;

    //Private Variables
    private Plane mapFloor;
    public GameObject Cursor;
    private Renderer cursorRenderer;
    private ParticleSystem.ShapeModule cursorShape;

    private GameObject gameController;
    private int towerCost = 0;
    private int money;

    private Vector2I lastPosition = new Vector2I(0, 0);

    public AudioSource placement;
    public AudioSource noPlacement;

    //Debug for walls
    //public GameObject WallMarkerPreFab;
    //public GameObject[,] Markers;
    //private GameObject markerParent;

    // Use this for initialization
    public void Setup() {
        //markerParent = new GameObject();
        //Markers = new GameObject[NumTiles.x, NumTiles.y];

        gameController = GameObject.FindGameObjectWithTag("GameController");

        Tiles = new Tile[NumTiles.x, NumTiles.y];
        for (int i = 0; i < NumTiles.x; i++) {
            for (int j = 0; j < NumTiles.y; j++) {
                Tiles[i, j] = new Tile(new Vector2I(i, j), false, 1, EndLocation - BottomLeftPosition);
            }
        }
        LoadWallFromFile();

        //Temporary until we get level loading
        Tiles[29, 29].IsGoal = true;
        Tiles[29, 30].IsGoal = true;
        Tiles[30, 29].IsGoal = true;
        Tiles[30, 30].IsGoal = true;

        PF = new PathFinder(Tiles);

        mapFloor = new Plane(Vector3.up, Vector3.zero);

        Cursor = Instantiate(CursorPrefab);
        cursorRenderer = Cursor.transform.GetChild(0).GetComponent<Renderer>();
        cursorShape = Cursor.GetComponent<ParticleSystem>().shape;
    }

    // Update is called once per frame
    private void Update() {
        var hoveredPos = GetHoveredTilePosition();
        if (!EventSystem.current.IsPointerOverGameObject() && AllowPlayerMovement && Input.GetMouseButtonDown(0)) {
            //Enable to allow editing of walls.
            //SetToWall(hoveredPos);
            if (hoveredPos.x > 1 && hoveredPos.x <= NumTiles.x - 1 && hoveredPos.y > 1 && hoveredPos.y <= NumTiles.y - 1)
                PlaceTower(hoveredPos);
        }
        Cursor.transform.position = TileToWorldPosition(hoveredPos) + new Vector3(0, 0.2f, 0);
        cursorShape.radius = CursorSizes[SelectedTower];
        if (hoveredPos.x < 1 || hoveredPos.x >= NumTiles.x - 1 || hoveredPos.y < 1 || hoveredPos.y >= NumTiles.y - 1) {
            cursorRenderer.material.color = Color.red;
        }
        else {
            if (Tiles[hoveredPos.x, hoveredPos.y].IsWall || Tiles[hoveredPos.x, hoveredPos.y].HasTower) {
                cursorRenderer.material.color = Color.red;
            }
            else {
                cursorRenderer.material.color = Color.green;
            }
            if (hoveredPos != lastPosition) {
                if (Tiles[lastPosition.x, lastPosition.y].HasTower) {
                    Tiles[lastPosition.x, lastPosition.y].DeactivateTowerIndicator();
                }
                if (Tiles[hoveredPos.x, hoveredPos.y].HasTower) {
                    Tiles[hoveredPos.x, hoveredPos.y].ActivateTowerIndicator();
                }
                lastPosition = hoveredPos;
            }
        }
            
        //Enable to allow saving of walls
        //if (Input.GetKeyDown(KeyCode.P))
        //    SaveWallsToFile();
    }

    public Vector2I GetHoveredTilePosition() {
        if (AllowPlayerMovement) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (mapFloor.Raycast(ray, out distance)) {
                Vector3 tilePos = ray.GetPoint(distance);
                return WorldToTilePosition(tilePos);
            }
            return new Vector2I(-1, -1);
        }
        return WorldToTilePosition(GameController.instance.AFK.Cursor.transform.position);
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

    public Vector2I WorldToTilePosition(Vector3 pos) {
        pos -= new Vector3(BottomLeftPosition.x, 0, BottomLeftPosition.y);
        return new Vector2I(Mathf.FloorToInt(pos.x + TileSize.x / 2), Mathf.FloorToInt(pos.z + TileSize.y / 2));
    }

    public Tile GetTileAtWorldPos(Vector3 pos) {
        var tilePos = WorldToTilePosition(pos);
        return Tiles[tilePos.x, tilePos.y];
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

    public void PlaceTower(Vector2I pos) {
        //Early return if tile already has tower
        var tile = Tiles[pos.x, pos.y];
        if (tile.HasTower || tile.IsWall) {
            PlaySoundOnFailedPurchase();
            return;
        }

        towerCost = Towers[SelectedTower].GetComponent<Tower>().Cost;
        money = gameController.GetComponent<ResourceScript>().GetTotalMoney();
        if (money >= towerCost) {
            tile.SetTower(Instantiate(Towers[SelectedTower], TileToWorldPosition(pos), Quaternion.identity));
            if (!AllowPlayerMovement)
                tile.DeactivateTowerIndicator();
            gameController.GetComponent<ResourceScript>().PurchaseItem(towerCost);
            PlaySoundOnPurchase();
        }
        else {
            PlaySoundOnFailedPurchase();
            //Debug.Log("Not enough money");
            //Debug.Log(Towers[SelectedTower].GetComponent<Tower>().Cost);
            //Debug.Log(money);
        }
    }

    private void SaveWallsToFile() {
        Debug.Log("Save Started");
        string Walls = "";
        for (int i = 0; i < NumTiles.x; i++)
            for (int j = 0; j < NumTiles.x; j++)
                Walls += Tiles[i, j].IsWall ? "1" : "0";
        using (var sw = new System.IO.StreamWriter(Application.dataPath + @"\Resources\SaveData\Walls.txt", false)) {
            sw.Write(Walls);
        }
        Debug.Log("Save Done.");
    }

    private void LoadWallFromFile() {
        //Need to double check this works when building
        var file = Resources.Load("SaveData/Walls") as TextAsset;
        string text = file.text;
        for (int i = 0; i < NumTiles.x; i++)
            for (int j = 0; j < NumTiles.x; j++) {
                Tiles[i, j].IsWall = text[i * NumTiles.x + j] == char.Parse("1");
                //Enable to see walls when loading
                //if (Tiles[i, j].IsWall)
                //    Markers[i, j] = Instantiate(WallMarkerPreFab, new Vector3(BottomLeftPosition.x + i * TileSize.x, 0, BottomLeftPosition.y + j * TileSize.y), transform.rotation);
            }
    }

    void PlaySoundOnPurchase() {
        if (placement != null) {
            placement.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            placement.Play();
        }
    }

    void PlaySoundOnFailedPurchase() {
        if (noPlacement != null) {
            noPlacement.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            noPlacement.Play();
        }
    }

}
