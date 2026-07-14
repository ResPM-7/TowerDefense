using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WaveData
{
    public float duration = 10f;
    public int normalEnemy = 5;
    public int specialEnemy = 2;
}

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WaveData[] waves;

    [SerializeField] private string normalEnemyName;
    [SerializeField] private string specialEnemyName;

    [SerializeField] private Transform[] wayPoints;

    private int currentWaveIndex = 0;
    private bool waveRunning = false;


    public void StartWave()
    {
        if (waveRunning) return;
        if (currentWaveIndex >= waves.Length) return;

        StartCoroutine(RunWave());
    }

    IEnumerator RunWave()
    {
        waveRunning = true;

        WaveData wave = waves[currentWaveIndex];
        //일반 몬스터 안나오고 스페셜 몬스터만 나올수 있기 때문  나누기 0 이면 무한대이기 때문
        float normalDelayTime = wave.normalEnemy > 0 ? (wave.duration / 3f) / wave.normalEnemy : 0f;
        WaitForSeconds normalWait = new WaitForSeconds(normalDelayTime);
        //마찬가지
        float specialDelayTime = wave.specialEnemy > 0 ? (wave.duration / 3f) / wave.specialEnemy : 0f;
        WaitForSeconds specialWait = new WaitForSeconds(specialDelayTime);

        for (int i = 0; i < wave.normalEnemy; i++)
        {
            SpawnEnemy(normalEnemyName);
            yield return normalWait;
        }

        for (int i = 0; i < wave.specialEnemy; i++)
        {
            SpawnEnemy(specialEnemyName);
            yield return specialWait;
        }

        yield return specialWait;
        currentWaveIndex++;
        waveRunning = false;

    }

    void SpawnEnemy(string name)
    {
        GameObject ob = ObjectPoolManager.instance.GetObject(name);
        if (ob != null)
        {
            ob.transform.position = wayPoints[0].position;
            ob.transform.rotation = wayPoints[0].rotation;
            EnemyMovement enemy = ob.GetComponent<EnemyMovement>();
            if (enemy != null)
            {
                enemy.wayPoints = wayPoints;
            }
        }
    }
}
