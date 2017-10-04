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
        camera.SetNewCameraMovePosition(new Vector3(-1, 0, -5), 2);
        yield return new WaitForSeconds(0.5f);
        cursor.SetNewCameraMovePosition(GameController.instance.TC.TileToWorldPosition(new Vector2I(40, 38)), 0.5f);
        yield return new WaitForSeconds(0.5f);
        GameController.instance.TC.PlaceTower(new Vector2I(40, 38));
        cursor.SetNewCameraMovePosition(GameController.instance.TC.TileToWorldPosition(new Vector2I(40, 42)), 0.2f);
        yield return new WaitForSeconds(0.2f);
        GameController.instance.TC.PlaceTower(new Vector2I(40, 42));
        cursor.SetNewCameraMovePosition(GameController.instance.TC.TileToWorldPosition(new Vector2I(46, 42)), 0.5f);
        yield return new WaitForSeconds(0.5f);
        GameController.instance.TC.PlaceTower(new Vector2I(46, 42));
        cursor.SetNewCameraMovePosition(GameController.instance.TC.TileToWorldPosition(new Vector2I(46, 38)), 0.2f);
        yield return new WaitForSeconds(0.2f);
        GameController.instance.TC.PlaceTower(new Vector2I(46, 38));
        cursor.SetNewCameraMovePosition(GameController.instance.TC.TileToWorldPosition(new Vector2I(45, 35)), 0.5f);
        yield return new WaitForSeconds(0.5f);
        GameController.instance.TC.PlaceTower(new Vector2I(45, 35));
        yield return new WaitForSeconds(1.5f);
        GameController.instance.CreateNewWave();
        Debug.Log("Hello");
    }
    // Update is called once per frame
    private void Update() {

    }
}
