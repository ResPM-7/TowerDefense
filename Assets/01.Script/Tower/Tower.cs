using System.Linq;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] protected PoolType towerType;
    [SerializeField] protected int myTowerNumber;
    [SerializeField] protected LayerMask enemyLayer;

    public PoolType TowerType
    {
        get { return towerType; }
    }

    protected TowerEntity myTowerData;
    public TowerEntity TowerData
    {
        get
        {
            if (myTowerData.number == 0)
                InitializeData();
            return myTowerData;
        }
    }

    protected float currentCooldown = 0f;

    protected void Awake()
    {
        if (enemyLayer.value == 0)
        {
            enemyLayer = LayerMask.GetMask("Enemy");
        }

        InitializeData();
    }

    protected void InitializeData()
    {
        if (GameData.Towers.TryGetValue(myTowerNumber, out TowerEntity data))
        {
            myTowerData = data;
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
