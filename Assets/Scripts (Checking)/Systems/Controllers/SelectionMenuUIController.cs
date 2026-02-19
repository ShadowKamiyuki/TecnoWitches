using System.Collections;
using UnityEngine;

public class SelectionMenuUIController : MonoBehaviour
{
    [SerializeField] private CanvasGroup menuCanvasGroup; // referencia al objeto raíz de la pantalla
    [SerializeField] private CanvasGroup confirmScreenCanvasGroup;

    private CharacterSelectState state;

    public void Initialize(CharacterSelectState state)
    {
        this.state = state;
        confirmScreenCanvasGroup.alpha = 0f;
        confirmScreenCanvasGroup.blocksRaycasts = false;
        confirmScreenCanvasGroup.interactable = false;
    }

    // llamado por botones de cada personaje
    public void OnCharacterButtonPressed(CharacterData character)
    {
        //state?.SetSelectedCharacter(character);
    }

    public void OnConfirmPressed()
    {
        //if (!state.HasSelection())
        //    return;

        ShowConfirmScreen();
    }

    public void OnConfirmYesPressed()
    {
        //state?.ConfirmSelection();
    }

    public void OnCancelSelectionPressed()
    {
        HideConfirmScreen();
    }

    public void OnBackPressed()
    {
        //state?.BackToMainMenu();
    }

    private void ShowConfirmScreen()
    {
        confirmScreenCanvasGroup.alpha = 1f;
        confirmScreenCanvasGroup.blocksRaycasts = true;
        confirmScreenCanvasGroup.interactable = true;
    }

    private void HideConfirmScreen()
    {
        confirmScreenCanvasGroup.alpha = 0f;
        confirmScreenCanvasGroup.blocksRaycasts = false;
        confirmScreenCanvasGroup.interactable = false;
    }

    // Fade in de la UI
    public IEnumerator FadeIn(float duration)
    {
        menuCanvasGroup.alpha = 0f;
        menuCanvasGroup.interactable = false;
        menuCanvasGroup.blocksRaycasts = false;

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            menuCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }

        menuCanvasGroup.alpha = 1f;
        menuCanvasGroup.interactable = true;
        menuCanvasGroup.blocksRaycasts = true;
    }

    // Fade out de la UI
    public IEnumerator FadeOut(float duration)
    {
        menuCanvasGroup.interactable = false;
        menuCanvasGroup.blocksRaycasts = false;

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            menuCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t / duration);
            yield return null;
        }

        menuCanvasGroup.alpha = 0f;
    }
}

