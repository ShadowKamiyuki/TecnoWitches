using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    [Header("Mixer principal")]
    [SerializeField] private AudioMixer mainMixer;

    [Header("Mixer Groups")]
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioMixerGroup uiGroup;

    private Dictionary<AudioChannel, AudioMixerGroup> mixerGroups;

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
    }

    protected override void OnDestroyed()
    {
        ServiceLocator.Unregister<AudioManager>();

        Debug.Log("AudioManager destruido");
    }

    public void PlayAudio(AudioEvent audioEvent)
    {
        if (audioEvent == null || audioEvent.clip == null)
            return;

        GameObject obj = new GameObject("Audio: " + audioEvent.clip.name);
        obj.transform.parent = transform;
        AudioSource source = obj.AddComponent<AudioSource>();

        source.clip = audioEvent.clip;
        source.loop = audioEvent.loop;
        source.volume = audioEvent.volume;
        source.pitch = audioEvent.pitch;

        // asigna el grupo correcto
        source.outputAudioMixerGroup = mixerGroups[audioEvent.channel];

        source.Play();

        if (!audioEvent.loop)
            Destroy(obj, audioEvent.clip.length / audioEvent.pitch);
    }

    public void SetVolume(string exposedParam, float volumeLinear)
    {
        // convertir [0-1] a decibeles
        float volumeDb = Mathf.Log10(Mathf.Clamp(volumeLinear, 0.0001f, 1f)) * 20f;
        mainMixer.SetFloat(exposedParam, volumeDb);
    }
}
