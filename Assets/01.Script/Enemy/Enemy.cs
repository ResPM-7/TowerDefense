using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private PoolType enemyType;

    //[SerializeField] private EnemyData enemyData;
    [SerializeField] private TowerDefenseDB enemyDB;
    [SerializeField] private int myEnemyNumber;

    protected EnemyEntity myEnemyData;
    public EnemyEntity EnemyData { get { return myEnemyData; } }

    public static event Action<int> OnEnemyDeadEvent;
    public static event Action<int> OnEnemyDeadScoreEvent;
    public static event Action<int> OnEnemyMoveEndPointEvent;

    protected float currentHP;
    private GameObject currentHPBar;

    private Slider hpSlider;

    private void Awake()
    {
        InitializeData();
    }

    protected void InitializeData()
    {
        if(enemyDB != null && enemyDB.Enemy != null)
        {
            myEnemyData = enemyDB.Enemy.FirstOrDefault(e =>e.number == myEnemyNumber);
        }
    }

    protected virtual void OnEnable()
    {
        currentHP = myEnemyData.hp;
        OnSpawn();
    }

    public void OnSpawn()
    {
        currentHPBar = ObjectPoolManager.instance.GetObject(PoolType.EnemyHPBar);

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
            OnEnemyDeadEvent?.Invoke(myEnemyData.dropCoins);
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
            ObjectPoolManager.instance.ReturnObject(PoolType.EnemyHPBar, currentHPBar.gameObject);
            currentHPBar = null;
            hpSlider = null;
        }

        ObjectPoolManager.instance.ReturnObject(enemyType, gameObject);
    }
}
