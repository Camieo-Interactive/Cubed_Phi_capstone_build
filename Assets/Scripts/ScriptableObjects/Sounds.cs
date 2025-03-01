using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Abstract Scriptable Object for sound
/// </summary>
public abstract class Sound : ScriptableObject {
    [Header("Sound")]
    [Tooltip("What is the name of this sound?")]
    public string soundName; 
    [Tooltip("What is the actual name?")]
    public string soundNameActual;
    [Tooltip("Who made this sound?")]
    public string soundAuth;
    [Tooltip("Any other Information we may want to know about this sound?")]
    public string soundDesc;
    public int nameHash; 
    [Tooltip("Audio clip in question?")]
    public AudioClip clip;
    [Tooltip("Which audio mixer does this use?")]
    public AudioMixerGroup audioMixerGroup;
    [Range(0f,1f)]
    [Tooltip("Whats the volume?")]
    public float volume = 0.5f;
    [Range(0.1f,3f)]
    public float pitch = 1.0f;
    public override string ToString() => $"{soundNameActual} by {soundAuth}";
};