using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    public float MoveSpeed { get { return enemyData.speed; } }

    protected float currentHP;

    protected virtual void OnEnable()
    {
        currentHP = enemyData.hp;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }

    }

    protected virtual void Die()
    {
        Despawn();
    }

    public void Despawn()
    {
        ObjectPoolManager.instance.ReturnObject(gameObject.name, gameObject);
    }
}
