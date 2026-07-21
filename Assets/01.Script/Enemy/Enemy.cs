using System;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    [SerializeField] private string hpBarName;

    public EnemyData EnemyData { get { return enemyData; } }
    //public float MoveSpeed { get { return enemyData.speed; } }

    public static event Action<int> OnEnemyDeadEvent;
    public static event Action<int> OnEnemyMoveEndPointEvent;

    protected float currentHP;
    private GameObject currentHPBar;

    private Slider hpSlider;

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

                hpSlider = follower.GetSlider();
                if(hpSlider != null)
                {
                    hpSlider.value = currentHP / enemyData.hp;
                }
            }
        }
    }

    public virtual void TakeDamage(float damage)
    {
        currentHP -= damage;

        if(hpSlider != null)
        {
            hpSlider.value = currentHP / enemyData.hp;
        }

        if (currentHP <= 0)
        {
            OnEnemyDeadEvent?.Invoke(enemyData.dropCoins);
            ScoreManager.instance.AddScore(enemyData.score);
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
        if (currentHPBar != null)
        {
            ObjectPoolManager.instance.ReturnObject(currentHPBar.name, currentHPBar.gameObject);
            currentHPBar = null;
            hpSlider = null;
        }

        ObjectPoolManager.instance.ReturnObject(gameObject.name, gameObject);
    }
}
