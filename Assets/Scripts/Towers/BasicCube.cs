using UnityEngine;

/// <summary>
/// A buildable unit that behaves like a turret, aiming and firing projectiles at targets within range.
/// </summary>
public class BasicCube : BuildableUnit
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

    /// <summary>
    /// Fires a projectile towards the aligned target if aligned.
    /// </summary>
    public override void Fire()
    {
        if (target == null || !IsAlignedWithTarget() || !IsInRange()) return;

        GameObject projectileObject = PoolManager.Instance.GetObject(stats.projectile, firePoint.position, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();

        Vector2 direction = (_targetPosition - firePoint.position).normalized;
        DamageValue damageValue = new() { damage = -stats.damage };
        projectile.Init(damageValue, direction);

        StartCoroutine(Cooldown(stats.fireRate));
    }

    /// <summary>
    /// Checks for targets within range and moves to align firePoint with the detected target.
    /// </summary>
    public override void Check()
    {
        // Find closest target within range
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, stats.range, stats.detectionMask);

        GameObject closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (Collider2D collider in colliders)
        {
            float distance = Vector2.Distance(transform.position, collider.transform.position);
            if (distance < closestDistance)
            {
                closestTarget = collider.gameObject;
                closestDistance = distance;
            }
        }

        target = closestTarget;

        if (target == null) return;

        _targetPosition = target.transform.position;

        // Rotate towards target
        RotateToTarget();

        if (CanAttack) Fire();
    }

    //  ------------------ Private ------------------

    private Vector3 _targetPosition;
    private GameObject target = null;

    /// <summary>
    /// Rotates the unit to align with the detected target.
    /// </summary>
    private void RotateToTarget()
    {
        if (target == null) return;

        Vector3 direction = (_targetPosition - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the entire turret
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), rotationSpeed * Time.deltaTime);

        // Update firepoint position based on the equipPoint's rotation
        UpdateFirePointPosition();

        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + direction * stats.range);
        }
    }

    /// <summary>
    /// Ensures the gun does not fire unless it is properly aligned with the target.
    /// </summary>
    private bool IsAlignedWithTarget()
    {
        Vector3 direction = (_targetPosition - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float currentAngle = transform.eulerAngles.z;

        return Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle)) <= fireAngleThreshold;
    }

    /// <summary>
    /// Checks if the target is within the attack range.
    /// </summary>
    private bool IsInRange() => Vector2.Distance(transform.position, _targetPosition) <= stats.range;
    

    /// <summary>
    /// Adjusts the firePoint to be positioned correctly relative to the equipPoint.
    /// </summary>
    private void UpdateFirePointPosition()
    {
        if (equipPoint == null) return;

        // Keep firePoint at the edge of the equipPoint in the direction it's facing
        firePoint.position = equipPoint.position + (Vector3)(equipPoint.right * 0.65f);
    }

    /// <summary>
    /// Initializes the turret upon building and sets the LineRenderer's range.
    /// </summary>
    private void Start()
    {
        OnBuild();
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + Vector3.right * stats.range);
        }
    }

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