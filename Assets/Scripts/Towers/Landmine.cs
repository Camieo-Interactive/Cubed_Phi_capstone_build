using UnityEngine;

using static CubedPhiUtils;
/// <summary>
/// Represents a grenade unit that can be fired and causes an explosion on impact.
/// </summary>
public class Landmine : Grenade
{
    //  ------------------ Public ------------------
    /// <summary>
    /// Placeholder method for validation checks.
    /// </summary>

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == ENEMY_LAYER) OnAttack();
    }
}
