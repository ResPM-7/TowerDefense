using UnityEngine;

public class EnemyFactory : Singleton<EnemyFactory>
{
    public Enemy SpawnEnemy(int enemyNumber, Vector3 spawnPosition)
    {
        //엑셀에서 파싱해 둔 데이터 확인
        if (!GameData.Enemies.TryGetValue(enemyNumber, out EnemyEntity enemyData))
        {
            return null;
        }

        string safeEnemyName = enemyData.enemyName.Trim();
        GameObject obj = ObjectPoolManager.instance.GetObject(safeEnemyName);

        if (obj == null)
        {
            return null;
        }

        // 위치 설정
        obj.transform.position = spawnPosition;

        //팩토리가 몬스터에게 엑셀 데이터 주입 (조립 과정)
        Enemy enemyComponent = obj.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.Setup(enemyData);
        }

        return enemyComponent;
    }
}
