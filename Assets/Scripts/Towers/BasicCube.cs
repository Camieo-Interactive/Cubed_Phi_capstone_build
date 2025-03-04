using UnityEngine;

public class BasicCube : BuildableUnit
{
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

    private Vector3 _targetPosition;
    private bool _hasTarget = false;

    /// <summary>
    /// Fires a projectile towards the aligned target.
    /// </summary>
    public override void Fire()
    {
        if (!_hasTarget || !IsAlignedWithTarget()) return;

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
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, stats.range, Vector2.zero, 0, stats.detectionMask);
        if (hit.collider == null)
        {
            _hasTarget = false;
            return;
        }

        _hasTarget = true;
        _targetPosition = hit.point;

        // Adjust firePoint slightly to the side of the target
        UpdateFirePointPosition();

        // Rotate towards target
        RotateToTarget();

        if (CanAttack) Fire();
    }

    /// <summary>
    /// Rotates the unit to align with the detected target.
    /// </summary>
    private void RotateToTarget()
    {
        if (!_hasTarget) return;

        Vector3 direction = (_targetPosition - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), rotationSpeed * Time.deltaTime);

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
    /// Adjusts the firePoint to be slightly offset next to the enemy.
    /// </summary>
    private void UpdateFirePointPosition()
    {
        if (equipPoint == null) return;

        Vector3 direction = (_targetPosition - transform.position).normalized;
        firePoint.position = equipPoint.position + direction * 0.65f; // Adjusting slight offset
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
}
