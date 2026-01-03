using UnityEngine;
using UnityEngine.UI;

public class LoadingUIController : MonoBehaviour, IUpdatable
{
    [SerializeField] private Slider progressBar;

    private SceneLoaderService loader;

    private void Awake()
    {
        loader = ServiceLocator.Get<SceneLoaderService>();
        progressBar.minValue = 0f;
        progressBar.maxValue = 1f;
    }

    public void Tick(float deltaTime)
    {
        if (loader == null)
            return;

        if (progressBar != null)
            progressBar.value = loader.Progress;
    }
}
