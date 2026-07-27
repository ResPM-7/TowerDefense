using System.Linq;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] protected TowerDB towerDB;
    [SerializeField] protected int myTowerNumber;
    [SerializeField] protected LayerMask enemyLayer = 1 << 6;

    protected TowerEntity myTowerData;

    public TowerEntity TowerData { get { return myTowerData; } }

    protected float currentCooldown = 0f;

    protected void Awake()
    {
        InitializeData();
    }

    protected void InitializeData()
    {
        if(towerDB != null && towerDB.Tower != null)
        {
            myTowerData = towerDB.Tower.FirstOrDefault(t => t.Number == myTowerNumber);
        }
    }

    protected void Update()
    {
        if (myTowerData == null) return;

        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
        else
        {
            if (CanAttack())
            {
                Attack();
                currentCooldown = myTowerData.attackCooldown;
            }
        }
    }

    protected abstract void Attack();
    protected abstract bool CanAttack();

    protected void OnDrawGizmosSelected()
    {
        if (myTowerData != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, myTowerData.attackRange);
        }
    }
}
