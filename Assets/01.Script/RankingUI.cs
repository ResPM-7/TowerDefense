using DG.Tweening;
using TMPro;
using UnityEngine;

public class RankingUI : MonoBehaviour
{
    [SerializeField] private GameObject rankingPanel;
    [SerializeField] private TextMeshProUGUI[] rankTexts;

    private void Start()
    {
        rankingPanel.transform.localScale = Vector3.zero;
        rankingPanel.SetActive(false);
    }

    public void OpenRanking()
    {
        rankingPanel.SetActive(true);

        rankingPanel.transform.DOScale(Vector3.one, 0.4f)
            .SetEase(Ease.OutBack);

        RefreshRankingUI();
    }

    public void CloseRanking()
    {
        rankingPanel.transform.DOScale(Vector3.zero, 0.3f)
            .SetEase(Ease.InBack)
            .OnComplete(() => rankingPanel.SetActive(false));
    }

    private void RefreshRankingUI()
    {

        RankList rankList = RankingManager.Instance.LoadRanking();

        for (int i = 0; i < rankTexts.Length; i++)
        {
            if (i < rankList.entries.Count)
            {
                RankEntry entry = rankList.entries[i];
                rankTexts[i].text = $"{i + 1}. {entry.playerName} | Wave {entry.wave} | {entry.score}Á¡";
                rankTexts[i].gameObject.SetActive(true);
            }
            else
            {
                rankTexts[i].gameObject.SetActive(false);
            }
        }
    }
}
