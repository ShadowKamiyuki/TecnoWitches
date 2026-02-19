using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LoadingView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider progressBar;

    [Header("Fade")]
    [SerializeField] private FadeController fadeController;

    private void Awake()
    {
        if (progressBar != null)
        {
            progressBar.minValue = 0f;
            progressBar.maxValue = 1f;
            progressBar.value = 0f;
        }
    }

    public void SetProgress(float value)
    {
        if (progressBar != null)
            progressBar.value = value;
    }

    public async Task FadeInAsync()
    {
        if (fadeController != null)
            await fadeController.FadeInAsync();
    }

    public async Task FadeOutAsync()
    {
        if (fadeController != null)
            await fadeController.FadeOutAsync();
    }
}
