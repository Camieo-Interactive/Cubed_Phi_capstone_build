using Unity.Mathematics;
using UnityEngine;

public class FatherSlime : BasicGunner
{
    //  ------------------ Private ------------------
    private bool _isDying = false;
    //  ------------------ Protected ------------------

    /// <summary>
    /// Handles the death of the slime enemy, including spawning additional slime enemies.
    /// </summary>
    protected override void OnDeath()
    {
        if (_isDying) return;
        base.OnDeath();

        // Randomly decide how many new enemies to spawn (1 to 3)
        int rand = UnityEngine.Random.Range(1, 4);
        for (int i = 0; i < rand; i++)
        {
            // Spawn a new instance of the enemy at the current position
            EnemyManager.Instance.SpawnEnemyInstance(stats.spawnable, transform.position);
        }
    }

    public override void OnAttack()
    {
        _canAttack = false;

        // Spawn projectile using object pool
        PoolManager.Instance.GetObject(attackStats.spawnablePrefab, firePoint.position, quaternion.identity);
        if(attackStats.attackParticleSystem != null) PoolManager.Instance.GetObject(attackStats.attackParticleSystem, transform.position, Quaternion.identity);

        // Start cooldown
        StartCoroutine(AttackCooldown());
    }
}