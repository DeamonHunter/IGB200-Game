using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour {
    public float TimeToFadeIn;
    public float MaxOpacity;

    private float curTime;
    private Image img;
    // Use this for initialization
    private void Start() {
        curTime = 0;
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    private void Update() {
        if (curTime < TimeToFadeIn) {
            curTime += Time.unscaledDeltaTime;
            img.color = new Color(img.color.r, img.color.g, img.color.b, Mathf.Lerp(0, MaxOpacity, curTime / TimeToFadeIn));
        }
    }
}
