using UnityEngine;

/// <summary>
/// A basic enemy that follows movement logic from EnemyBase and attacks towers within range.
/// </summary>
public class BasicEnemy : EnemyBase, IAttackable
{
    //  ------------------ Public ------------------

    [Header("Attack Stats")]
    [Tooltip("Reference to the attack settings for this enemy.")]
    public EnemyAttackStats attackStats;

    //  ------------------ Private ------------------

    private float _attackTimer = 0f;
    private bool _isAttackingTower = false;

    /// <summary>
    /// Called when the enemy is in range of a tower and can attack.
    /// </summary>
    public void OnAttack()
    {
        _attackTimer += Time.deltaTime;

        if (_attackTimer >= attackStats.AttackDuration)
        {
            _attackTimer = 0f;

            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                moveDirection.normalized,
                attackStats.AttackRange,
                attackStats.AttackMask
            );

            if (hit.collider != null && hit.collider.TryGetComponent(out HealthComponent health))
            {
                health.ChangeHealth(new DamageValue { damage = -attackStats.attackDamage, damageStatus = DamageStatus.NONE, statusDuration = 0.0f });
            }
        }
    }

    /// <summary>
    /// Updates the enemy behavior every frame.
    /// </summary>
    private void Update()
    {
        _isAttackingTower = Physics2D.Raycast(
            transform.position,
            moveDirection.normalized,
            attackStats.AttackRange,
            attackStats.AttackMask
        );

        if (_isAttackingTower)
            OnAttack();
        else
            Move();
    }
}
