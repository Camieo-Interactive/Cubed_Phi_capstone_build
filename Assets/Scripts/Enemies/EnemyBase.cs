using Unity.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [ReadOnly] public EnemyStats stats;
    public HealthComponent healthComponent;
    [Header("Enemy Movement")]
    [Tooltip("Direction of movement (normalized).")]
    public Vector2 moveDirection = Vector2.left;
    public GameObject bitsParticleSystem;
    public System.Action OnDeathCallback;
    public virtual void Init() => healthComponent.InitializeHealth(stats.health);
    protected virtual void Move()
    {
        float speed = (healthComponent.currentStatus != DamageStatus.SLOW) ? stats.movementSpeed : (stats.movementSpeed * 0.25f);
        transform.position += (Vector3)(moveDirection.normalized * speed * Time.deltaTime);
    }
    protected virtual void OnDeath()
    {
        // Enemy Manager
        OnDeathCallback?.Invoke();
        PoolManager.Instance.GetObject(bitsParticleSystem, transform.position, Quaternion.identity)
            .GetComponent<BitsController>().StartBits(25, GameManager.Instance.mouseTrigger);
        PoolManager.Instance.ReturnObject(gameObject);
    }

    protected virtual void OnEndReached()
    {
        Debug.Log("Enemy Hit the end");
    }

    //  ------------------ Private --------------------

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("EndTrigger")) return;
        OnEndReached();
    }

    private void OnEnable() =>
        healthComponent.onDeath += OnDeath;

    private void OnDisable() =>
        TickSystem.OnTickAction -= OnDeath;
}