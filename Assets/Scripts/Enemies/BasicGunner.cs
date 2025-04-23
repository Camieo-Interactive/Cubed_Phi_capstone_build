using System.Collections;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// A basic enemy that shoots at towers using projectiles.
/// </summary>
public class BasicGunner : EnemyBase, IAttackable
{
    [Header("Attack Settings")]
    [Tooltip("Stats defining the gunner's attack behavior.")]
    public EnemyAttackStats attackStats;

    [Tooltip("Point from which projectiles are fired.")]
    public Transform firePoint;

    /// <summary>
    /// Fires a projectile and starts the attack cooldown.
    /// </summary>
    public virtual void OnAttack()
    {
        _canAttack = false;

        // Spawn projectile using object pool
        GameObject projectileObj = PoolManager.Instance.GetObject(attackStats.spawnablePrefab, firePoint.position, quaternion.identity);
        Projectile projectile = projectileObj.GetComponent<Projectile>();


        DamageValue damage = new DamageValue
        {
            damage = -attackStats.attackDamage,
            damageStatus = DamageStatus.NONE,
            statusDuration = 0f
        };

        projectile.Init(damage, Vector2.left, true);


        // Start cooldown
        StartCoroutine(AttackCooldown());
    }

    /// <summary>
    /// Handles enemy scanning and attacking logic per frame.
    /// </summary>
    protected virtual void HandleCombatTick()
    {
        if (!_canAttack) return;

        RaycastHit2D hit = Physics2D.Raycast(
            firePoint.position,
            Vector2.left,
            attackStats.AttackRange,
            attackStats.AttackMask
        );

        if (hit.collider != null)
        {
            OnAttack();
        }

        Move(); // Optional: comment this if you want stationary firing
    }

    /// <summary>
    /// Handles attack cooldown timing.
    /// </summary>
    protected IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackStats.AttackCooldownDuration);
        _canAttack = true;
    }
    protected bool _canAttack = true;
    private void Update() => HandleCombatTick();

    private void OnDrawGizmosSelected()
    {
        if (firePoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(firePoint.position, Vector2.left * attackStats.AttackRange);
    }
}
