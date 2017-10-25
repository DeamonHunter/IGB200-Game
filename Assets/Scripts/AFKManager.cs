using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFKManager : MonoBehaviour {
    public CameraMove camera;
    public GameObject Cursor;

    private CameraMove cursor;
    // Use this for initialization
    private void Start() {
        camera.AllowPayerControl = false;
        cursor = Cursor.GetComponent<CameraMove>();
        cursor.AllowPayerControl = false;
        StartCoroutine(AFKScript());
    }


    IEnumerator AFKScript() {
        //camera.SetNewCameraMovePosition(new Vector3(-1, 0, -5), 2);
        yield return new WaitForSeconds(0.5f);
        cursor.SetNewCameraMovePosition(GameController.instance.TC.TileToWorldPosition(new Vector2I(29, 26)), 0.4f);
        yield return new WaitForSeconds(0.4f);
        GameController.instance.TC.PlaceTower(new Vector2I(29, 26));

        cursor.SetNewCameraMovePosition(GameController.instance.TC.TileToWorldPosition(new Vector2I(27, 30)), 0.4f);
        yield return new WaitForSeconds(0.4f);
        GameController.instance.TC.PlaceTower(new Vector2I(27, 30));

        cursor.SetNewCameraMovePosition(GameController.instance.TC.TileToWorldPosition(new Vector2I(32, 35)), 0.4f);
        yield return new WaitForSeconds(0.4f);
        GameController.instance.TC.PlaceTower(new Vector2I(32, 35));

        cursor.SetNewCameraMovePosition(GameController.instance.TC.TileToWorldPosition(new Vector2I(36, 30)), 0.4f);
        yield return new WaitForSeconds(0.4f);
        GameController.instance.TC.PlaceTower(new Vector2I(36, 30));

        cursor.SetNewCameraMovePosition(GameController.instance.TC.TileToWorldPosition(new Vector2I(31, 26)), 0.4f);
        yield return new WaitForSeconds(0.4f);
        GameController.instance.TC.PlaceTower(new Vector2I(31, 26));

        //Start wave 1
        yield return new WaitForSeconds(1f);
        GameController.instance.CreateNewWave();

        GameController.instance.TC.SelectedTower = 2;
    }
    // Update is called once per frame
    private void Update() {

    }
}
