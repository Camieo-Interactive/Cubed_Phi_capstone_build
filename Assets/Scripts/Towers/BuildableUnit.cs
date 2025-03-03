using System.Collections;
using Unity.Collections;
using UnityEngine;

/// <summary>
/// Base class for all buildable units in the game.
/// Implements core behaviors such as damage handling, firing, and lifecycle events.
/// </summary>
public abstract class BuildableUnit : MonoBehaviour, IDamageable, IBuildable
{
    //  ------------------ Public ------------------

    [ReadOnly]
    [Tooltip("The stats that define the unit's attributes.")]
    public TowerStats stats;

    [Tooltip("The health component responsible for managing this unit's health.")]
    public HealthComponent healthComponent;

    [Tooltip("The animator used for handling the buildable unit's animations.")]
    public Animator buildableAnimator;

    /// <summary>
    /// Called when the unit is built.
    /// </summary>
    public virtual void OnBuild()
    {
        _grid = GameManager.Instance.grid;
        GameManager.Instance.buildingLocations.Add(_grid.WorldToCell(transform.position), true);
    }

    /// <summary>
    /// Called when the unit is destroyed.
    /// </summary>
    public virtual void OnBuildingDestroy()
    {
        GameManager.Instance.buildingLocations.Remove(_grid.WorldToCell(transform.position));
    }

    /// <summary>
    /// Fires at the current target.
    /// </summary>
    public abstract void Fire();

    /// <summary>
    /// Checks for conditions every tick.
    /// </summary>
    public abstract void Check();

    /// <summary>
    /// Handles the cooldown period between attacks.
    /// </summary>
    public virtual IEnumerator Cooldown(float time)
    {
        if (CanAttack) yield return null;
        CanAttack = false;
        yield return new WaitForSeconds(time);
        CanAttack = true;
    }

    /// <summary>
    /// Applies damage to the unit.
    /// </summary>
    /// <param name="damageDelta">The amount of damage received.</param>
    public virtual void ChangeHealth(int damageDelta)
    {
        DamageValue damage = new() { damage = damageDelta };
        healthComponent.ChangeHealth(damage);
    }

    //  ------------------ Protected ------------------

    protected bool CanAttack = true;

    //  ------------------ Private ------------------

    private Grid _grid;

    private void OnEnable()
    {
        TickSystem.OnTickAction += Check;
        healthComponent.onDeath += OnBuildingDestroy;
    }

    private void OnDisable()
    {
        healthComponent.onDeath -= OnBuildingDestroy;
        TickSystem.OnTickAction -= Check;
    }
}
