using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    [SerializeField] private string hpBarName;

    public float MoveSpeed { get { return enemyData.speed; } }

    public static event Action<int> OnEnemyDeadEvent;
    public static event Action<int> OnEnemyMoveEndPointEvent;

    protected float currentHP;
    private GameObject currentHPBar;

    protected virtual void OnEnable()
    {
        currentHP = enemyData.hp;
        OnSpawn();
    }

    public void OnSpawn()
    {
        currentHPBar = ObjectPoolManager.instance.GetObject(hpBarName);

        if (currentHPBar != null)
        {
            var follower = currentHPBar.GetComponent<HPBarFollower>();

            if (follower != null)
            {
                follower.SetTarget(this.transform);
            }
        }
    }

    public virtual void TakeDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            OnEnemyDeadEvent?.Invoke(enemyData.dropCoins);
            Die();
        }

    }

    protected virtual void Die()
    {
        Despawn();
    }

    public void MoveEndPoint()
    {
        OnEnemyMoveEndPointEvent?.Invoke(enemyData.damage);

        Despawn();
    }

    public void Despawn()
    {
        ObjectPoolManager.instance.ReturnObject(gameObject.name, gameObject);
    }
}
