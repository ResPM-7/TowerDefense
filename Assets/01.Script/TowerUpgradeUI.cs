using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeUI : MonoBehaviour
{
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button upgradeButton;

    [SerializeField] private Vector3 offset = new Vector3(0, -1f, 0);

    [SerializeField] private TowerDefenseDB towerDB;

    private Tower targetTower;

    private TowerEntity nextUpgradeData;

    private void Start()
    {
        HideUI();
        upgradeButton.onClick.AddListener(ExecuteUpgrade);
    }

    public void SetTargetTower(Tower tower)
    {
        targetTower = tower;
        TowerEntity currentData = tower.TowerData;

        if (currentData == null || towerDB == null) return;

        if(currentData.nextUpgradeDataNum != 0)
        {
            nextUpgradeData = towerDB.Tower.FirstOrDefault(t=>t.number == currentData.nextUpgradeDataNum);
        }
        else
        {
            nextUpgradeData = null;
        }

        if (nextUpgradeData != null)
        {
            uiPanel.SetActive(true);

            if (costText != null)
                costText.text = nextUpgradeData.cost.ToString();

            uiPanel.transform.position = targetTower.transform.position + offset;
        }
        else
        {
            uiPanel.SetActive(false);
        }
    }

    public void HideUI()
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

        int cost = nextUpgradeData.cost;
        bool canbought = CoinManager.instance.HasEnoughCoins(cost);

        if (canbought)
        {
            CoinManager.instance.UpdateCoins(-cost);

            Vector3 pos = targetTower.transform.position;
            Quaternion rot = targetTower.transform.rotation;

            string oldName = targetTower.TowerData.towerName;

            ObjectPoolManager.instance.ReturnObject(oldName, targetTower.gameObject);

            GameObject newTower = ObjectPoolManager.instance.GetObject(nextUpgradeData.towerName);

            if (newTower != null)
            {
                newTower.transform.position = pos;
                newTower.transform.rotation = rot;
            }

            TowerSelector.instance.DeselectTower();
        }
    }
}
