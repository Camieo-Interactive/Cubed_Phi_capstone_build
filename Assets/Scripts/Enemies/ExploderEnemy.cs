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
    private bool _isExploding = false;

    /// <summary>
    /// Updates enemy behavior each frame, checking for tower proximity.
    /// </summary>
    private void Update()
    {
        if (_isExploding)
            return;

        bool nearTower = Physics2D.OverlapCircle(
            transform.position,
            attackStats.AttackRange,
            attackStats.AttackMask
        );

        if (nearTower && !_hasTriggeredExplosion)
        {
            _hasTriggeredExplosion = true;
            exploderAnimator.Play("ExplosionStart", 0, 0f); // Make sure this state exists
        }

        if (!_hasTriggeredExplosion)
        {
            Move(); // Still allowed to move until explosion animation starts
        }
    }

    /// <summary>
    /// Called via animation event to execute explosion logic.
    /// </summary>
    public void TriggerExplosion()
    {
        if (_isExploding) return;

        _isExploding = true;
        SpawnExplosion(false);

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
        if (!_isExploding)
            SpawnExplosion(true);

        base.OnDeath();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackStats != null ? attackStats.AttackRange : 1f);
    }
}
