using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Data/Tower")]
public class TowerData : ScriptableObject
{
    public string towerName;
    public GameObject towerPrefab;

    public int cost;
    public float damage;
    public float attackRanage;
    public float attackColldown;

    public TowerData nextUpgradeData;
}
