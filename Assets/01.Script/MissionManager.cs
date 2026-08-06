using UnityEngine;


public class MissionManager : Singleton<MissionManager>
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
            if (CoinManager.instance.HasEnoughCoins(missionData.cost))
            {
                // 1. 코인 차감
                CoinManager.instance.UpdateCoins(-missionData.cost);

                // 2. Enum 파싱이나 에너미 데이터 탐색 로직 완전 삭제!
                // 앞서 수정해둔 WaveManager가 '몬스터 번호(int)'만 받으면 알아서 스폰해주므로 바로 넘겨줍니다.
                WaveManager.instance.SpawnEnemy(missionData.monsterSpawnNumber);

                return true;
            }
        }
        return false;
    }
}
