using System;
using UnityEngine;

/// <summary>
/// Represents different statuses that can be applied through damage.
/// </summary>
[Serializable]
public enum DamageStatus
{
    [Tooltip("No status effect.")]
    NONE,

    [Tooltip("Slows the target.")]
    SLOW,

    [Tooltip("Applies poison damage over time.")]
    POISON,

    [Tooltip("Sets the target on fire, dealing continuous damage.")]
    FIRE
}
