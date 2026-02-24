using UnityEngine;
using UnityEngine.UI;

public class LoadingUIController : MonoBehaviour
{
    [SerializeField] private Slider progressBar;

    private ISceneLoader _loader;

    private void Awake()
    {
        _loader = ServiceLocator.Get<ISceneLoader>();

        progressBar.minValue = 0f;
        progressBar.maxValue = 1f;
    }

    private void Update()
    {
        if (_loader == null)
            return;

        progressBar.value = _loader.Progress;
    }
}
