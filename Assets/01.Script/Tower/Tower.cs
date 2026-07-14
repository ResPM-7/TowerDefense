using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] protected TowerData towerData;
    [SerializeField] protected LayerMask enemyLayer = 1<<6;

    protected float currentCooldown = 0f;

    protected void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
        else
        {
            if (CanAttack())
            {
                Attack();
                currentCooldown = towerData.attackCooldown;
            }
        }
    }

    protected abstract void Attack();
    protected abstract bool CanAttack();

    protected void OnDrawGizmosSelected()
    {
        if (towerData != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, towerData.attackRange);
        }
    }
}
