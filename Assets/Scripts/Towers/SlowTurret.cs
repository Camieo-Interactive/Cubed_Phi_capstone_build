using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Represents a slow turret that applies a slowing effect to enemies within range.
/// </summary>
public class SlowTurret : BuildableUnit, IAttackable
{
    //  ------------------ Public ------------------

    [Header("Turret Components")]
    [Tooltip("The point from which the slow effect is applied.")]
    public Transform firePoint;

    [Tooltip("LineRenderer component used to visualize the attack range.")]
    public LineRenderer lineRenderer;

    /// <summary>
    /// Fires the turret, applying a cooldown before it can attack again.
    /// </summary>
    public void OnAttack() => StartCoroutine(Cooldown(stats.fireRate));

    /// <summary>
    /// Checks for targets within the turret's range and applies a slowing effect if possible.
    /// </summary>
    public override void Check()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, Vector2.right, stats.range, stats.detectionMask);
        
        if (hit.collider == null)
        {
            lineRenderer.enabled = false;
            return;
        }

        // Target detected, enable the line renderer
        lineRenderer.enabled = true;

        // Convert the hit position to local space and update the line renderer
        Vector3 localHitPosition = firePoint.InverseTransformPoint(hit.point);
        lineRenderer.SetPosition(1, localHitPosition);

        // Apply slow effect if the turret can attack
        if (CanAttack)
        {
            HealthComponent healthComponent = hit.collider.gameObject.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.ChangeHealth(new() 
                { 
                    damage = -stats.damage, 
                    damageStatus = stats.status, 
                    statusDuration = 1f 
                });
                OnAttack();
            }
        }
    }

    public override void OnBuild()
    {
        base.OnBuild();
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, firePoint.right * stats.range);
    }
}
