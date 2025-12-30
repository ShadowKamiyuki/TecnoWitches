using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup loadingCanvasGroup;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private GameObject[] menus;

    private ASyncLoader loader;

    private void OnEnable()
    {
        loader = ServiceLocator.Get<ASyncLoader>();

        if (loader == null)
            return;

        loader.OnProgress += UpdateProgress;
        loader.OnLoadingStateChanged += ToggleLoading;
    }

    private void OnDisable()
    {
        if (loader == null)
            return;

        loader.OnProgress -= UpdateProgress;
        loader.OnLoadingStateChanged -= ToggleLoading;
    }

    private void ToggleLoading(bool isLoading)
    {
        loadingCanvasGroup.blocksRaycasts = isLoading;

        if (isLoading)
        {
            loadingCanvasGroup.alpha = 1f;
            foreach (GameObject menu in menus)
                menu.SetActive(false);
        }
        else
        {
            loadingCanvasGroup.alpha = 0f;
            foreach (GameObject menu in menus)
                menu.SetActive(true);
        }
    }

    private void UpdateProgress(float value)
    {
        loadingSlider.value = value;
    }
}
