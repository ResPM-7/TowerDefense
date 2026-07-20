using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField] private RectTransform shopPanel;
    [SerializeField] private Button shopButton;

    [SerializeField] private Vector2 openPos;
    [SerializeField] private Vector2 closePos;

    [SerializeField] private float duration = 0.5f;

    private bool isOpen = false;

    private void Start()
    {
        shopPanel.anchoredPosition = closePos;

        if (shopButton != null)
        {
            shopButton.onClick.AddListener(ToggleShop);
        }
    }

    public void ToggleShop()
    {
        isOpen = !isOpen;

        shopPanel.DOKill();

        if(isOpen)
        {
            shopPanel.DOAnchorPos(openPos, duration).SetEase(Ease.OutSine);
        }
        else
        {
            shopPanel.DOAnchorPos(closePos, duration).SetEase(Ease.InSine);
        }
    }
}
