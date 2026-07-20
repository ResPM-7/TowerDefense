using TMPro;
using UnityEngine;

public class SpeedUpManager : MonoBehaviour
{
    [SerializeField] private bool speedUp;
    [SerializeField] private TextMeshProUGUI speedUpText;

    private void Start()
    {
        speedUpText.text = "1x";
    }

    public void ToggleSpeedUp()
    {
        speedUp = !speedUp;

        if(speedUp)
        {
            Time.timeScale = 2f;
            speedUpText.text = "2x";
        }
        else
        {
            Time.timeScale = 1f;
            speedUpText.text = "1x";
        }    
    }
}
