using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class TileController : MonoBehaviour {
    public GameObject TileMarker;
    public GameObject WallMarker;
    public GameObject Enemy;

    public Vector2 TileSize;
    public Vector2 BottomLeftPosition;
    public Vector2 EndLocation;

    public Vector2I NumTiles;
    public Tile[,] Tiles;
    public GameObject[,] Markers;
    public Vector2I[] StartPos;

    public PathFinder PF;

    private Plane mapFloor;

    private GameObject markerParent;

    // Use this for initialization
    private void Start() {
        markerParent = new GameObject();

        Tiles = new Tile[NumTiles.x, NumTiles.y];
        Markers = new GameObject[NumTiles.x, NumTiles.y];
        for (int i = 0; i < NumTiles.x; i++) {
            for (int j = 0; j < NumTiles.y; j++) {
                Tiles[i, j] = new Tile(new Vector2I(i, j), false, 1, EndLocation - BottomLeftPosition);
                //Markers[i, j] = Instantiate(TileMarker, new Vector3(BottomLeftPosition.x + i * TileSize.x, 0, BottomLeftPosition.y + j * TileSize.y), transform.rotation);
            }
        }
        LoadWallFromFile();

        //Temporary until we get level loading
        Tiles[44, 40].IsGoal = true;
        Tiles[44, 39].IsGoal = true;
        Tiles[43, 40].IsGoal = true;
        Tiles[43, 39].IsGoal = true;


        PF = new PathFinder(Tiles);

        foreach (var start in StartPos) {
            var path = PF.CalculatePath(start);
            var enemy = Instantiate(Enemy, TileToWorldPosition(start), transform.rotation).GetComponent<BasicEnemy>();
            enemy.SetPath(TileToWorldPosition(path));
            foreach (var pos in path) {
                Instantiate(TileMarker, TileToWorldPosition(pos), transform.rotation, markerParent.transform);
            }
        }

        mapFloor = new Plane(Vector3.up, Vector3.zero);
    }

    // Update is called once per frame
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (mapFloor.Raycast(ray, out distance)) {
                Vector3 tilePos = ray.GetPoint(distance);
                Debug.Log("Position: " + tilePos);
                tilePos -= new Vector3(BottomLeftPosition.x, 0, BottomLeftPosition.y);
                Debug.Log("Tile Position: " + tilePos);

                Vector2I tile = new Vector2I(Mathf.FloorToInt(tilePos.x + TileSize.x / 2), Mathf.FloorToInt(tilePos.z + TileSize.y / 2));
                Debug.Log("Tile: " + tile);

                //Enable to allow editing of walls.
                //SetToWall(tile);
            }
        }
        //Enable to allow saving of walls
        //if (Input.GetKeyDown(KeyCode.P))
        //    SaveWallsToFile();
    }

    private Vector3 TileToWorldPosition(Vector2I pos) {
        var worldPos = new Vector3(BottomLeftPosition.x + pos.x * TileSize.x, 0, BottomLeftPosition.y + pos.y * TileSize.y);
        return worldPos;
    }

    private List<Vector3> TileToWorldPosition(List<Vector2I> pos) {
        var worldPos = new List<Vector3>();
        foreach (var tile in pos) {
            worldPos.Add(TileToWorldPosition(tile));
        }
        return worldPos;
    }

    private void SetToWall(Vector2I pos) {
        Tiles[pos.x, pos.y].IsWall = !Tiles[pos.x, pos.y].IsWall;
        if (Markers[pos.x, pos.y] == null)
            Markers[pos.x, pos.y] = Instantiate(WallMarker, new Vector3(BottomLeftPosition.x + pos.x * TileSize.x, 0, BottomLeftPosition.y + pos.y * TileSize.y), transform.rotation);
        else
            Destroy(Markers[pos.x, pos.y]);
        foreach (Transform child in markerParent.transform) {
            Destroy(child.gameObject);
        }

        foreach (var start in StartPos) {
            var path = PF.CalculatePath(start);
            foreach (var point in path) {
                Instantiate(TileMarker, new Vector3(BottomLeftPosition.x + point.x * TileSize.x, 0, BottomLeftPosition.y + point.y * TileSize.y), transform.rotation, markerParent.transform);
            }
        }
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
                    //    Markers[i, j] = Instantiate(WallMarker, new Vector3(BottomLeftPosition.x + i * TileSize.x, 0, BottomLeftPosition.y + j * TileSize.y), transform.rotation);
                }
        }
    }
}
