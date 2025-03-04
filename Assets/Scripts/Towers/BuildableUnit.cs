using System.Collections;
using Unity.Collections;
using UnityEngine;

/// <summary>
/// Base class for all buildable units in the game.
/// Implements core behaviors such as damage handling, firing, and lifecycle events.
/// </summary>
public abstract class BuildableUnit : MonoBehaviour, IBuildable
{
    //  ------------------ Public ------------------

    [Tooltip("The stats that define the unit's attributes.")]
    [ReadOnly] public TowerStats stats;

    [Tooltip("The health component responsible for managing this unit's health.")]
    public HealthComponent healthComponent;

    [Tooltip("The animator used for handling the buildable unit's animations.")]
    public Animator buildableAnimator;

    /// <summary>
    /// Called when the unit is built.
    /// </summary>
    public virtual void OnBuild()
    {
        healthComponent.InitializeHealth(stats.health);
        Grid = GameManager.Instance.grid;
        GameManager.Instance.buildingLocations.Add(Grid.WorldToCell(transform.position), true);
    }

    /// <summary>
    /// Called when the unit is destroyed.
    /// </summary>
    public virtual void OnBuildingDestroy()
    {
        GameManager.Instance.buildingLocations.Remove(Grid.WorldToCell(transform.position));
        PoolManager.Instance.ReturnObject(gameObject);
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


    //  ------------------ Protected ------------------

    protected bool CanAttack = true;

    protected Grid Grid;

    //  ------------------ Private --------------------


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
