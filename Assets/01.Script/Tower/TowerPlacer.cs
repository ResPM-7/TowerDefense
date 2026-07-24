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

    private Camera mainCam;
    private GameObject lastSelectedPrefab = null;
    private int cachedTowerCost = 0;
    private string cachedTowerName = "";

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if(Mouse.current.rightButton.wasPressedThisFrame)
        {
            CancelPlacement();
        }

        HandlePlacement();
    }

    private void CancelPlacement()
    {
        TowerSelectionUI.selectedTowerPrefab = null;

        if(ghostPrefab != null && ghostPrefab.activeSelf)
        {
            ghostPrefab.SetActive(false);
        }

        lastSelectedPrefab = null;
        cachedTowerCost = 0;
        cachedTowerName = "";
    }

    void HandlePlacement()
    {
        GameObject currentSelected = TowerSelectionUI.selectedTowerPrefab;

        if (TowerSelectionUI.selectedTowerPrefab == null)
        {
            if (ghostInstance != null && ghostInstance.activeSelf)
                ghostInstance.SetActive(false);

            lastSelectedPrefab = null;
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

        if(currentSelected != lastSelectedPrefab)
        {
            lastSelectedPrefab = currentSelected;

            ghostSpriteRenderer.sprite = currentSelected.GetComponent<SpriteRenderer>().sprite;

            cachedTowerName = currentSelected.name;

            Tower towerComponent = currentSelected.GetComponent<Tower>();
            cachedTowerCost = towerComponent != null ? towerComponent.TowerData.cost : 0;
        }

        Vector3 mouseWroldPos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWroldPos.z = 0f;

        Vector3Int cellPos = placementMap.WorldToCell(mouseWroldPos);
        Vector3 worldCenter = placementMap.GetCellCenterWorld(cellPos);
        worldCenter.z = 0f;

        ghostInstance.transform.position = worldCenter + new Vector3(0, placementMap.cellSize.y * 0.25f);

        bool hasEnoughCost = CoinManager.instance.HasEnoughCoins(cachedTowerCost);


        bool isValid = placementMap.HasTile(cellPos)
            && !unplacementMap.HasTile(cellPos)
            && !occupiedTiles.Contains(cellPos)
            && hasEnoughCost;


        ghostTowerComponent.SetValid(isValid);

        if (Mouse.current.leftButton.wasPressedThisFrame && isValid)
        {
            GameObject tower = ObjectPoolManager.instance.GetObject(cachedTowerName);

            if (tower != null)
            {
                tower.transform.position = ghostInstance.transform.position;
                tower.transform.rotation = Quaternion.identity;
                occupiedTiles.Add(cellPos);

                CoinManager.instance.UpdateCoins(-cachedTowerCost);
            }
        }
    }
}
