using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] private List<AudioClip> musicClips;
    [SerializeField] private List<AudioClip> sfxClips;

    private Dictionary<string, AudioClip> musicDictionary;
    private Dictionary<string, AudioClip> sfxDictionary;

    protected override void OnAwaken()
    {
        Debug.Log("AudioManager inicializado");
        ServiceLocator.Register<AudioManager>(this);

        musicDictionary = new Dictionary<string, AudioClip>();

        foreach (AudioClip clip in musicClips)
        {
            musicDictionary[clip.name] = clip;
        }

        sfxDictionary = new Dictionary<string, AudioClip>();

        foreach (var clip in sfxClips)
        {
            sfxDictionary[clip.name] = clip;
        }
    }

    protected override void OnDestroyed()
    {
        ServiceLocator.Unregister<AudioManager>();

        Debug.Log("AudioManager destruido");
    }

    public void PlayMusic(string clipName, bool loop = true)
    {
        if (musicDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"{clipName} not found");
        }
    }

    public void PlaySFX(string clipName)
    {
        if (sfxDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"{clipName} not found");
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
