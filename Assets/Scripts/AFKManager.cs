using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        camera.SetNewCameraMovePosition(new Vector3(-10, 0, -15), 5);
        yield return new WaitForSeconds(10f);
        camera.SetNewCameraMovePosition(new Vector3(0, 0, 0), 10);
        yield return new WaitForSeconds(6f);

        cursor.SetNewCameraMovePosition(GameController.instance.TC.TileToWorldPosition(new Vector2I(25, 31)), 0.4f);
        yield return new WaitForSeconds(0.4f);
        GameController.instance.TC.PlaceTower(new Vector2I(25, 31));

        //Start wave 2
        yield return new WaitForSeconds(1f);
        GameController.instance.CreateNewWave();
        camera.SetNewCameraMovePosition(new Vector3(-10, 0, 15), 5);
        yield return new WaitForSeconds(5f);
        camera.SetNewCameraMovePosition(new Vector3(0, 0, 0), 10);
        yield return new WaitForSeconds(15f);

        GameController.instance.TC.SelectedTower = 1;

        cursor.SetNewCameraMovePosition(GameController.instance.TC.TileToWorldPosition(new Vector2I(32, 24)), 0.4f);
        yield return new WaitForSeconds(0.4f);
        GameController.instance.TC.PlaceTower(new Vector2I(32, 24));


        //Start wave 3
        yield return new WaitForSeconds(1f);
        GameController.instance.CreateNewWave();
        camera.SetNewCameraMovePosition(new Vector3(-4, 0, 0), 5);
        yield return new WaitForSeconds(10f);
        camera.SetNewCameraMovePosition(new Vector3(0, 0, 0), 5);
        yield return new WaitForSeconds(10f);

        cursor.SetNewCameraMovePosition(GameController.instance.TC.TileToWorldPosition(new Vector2I(37, 31)), 0.4f);
        yield return new WaitForSeconds(0.4f);
        GameController.instance.TC.PlaceTower(new Vector2I(37, 31));

        GameController.instance.TC.SelectedTower = 2;

        cursor.SetNewCameraMovePosition(GameController.instance.TC.TileToWorldPosition(new Vector2I(36, 33)), 0.4f);
        yield return new WaitForSeconds(0.4f);
        GameController.instance.TC.PlaceTower(new Vector2I(36, 33));

        //Start wave 4
        yield return new WaitForSeconds(1f);
        GameController.instance.CreateNewWave();
        camera.SetNewCameraMovePosition(new Vector3(15, 0, 0),10);
        yield return new WaitForSeconds(16f);
        camera.SetNewCameraMovePosition(new Vector3(0, 0, 0), 5);
        yield return new WaitForSeconds(10f);

        GameController.instance.TC.SelectedTower = 0;

        cursor.SetNewCameraMovePosition(GameController.instance.TC.TileToWorldPosition(new Vector2I(31, 36)), 0.4f);
        yield return new WaitForSeconds(0.4f);
        GameController.instance.TC.PlaceTower(new Vector2I(31, 36));

        GameController.instance.TC.SelectedTower = 4;

        cursor.SetNewCameraMovePosition(GameController.instance.TC.TileToWorldPosition(new Vector2I(32, 40)), 0.4f);
        yield return new WaitForSeconds(0.4f);
        GameController.instance.TC.PlaceTower(new Vector2I(32, 40));

        //Start wave 5
        yield return new WaitForSeconds(1f);
        GameController.instance.CreateNewWave();
        camera.SetNewCameraMovePosition(new Vector3(-5, 0, 0), 10);
        yield return new WaitForSeconds(10f);
        camera.SetNewCameraMovePosition(new Vector3(0, 0, 0), 5);
        yield return new WaitForSeconds(5f);
        Debug.Log("Done Wave 5.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    // Update is called once per frame
    private void Update() {

    }
}
