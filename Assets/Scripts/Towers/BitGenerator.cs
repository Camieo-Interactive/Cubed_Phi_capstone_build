using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Generates bits at regular intervals by firing a projectile.
/// </summary>
public class BitGenerator : BuildableUnit, IAttackable
{
    public AudioSource AudioSrc;
    /// <summary>
    /// Checks the time elapsed and triggers bit generation if the recharge time has passed.
    /// </summary>
    public override void Check()
    {
        timePassed += 0.1f;
        if (timePassed > stats.rechargeTime)
        {
            timePassed = 0;
            OnAttack();
            buildableAnimator.Play("GeneratedBits");
            AudioSrc.Play();
        }
    }

    /// <summary>
    /// Fires a projectile that generates bits and applies the defined damage.
    /// </summary>
    public void OnAttack()
    {
        PoolManager.Instance.GetObject(stats.projectile, transform.position, quaternion.identity)
            .GetComponent<BitsController>().StartBits(stats.damage);
    }

    //  ------------------ Private ------------------

    private float timePassed = 0;
}
