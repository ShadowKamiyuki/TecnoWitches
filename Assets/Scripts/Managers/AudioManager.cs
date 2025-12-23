using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    [Header("Main Mixer")]
    [SerializeField] private AudioMixer mainMixer;

    [Header("Mixer Groups")]
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioMixerGroup uiGroup;

    [Header("Prefabs")]
    [SerializeField] private GameObject sfxPrefab;

    [Header("Music Settings")]
    [SerializeField] private float musicCrossfadeTime = 2f;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f;

    private Dictionary<AudioChannel, AudioMixerGroup> mixerGroups;

    // music with crossfade
    private AudioSource musicSourceA;
    private AudioSource musicSourceB;
    private AudioSource activeMusicSource;
    private Coroutine musicFadeCoroutine;
    private Coroutine fadeCoroutine;

    protected override void OnAwaken()
    {
        Debug.Log("AudioManager inicializado");
        ServiceLocator.Register<AudioManager>(this);

        mixerGroups = new Dictionary<AudioChannel, AudioMixerGroup>
        {
            { AudioChannel.Music, musicGroup },
            { AudioChannel.SFX, sfxGroup },
            { AudioChannel.UI, uiGroup }
        };

        musicSourceA = CreateMusicSource("Music Source A");
        musicSourceB = CreateMusicSource("Music Source B");

        activeMusicSource = musicSourceA;
    }

    protected override void OnDestroyed()
    {
        ServiceLocator.Unregister<AudioManager>();

        Debug.Log("AudioManager destruido");
    }

    /// <summary>
    /// Reproduce cualquier AudioEvent. Música/Loop/UI/SFX se manejan automáticamente.
    /// </summary>
    public void PlayAudio(AudioEvent audioEvent, Vector3 position = default)
    {
        if (audioEvent == null || audioEvent.clip == null)
            return;

        switch (audioEvent.channel)
        {
            case AudioChannel.Music:
                PlayMusic(audioEvent);
                break;

            case AudioChannel.SFX:
                PlaySFX(audioEvent, position);
                break;

            case AudioChannel.UI:
                PlayUI(audioEvent);
                break;
        }
    }

    /// <summary>
    /// Cambia el volumen de un exposed parameter (0-1)
    /// </summary>
    public void SetVolume(string exposedParam, float volumeLinear)
    {
        float volumeDb = Mathf.Log10(Mathf.Clamp(volumeLinear, 0.0001f, 1f)) * 20f;
        mainMixer.SetFloat(exposedParam, volumeDb);
    }

    /// <summary>
    /// Baja suavemente la música a 0.
    /// </summary>
    public void FadeOutMusic(System.Action onComplete = null)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeMusicCoroutine(0f, fadeDuration, onComplete));
    }

    /// <summary>
    /// Sube suavemente la música al volumen normal.
    /// </summary>
    public void FadeInMusic(System.Action onComplete = null)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeMusicCoroutine(1f, fadeDuration, onComplete));
    }

    private IEnumerator FadeMusicCoroutine(float targetVolume, float duration, System.Action onComplete)
    {
        float t = 0f;

        // Determinar la fuente activa
        AudioSource source = activeMusicSource;
        float startVolume = source.volume;

        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = t / duration;
            source.volume = Mathf.Lerp(startVolume, targetVolume, normalized);
            yield return null;
        }

        source.volume = targetVolume;
        onComplete?.Invoke();
    }

    private AudioSource CreateMusicSource(string name)
    {
        GameObject obj = new GameObject(name);
        obj.transform.parent = transform;

        AudioSource source = obj.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = true;
        source.volume = 0f;
        source.spatialBlend = 0f; // música siempre 2D
        source.outputAudioMixerGroup = musicGroup;

        return source;
    }

    private void PlayMusic(AudioEvent musicEvent)
    {
        if (activeMusicSource.clip == musicEvent.clip)
            return;

        AudioSource nextSource = activeMusicSource == musicSourceA ? musicSourceB : musicSourceA;
        nextSource.clip = musicEvent.clip;
        nextSource.pitch = musicEvent.pitch;
        nextSource.volume = 0f;
        nextSource.loop = true;
        nextSource.Play();

        if (musicFadeCoroutine != null)
            StopCoroutine(musicFadeCoroutine);

        musicFadeCoroutine = StartCoroutine(CrossfadeMusic(activeMusicSource, nextSource, musicCrossfadeTime));
        activeMusicSource = nextSource;
    }

    private IEnumerator CrossfadeMusic(AudioSource from, AudioSource to, float time)
    {
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            float normalized = t / time;

            from.volume = Mathf.Lerp(1f, 0f, normalized);
            to.volume = Mathf.Lerp(0f, 1f, normalized);

            yield return null;
        }

        from.Stop();
        from.clip = null;
    }

    private void PlaySFX(AudioEvent audioEvent, Vector3 position)
    {
        // use service locator
        GameObject obj = PoolManager.Instance.Spawn(sfxPrefab, position, Quaternion.identity);
        AudioSource source = obj.GetComponent<AudioSource>();

        source.clip = audioEvent.clip;
        source.volume = audioEvent.volume;
        source.pitch = audioEvent.pitch;
        source.loop = audioEvent.loop;
        source.outputAudioMixerGroup = mixerGroups[audioEvent.channel];

        source.Play();

        if (!audioEvent.loop)
            StartCoroutine(ReturnToPoolAfterPlay(obj, audioEvent.clip.length / audioEvent.pitch));
    }

    private IEnumerator ReturnToPoolAfterPlay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        var poolable = obj.GetComponent<PoolableObject>();
        poolable?.Despawn();
    }

    private void PlayUI(AudioEvent audioEvent)
    {
        GameObject obj = new GameObject("UI Audio: " + audioEvent.clip.name);
        obj.transform.parent = transform;

        AudioSource source = obj.AddComponent<AudioSource>();
        source.clip = audioEvent.clip;
        source.volume = audioEvent.volume;
        source.pitch = audioEvent.pitch;
        source.loop = audioEvent.loop;
        source.outputAudioMixerGroup = uiGroup;
        source.spatialBlend = 0f; // UI siempre 2D
        source.Play();

        if (!audioEvent.loop)
            Destroy(obj, audioEvent.clip.length / audioEvent.pitch);
    }
}
