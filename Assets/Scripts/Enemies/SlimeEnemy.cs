using UnityEngine;

/// <summary>
/// A slime enemy that spawns additional enemies upon death.
/// </summary>
public class SlimeEnemy : BasicEnemy
{
    //  ------------------ Private ------------------
    private bool _isDying = false;
    //  ------------------ Protected ------------------

    /// <summary>
    /// Handles the death of the slime enemy, including spawning additional slime enemies.
    /// </summary>
    protected override void OnDeath()
    {
        if(_isDying) return;
        base.OnDeath();
        
        // Randomly decide how many new enemies to spawn (1 to 3)
        int rand = Random.Range(1, 4);
        for (int i = 0; i < rand; i++)
        {
            // Spawn a new instance of the enemy at the current position
            EnemyManager.Instance.SpawnEnemyInstance(stats.spawnable, transform.position);
        }
    }
}
