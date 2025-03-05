using UnityEngine;

/// <summary>
/// A basic enemy that follows the movement behavior defined in the EnemyBase class.
/// </summary>
public class BasicEnemy : EnemyBase
{
    //  ------------------ Private ------------------

    /// <summary>
    /// Updates the enemy's movement every frame.
    /// </summary>
    private void Update() => Move();
}
