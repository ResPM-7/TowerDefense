using UnityEngine;

[CreateAssetMenu(fileName = "MissionData", menuName = "Data/MissionData")]
public class MissionData : ScriptableObject
{
    public EnemyData enemyData;

    public int cost;
    public float cooldown;
}
