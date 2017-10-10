using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tunneller : BasicEnemy
{
	private bool aboveSpeedThreshold = false;
	private float speedIncrease = 1.0f;
	private float speedMax = 20.0f;
	private float speedTunnel = 10.0f;
	private float rampTime = 1.0f;

	private float timeCount;
	private float lastUpdate;

	protected override void Update() {
		base.Update();
		rampSpeed();

		if (curSpeed >= speedTunnel) {
			aboveSpeedThreshold = true;
			UpdatePath();
		} else {
			aboveSpeedThreshold = false;
			UpdatePath();
		}
	}

    protected override void MoveToNextPath()
    {
		if (!aboveSpeedThreshold) {
			base.MoveToNextPath();
		} else {
			var pos = transform.position;
			if ((path [pathNum] - pos).sqrMagnitude <= MinDistance) {
				pathNum++;
				if (pathNum >= path.Count)
					return;
			} 
			transform.position = Vector3.MoveTowards(pos, path[pathNum], curSpeed * Time.deltaTime);
			transform.LookAt(path[pathNum]);
		}
	}

    public override void UpdatePath()
    {
		if (!aboveSpeedThreshold) {
			base.UpdatePath();
		} else {
			var tile = GameController.instance.TC.WorldToTilePosition (transform.position);
			path = GameController.instance.TC.TileToWorldPosition (GameController.instance.TC.PF.CalculatePath (tile, true));
			pathNum = 0;
		}
    }

	public void rampSpeed() {
		if (!slowed && curSpeed <= speedMax) {
			timeCount = Time.time;
			if (timeCount - lastUpdate >= rampTime) {
				curSpeed += speedIncrease;
				lastUpdate = Time.time;
			} 
		} else {
			curSpeed = baseSpeed;
		}
	}
}