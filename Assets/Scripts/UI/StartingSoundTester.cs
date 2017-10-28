using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingSoundTester : MonoBehaviour {

    public AudioSource typing;
    private float soundTimer = 0.0f;
    public float runTime;

	void Update () {
        TestForSound();
	}

    void TestForSound() {
        if (soundTimer >= runTime || typing == null) {
            typing.Stop();
        }
        soundTimer += Time.deltaTime;
    }
}
