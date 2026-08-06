using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected string poolName;

    protected EnemyEntity myEnemyData;
    public EnemyEntity EnemyData => myEnemyData;

    private string enemyHPBar = "EnemyHPBar";

    protected float currentHP;
    private GameObject currentHPBar;

    private Slider hpSlider;

    public static event Action<int> OnEnemyDeadDropCoinEvent;
    public static event Action<int> OnEnemyDeadScoreEvent;
    public static event Action<int> OnEnemyMoveEndPointEvent;

    public virtual void Setup(EnemyEntity data)
    {
        myEnemyData = data;
        currentHP = myEnemyData.hp;

        OnSpawn();
    }

    public void OnSpawn()
    {
        currentHPBar = ObjectPoolManager.instance.GetObject(enemyHPBar);

        if (currentHPBar != null)
        {
            var follower = currentHPBar.GetComponent<HPBarFollower>();

            if (follower != null)
            {
                follower.SetTarget(this.transform);

                hpSlider = follower.GetSlider();
                if(hpSlider != null)
                {
                    hpSlider.value = currentHP / myEnemyData.hp;
                }
            }
        }
    }

    public virtual void TakeDamage(float damage)
    {
        currentHP -= damage;

        if(hpSlider != null)
        {
            hpSlider.value = currentHP / myEnemyData.hp;
        }

        if (currentHP <= 0)
        {
            OnEnemyDeadDropCoinEvent?.Invoke(myEnemyData.dropCoins);
            OnEnemyDeadScoreEvent?.Invoke(myEnemyData.score);
            Die();
        }

    }

    protected virtual void Die()
    {
        Despawn();
    }

    public void MoveEndPoint()
    {
        OnEnemyMoveEndPointEvent?.Invoke(myEnemyData.damage);

        Despawn();
    }

    public void Despawn()
    {
        if (currentHPBar != null)
        {
            // Enum 대신 문자열 "EnemyHPBar" 사용
            ObjectPoolManager.instance.ReturnObject(enemyHPBar, currentHPBar.gameObject);
            currentHPBar = null;
            hpSlider = null;
        }

        // Enum 대신 문자열 변수 사용
        ObjectPoolManager.instance.ReturnObject(poolName, gameObject);
    }
}
