using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Abstract ScriptableObject representing a sound asset.
/// </summary>
/// <remarks>
/// Contains metadata and audio settings used to define a sound asset.
/// </remarks>
public abstract class Sound : ScriptableObject
{
    //  ------------------ Public ------------------
    
    [Header("Sound Details")]
    [Tooltip("Display name for this sound.")]
    public string soundName;

    [Tooltip("Actual name of the sound file.")]
    public string soundNameActual;

    [Tooltip("Name of the creator/author of this sound.")]
    public string soundAuth;

    [Tooltip("Additional description or notes about this sound.")]
    public string soundDesc;

    [Tooltip("Hash of the sound name for quick lookup.")]
    public int nameHash;

    [Header("Audio Settings")]
    [Tooltip("Audio clip for this sound.")]
    public AudioClip clip;

    [Tooltip("Audio mixer group to route this sound through.")]
    public AudioMixerGroup audioMixerGroup;

    [Range(0f, 1f)]
    [Tooltip("Volume level for this sound.")]
    public float volume = 0.5f;

    [Range(0.1f, 3f)]
    [Tooltip("Pitch level for this sound.")]
    public float pitch = 1.0f;

    /// <summary>
    /// Returns a string representation of the sound.
    /// </summary>
    /// <returns>A formatted string including the sound's actual name and author.</returns>
    public override string ToString() => $"{soundNameActual} by {soundAuth}";
}
