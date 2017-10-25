using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Vector2I {
    public int x;
    public int y;

    public Vector2I(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public static Vector2 Difference(Vector3 left, Vector2I right) {
        var vec2 = new Vector3(left.x - right.x, left.z - right.y);
        return vec2;
    }

    public static bool operator ==(Vector2I left, Vector2I right) {
        return left.x == right.x && left.y == right.y;
    }

    public static bool operator !=(Vector2I left, Vector2I right) {
        return left.x != right.x || left.y != right.y;
    }

    public override string ToString() {
        return "{" + x + ", " + y + "}";
    }
}
