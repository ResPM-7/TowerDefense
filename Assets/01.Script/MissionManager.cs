using System;
using System.Linq;
using UnityEngine;


public class MissionManager : MonoBehaviour
{
    [Header("미션관리")]
    [SerializeField] private TowerDefenseDB missionDB;


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
            if (GameManager.Instance.Coin.HasEnoughCoins(missionData.cost))
            {
                var enemyData = missionDB.Enemy.FirstOrDefault(m => m.number == missionData.monsterSpawnNumber);

                if (enemyData != null)
                {
                    GameManager.Instance.Coin.UpdateCoins(-missionData.cost);
                    GameManager.Instance.Wave.SpawnEnemy(enemyData.enemyName);
                    return true;
                }

            }
        }
        return false;
    }
}
