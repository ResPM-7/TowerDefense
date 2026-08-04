using UnityEngine;


public class MissionManager : MonoBehaviour
{

    public float GetMissionCooldown(int missionNum)
    {
        if (GameData.Missions.TryGetValue(missionNum, out MissionEntity data))
        {
            return data.cooldown;
        }
        return 0;
    }

    public int GetMissionCost(int missionNum)
    {
        if (GameData.Missions.TryGetValue(missionNum, out MissionEntity data))
        {
            return data.cost;
        }
        return 0;
    }

    public bool SpawnMissionEnemy(int missionNum)
    {
        if (GameData.Missions.TryGetValue(missionNum, out MissionEntity missionData))
        {
            if (GameManager.Instance.Coin.HasEnoughCoins(missionData.cost))
            {
                if (GameData.Enemies.TryGetValue(missionData.monsterSpawnNumber, out EnemyEntity enemyData))
                {
                    if (System.Enum.TryParse(enemyData.enemyName, out PoolType enemyType))
                    {
                        GameManager.Instance.Coin.UpdateCoins(-missionData.cost);
                        GameManager.Instance.Wave.SpawnEnemy(enemyType);
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
