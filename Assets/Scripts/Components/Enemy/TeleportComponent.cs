using UnityEngine;

/// <summary>
/// Instantly teleports an enemy vertically to another valid lane in 2D space.
/// </summary>
/// <remarks>
/// Moves the enemy up or down by a fixed stride, respecting cooldown and lane boundaries.
/// </remarks>
public class TeleportComponent : MovementComponent
{
    //  ------------------ Public ------------------

    [Tooltip("Stride by which to teleport vertically when switching lanes.")]
    public int LaneStride = 2;

    [Tooltip("Cooldown time before another teleport can occur.")]
    public float teleportCooldown = 3f;

    public bool CanTeleport => Time.time >= _nextTeleportTime;

    /// <summary>
    /// Attempts to teleport the enemy vertically by one lane stride, clamped within valid boundaries.
    /// </summary>
    public void TryTeleport()
    {
        if (!CanTeleport || EnemyManager.Instance == null)
            return;

        float currentY = transform.position.y;
        int strideDirection = Random.value < 0.5f ? 1 : -1;
        float stride = LaneStride * strideDirection;
        float targetY = currentY + stride;

        float minY = EnemyManager.Instance.LowestLane;
        float maxY = EnemyManager.Instance.highestLane;

        // Clamp to valid lanes
        targetY = Mathf.Clamp(targetY, minY, maxY);

        // Skip teleport if already at an edge lane
        if (Mathf.Approximately(currentY, targetY))
            return;

        transform.position = new Vector2(transform.position.x, targetY);
        _nextTeleportTime = Time.time + teleportCooldown;
    }


    //  ------------------ Private ------------------

    private float _nextTeleportTime = 0f;

    private void OnDisable()
    {
        _nextTeleportTime = 0f;
    }

    private void OnEnable()
    {
        _nextTeleportTime = 0f;
    }
}
