using UnityEngine;
using System.Threading.Tasks;

public class TransitionService : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float duration = 0.5f;

    public async Task FadeIn()
    {
        await Fade(0f, 1f);
    }

    public async Task FadeOut()
    {
        await Fade(1f, 0f);
    }

    private async Task Fade(float from, float to)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, time / duration);
            await Task.Yield();
        }

        canvasGroup.alpha = to;
    }
}