using Unity.Collections;
using UnityEngine;

/// <summary>
/// Base class for enemy behavior, including movement, health management, and death handling.
/// </summary>
public abstract class EnemyBase : MonoBehaviour
{
    //  ------------------ Public ------------------
    [Header("Enemy Base")]
    [ReadOnly]
    [Tooltip("The stats that define the enemy's attributes.")]
    public EnemyStats stats;

    [Tooltip("The health component of the enemy.")]
    public HealthComponent healthComponent;

    [Header("Enemy Movement")]
    [Tooltip("Direction of movement (normalized).")]
    public Vector2 moveDirection = Vector2.left;

    public MovementComponent MovementComponent;

    [Tooltip("The particle system that spawns upon enemy death.")]
    public GameObject bitsParticleSystem;

    [Tooltip("Callback invoked when the enemy dies.")]
    public System.Action<GameObject> OnDeathCallback;

    /// <summary>
    /// Initializes the enemy's health using the stats.
    /// </summary>
    public virtual void Init() => healthComponent.InitializeHealth(stats.health);

    //  ------------------ Protected ------------------

    /// <summary>
    /// Moves the enemy in the specified direction, adjusting speed based on status effects.
    /// </summary>
    protected virtual void Move()
    {
        // Handle Lane switches.. 
        if (_laneSwitcher != null) HandleLaneSwitch();
        if (_teleportComp != null) HandleTeleport();
        float speed = (healthComponent.currentStatus != DamageStatus.SLOW) ? stats.movementSpeed : (stats.movementSpeed * 0.25f);
        speed = (healthComponent.currentStatus != DamageStatus.STUN) ? speed : 0;
        transform.position += (Vector3)(moveDirection.normalized * speed * Time.deltaTime);
    }
    protected virtual bool movementConditional() => healthComponent.CurrentPercent < 0.5f;
    protected void HandleLaneSwitch() => moveDirection = (!_laneSwitcher.IsSwitchingLane && movementConditional()) ? _laneSwitcher.GetLaneSwitchDirection(moveDirection) : _laneSwitcher.MaybeResetDirection(moveDirection);
    protected void HandleTeleport()
    {
        if (movementConditional()) _teleportComp.TryTeleport();
    }

    /// <summary>
    /// Handles the death of the enemy, including spawning bits and notifying the enemy manager.
    /// </summary>
    protected virtual void OnDeath()
    {
        // Notify the enemy manager of the death
        OnDeathCallback?.Invoke(gameObject);
        if(stats.deathParticleSystem != null) PoolManager.Instance.GetObject(stats.deathParticleSystem, transform.position, Quaternion.identity);
        // Return the enemy object to the pool
        try
        {
            PoolManager.Instance.ReturnObject(gameObject);
        }
        catch
        {
            // We are testing right now. soo don't worry about it. 
            Debug.LogWarning("This instance is not in the object pool!");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Called when the enemy reaches the end of the level, triggering a game over.
    /// </summary>
    protected virtual void OnEndReached()
    {
        EnemyManager.Instance.GameOver();
        // TODO: Make a health system.. 
    }

    protected virtual void PostEnable()
    {
        healthComponent.OnDeath += OnDeath;
        _laneSwitcher = GetComponent<LaneSwitcherComponent>();
        _teleportComp = GetComponent<TeleportComponent>();
        Init();
    }
    protected virtual void PostDisable() => healthComponent.OnDeath -= OnDeath;

    //  ------------------ Private ------------------

    private LaneSwitcherComponent _laneSwitcher;
    private TeleportComponent _teleportComp;

    /// <summary>
    /// Detects when the enemy enters the "EndTrigger" area and triggers game over.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("EndTrigger")) return;
        OnEndReached();
    }

    /// <summary>
    /// Subscribes to the death event when the object is enabled.
    /// </summary>
    private void OnEnable() => PostEnable();

    /// <summary>
    /// Unsubscribes from the death event when the object is disabled.
    /// </summary>
    private void OnDisable() => PostDisable();

}
