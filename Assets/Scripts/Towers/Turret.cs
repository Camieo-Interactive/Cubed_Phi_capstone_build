using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Represents a turret unit that fires projectiles at detected targets.
/// </summary>
public class Turret : BuildableUnit, IAttackable
{
    //  ------------------ Public ------------------

    [Header("Turret Components")]
    [Tooltip("The point from which projectiles are fired.")]
    public Transform firePoint;

    [Tooltip("LineRenderer component used for visualizing the attack range.")]
    public LineRenderer lineRenderer;
    [Tooltip("Audio source for the attack sound effect.")]
    public AudioSource attackAudioSource;

    /// <summary>
    /// Fires a projectile from the turret.
    /// </summary>
    public void OnAttack()
    {
        // Instantiate the projectile
        GameObject projectileObject = PoolManager.Instance.GetObject(stats.projectile, firePoint.position, quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        if (stats.shootParticleSystem != null) PoolManager.Instance.GetObject(stats.shootParticleSystem, firePoint.position, firePoint.rotation);
        // Initialize the projectile with damage values
        DamageValue damageValue = new() { damage = -stats.damage };
        projectile.Init(damageValue, Vector2.right);
        if(attackAudioSource != null) attackAudioSource.Play();
        // Start the cooldown before the turret can fire again
        StartCoroutine(Cooldown(stats.fireRate));
    }

    /// <summary>
    /// Checks for targets within the turret's range and fires if possible.
    /// </summary>
    public override void Check()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, Vector2.right, stats.range, stats.detectionMask);
        if (hit.collider == null) return;

        // If a target is detected and the turret can attack, fire.
        if (CanAttack) OnAttack();
    }

    public override void OnBuild()
    {
        base.OnBuild();
        lineRenderer.SetPosition(1, new Vector3(0.0f, stats.range, 0.0f));
    }
}
