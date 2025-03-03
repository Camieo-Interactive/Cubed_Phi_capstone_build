using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Represents a turret unit that fires projectiles at detected targets.
/// </summary>
public class Turret : BuildableUnit
{
    //  ------------------ Public ------------------

    [Header("Turret Components")]
    [Tooltip("The point from which projectiles are fired.")]
    public Transform firePoint;

    [Tooltip("LineRenderer component used for visualizing the attack range.")]
    public LineRenderer lineRenderer;

    /// <summary>
    /// Fires a projectile from the turret.
    /// </summary>
    public override void Fire()
    {    
        // Instantiate the projectile
        GameObject projectileObject = PoolManager.Instance.GetObject(stats.projectile, firePoint.position, quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();

        // Initialize the projectile with damage values
        DamageValue damageValue = new() { damage = -stats.damage };
        projectile.Init(damageValue, Vector2.right);

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
        if (CanAttack) Fire();
    }

    //  ------------------ Private ------------------

    /// <summary>
    /// Initializes the turret upon building and sets the LineRenderer's range.
    /// </summary>
    private void Start()
    {
        OnBuild();
        lineRenderer.SetPosition(1, new Vector3(0.0f, stats.range, 0.0f));
    }
}
