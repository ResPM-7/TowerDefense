using System.Linq;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] protected string poolName;
    [SerializeField] protected LayerMask enemyLayer;
    public string PoolName => poolName;

    [SerializeField] protected int myTowerNumber;
    public int TowerNumber => myTowerNumber;

    protected TowerEntity myTowerData;

    public TowerEntity TowerData => myTowerData;

    protected float currentCooldown = 0f;

    protected void Awake()
    {
        if (enemyLayer.value == 0)
        {
            enemyLayer = LayerMask.GetMask("Enemy");
        }

    }

    public virtual void Setup(TowerEntity data)
    {
        myTowerData = data;
        currentCooldown = 0f; // 새로 배치될 때 쿨다운 초기화

        OnSpawn();
    }

    // 자식 클래스(예: ArcherTower)에서 추가 초기화가 필요할 때 오버라이드할 수 있는 가상 함수
    protected virtual void OnSpawn()
    {
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
            Gizmos.DrawWireSphere(transform.position, myTowerData.attackRange);
        }
    }
}
