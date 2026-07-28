using System;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private TextMeshProUGUI healthText;

    public static event Action OnGameOverEvent;

    private void Start()
    {
        UpdateHealth(0);
    }

    private void OnEnable()
    {
        Enemy.OnEnemyMoveEndPointEvent += UpdateHealth;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyMoveEndPointEvent -= UpdateHealth;
    }

    public void UpdateHealth(int changeAmount)
    {
        health -= changeAmount;
        healthText.text = health.ToString();

        if (health <= 0)
        {
            health = 0;
            healthText.text = health.ToString();

            OnGameOverEvent?.Invoke();
        }
    }

}
