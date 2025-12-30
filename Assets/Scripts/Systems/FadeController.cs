using System.Collections;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private float fadeDuration = 0.5f;

    private ASyncLoader loader;
    private Coroutine fadeRoutine;
    private bool hasInitialized;

    private void OnEnable()
    {
        loader = ServiceLocator.Get<ASyncLoader>();

        if (loader == null)
        {
            Debug.LogError("ASyncLoader no encontrado");
            return;
        }

        loader.OnFadeOutRequested += RequestFadeOut;
        loader.OnFadeInRequested += RequestFadeIn;
    }

    private void OnDisable()
    {
        if (loader == null)
            return;

        loader.OnFadeOutRequested -= RequestFadeOut;
        loader.OnFadeInRequested -= RequestFadeIn;
    }

    private void Start()
    {
        // Forzar inicio en negro y hacer fade in
        fadeCanvas.alpha = 1f;
        fadeCanvas.blocksRaycasts = false;

        hasInitialized = true;
        StartFade(FadeIn());
    }

    private void RequestFadeOut()
    {
        if (!hasInitialized)
            return;

        StartFade(FadeOut());
    }

    private void RequestFadeIn()
    {
        if (!hasInitialized)
            return;

        StartFade(FadeIn());
    }

    private void StartFade(IEnumerator routine)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(routine);
    }

    private IEnumerator FadeOut()
    {
        fadeCanvas.blocksRaycasts = true;
        float t = 0f;
        float startAlpha = fadeCanvas.alpha;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        fadeCanvas.alpha = 1f;
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        float startAlpha = fadeCanvas.alpha;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        fadeCanvas.alpha = 0f;
        fadeCanvas.blocksRaycasts = false;
    }
}
