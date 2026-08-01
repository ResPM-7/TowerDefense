using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SlidePanel
{
    public RectTransform togglePanel;
    public Button toggleButton;

    [HideInInspector]
    public bool isOpen = false;
}

public class UIPanelSlide : MonoBehaviour
{
    [SerializeField] private SlidePanel[] slidePanels;

    [SerializeField] private Vector2 openPos;
    [SerializeField] private Vector2 closePos;

    [SerializeField] private float duration = 0.5f;


    private void Start()
    {
        for (int i = 0; i < slidePanels.Length; i++)
        {
            int currentIndex = i;

            SlidePanel panelData = slidePanels[currentIndex];

            if(panelData.togglePanel != null)
            {
                panelData.togglePanel.anchoredPosition = closePos;
                panelData.toggleButton.onClick.AddListener(() => TogglePanel(currentIndex));
            }
        }
    }

    public void TogglePanel(int index)
    {
        SlidePanel panelData = slidePanels[index];

        panelData.isOpen = !panelData.isOpen;

        panelData.togglePanel.DOKill();

        if (panelData.isOpen)
        {
            panelData.togglePanel.SetAsLastSibling();
            panelData.togglePanel.DOAnchorPos(openPos, duration).SetEase(Ease.OutSine);
        }
        else
        {
            panelData.togglePanel.DOAnchorPos(closePos, duration).SetEase(Ease.InSine);
        }
    }
}
