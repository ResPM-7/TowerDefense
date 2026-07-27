using System;
using System.Linq;
using UnityEngine;


public class MissionManager : MonoBehaviour
{
    [Header("미션관리")]
    public static MissionManager instance;

    [SerializeField] private TowerDefenseDB missionDB;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public float GetMissionCooldown(int missionNum)
    {
        if (missionDB != null && missionDB.Mission != null)
        {
            var data = missionDB.Mission.FirstOrDefault(m => m.number == missionNum);
            if (data != null) return data.cooldown;
        }
        return 0;
    }

    public int GetMissionCost(int missionNum)
    {
        if (missionDB != null && missionDB.Mission != null)
        {
            var data = missionDB.Mission.FirstOrDefault(m => m.number == missionNum);
            if (data != null) return data.cost;
        }
        return 0;
    }

    public bool SpawnMissionEnemy(int missionNum)
    {
        if (missionDB == null) return false;

        var missionData = missionDB.Mission.FirstOrDefault(m => m.number == missionNum);

        if (missionData != null)
        {
            if (CoinManager.instance.HasEnoughCoins(missionData.cost))
            {
                var enemyData = missionDB.Enemy.FirstOrDefault(m => m.number == missionData.monsterSpawnNumber);

                if (enemyData != null)
                {
                    CoinManager.instance.UpdateCoins(-missionData.cost);
                    WaveManager.instance.SpawnEnemy(enemyData.enemyName);
                    return true;
                }

            }
        }
        return false;
    }
}
