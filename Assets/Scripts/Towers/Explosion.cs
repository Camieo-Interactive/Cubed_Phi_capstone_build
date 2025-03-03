using System.Collections;
using UnityEngine;

/// <summary>
/// Represents an explosion that deals damage to surrounding enemies and triggers particle effects.
/// </summary>
public class Explosion : MonoBehaviour
{
    //  ------------------ Public ------------------

    [Header("Explosion Settings")]
    [Tooltip("Main explosion particle effect.")]
    public ParticleSystem explosionParticleSystem;

    [Tooltip("Shrapnel particle effect.")]
    public ParticleSystem shrapnelParticleSystem;

    /// <summary>
    /// Initializes and triggers the explosion.
    /// </summary>
    /// <param name="damage">The damage to apply to affected enemies.</param>
    /// <param name="range">The range of the explosion.</param>
    public void Init(DamageValue damage, float range)
    {
        // Play explosion and shrapnel particle effects
        explosionParticleSystem.Play();
        shrapnelParticleSystem.Play();

        // Detect all colliders within the explosion radius
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, Vector2.zero);

        // Apply damage to any colliders that have a HealthComponent
        foreach (RaycastHit2D hit in hits)
        {
            HealthComponent health = hit.collider.GetComponent<HealthComponent>();
            if (health != null)
            {
                health.ChangeHealth(damage);
            }
        }

        // Start the destruction process after the effects have played
        StartCoroutine(DestroyAfterEffects());
    }

    //  ------------------ Private ------------------

    /// <summary>
    /// Waits for the particle effects to finish before deactivating the explosion object.
    /// </summary>
    private IEnumerator DestroyAfterEffects()
    {
        // Wait for the main explosion effect's duration
        yield return new WaitForSeconds(explosionParticleSystem.main.duration);
        
        // Return the explosion object to the pool for reuse
        PoolManager.Instance.ReturnObject(gameObject);
    }
}
