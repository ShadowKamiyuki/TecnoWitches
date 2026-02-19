using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.5f;
    
    public async Task FadeInAsync()
    {
        await FadeAsync(0f, 1f);
    }

    public async Task FadeOutAsync()
    {
        await FadeAsync(1f, 0f);
    }

    private async Task FadeAsync(float from, float to)
    {
        float t = 0f;
        Color color = fadeImage.color;
        color.a = from;
        fadeImage.color = color;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            color.a =Mathf.Lerp(from, to, t / fadeDuration);
            fadeImage.color = color;
            await Task.Yield();
        }

        color.a = to;
        fadeImage.color = color;
    }
}
