using System.Collections;
using UnityEngine;

using static CubedPhiUtils;

public class WormEnemy : BasicGunner
{
    //  ------------------ Public ------------------

    [Tooltip("Particle system used for visualizing the attack.")]
    public ParticleSystem attackParticles;

    //  ------------------ Private ------------------

    private bool _isAttacking = false;

    /// <summary>
    /// Triggers the worm's particle attack.
    /// </summary>
    public override void OnAttack()
    {
        if (_isAttacking) return;

        _isAttacking = true;
        attackParticles.Play();

        StartCoroutine(HandleParticleAttack());
        StartCoroutine(AttackCooldown());
    }

    /// <summary>
    /// Continuously deals damage while the particle attack is active.
    /// </summary>
    private IEnumerator HandleParticleAttack()
    {
        while (attackParticles.isPlaying)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(
                firePoint.position,
                Vector2.left,
                attackStats.AttackRange,
                attackStats.AttackMask
            );

            foreach (var hit in hits)
            {
                if (hit.collider == null || hit.collider.gameObject.layer != TOWER_LAYER) continue;

                if (!hit.collider.TryGetComponent<HealthComponent>(out var target)) continue;

                DamageValue damage = new()
                {
                    damage = -attackStats.attackDamage,
                    damageStatus = DamageStatus.NONE,
                    statusDuration = 5f
                };

                target.ChangeHealth(damage);
            }

            // Always yield to prevent infinite loop
            yield return new WaitForSeconds(attackStats.AttackDuration);
        }


        _isAttacking = false;
    }

    /// <summary>
    /// Waits for the attack cooldown before allowing the next attack.
    /// </summary>
    private new IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackStats.AttackCooldownDuration);
        _isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(firePoint.position, Vector2.left * attackStats.AttackRange);
    }

    protected override void OnDeath()
    {
        attackParticles.Stop();
        _isAttacking = false;
        base.OnDeath();
    }
}