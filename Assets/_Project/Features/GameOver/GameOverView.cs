using UnityEngine;
using UnityEngine.UI;
using System;

public class GameOverView : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button backToMenuButton;

    public event Action OnBackToMenu;

    private void Awake()
    {
        backToMenuButton.onClick.AddListener(() => OnBackToMenu?.Invoke());
    }

    public void Show()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
