using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    public static int CurrentScore { get; private set; } = 0;

    private void Awake()
    {
        CurrentScore = 0;
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDeadScoreEvent += AddScore;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDeadScoreEvent -= AddScore;
    }

    public void AddScore(int amount)
    {
        CurrentScore += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = CurrentScore.ToString("D6");
        }
    }
}
