using System.Linq;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] protected TowerDefenseDB towerDB;
    [SerializeField] public int myTowerNumber;
    [SerializeField] protected LayerMask enemyLayer = 1 << 6;

    protected TowerEntity myTowerData;

    public TowerEntity TowerData
    {
        get
        {
            // ★ 수정됨: myTowerData가 Null이거나, 유니티가 만든 가짜 껍데기(number가 0)라면 무조건 DB에서 다시 찾습니다!!!
            if (myTowerData == null || myTowerData.number == 0)
            {
                if (towerDB != null && towerDB.Tower != null)
                {
                    myTowerData = towerDB.Tower.FirstOrDefault(t => t.number == myTowerNumber);
                }
            }
            return myTowerData;
        }
    }

    protected float currentCooldown = 0f;

    protected void Awake()
    {
        InitializeData();
    }

    protected void InitializeData()
    {
        if(towerDB != null && towerDB.Tower != null)
        {
            myTowerData = towerDB.Tower.FirstOrDefault(t => t.number == myTowerNumber);
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
