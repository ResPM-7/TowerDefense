using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private string[] missionEnemyName;
    [SerializeField] private int[] spawnCost;

    public void SpawnMissionEnemy(int i)
    {
        if (missionEnemyName != null && i < missionEnemyName.Length)
        {
            int cost = (spawnCost != null && i < spawnCost.Length) ? spawnCost[i] : 0;

            if (CoinManager.instance.HasEnoughCoins(cost))
            {
                CoinManager.instance.UpdateCoins(-cost);
                WaveManager.instance.SpawnEnemy(missionEnemyName[i]);
            }
        }
    }
}
