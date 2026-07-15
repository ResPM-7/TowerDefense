using UnityEngine;

public class ArcherTower : Tower
{
    [SerializeField] private string arrowName;
    [SerializeField] private Transform firePoint;

    private Transform target;

    protected override bool CanAttack()
    {
        target = null;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, towerData.attackRange, enemyLayer);

        if (enemies.Length == 0) return false;

        float shortestDistance = Mathf.Infinity;

        foreach (Collider2D enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= towerData.attackRange && distance < shortestDistance)
            {
                shortestDistance = distance;
                target = enemy.transform;
            }
        }
        return target != null;
    }

    protected override void Attack()
    {
        if (target == null) return;
        GameObject arrowObj = ObjectPoolManager.instance.GetObject(arrowName);

        if (arrowObj != null)
        {
            arrowObj.transform.position = firePoint.position;
            arrowObj.transform.rotation = Quaternion.identity;

            TowerBullet arrowScript = arrowObj.GetComponent<TowerBullet>();
            if (arrowScript != null)
            {
                arrowScript.SetTarget(target, towerData.damage);
            }
        }
    }
}