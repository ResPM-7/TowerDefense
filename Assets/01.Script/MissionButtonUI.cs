using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionButtonUI : MonoBehaviour
{
    [SerializeField] private int missionIndex;
    [SerializeField] private Image coolTimeImg;
    [SerializeField] private TextMeshProUGUI missionCostText;

    private Button missionButton;
    private float currentCoolTime = 0f;
    private float maxCoolTime;
    private bool isCooldown = false;

    private void Awake()
    {
        missionButton = GetComponent<Button>();
    }

    private void Start()
    {
        if (coolTimeImg != null) coolTimeImg.fillAmount = 0f;

        if (missionButton != null)
            missionButton.onClick.AddListener(TrySpawnMission);

        if (missionCostText != null)
        {
            int cost = MissionManager.instance.GetMissionCost(missionIndex);
            missionCostText.text = cost.ToString();
        }
    }

    private void Update()
    {
        if (isCooldown)
        {
            currentCoolTime -= Time.deltaTime;

            if (coolTimeImg != null)
            {
                coolTimeImg.fillAmount = currentCoolTime / maxCoolTime;
            }

            if (currentCoolTime <= 0f)
            {
                isCooldown = false;
                if (coolTimeImg != null) coolTimeImg.fillAmount = 0f;
                if (missionButton != null) missionButton.interactable = true;
            }
        }
    }

    private void TrySpawnMission()
    {
        if (isCooldown || MissionManager.instance == null) return;

        bool isSuccess = MissionManager.instance.SpawnMissionEnemy(missionIndex);

        if (isSuccess)
        {
            isCooldown = true;

            maxCoolTime = MissionManager.instance.GetMissionCooldown(missionIndex);
            currentCoolTime = maxCoolTime;

            if (coolTimeImg != null) coolTimeImg.fillAmount = 1f;
            if (missionButton != null) missionButton.interactable = false;
        }
    }
}
