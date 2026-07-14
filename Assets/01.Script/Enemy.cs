using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float maxHp = 10f;
    [SerializeField] protected float moveSpeed = 2f;
    public float MoveSpeed { get { return moveSpeed; } }

    protected float currentHP;

    protected virtual void OnEnable()
    {
        currentHP = maxHp;
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
