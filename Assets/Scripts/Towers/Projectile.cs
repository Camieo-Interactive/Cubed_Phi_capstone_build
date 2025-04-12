using System.Collections;
using UnityEngine;

/// <summary>
/// Handles projectile behavior, including movement, collisions, and object pooling.
/// </summary>
public class Projectile : MonoBehaviour
{
    //  ------------------ Public ------------------

    [Header("Projectile Data")]
    [Tooltip("Stats defining projectile properties.")]
    public ProjectileStats stats;

    [Tooltip("Rigidbody for movement.")]
    public new Rigidbody2D rigidbody2D;

    [Tooltip("Trail renderer for visual effects.")]
    public TrailRenderer trailRenderer;


    /// <summary>
    /// Spawns any additional effects and returns the projectile to the object pool.
    /// </summary>
    public void CreateSpawnable()
    {
        Instantiate(stats.spawnable, transform.position, Quaternion.Inverse(transform.rotation));
        CleanupAndReturn();
    }


    /// <summary>
    /// Initializes the projectile when retrieved from the object pool.
    /// </summary>
    public void Init(DamageValue damage, Vector2 direction, bool isEnemy = false)
    {
        _damage = damage;
        _dir = direction;
        _isEnemy = isEnemy;
        _isActive = true;

        // Ensure trail is reset when reusing the object
        ResetTrailRenderer();

        // Start the destroy timer
        _destroyCoroutine = StartCoroutine(WaitForDestroy());
    }


    //  ------------------ Private ------------------

    private bool _isActive = false;
    private DamageValue _damage;
    private Vector2 _dir;
    private bool _isEnemy = false;
    private Coroutine _destroyCoroutine;
    
    /// <summary>
    /// Resets the trail renderer to avoid visual artifacts when reusing the projectile.
    /// </summary>
    private void ResetTrailRenderer()
    {
        if (trailRenderer != null)
        {
            trailRenderer.Clear();
        }
    }

    /// <summary>
    /// Updates the projectile's velocity based on its direction and speed.
    /// </summary>
    private void Update()
    {
        if (!_isActive) return;
        rigidbody2D.linearVelocity = _dir * stats.projectileSpeed;
    }


    /// <summary>
    /// Handles the collision with other game objects and applies damage or spawn effects.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        int layer = collision.gameObject.layer;
        if ((_isEnemy && layer == 6) || (!_isEnemy && layer == 7)) return;

        HealthComponent healthComponent = collision.gameObject.GetComponent<HealthComponent>();
        if (healthComponent != null)
        {
            healthComponent.ChangeHealth(_damage);
        }

        if (stats.spawnOnHit)
        {
            CreateSpawnable();
            return;
        }

        CleanupAndReturn();
    }

    /// <summary>
    /// Handles returning the projectile to the object pool safely.
    /// </summary>
    private void CleanupAndReturn()
    {
        _isActive = false;

        // Stop the destroy timer if it's still running
        if (_destroyCoroutine != null)
        {
            StopCoroutine(_destroyCoroutine);
            _destroyCoroutine = null;
        }

        ResetTrailRenderer();
        PoolManager.Instance.ReturnObject(gameObject);
    }

    /// <summary>
    /// Waits for a specified time before automatically returning the projectile to the pool.
    /// </summary>
    private IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(stats.bulletLifeTime);
        CleanupAndReturn();
    }
}
