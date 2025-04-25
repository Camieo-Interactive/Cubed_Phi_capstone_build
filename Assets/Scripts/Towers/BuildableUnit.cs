using System;
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

    [Tooltip("The Audio source use for handling the audio for the buildable unit's sounds.")]
    public AudioSource audioSource; 

    [Tooltip("The animator used for handling the buildable unit's animations.")]
    public Animator buildableAnimator;

    [Tooltip("Main Object refrence for pooling")]
    public GameObject baseObject;

    /// <summary>
    /// Called when the unit is built.
    /// </summary>
    public virtual void OnBuild()
    {
        healthComponent.InitializeHealth(stats.health);
        Grid = GameManager.Instance.grid;
    }

    /// <summary>
    /// Called when the unit is destroyed.
    /// </summary>
    public virtual void OnBuildingDestroy()
    {
        if(stats.deathParticleSystem != null) PoolManager.Instance.GetObject(stats.deathParticleSystem, transform.position, Quaternion.identity);
        try {
        GameManager.Instance.buildingLocations.Remove(Grid.WorldToCell(transform.position));
        }
        catch {
            Debug.LogWarning($"No instance of GameManager in the scene!!");
        }
        try
        {
            PoolManager.Instance.ReturnObject(baseObject);
        }
        catch
        {
            // We are testing right now. soo don't worry about it. 
            Debug.LogWarning($"This instance of {baseObject.name} is not in the object pool!");
            Destroy(baseObject);
        }
    }

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

    public virtual int Sell() {
        int value = stats.sellValue;
        OnBuildingDestroy();
        return value;
    }

    //  ------------------ Protected ------------------

    protected bool CanAttack = true;

    protected Grid Grid;

    //  ------------------ Private --------------------


    private void OnEnable()
    {
        TickSystem.OnTickAction += Check;
        healthComponent.OnDeath += OnBuildingDestroy;
        OnBuild();
    }

    private void OnDisable()
    {
        healthComponent.OnDeath -= OnBuildingDestroy;
        TickSystem.OnTickAction -= Check;
    }
}
