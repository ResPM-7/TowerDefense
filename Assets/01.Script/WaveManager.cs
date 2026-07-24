using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SpawnGroup
{
    public string enemyName;
    public int count;
    public float spawnInterval = 1f;
}

[System.Serializable]
public class WaveData
{
    public SpawnGroup[] spawnGroups;

    public float timeToNextWave = 10f;
}

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [SerializeField] private WaveData[] waves;

    [SerializeField] private Transform[] wayPoints;

    [SerializeField] private TextMeshProUGUI waveNumberText;
    [SerializeField] private TextMeshProUGUI countdownText;

    [SerializeField] private Button startWaveButton;

    private int currentWaveIndex = 0;
    public int CurrentWave { get { return currentWaveIndex; } }

    private bool waveRunning = false;
    private bool isWaitingForNextWave = false;
    private Coroutine countdownCoroutine;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateWaveUI();
        if (countdownText != null)
        {
            countdownText.text = "대기 중";
            countdownText.gameObject.SetActive(true);
        }
        if (startWaveButton != null)
        {
            startWaveButton.onClick.RemoveAllListeners();
            startWaveButton.onClick.AddListener(StartWave);
        }
    }

    public void StartWave()
    {
        if (currentWaveIndex >= waves.Length) return;

        if (waveRunning) return;
        if (isWaitingForNextWave && countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            isWaitingForNextWave = false;
        }

        StartCoroutine(RunWave());
    }

    IEnumerator RunWave()
    {
        waveRunning = true;
        isWaitingForNextWave = false;

        UpdateWaveUI();

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        WaveData wave = waves[currentWaveIndex];

        if(wave.spawnGroups != null)
        {
            foreach (SpawnGroup group in wave.spawnGroups)
            {
                WaitForSeconds wait = new WaitForSeconds(group.spawnInterval);

                for(int i= 0; i<group.count; i++)
                {
                    SpawnEnemy(group.enemyName);
                    yield return wait;
                }
            }
        }

        waveRunning = false;
        currentWaveIndex++;
        UpdateWaveUI();

        if (currentWaveIndex < waves.Length)
        {
            countdownCoroutine = StartCoroutine(WaitForNextWave(wave.timeToNextWave));
        }
        else
        {
            if (countdownText != null)
            {
                countdownText.text = "모든 웨이브 클리어";
                countdownText.gameObject.SetActive(true);
            }
            if (startWaveButton != null)
            {
                startWaveButton.interactable = false;
            }

        }

    }

    IEnumerator WaitForNextWave(float waitTime)
    {
        isWaitingForNextWave = true;
        float timer = waitTime;

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
        }

        while (timer > 0f)
        {
            if (countdownText != null)
            {
                countdownText.text = $"다음 웨이브까지: {timer:F1}초";
            }

            timer -= Time.deltaTime;
            yield return null;
        }

        if (countdownText != null)
        {
            countdownText.text = "웨이브 시작!";
        }

        isWaitingForNextWave = false;
        StartWave();
    }

    // UI 업데이트 헬퍼 함수
    private void UpdateWaveUI()
    {
        if (waveNumberText != null)
        {
            
            int displayWave = Mathf.Min(currentWaveIndex + 1, waves.Length);
            waveNumberText.text = $"Wave {displayWave}";
        }
    }

    public void SpawnEnemy(string name)
    {
        if (string.IsNullOrEmpty(name)) return;

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
