using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public WaveManager Wave { get; private set; }
    public CoinManager Coin { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

            Wave = GetComponent<WaveManager>();
            Coin = GetComponent<CoinManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
