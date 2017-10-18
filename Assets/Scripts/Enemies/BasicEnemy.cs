using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public bool AllowMove;
    public float MinDistance;
    public float BaseHealth;
    public float Speed;
    public float Damage;
	public int BaseDamage = 1;
    public float AttackSpeed;
    public bool IgnoresWalls;

    public int carriedGold = 5;

    private float attackTimer;
    protected List<Vector3> path;
    protected int pathNum = 0;
    protected bool Attacking;
	public float curSpeed;
	protected float curHealth;

	protected bool slowed = false;

	protected float baseSpeed;

    protected GameObject gameController;

    // Use this for initialization
    private void Start()
    {
        curSpeed = Speed;
		baseSpeed = curSpeed;
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    // Update is called once per frame
	protected virtual void Update()
    {
        if (AllowMove && !Attacking && pathNum < path.Count)
            MoveToNextPath();
        if (Attacking)
            AttackNextPos();
    }

    protected virtual void MoveToNextPath()
    {
        var pos = transform.position;
        if ((path[pathNum] - pos).sqrMagnitude <= MinDistance)
        {
            pathNum++;
            if (pathNum >= path.Count)
                return;
            if (GameController.instance.TC.GetTileAtWorldPos(path[pathNum]).HasTower)
            {
                Attacking = true;
                return;
            }
        }
        transform.position = Vector3.MoveTowards(pos, path[pathNum], curSpeed * Time.deltaTime);
        transform.LookAt(path[pathNum]);
    }

    private void AttackNextPos()
    {
        if (Time.time < attackTimer)
            return;
        Tile tile = GameController.instance.TC.GetTileAtWorldPos(path[pathNum]);
        if (tile.DamageTower(Damage))
        {
            Attacking = false;
        }
    }

	public void ChangeSpeed(float percent, bool isSlowed)
    {
        curSpeed = percent * Speed;
		slowed = isSlowed;
    }

    public void SetHealth(float healthMultiplier)
    {
        curHealth = BaseHealth * healthMultiplier;
    }

	public void SetPath(List<Vector3> positions, int curPathNum)
    {
        AllowMove = true;
        path = positions;
		pathNum = curPathNum;
    }

    public virtual void TakeDamage(float amount)
    {
        curHealth -= amount;
        if (curHealth <= 0)
        {
            //Gain resources.
            gameController.GetComponent<ResourceScript>().GainMoney(carriedGold);
            Destroy(this.gameObject);
        }
    }

    public virtual void UpdatePath()
    {
        var tile = GameController.instance.TC.WorldToTilePosition(transform.position);
        path = GameController.instance.TC.TileToWorldPosition(GameController.instance.TC.PF.CalculatePath(tile));
        pathNum = 0;
        if (GameController.instance.TC.GetTileAtWorldPos(path[pathNum]).HasTower)
            Attacking = true;
    }

	public int GetBaseDamage() {
		return BaseDamage;
	}
}
