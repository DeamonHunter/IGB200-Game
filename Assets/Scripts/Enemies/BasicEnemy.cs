using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour {
    public bool AllowMove;
    public float MinDistance;
    public float Health;
    public float Speed;

    private List<Vector3> path;
    private int pathNum = 0;
    private float curSpeed;

    // Use this for initialization
    private void Start() {
        curSpeed = Speed;
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
        transform.position = Vector3.MoveTowards(pos, path[pathNum], curSpeed * Time.deltaTime);
    }

    public void ChangeSpeed(float percent) {
        curSpeed = percent * Speed;
    }


    public void SetPath(List<Vector3> positions) {
        AllowMove = true;
        path = positions;
        pathNum = 0;
    }

    public void TakeDamage(float amount) {
        Health -= amount;
        if (Health <= 0) {
            //Need to gain resources here.
            Destroy(gameObject);
        }
    }
}
