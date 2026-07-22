using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;



public class UIPanelSlide : MonoBehaviour
{

    [SerializeField] private RectTransform togglePanel;
    [SerializeField] private Button toggleButton;

    [SerializeField] private Vector2 openPos;
    [SerializeField] private Vector2 closePos;

    [SerializeField] private float duration = 0.5f;

    private bool isOpen = false;

    private void Start()
    {
        togglePanel.anchoredPosition = closePos;

        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleShop);
        }
    }

    public void ToggleShop()
    {
        isOpen = !isOpen;

        togglePanel.DOKill();

        if(isOpen)
        {
            togglePanel.SetAsLastSibling();

            togglePanel.DOAnchorPos(openPos, duration).SetEase(Ease.OutSine);
        }
        else
        {
            togglePanel.DOAnchorPos(closePos, duration).SetEase(Ease.InSine);
        }
    }
}
