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
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UpdateCoins(0);
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDeath += UpdateCoins;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDeath -= UpdateCoins;
    }

    public void UpdateCoins(int changeAmount)
    {
        coins += changeAmount;

        coinText.text = coins.ToString();
    }
}
