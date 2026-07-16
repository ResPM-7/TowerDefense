using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeUI : MonoBehaviour
{
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button upgradeButton;

    [SerializeField] private Vector3 offset = new Vector3(0, -1f, 0);

    private Tower targetTower;
    private TowerData nextUpgradeData;

    private void Start()
    {
        Hide();

        upgradeButton.onClick.AddListener(ExecuteUpgrade);
    }

    public void SetTargetTower(Tower tower)
    {
        targetTower = tower;
        nextUpgradeData = tower.TowerData.nextUpgradeData;

        if (nextUpgradeData != null)
        {
            uiPanel.SetActive(true);

            if (costText != null)
                costText.text = nextUpgradeData.cost.ToString();

            transform.position = targetTower.transform.position + offset;
        }
        else
        {

        }
    }

    public void Hide()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }

        targetTower = null;
        nextUpgradeData = null;
    }

    private void ExecuteUpgrade()
    {
        if (targetTower == null || nextUpgradeData == null) return;

        int cost =nextUpgradeData.cost;
        bool canbought = CoinManager.instance.HasEnoughCoins(cost);

        if (canbought)
        {
            CoinManager.instance.UpdateCoins(-cost);

            Vector3 pos = targetTower.transform.position;
            Quaternion rot = targetTower.transform.rotation;
        }
    }
}
