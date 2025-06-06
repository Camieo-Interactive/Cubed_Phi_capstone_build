using UnityEngine;

/// <summary>
/// A buildable unit that behaves like a turret, aiming and firing projectiles at targets within range.
/// </summary>
public class AutoCube : BuildableUnit, IAttackable
{
    //  ------------------ Public ------------------

    [Header("Turret Components")]
    [Tooltip("The point from which projectiles are fired.")]
    public Transform firePoint;

    [Tooltip("Equip point offset for turret aiming.")]
    public Transform equipPoint;

    [Tooltip("LineRenderer component used for visualizing the attack range.")]
    public LineRenderer lineRenderer;

    [Header("Movement Settings")]
    [Tooltip("Rotation speed when aligning with the target.")]
    public float rotationSpeed = 5f;

    [Tooltip("Maximum angle difference allowed for firing.")]
    public float fireAngleThreshold = 5f;
    [Tooltip("Audio source for the attack sound effect.")]
    public AudioSource attackAudioSource;


    /// <summary>
    /// Fires a projectile towards the aligned target if aligned.
    /// </summary>
    public void OnAttack()
    {
        if (!IsAlignedWithTarget() || !IsInRange()) return;

        GameObject projectileObject = PoolManager.Instance.GetObject(stats.projectile, firePoint.position, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        Vector2 direction = (_targetPosition - firePoint.position).normalized;
        if (stats.shootParticleSystem != null) PoolManager.Instance.GetObject(stats.shootParticleSystem, firePoint.position, firePoint.rotation);
        DamageValue damageValue = new() { damage = -stats.damage };
        projectile.Init(damageValue, direction);
        if(attackAudioSource != null) attackAudioSource.Play();

        StartCoroutine(Cooldown(stats.fireRate));
    }

    /// <summary>
    /// Checks for targets within range and rotates the entire turret to aim at the target.
    /// </summary>
    public override void Check()
    {
        var vector = GameControls.CurrentTileSelected;
        if (vector == null) return;
        _targetPosition = (Vector3)vector;

        // Rotate the entire turret towards target
        RotateTurretToTarget();

        if (CanAttack) OnAttack();
    }

    //  ------------------ Private ------------------

    private Vector3 _targetPosition;
    /// <summary>
    /// Rotates the entire turret to align the firePoint with the target.
    /// </summary>
    private void RotateTurretToTarget()
    {
        if (firePoint == null) return;

        // Calculate the vector from firePoint to target
        Vector3 toTarget = _targetPosition - firePoint.position;

        // Skip rotation if target is too close (prevents fidgeting)
        float distanceToTarget = toTarget.magnitude;
        if (distanceToTarget < 1.25f) return; // Adjust this minimum distance as needed

        // Calculate the angle needed to align firePoint with target
        float targetAngle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;

        // Apply rotation to the entire turret
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), rotationSpeed * Time.deltaTime);

        // Update line renderer to show the actual firing trajectory
        if (lineRenderer != null)
        {
            Vector3 firingDirection = toTarget.normalized;
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firingDirection * stats.range);
        }
    }


    /// <summary>
    /// Ensures the turret does not fire unless the firePoint is properly aligned with the target.
    /// </summary>
    private bool IsAlignedWithTarget()
    {
        if (firePoint == null) return false;

        // Calculate the direction vectors
        Vector3 toTarget = _targetPosition - firePoint.position;
        Vector3 firingDirection = transform.right; // Assuming firePoint is aligned with local X-axis

        // Calculate the angle between the firing direction and direction to target
        float angle = Vector3.Angle(firingDirection, toTarget);

        return angle <= fireAngleThreshold;
    }

    /// <summary>
    /// Checks if the target is within the attack range.
    /// </summary>
    private bool IsInRange() => Vector2.Distance(transform.position, _targetPosition) <= stats.range;

    /// <summary>
    /// Draws a gizmo to visualize the turret's attack range in the editor.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (stats == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stats.range);
    }
}