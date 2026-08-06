using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Button titleButton;

    private RectTransform panelRect;


    private void Start()
    {
        gameOverPanel.transform.localScale = Vector3.zero;
        gameOverPanel.SetActive(false);

        if (titleButton != null)
        {
            titleButton.onClick.AddListener(GoToTitle);
        }

        panelRect = gameOverPanel.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        HealthManager.OnGameOverEvent += HandleGameOver;
    }

    private void OnDisable()
    {
        HealthManager.OnGameOverEvent -= HandleGameOver;
    }

    private void HandleGameOver()
    {
        int finalWave = 0;
        if(WaveManager.instance != null)
        {
            finalWave = WaveManager.instance.CurrentWave;
        }
        int finalScore = ScoreManager.CurrentScore;

        ShowResultPanel(finalWave, finalScore);
    }

    public void ShowResultPanel(int finalWave, int finalScore)
    {
        panelRect.SetAsLastSibling();

        gameOverPanel.SetActive(true);

        gameOverPanel.transform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBounce)
            .SetUpdate(true);

        if(resultText != null)
        {
            resultText.text = $"도달한 웨이브: Wave {finalWave}\n 최종 점수: {finalScore} 점";
        }

        if (RankingManager.instance != null)
        { 
            RankingManager.instance.AddRankAndSave(finalWave, finalScore);
        }

        Time.timeScale = 0f;
    }

    public void ShowGameClear(int finalWave, int finalScore)
    {
        panelRect.SetAsLastSibling();

        gameOverPanel.SetActive(true);

        gameOverPanel.transform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBounce)
            .SetUpdate(true);


        if (resultText != null)
        {
            resultText.text = $"도달한 웨이브: Wave {finalWave}\n 최종 점수: {finalScore} 점";
        }

        if (RankingManager.instance != null)
        {
            RankingManager.instance.AddRankAndSave(finalWave, finalScore);
        }

        Time.timeScale = 0f;
    }

    public void GoToTitle()
    {
        if (SceneChangeManager.instance != null)
        {
            SceneChangeManager.instance.LoadScene(0);
        }
    }
}
