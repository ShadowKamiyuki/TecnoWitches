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
    [SerializeField] private GameObject uiPrefab;

    [Header("Music Settings")]
    [SerializeField] private float musicCrossfadeTime = 2f;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f;

    private Dictionary<AudioChannel, AudioMixerGroup> mixerGroups;

    private AudioSource musicSourceA;
    private AudioSource musicSourceB;
    private AudioSource activeMusicSource;

    private Coroutine musicFadeCoroutine;

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
        activeMusicSource.volume = 1f; // volumen inicial
    }

    protected override void OnDestroyed()
    {
        ServiceLocator.Unregister<AudioManager>();
        Debug.Log("AudioManager destruido");
    }

    #region Public Methods

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

    public void SetVolume(string exposedParam, float volumeLinear)
    {
        float volumeDb = Mathf.Log10(Mathf.Clamp(volumeLinear, 0.0001f, 1f)) * 20f;
        mainMixer.SetFloat(exposedParam, volumeDb);
    }

    public void FadeOutMusic(System.Action onComplete = null)
    {
        StartFade(activeMusicSource, 0f, fadeDuration, onComplete);
    }

    public void FadeInMusic(System.Action onComplete = null)
    {
        StartFade(activeMusicSource, 1f, fadeDuration, onComplete);
    }

    #endregion

    #region Music Methods

    private void PlayMusic(AudioEvent musicEvent)
    {
        if (activeMusicSource.clip == musicEvent.clip)
            return;

        AudioSource nextSource = (activeMusicSource == musicSourceA) ? musicSourceB : musicSourceA;
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

    private IEnumerator CrossfadeMusic(AudioSource from, AudioSource to, float duration)
    {
        float t = 0f;
        float fromStartVolume = from.volume;
        float toTargetVolume = 1f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = t / duration;

            // Fade exponencial (más natural)
            from.volume = Mathf.Lerp(fromStartVolume, 0f, Mathf.Pow(normalized, 2));
            to.volume = Mathf.Lerp(0f, toTargetVolume, Mathf.Pow(normalized, 2));

            yield return null;
        }

        from.volume = 0f;
        to.volume = toTargetVolume;

        from.Stop();
        from.clip = null;
    }

    private void StartFade(AudioSource source, float targetVolume, float duration, System.Action onComplete)
    {
        if (musicFadeCoroutine != null)
            StopCoroutine(musicFadeCoroutine);

        musicFadeCoroutine = StartCoroutine(FadeCoroutine(source, targetVolume, duration, onComplete));
    }

    private IEnumerator FadeCoroutine(AudioSource source, float targetVolume, float duration, System.Action onComplete)
    {
        float startVolume = source.volume;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = t / duration;
            source.volume = Mathf.Lerp(startVolume, targetVolume, Mathf.Pow(normalized, 2));
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
        source.spatialBlend = 0f; // Música 2D
        source.outputAudioMixerGroup = musicGroup;

        return source;
    }

    #endregion

    #region SFX & UI Methods

    private void PlaySFX(AudioEvent audioEvent, Vector3 position)
    {
        // change the instance to service locator getter
        GameObject obj = PoolManager.Instance.Spawn(sfxPrefab, position, Quaternion.identity);
        AudioSource source = obj.GetComponent<AudioSource>();

        source.clip = audioEvent.clip;
        source.volume = audioEvent.volume;
        source.pitch = audioEvent.pitch;
        source.loop = audioEvent.loop;

        if (mixerGroups.TryGetValue(audioEvent.channel, out var group))
            source.outputAudioMixerGroup = group;

        source.Play();

        if (!audioEvent.loop)
            StartCoroutine(ReturnToPoolAfterPlay(obj, audioEvent.clip.length / audioEvent.pitch));
    }

    private void PlayUI(AudioEvent audioEvent)
    {
        GameObject obj = PoolManager.Instance.Spawn(uiPrefab, Vector3.zero, Quaternion.identity);
        obj.transform.parent = transform;

        AudioSource source = obj.GetComponent<AudioSource>();
        source.clip = audioEvent.clip;
        source.volume = audioEvent.volume;
        source.pitch = audioEvent.pitch;
        source.loop = audioEvent.loop;
        source.outputAudioMixerGroup = uiGroup;
        source.spatialBlend = 0f; // UI 2D

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

    #endregion
}
