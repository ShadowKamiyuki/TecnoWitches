using UnityEngine;
using System.Collections.Generic;

public class CharacterSelectView : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private List<CharacterButtonView> characterButtons;
    [SerializeField] private CharacterDatabase database;

    public IReadOnlyList<CharacterButtonView> Characters => characterButtons;
    public CharacterDatabase Database => database;

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
