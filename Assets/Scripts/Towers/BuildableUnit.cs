using System.Collections;
using UnityEngine;

/// <summary>
/// Base class for all buildable units in the game.
/// Implements core behaviors such as damage handling, firing, and lifecycle events.
/// </summary>
public abstract class BuildableUnit : MonoBehaviour, IDamageable, IBuildable
{
    //  ------------------ Public ------------------

    /// <summary>
    /// Called when the unit is built.
    /// </summary>
    public abstract void OnBuild();

    /// <summary>
    /// Called when the unit is destroyed.
    /// </summary>
    public abstract void OnDestroy();

    /// <summary>
    /// Fires at the current target.
    /// </summary>
    public abstract void Fire();

    /// <summary>
    /// Handles the cooldown period between attacks.
    /// </summary>
    public abstract IEnumerator Cooldown();

    /// <summary>
    /// Applies damage to the unit.
    /// </summary>
    /// <param name="damageDelta">The amount of damage received.</param>
    public abstract void ChangeHealth(int damageDelta);
}
