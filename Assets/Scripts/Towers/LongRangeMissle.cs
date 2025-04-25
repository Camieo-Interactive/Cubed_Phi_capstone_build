using System.Collections;
using UnityEngine;

/// <summary>
/// Launches a long-range missile from off-screen that explodes on contact.
/// </summary>
public class LongRangeMissile : BuildableUnit, IAttackable
{
    //  ------------------ Public ------------------
    public void OnAttack()
    {
        PoolManager.Instance.GetObject(stats.shootParticleSystem, transform.position, Quaternion.identity);
        for (int i = 0; i < 5; i++)
        {
            Vector2 baseTargetPosition = transform.position;
            Vector2 targetPosition = baseTargetPosition + Random.insideUnitCircle * 2.5f; 

            Vector2 baseSpawnPosition = GameManager.Instance.GetRandomOffScreenPoint(_spawnDistanceFromEdge);
            Vector2 spawnPosition = baseSpawnPosition + Random.insideUnitCircle * 3.5f;
            Vector2 direction = (targetPosition - spawnPosition).normalized;

            GameObject projectileObj = PoolManager.Instance.GetObject(stats.projectile, spawnPosition, Quaternion.identity);
            Projectile projectile = projectileObj.GetComponent<Projectile>();

            projectile.Init(
                new DamageValue()
                {
                    damage = -stats.damage / 5,
                    damageStatus = DamageStatus.NONE,
                    statusDuration = 0f
                },
                direction,
                false,
                stats.range,
                targetPosition
            );
        }
        StartCoroutine(_destroyDelay());
    }
    public override void OnBuild()
    {
        base.OnBuild();
        StartCoroutine(_attackDelay());
    }

    public override void Check() { }

    //  ------------------ Private ------------------

    private const float _spawnDistanceFromEdge = 10f;
    private IEnumerator _destroyDelay()
    {
        yield return new WaitForSeconds(1.0f);
        base.OnBuildingDestroy();
    }

    private IEnumerator _attackDelay()
    {
        yield return new WaitForSeconds(1.0f);
        OnAttack();
    }

    /// <summary>
    /// Launches a missile from off-screen toward this tower's position.
    /// </summary>

}
