using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIManager : MonoBehaviour
{
    public static GameOverUIManager instance;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Button titleButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameOverPanel.transform.localScale = Vector3.zero;
        gameOverPanel.SetActive(false);

        if (titleButton != null)
        {
            titleButton.onClick.AddListener(GoToTitle);
        }
    }

    public void ShowGameOver(int finalWave, int finalScore)
    {
        gameOverPanel.SetActive(true);

        gameOverPanel.transform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBounce)
            .SetUpdate(true);

        if(resultText != null)
        {
            resultText.text = $"도달한 웨이브: Wave {finalWave}\n 최종 점수: {finalScore} 점";
        }

        if (RankingManager.Instance != null)
        { 
            RankingManager.Instance.AddRankAndSave("Player", finalWave, finalScore);
        }

        Time.timeScale = 0f;
    }

    public void GoToTitle()
    {
        Time.timeScale = 0f;

        if (SceneChangeManager.instance != null)
        {
            SceneChangeManager.instance.LoadScene(0);
        }
    }
}
