using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Represents a shotgun turret that fires three projectiles in a spread pattern.
/// </summary>
public class ShotgunTurret : BuildableUnit, IAttackable
{
    //  ------------------ Public ------------------

    [Header("Turret Components")]
    [Tooltip("The point from which the projectiles are fired.")]
    public Transform firePoint;

    [Header("Turret Visuals")]
    [Tooltip("LineRenderer to visualize the main turret firing.")]
    public LineRenderer lineRenderer;

    [Tooltip("Left aiming line renderer.")]
    public LineRenderer Left;

    [Tooltip("Right aiming line renderer.")]
    public LineRenderer Right;

    /// <summary>
    /// Checks if there are any targets within the turret's range and fires if applicable.
    /// </summary>
    public override void Check()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, Vector2.right, stats.range, stats.detectionMask);
        if (hit.collider == null) return;

        // If a target is detected and the turret can attack, fire.
        if (CanAttack) OnAttack();
    }

    /// <summary>
    /// Fires three projectiles in a spread pattern (-15°, 0°, +15°).
    /// </summary>
    public void OnAttack()
    {
        float[] angles = { -15f, 0f, 15f };

        foreach (float angle in angles)
        {
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;
            GameObject projectileObject = PoolManager.Instance.GetObject(stats.projectile, firePoint.position, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();

            DamageValue damageValue = new() { damage = -stats.damage };
            projectile.Init(damageValue, direction);
        }

        // Start the cooldown before the turret can fire again
        StartCoroutine(Cooldown(stats.fireRate));
    }

    public override void OnBuild()
    {
        base.OnBuild();
        lineRenderer.SetPosition(1, new Vector3(0.0f, stats.range, 0.0f));
        Left.SetPosition(1, new Vector3(0.0f, stats.range, 0.0f));
        Right.SetPosition(1, new Vector3(0.0f, stats.range, 0.0f));
    }
}
