using System.Collections;
using UnityEngine;

public class MainMenuStateController : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup; // referencia al objeto raíz del menu
    private MainMenuState state;

    // Inicializamos desde el estado
    public void Initialize(MainMenuState state)
    {
        this.state = state;
    }

    // Botón "Play"
    public void OnPlayPressed()
    {
        state?.GoToCharacterSelect();
    }

    // Botón "Quit"
    public void OnQuitPressed()
    {
        Application.Quit();
    }

    // Opcional: Botón "Options"
    public void OnOptionsPressed()
    {
        //state?.ShowOptionsMenu(); // si implementás opciones en el estado
    }

    // Fade in/out de la pantalla
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
