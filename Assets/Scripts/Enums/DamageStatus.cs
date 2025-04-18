using UnityEngine;

/// <summary>
/// Enum representing different damage statuses with tooltips for each status.
/// </summary>
public enum DamageStatus
{
    [Tooltip("No damage status.")]
    NONE, // Nothing
    [Tooltip("The entity is stunned.")]
    STUN, // Stun 
    [Tooltip("The entity is poisoned.")]
    POISON, // Poison
    [Tooltip("The entity is on fire.")]
    FIRE, // On Fire
    [Tooltip("The entity is Slowed")]
    SLOW, // Slowed
}