using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideShowUI : MonoBehaviour {
    public KeyCode UIShowKey;
    public GameObject UI;
    public GameObject CloseWhenEnabled;

    // Update is called once per frame
    private void Update() {
        if (Input.GetKeyDown(UIShowKey))
            ToggleUI();
    }

    public void ToggleUI() {
        UI.SetActive(!UI.activeSelf);
        if (UI.activeSelf && CloseWhenEnabled.activeSelf)
            CloseWhenEnabled.SetActive(false);
    }
}
