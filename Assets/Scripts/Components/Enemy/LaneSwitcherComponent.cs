using UnityEngine;

/// <summary>
/// Provides direction changes for vertical lane switching based on enemy state.
/// </summary>
public class LaneSwitcherComponent : MovementComponent
{
    //  ------------------ Public ------------------

    [Tooltip("Stride by which to shift vertically when switching lanes.")]
    public int LaneStride = 2;

    [Tooltip("Cooldown time before another lane switch can occur.")]
    public float switchCooldown = 3f;

    public bool IsSwitchingLane => _isSwitching;

    /// <summary>
    /// Attempts to initiate a lane switch and returns a modified direction if valid.
    /// </summary>
    public Vector2 GetLaneSwitchDirection(Vector2 baseDirection)
    {
        if (_isSwitching || Time.time < _nextSwitchTime || EnemyManager.Instance == null)
            return baseDirection;

        float currentY = transform.position.y;
        int strideDirection = Random.value < 0.5f ? 1 : -1;
        float stride = LaneStride * strideDirection;
        float targetY = currentY + stride;

        float minY = EnemyManager.Instance.LowestLane;
        float maxY = EnemyManager.Instance.highestLane;

        targetY = Mathf.Clamp(targetY, minY, maxY);

        if (Mathf.Approximately(currentY, targetY)) return baseDirection;

        _isSwitching = true;
        _startingY = currentY;
        _targetDeltaY = targetY - currentY;
        _nextSwitchTime = Time.time + switchCooldown;

        return new Vector2(baseDirection.x, _targetDeltaY);
    }

    /// <summary>
    /// Resets the move direction once the lane switch distance is covered.
    /// </summary>
    public Vector2 MaybeResetDirection(Vector2 currentDirection)
    {
        if (!_isSwitching) return currentDirection;

        float currentY = transform.position.y;
        float traveledY = Mathf.Abs(currentY - _startingY);

        if (traveledY >= Mathf.Abs(_targetDeltaY))
        {
            _isSwitching = false;
            return new Vector2(currentDirection.x, 0f);
        }

        return currentDirection;
    }

    //  ------------------ Private ------------------

    private bool _isSwitching = false;
    private float _nextSwitchTime = 0f;
    private float _startingY = 0f;
    private float _targetDeltaY = 0f;

    private void OnDisable()
    {
        _isSwitching = false;
        _nextSwitchTime = 0f;
        _startingY = 0f;
        _targetDeltaY = 0f;
    }

    private void OnEnable()
    {
        _isSwitching = false;
        _nextSwitchTime = 0f;
        _startingY = 0f;
        _targetDeltaY = 0f;
    }
}