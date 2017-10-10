using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tunneller : BasicEnemy
{

    protected override void MoveToNextPath()
    {
        var pos = transform.position;
        if ((path[pathNum] - pos).sqrMagnitude <= MinDistance)
        {
            pathNum++;
            if (pathNum >= path.Count)
                return;
        }
        transform.position = Vector3.MoveTowards(pos, path[pathNum], curSpeed * Time.deltaTime);
    }

    public override void UpdatePath()
    {
        var tile = GameController.instance.TC.WorldToTilePosition(transform.position);
        path = GameController.instance.TC.TileToWorldPosition(GameController.instance.TC.PF.CalculatePath(tile, true));
        pathNum = 0;
    }
}
