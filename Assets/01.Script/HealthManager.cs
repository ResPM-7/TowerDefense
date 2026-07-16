using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;

    [SerializeField] private int health = 100;
    [SerializeField] private TextMeshProUGUI healthText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

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

        if (health < 0)
        {
            health = 0;
            //Á×À¸¸é ·©Å© ±¸Çö ¿¹Á¤
        }
    }
}
