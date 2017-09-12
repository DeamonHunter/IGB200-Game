using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideShowEnemyUI : MonoBehaviour {
    public KeyCode UIShowKey;
    public GameObject EnemyUI;
    public GameObject WeaponUI;

    // Update is called once per frame
    private void Update() {
        if (Input.GetKeyDown(UIShowKey))
            ToggleUI();
    }

    public void ToggleUI() {
        EnemyUI.SetActive(!EnemyUI.activeSelf);
        WeaponUI.SetActive(!EnemyUI.activeSelf);
    }
}
