using UnityEngine;

/// <summary>
/// Represents a grenade unit that can be fired and causes an explosion on impact.
/// </summary>
public class Grenade : BuildableUnit
{
    //  ------------------ Public ------------------

    /// <summary>
    /// Placeholder method for validation checks.
    /// </summary>
    public override void Check() { }

    /// <summary>
    /// Fires the grenade, instantiates an explosion, and destroys itself.
    /// </summary>
    public override void Fire()
    {
        // Instantiate the explosion
        GameObject explosion = PoolManager.Instance.GetObject(stats.projectile, transform.position, Quaternion.identity);
        
        // Initialize the explosion with damage and range
        explosion.GetComponent<Explosion>().Init(new() { damage = -stats.damage }, stats.range);
        
        // Trigger building destruction logic
        OnBuildingDestroy();

        // Destroy the grenade object after firing
        PoolManager.Instance.ReturnObject(gameObject);
    }

    //  ------------------ Private ------------------

    /// <summary>
    /// Initializes the grenade upon being built.
    /// </summary>
    private void Start() => OnBuild();
}
