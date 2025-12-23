using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioEvent", menuName = "TecnoWitches/Audio/Audio Event")]
public class AudioEvent : ScriptableObject
{
    [Header("Audio Clip")]
    public AudioClip clip;

    [Header("Opciones")]
    public bool loop = false;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.5f, 2f)] public float pitch = 1f;

    [Header("Mixer Group")]
    public AudioChannel channel; // Música, SFX o UI
}

public enum AudioChannel
{
    Music,
    SFX,
    UI
}
