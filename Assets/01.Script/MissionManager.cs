using System;
using UnityEngine;


public class MissionManager : MonoBehaviour
{
    [Header("미션관리")]
    public static MissionManager instance;

    [SerializeField] private MissionData[] missions;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public float GetMissionCooldown(int index)
    {
        if(missions != null && index >= 0 && index < missions.Length)
        {
            return missions[index].cooldown;
        }
        return 0;
    }

    public int GetMissionCost(int index)
    {
        if (missions != null && index >= 0 && index < missions.Length)
        {
            return missions[index].cost;
        }
        return 0;
    }

    public bool SpawnMissionEnemy(int index)
    {
        if (missions != null && index >=0 && index <missions.Length )
        {
            MissionData data = missions[index];

            if (CoinManager.instance.HasEnoughCoins(data.cost))
            {
                CoinManager.instance.UpdateCoins(-data.cost);
                WaveManager.instance.SpawnEnemy(data.enemyData.enemyName);

                return true;
            }
        }
        return false;
    }
}
