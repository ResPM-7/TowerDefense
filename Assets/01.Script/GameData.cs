using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameData
{
    public static Dictionary<int, TowerEntity> Towers { get; private set; }
    public static Dictionary<int, EnemyEntity> Enemies { get; private set; }
    public static Dictionary<int, MissionEntity> Missions { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        TowerDefenseDB db = Resources.Load<TowerDefenseDB>("TowerDefenseDB");

        if (db != null)
        {
            Towers = db.Tower.ToDictionary(t => t.number);
            Enemies = db.Enemy.ToDictionary(e => e.number);
            Missions = db.Mission.ToDictionary(m => m.number);
        }
    }
}
