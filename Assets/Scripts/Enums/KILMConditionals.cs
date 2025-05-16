using UnityEngine;

/// <summary>
/// Represents a set of conditional states or emotions that can be used
/// to define relationships or interactions between entities.
/// </summary>
public enum KILMConditionals
{
    /// <summary>
    /// Represents a state of affection or fondness.
    /// </summary>
    [Tooltip("Represents a state of affection or fondness.")]
    Affection,

    /// <summary>
    /// Represents a state of hatred or intense dislike.
    /// </summary>
    [Tooltip("Represents a state of hatred or intense dislike.")]
    Hatred,

    /// <summary>
    /// Represents a state of trust or confidence.
    /// </summary>
    [Tooltip("Represents a state of trust or confidence.")]
    Trust,

    /// <summary>
    /// Represents a state of fear or apprehension.
    /// </summary>
    [Tooltip("Represents a state of fear or apprehension.")]
    Fear,

    /// <summary>
    /// Represents a state of annoyance or irritation.
    /// </summary>
    [Tooltip("Represents a state of annoyance or irritation.")]
    Annoyance,

    /// <summary>
    /// Represents a state where something has been unlocked or made accessible.
    /// </summary>
    [Tooltip("Represents a state where something has been unlocked or made accessible.")]
    Unlocked,

    /// <summary>
    /// Represents a state where the player was blocked.
    /// </summary>
    [Tooltip("Represents a state where the player was blocked.")]
    Blocked,

}