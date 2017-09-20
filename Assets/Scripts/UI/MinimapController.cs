using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour {
    public Color WallColour;
    public Color TowerColour;
    public Color EnemyColour;
    public Color GroundColour;

    //Allow turning minimap on and off
    private bool display = false;
    private Texture2D tex;
    private RawImage img;

    public void Setup(Vector2I mapSize) {
        display = true;
        tex = new Texture2D(mapSize.x, mapSize.y);
        img = GetComponent<RawImage>();
        img.texture = tex;
    }

    // Update is called once per frame
    private void Update() {
        if (!display)
            return;
        for (int i = 0; i < tex.width; i++) {
            for (int j = 0; j < tex.width; j++) {
                Color col;
                var tile = GameController.instance.TC.Tiles[i, j];
                if (tile.IsWall)
                    col = WallColour;
                else if (tile.HasTower)
                    col = TowerColour;
                else
                    col = GroundColour;
                tex.SetPixel(i, j, col);
            }
        }
        for (int i = 0; i < GameController.instance.EnemyParent.transform.childCount; i++) {
            var enemy = GameController.instance.EnemyParent.transform.GetChild(i).GetComponent<BasicEnemy>();
            var pos = GameController.instance.TC.WorldToTilePosition(enemy.transform.position);
            tex.SetPixel(pos.x, pos.y, EnemyColour);
            tex.SetPixel(pos.x - 1, pos.y, EnemyColour);
            tex.SetPixel(pos.x + 1, pos.y, EnemyColour);
            tex.SetPixel(pos.x, pos.y - 1, EnemyColour);
            tex.SetPixel(pos.x, pos.y + 1, EnemyColour);
        }

        tex.Apply();
    }
}
