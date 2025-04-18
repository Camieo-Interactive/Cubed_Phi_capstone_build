using UnityEngine;

/// <summary>
/// A basic enemy that follows the movement behavior defined in the EnemyBase class.
/// </summary>
public abstract class LaneSwitcherEnemy : EnemyBase
{

    protected void LaneSwitch() {
        
    }
    //  ------------------ Private ------------------

    /// <summary>
    /// Updates the enemy's movement every frame.
    /// </summary>
    private void Update() => Move();
}
