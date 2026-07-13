using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TowerPlacer : MonoBehaviour
{
    [SerializeField] private Tilemap placementMap;
    [SerializeField] private Tilemap unplacementMap;

    [SerializeField] private GameObject ghostPrefab;

    private HashSet<Vector3Int> occupiedTiles = new HashSet<Vector3Int>();
    private GameObject ghostInstance;


    private void Update()
    {
        HandlePlacementHover();
        HandlePlacementClick();
    }

    void HandlePlacementHover()
    {
        if (TowerSelectionUI.selectedTowerPrefab == null)
        {
            if (ghostInstance != null)
                Destroy(ghostInstance);
            return;
        }

        if (ghostInstance == null)
        {
            ghostInstance = Instantiate(ghostPrefab);
        }

        ghostInstance.GetComponent<SpriteRenderer>().sprite = TowerSelectionUI.selectedTowerPrefab.GetComponent<SpriteRenderer>().sprite;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3Int cellpos = placementMap.WorldToCell(mouseWorldPos);

        Vector3 worldCenter = placementMap.GetCellCenterWorld(cellpos);
        worldCenter.z = 0f;

        ghostInstance.transform.position = worldCenter + new Vector3(0, placementMap.cellSize.y * 0.25f);

        bool valid = placementMap.HasTile(cellpos) && !occupiedTiles.Contains(cellpos);

        ghostInstance.GetComponent<GhostTower>().SetValid(valid);
    }

    void HandlePlacementClick()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;

        if (TowerSelectionUI.selectedTowerPrefab != null) return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3Int cellPos = placementMap.WorldToCell(mouseWorldPos);

        if (!placementMap.HasTile(cellPos)) return;
        if (occupiedTiles.Contains(cellPos)) return;

        Instantiate(TowerSelectionUI.selectedTowerPrefab, ghostInstance.transform.position, Quaternion.identity);

        TowerSelectionUI.selectedTowerPrefab = null;
        occupiedTiles.Add(cellPos);
    }
}
