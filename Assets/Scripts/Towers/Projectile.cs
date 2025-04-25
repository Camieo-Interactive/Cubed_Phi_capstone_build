using System.Collections;
using UnityEngine;

using static CubedPhiUtils;
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
        GameObject obj = PoolManager.Instance.GetObject(stats.spawnable, transform.position, transform.rotation);
        obj.GetComponent<Explosion>()?.Init(_damage, 2.0f, false, DamageStatus.STUN);
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

        // Set the appropriate layer based on who fired the projectile
        gameObject.layer = _isEnemy ? ENEMY_BULLET_LAYER : TOWER_BULLET_LAYER;

        // Force the collision setting for this specific instance
        if (!_isEnemy)
        {
            // For tower bullets - ignore tower layer
            Physics2D.IgnoreLayerCollision(TOWER_BULLET_LAYER, TOWER_LAYER, true);
        }
        else
        {
            // For enemy bullets - ignore enemy layer
            Physics2D.IgnoreLayerCollision(ENEMY_BULLET_LAYER, ENEMY_LAYER, true);
        }

        // Rotate the projectile to face the direction it's moving
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

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
    /// Called when the game object is first created
    /// </summary>
    private void Awake()
    {
        // Global setting - Set these at project level in Physics2D settings too
        Physics2D.IgnoreLayerCollision(TOWER_BULLET_LAYER, TOWER_LAYER, true);
        Physics2D.IgnoreLayerCollision(ENEMY_BULLET_LAYER, ENEMY_LAYER, true);
    }

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
        // Skip processing if inactive
        if (!_isActive) return;

        int layer = collision.gameObject.layer;
        
        // Emergency runtime ignore - this is a last resort to prevent the collision from being processed
        if ((!_isEnemy && layer == TOWER_LAYER) || (_isEnemy && layer == ENEMY_LAYER))
        {
            
            // Ignore this specific collision
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider, true);
            return;
        }

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
    /// Called when the object is enabled
    /// </summary>
    private void OnEnable()
    {     
        // Enforce the layer collision settings again
        Physics2D.IgnoreLayerCollision(TOWER_BULLET_LAYER, TOWER_LAYER, true);
        Physics2D.IgnoreLayerCollision(ENEMY_BULLET_LAYER, ENEMY_LAYER, true);
    }

    /// <summary>
    /// Handles returning the projectile to the object pool safely.
    /// </summary>
    private void CleanupAndReturn()
    {
        _isActive = false;
        _isEnemy = false;
        
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