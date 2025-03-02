using System;
using UnityEngine;

/// <summary>
/// Defines the various sound modes for different game contexts.
/// </summary>
[Serializable]
public enum SoundModes
{
    [Tooltip("Unknown or undefined sound mode.")]
    UNKNOWN,

    [Tooltip("Sound mode for in-level gameplay.")]
    IN_LEVEL,

    [Tooltip("Sound mode for the main menu.")]
    IN_MAIN_MENU,

    [Tooltip("Sound mode for the pause menu.")]
    IN_PAUSE_MENU
}
