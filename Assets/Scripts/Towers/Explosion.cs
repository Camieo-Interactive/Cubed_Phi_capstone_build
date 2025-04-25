using System.Collections;
using UnityEngine;

using static CubedPhiUtils;

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

        // Apply damage immediately
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D hit in hits)
        {
            int layer = hit.gameObject.layer;

            if ((isEnemy && layer == ENEMY_LAYER) || (!isEnemy && layer == TOWER_LAYER)) continue;

            HealthComponent health = hit.GetComponent<HealthComponent>();
            if (health != null)
            {
                health.ChangeHealth(damage);
            }
        }

        // Play visual effects
        explosionParticleSystem.Play();
        shrapnelParticleSystem.Play();

        // Start sequence: sound effect → wait → return to pool
        StartCoroutine(HandleEffectsWithAudio());
    }


    //  ------------------ Private ------------------

    private IEnumerator HandleEffectsWithAudio()
    {
        AudioSrc.Play();

        // Wait for the sound to finish plus an extra second
        yield return new WaitForSeconds(4f);

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
