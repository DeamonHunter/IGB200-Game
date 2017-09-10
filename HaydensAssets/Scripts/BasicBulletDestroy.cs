using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBulletDestroy : MonoBehaviour {

    /// <summary>
    /// Literally just destroys the bullet prefab after 2.5 seconds so the scene doesn't lag with unnecesary objects.
    /// </summary>
	
	// Update is called once per frame
	void Update () {

        Destroy();

	}

    //Will probably need to be made better so that the bullet gets destroyed once it hits an enemy as well.
    void Destroy() {

        Destroy(this.gameObject, 2.5f);

    }
}
