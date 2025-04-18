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
    public AudioSource AudioSrc;
    private float _range = 10f;

    /// <summary>
    /// Initializes and triggers the explosion.
    /// </summary>
    /// <param name="damage">The damage to apply to affected enemies.</param>
    /// <param name="range">The range of the explosion.</param>
    public void Init(DamageValue damage, float range, bool isEnemy = false, DamageStatus stats = DamageStatus.NONE)
    {
        _range = range;
        AudioSrc.Play();
        // Play explosion and shrapnel particle effects
        explosionParticleSystem.Play();
        shrapnelParticleSystem.Play();

        // Detect all colliders within the explosion radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);

        // Apply damage to valid targets
        foreach (Collider2D hit in hits)
        {
            int layer = hit.gameObject.layer;

            // Skip damage if the target is the same faction as the explosion
            if ((isEnemy && layer == 6) || (!isEnemy && layer == 7)) continue;

            HealthComponent health = hit.GetComponent<HealthComponent>();
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
        yield return new WaitForSeconds(explosionParticleSystem.main.duration + 1f);

        // Return the explosion object to the pool for reuse
        PoolManager.Instance.ReturnObject(gameObject);
    }

    /// <summary>
    /// Draws a gizmo in the editor to visualize the explosion range.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _range);
    }

    /// <summary>
    /// Handles cleanup when the particle system finishes playing.
    /// </summary>
    private void OnParticleSystemStopped()
    {
        PoolManager.Instance.ReturnObject(gameObject);
    }
}
