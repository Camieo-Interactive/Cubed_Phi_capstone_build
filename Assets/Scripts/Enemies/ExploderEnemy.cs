using System.Collections;
using UnityEngine;

/// <summary>
/// A slime enemy that spawns additional enemies upon death.
/// </summary>
public class ExploderEnemy : EnemyBase
{
    //  ------------------ Protected ------------------

    /// <summary>
    /// Handles the death of the slime enemy, including spawning additional slime enemies.
    /// </summary>
    protected override void OnDeath()
    {
        base.OnDeath();

    }
    //  ------------------ Private ------------------

    /// <summary>
    /// Updates the enemy's movement every frame.
    /// </summary>
    private void Update() => Move();
}
