/// <summary>
/// Interface for damageable entities.
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Changes the health of the entity.
    /// </summary>
    /// <param name="damageDelta">The amount of damage or healing to apply.</param>
    void ChangeHealth(int damageDelta);
}
