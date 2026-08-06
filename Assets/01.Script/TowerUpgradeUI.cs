using System;
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

    private Tower targetTower;
    private TowerEntity nextUpgradeData;

    public event Action OnUpgradeComplete;

    private void Start()
    {
        HideUI();
        upgradeButton.onClick.AddListener(ExecuteUpgrade);
    }

    public void SetTargetTower(Tower tower)
    {
        targetTower = tower;
        TowerEntity currentData = tower.TowerData;

        if (currentData == null) return;

        if (currentData.nextUpgradeDataNum != 0 && GameData.Towers.TryGetValue(currentData.nextUpgradeDataNum, out TowerEntity data))
        {
            nextUpgradeData = data;
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

            string oldPoolName = targetTower.PoolName;
            ObjectPoolManager.instance.ReturnObject(oldPoolName, targetTower.gameObject);

            GameObject newTowerObj = ObjectPoolManager.instance.GetObject(nextUpgradeData.towerName);

            if (newTowerObj != null)
            {
                newTowerObj.transform.position = pos;
                newTowerObj.transform.rotation = rot;

                Tower newTowerScript = newTowerObj.GetComponent<Tower>();
                if (newTowerScript != null)
                {
                    newTowerScript.Setup(nextUpgradeData);
                }
            }

            OnUpgradeComplete?.Invoke();

            HideUI();
        }
    }
}