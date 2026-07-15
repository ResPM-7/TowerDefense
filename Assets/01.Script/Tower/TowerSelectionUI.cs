using UnityEngine;

public class TowerSelectionUI : MonoBehaviour
{
    public static GameObject selectedTowerPrefab;

    public void SelectTower(GameObject towerPrefab)
    {
        if (towerPrefab == selectedTowerPrefab)
        {
            selectedTowerPrefab = null;
            return;
        }
        else
            selectedTowerPrefab = towerPrefab;
    }
}
