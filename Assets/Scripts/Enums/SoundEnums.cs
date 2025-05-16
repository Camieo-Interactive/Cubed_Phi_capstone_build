using System;
using UnityEngine;

/// <summary>
/// Represents the types of sounds used in the application.
/// </summary>
[Serializable]
public enum SoundEnums
{
    /// <summary>
    /// Background music.
    /// </summary>
    [Tooltip("Background music.")]
    BGM,

    /// <summary>
    /// Sound effects.
    /// </summary>
    [Tooltip("Sound effects.")]
    SFX
}
