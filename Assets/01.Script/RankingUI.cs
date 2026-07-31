using DG.Tweening;
using TMPro;
using UnityEngine;

public class RankingUI : MonoBehaviour
{
    [SerializeField] private GameObject rankingPanel;
    [SerializeField] private TextMeshProUGUI[] rankTexts;

    private Vector2 openPos;
    private Vector2 closePos;

    private RectTransform panelRect;

    private void Awake()
    {
        panelRect = rankingPanel.GetComponent<RectTransform>();
    }

    private void Start()
    {
        openPos = Vector2.zero;
        closePos = panelRect.anchoredPosition;

        rankingPanel.SetActive(false);
    }

    public void OpenRanking()
    {
        rankingPanel.SetActive(true);

        panelRect.DOAnchorPos(openPos, 0.5f)
            .SetEase(Ease.OutExpo);

        RefreshRankingUI();
    }

    public void CloseRanking()
    {
        panelRect.DOAnchorPos(closePos, 0.4f)
            .SetEase(Ease.InExpo)
            .OnComplete(() => rankingPanel.SetActive(false));
    }

    private void RefreshRankingUI()
    {

        RankingData rankList = RankingManager.instance.LoadRanking();

        for (int i = 0; i < rankTexts.Length; i++)
        {
            if (i < rankList.entries.Count)
            {
                RankEntry entry = rankList.entries[i];
                rankTexts[i].text = $"{i + 1}. Wave {entry.wave} | {entry.score}Á¡";
                rankTexts[i].gameObject.SetActive(true);
            }
            else
            {
                rankTexts[i].gameObject.SetActive(false);
            }
        }
    }
}
