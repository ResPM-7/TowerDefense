using UnityEngine;

public class HealthUI : MonoBehaviour
{
    PlayerHealth playerHealth;

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateUI;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= UpdateUI;
    }

    private void UpdateUI(int now, int max)
    {
        Debug.Log($"{now} / {max}");   
    }
}
