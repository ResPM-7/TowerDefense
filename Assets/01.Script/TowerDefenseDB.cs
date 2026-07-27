using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class TowerDefenseDB : ScriptableObject
{
	public List<TowerEntity> Tower;
	public List<EnemyEntity> Enemy;
	public List<MissionEntity> Mission;
}
