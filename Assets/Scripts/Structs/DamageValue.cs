using System;
using UnityEngine;

/// <summary>
/// A struct to hold data from damage calculations.
/// </summary>
/// <remarks>
/// Stores information about damage amount, critical tier, status effects, and their duration.
/// </remarks>
[Serializable]
public struct DamageValue
{
    /// <summary>
    /// The amount of damage inflicted.
    /// </summary>
    [Tooltip("The amount of damage inflicted.")]
    public int damage;

    /// <summary>
    /// The critical tier of the damage, if applicable.
    /// </summary>
    [Tooltip("The critical tier of the damage, if applicable.")]
    public int critTier;

    /// <summary>
    /// The status effect associated with the damage.
    /// </summary>
    [Tooltip("The status effect associated with the damage.")]
    public DamageStatus damageStatus;

    /// <summary>
    /// Duration of the applied status effect.
    /// </summary>
    [Tooltip("Duration of the applied status effect.")]
    public float statusDuration;
}
