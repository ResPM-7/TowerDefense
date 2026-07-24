using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    [SerializeField] private int coins;
    [SerializeField] private TextMeshProUGUI coinText;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        coins = 10;

        UpdateCoins(0);
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDeadEvent += UpdateCoins;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDeadEvent -= UpdateCoins;
    }

    public void UpdateCoins(int changeAmount)
    {
        coins += changeAmount;

        coinText.text = coins.ToString();
    }

    public bool HasEnoughCoins(int cash)
    {
        if (coins >= cash)
        {
            return true;
        }
        return false;
    }
}
