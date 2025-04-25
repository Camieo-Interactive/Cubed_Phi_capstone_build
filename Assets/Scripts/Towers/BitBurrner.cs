using System.Collections;
using UnityEngine;

/// <summary>
/// BitBurner tower — generates bits and applies fire damage to enemies in range.
/// </summary>
public class BitBurner : BuildableUnit, IAttackable
{
    [Header("Components")]
    public AudioSource AudioSrc;

    /// <summary>
    /// Checks if the tower can attack based on cooldown, then triggers attack.
    /// </summary>
    public override void Check()
    {
        if (_isAttacking) return;
        StartCoroutine(ActivateCooldown());
        OnAttack();
        
    }

    /// <summary>
    /// Executes the fire pulse: animates, finds enemies, applies damage.
    /// </summary>
    public void OnAttack()
    {

        AnimatorStateInfo stateInfo = buildableAnimator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("GeneratedBits") || stateInfo.normalizedTime >= 1f)
        {
            buildableAnimator.Play("GeneratedBits", 0, 0f);
        }


        // Detect enemies within range
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, stats.range, stats.detectionMask);
        foreach (Collider2D enemy in enemies)
        {
            HealthComponent health = enemy.GetComponent<HealthComponent>();
            if (health != null)
            {
                health.ChangeHealth(new DamageValue
                {
                    damage = -stats.damage,
                    damageStatus = DamageStatus.FIRE,
                    statusDuration = 5f
                });
            }
        }

        // Optional: play sound
        if (AudioSrc != null)
        {
            // AudioSrc.Play();
        }
    }

    private bool _isAttacking = false;

    /// <summary>
    /// Handles attack cooldown timing.
    /// </summary>
    private IEnumerator ActivateCooldown()
    {
        _isAttacking = true;
        yield return new WaitForSeconds(stats.rechargeTime);
        _isAttacking = false;
    }
    
    /// <summary>
    /// Visualizes attack range in the scene editor.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (stats == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stats.range);
    }
}
