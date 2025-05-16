using UnityEngine;

/// <summary>
/// Represents the different types of towers available in the game.
/// </summary>
[System.Serializable]
public enum TowerTypes
{
    /// <summary>
    /// A tower that generates resources or energy.
    /// </summary>
    [Tooltip("A tower that generates resources or energy.")]
    GENERATOR,

    /// <summary>
    /// A tower designed to attack enemies with projectiles or other means.
    /// </summary>
    [Tooltip("A tower designed to attack enemies with projectiles or other means.")]
    TURRET,

    /// <summary>
    /// A cube-shaped tower with unique properties or abilities.
    /// </summary>
    [Tooltip("A cube-shaped tower with unique properties or abilities.")]
    CUBE,

    /// <summary>
    /// A tower that provides support or buffs to other towers or units.
    /// </summary>
    [Tooltip("A tower that provides support or buffs to other towers or units.")]
    SUPPORT,

    /// <summary>
    /// A tower that can only be used once and is consumed after use.
    /// </summary>
    [Tooltip("A tower that can only be used once and is consumed after use.")]
    SINGLE_USE,
}