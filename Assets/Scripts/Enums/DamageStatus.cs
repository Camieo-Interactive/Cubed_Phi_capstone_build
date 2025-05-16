using UnityEngine;

/// <summary>
/// Represents the various damage statuses that an entity can have.
/// </summary>
[Tooltip("Represents the various damage statuses that an entity can have.")]
public enum DamageStatus
{
    /// <summary>
    /// No damage status.
    /// </summary>
    [Tooltip("No damage status.")]
    NONE,

    /// <summary>
    /// The entity is stunned.
    /// </summary>
    [Tooltip("The entity is stunned.")]
    STUN,

    /// <summary>
    /// The entity is poisoned.
    /// </summary>
    [Tooltip("The entity is poisoned.")]
    POISON,

    /// <summary>
    /// The entity is on fire.
    /// </summary>
    [Tooltip("The entity is on fire.")]
    FIRE,

    /// <summary>
    /// The entity is slowed.
    /// </summary>
    [Tooltip("The entity is slowed.")]
    SLOW,
}