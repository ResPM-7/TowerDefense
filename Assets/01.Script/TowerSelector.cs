using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TowerSelector : MonoBehaviour
{
    public static TowerSelector Instance;

    [SerializeField] private TowerUpgradeUI upgradeUI;
    [SerializeField] private Transform rangeIndicator;

    private Tower currentSelectedTower;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (rangeIndicator != null)
            rangeIndicator.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Tower clickedTower = hit.collider.GetComponent<Tower>();
                if (clickedTower != null)
                {
                    SelectTower(clickedTower);
                }
                else
                {
                    DeselectTower();
                }
            }
            else
            {
                DeselectTower();
            }
        }
    }
    public void SelectTower(Tower tower)
    {
        if (currentSelectedTower == tower)
        {
            DeselectTower();
            return;
        }

        currentSelectedTower = tower;
        TowerData towerData = tower.TowerData;

        if (rangeIndicator != null)
        {
            rangeIndicator.gameObject.SetActive(true);
            rangeIndicator.position = tower.transform.position;
            rangeIndicator.localScale = new Vector3(towerData.attackRange * 2, towerData.attackRange * 2, 1f);
        }

        if (upgradeUI != null)
        {
            upgradeUI.SetTargetTower(tower);
        }
    }

    public void DeselectTower()
    {
        currentSelectedTower = null;

        if (rangeIndicator != null)
        {
            rangeIndicator.gameObject.SetActive(false);
        }

        if(upgradeUI != null)
        {
            upgradeUI.HideUI();
        }
    }
}
