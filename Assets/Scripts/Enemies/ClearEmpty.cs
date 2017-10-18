using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEmpty : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (this.gameObject.transform.childCount <= 0) {
			Destroy (this.gameObject);
		}
	}
}
