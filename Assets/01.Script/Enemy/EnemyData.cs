using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/Enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public GameObject enemyPrefab;

    public float hp;
    public float speed;
    public int damage;
    public int dropCoins;
}
