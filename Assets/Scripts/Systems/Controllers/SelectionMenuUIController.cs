using System.Collections;
using UnityEngine;

public class SelectionMenuUIController : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup; // referencia al objeto raíz de la pantalla
    private CharacterSelectState state;

    public void Initialize(CharacterSelectState state)
    {
        this.state = state;
    }

    // llamado por botones de cada personaje
    public void OnCharacterButtonPressed(CharacterData character)
    {
        state?.SetSelectedCharacter(character);
    }

    public void OnConfirmPressed()
    {
        state?.ConfirmSelection();
    }

    public void OnBackPressed()
    {
        state?.BackToMainMenu();
    }

    // Fade in de la UI
    public IEnumerator FadeIn(float duration)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    // Fade out de la UI
    public IEnumerator FadeOut(float duration)
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}

