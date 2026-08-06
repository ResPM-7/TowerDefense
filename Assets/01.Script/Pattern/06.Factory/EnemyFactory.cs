using UnityEngine;

public class EnemyFactory : Singleton<EnemyFactory>
{
    public Enemy SpawnEnemy(int enemyNumber, Vector3 spawnPosition)
    {
        // 1. 엑셀에서 파싱해 둔 데이터(GameData) 로드
        if (!GameData.Enemies.TryGetValue(enemyNumber, out EnemyEntity enemyData))
        {
            Debug.LogError($"해당 번호({enemyNumber})의 몬스터 데이터가 엑셀에 없습니다!");
            return null;
        }

        GameObject obj = ObjectPoolManager.instance.GetObject(enemyData.enemyName);

        if (obj == null) return null;

        // 위치 설정
        obj.transform.position = spawnPosition;

        // 4. 팩토리가 몬스터에게 엑셀 데이터 주입 (조립 과정)
        Enemy enemyComponent = obj.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.Setup(enemyData);
        }

        return enemyComponent;
    }
}
