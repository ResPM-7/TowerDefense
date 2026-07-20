using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Data/Tower")]
public class TowerData : ScriptableObject
{
    public string towerName;

    public int cost;
    public float damage;
    public float attackRange;
    public float attackCooldown;

    public TowerData nextUpgradeData;
}
