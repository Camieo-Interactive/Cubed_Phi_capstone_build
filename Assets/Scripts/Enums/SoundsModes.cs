using System;
using UnityEngine;

/// <summary>
/// Represents the different sound modes available in the application.
/// </summary>
[Serializable]
public enum SoundModes
{
    /// <summary>
    /// Unknown or undefined sound mode.
    /// </summary>
    [Tooltip("Unknown or undefined sound mode.")]
    UNKNOWN,

    /// <summary>
    /// Sound mode for in-level gameplay.
    /// </summary>
    [Tooltip("Sound mode for in-level gameplay.")]
    IN_LEVEL,

    /// <summary>
    /// Sound mode for the main menu.
    /// </summary>
    [Tooltip("Sound mode for the main menu.")]
    IN_MAIN_MENU,

    /// <summary>
    /// Sound mode for the pause menu.
    /// </summary>
    [Tooltip("Sound mode for the pause menu.")]
    IN_PAUSE_MENU
}
