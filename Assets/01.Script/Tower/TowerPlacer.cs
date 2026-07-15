using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] private Tilemap placementMap;
    [SerializeField] private Tilemap unplacementMap;

    [SerializeField] private GameObject ghostPrefab;

    private HashSet<Vector3Int> occupiedTiles = new HashSet<Vector3Int>();
    private GameObject ghostInstance;
    private GhostTower ghostTowerComponent;
    private SpriteRenderer ghostSpriteRenderer;


    private void Update()
    {
        HandlePlacement();
    }

    void HandlePlacement()
    {
        if (TowerSelectionUI.selectedTowerPrefab == null)
        {
            if (ghostInstance != null && ghostInstance.activeSelf)
                ghostInstance.SetActive(false);
            return;
        }

        if (ghostInstance == null)
        {
            ghostInstance = Instantiate(ghostPrefab);
            ghostTowerComponent = ghostInstance.GetComponent<GhostTower>();
            ghostSpriteRenderer = ghostInstance.GetComponent<SpriteRenderer>();
        }

        if (!ghostInstance.activeSelf)
            ghostInstance.SetActive(true);

        //UI클릭시 타워 설치 막아주는 코드
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            ghostInstance.SetActive(false);
            return;
        }

        Vector3 mouseWroldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWroldPos.z = 0f;

        Vector3Int cellPos = placementMap.WorldToCell(mouseWroldPos);
        Vector3 worldCenter = placementMap.GetCellCenterWorld(cellPos);
        worldCenter.z = 0f;

        ghostInstance.transform.position = worldCenter + new Vector3(0, placementMap.cellSize.y * 0.25f);
        ghostSpriteRenderer.sprite = TowerSelectionUI.selectedTowerPrefab.GetComponent<SpriteRenderer>().sprite;

        Tower towerComponent = TowerSelectionUI.selectedTowerPrefab.GetComponent<Tower>();
        int towerCost = towerComponent != null ? towerComponent.Cost : 0;
        bool hasEnoughCost = CoinManager.instance.HasEnoughCoins(towerCost);


        bool isValid = placementMap.HasTile(cellPos)
            && !unplacementMap.HasTile(cellPos)
            && !occupiedTiles.Contains(cellPos)
            && hasEnoughCost;


        ghostTowerComponent.SetValid(isValid);

        if (Mouse.current.leftButton.wasPressedThisFrame && isValid)
        {
            string towerName = TowerSelectionUI.selectedTowerPrefab.name;

            GameObject tower = ObjectPoolManager.instance.GetObject(towerName);

            if (tower != null)
            {
                tower.transform.position = ghostInstance.transform.position;
                tower.transform.rotation = Quaternion.identity;
                occupiedTiles.Add(cellPos);

                CoinManager.instance.UpdateCoins(-towerCost);
            }
        }
    }
}
