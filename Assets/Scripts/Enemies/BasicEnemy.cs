using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour {
    public bool AllowMove;
    public float Speed;
    public float MinDistance;

    private List<Vector3> path;
    private int pathNum = 0;

    // Use this for initialization
    private void Start() {

    }

    // Update is called once per frame
    private void Update() {
        if (AllowMove && pathNum < path.Count)
            MoveToNextPath();
    }

    private void MoveToNextPath() {
        var pos = transform.position;
        if ((path[pathNum] - pos).sqrMagnitude <= MinDistance) {
            pathNum++;
            if (pathNum >= path.Count)
                return;
        }
        transform.position = Vector3.MoveTowards(pos, path[pathNum], Speed * Time.deltaTime);
    }

    public void SetPath(List<Vector3> positions) {
        AllowMove = true;
        path = positions;
        pathNum = 0;
    }
}
