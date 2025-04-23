using UnityEngine;

/// <summary>
/// A slime enemy that explodes near towers and deals damage using a pooled explosion.
/// </summary>
public class ExploderEnemy : EnemyBase
{
    //  ------------------ Public ------------------

    [Header("Explosion Settings")]
    [Tooltip("Explosion prefab to spawn on death.")]
    public GameObject explosionPrefab;

    [Tooltip("Stats that define how the exploder attacks.")]
    public EnemyAttackStats attackStats;

    [Tooltip("Animator used to play explosion animation.")]
    public Animator exploderAnimator;

    //  ------------------ Private ------------------

    private bool _hasTriggeredExplosion = false;
    private bool _shouldExplodeOnDeath = false;
    private bool _isExploding = false;
    private bool _isDying = false;

    /// <summary>
    /// Updates enemy behavior each frame, checking for tower proximity.
    /// </summary>
    protected virtual void Check()
    {
        if (_isExploding)
            return;

        bool nearTower = Physics2D.OverlapCircle(
            transform.position,
            attackStats.AttackDetectionRange,
            attackStats.AttackMask
        );

        if (nearTower && !_hasTriggeredExplosion)
        {
            _hasTriggeredExplosion = true;
            _shouldExplodeOnDeath = true;
            exploderAnimator.Play("ExplosionStart", 0, 0f); // Make sure this state exists
        }
    }

    private void Update()
    {
        if (!_hasTriggeredExplosion) Move();
    }

    /// <summary>
    /// Called via animation event to execute explosion logic.
    /// </summary>
    public void TriggerExplosion()
    {
        if (_isExploding) return;

        _isExploding = true;
        _shouldExplodeOnDeath = false;
        SpawnExplosion(_shouldExplodeOnDeath);

        healthComponent.ChangeHealth(new DamageValue
        {
            damage = -9999,
            damageStatus = DamageStatus.NONE,
            statusDuration = 0.0f
        });
    }

    /// <summary>
    /// Spawns and configures the explosion effect.
    /// </summary>
    private void SpawnExplosion(bool isDefused)
    {
        Debug.Log("Spawning Explosion");
        GameObject fx = PoolManager.Instance.GetObject(explosionPrefab, transform.position, Quaternion.identity);

        if (fx.TryGetComponent(out Explosion explosion))
        {
            explosion.Init(
                new DamageValue
                {
                    damage = -attackStats.attackDamage,
                    damageStatus = DamageStatus.NONE,
                    statusDuration = 0.0f
                },
                attackStats.AttackRange,
                isEnemy: !isDefused
            );
        }
    }

    /// <summary>
    /// Ensures explosion still occurs if killed before triggering manually.
    /// </summary>
    protected override void OnDeath()
    {
        if (_isDying) return;

        _isDying = true;
        Debug.Log("Spawning OnDeath!");
        if (!_isExploding)
        {
            exploderAnimator.Play("ExplosionDefault", 0, 0f);
            SpawnExplosion(!_shouldExplodeOnDeath);
        }

        base.OnDeath();
    }

    public override void Init()
    {
        base.Init();
        exploderAnimator.Play("ExplosionDefault", 0, 0f);
        _hasTriggeredExplosion = false;
        _shouldExplodeOnDeath = false;
        _isExploding = false;
        _isDying = false;
    }

    /// <summary>
    /// Registers tick events and triggers on enable.
    /// </summary>
    protected override void PostEnable()
    {
        base.PostEnable();
        TickSystem.OnTickAction += Check;
    }

    /// <summary>
    /// Unregisters tick events and triggers on disable.
    /// </summary>
    protected override void PostDisable()
    {
        base.PostDisable();
        TickSystem.OnTickAction -= Check;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackStats != null ? attackStats.AttackRange : 1f);
    }
}
