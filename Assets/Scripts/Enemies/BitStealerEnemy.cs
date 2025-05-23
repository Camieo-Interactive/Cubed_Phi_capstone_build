using System.Collections;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// A enemy that sucks in bits from bit collectors.
/// </summary>
/// <remarks>
/// Uses coroutine-based cooldowns and retains per-frame movement.
/// </remarks>
public class BitStealerEnemy : EnemyBase, IAttackable
{
    //  ------------------ Public ------------------

    [Header("Attack Settings")]
    [Tooltip("Stats related to this enemy's attack behavior.")]
    public EnemyAttackStats attackStats;

    [Header("VFX References")]
    [Tooltip("Force field used during the sucking attack.")]
    public ParticleSystemForceField forceField;

    [Tooltip("Trigger object enabling BitStealer behavior.")]
    public GameObject bitStealerTriggerObject;

    [Tooltip("Particle system for the sucking visual effect.")]
    public ParticleSystem suckField;

    [Tooltip("Steal trigger refrence to refund bits")]
    public StealBitTrigger stealBitTrigger;

    /// <summary>
    /// Triggers the enemy's attack sequence if not already attacking or cooling down.
    /// </summary>
    public void OnAttack()
    {
        if (_isAttacking || _isCoolingDown) return;

        if (!IsBitGeneratorInRange()) return;

        _isAttacking = true;
        
        if (gameObject.activeSelf) StartCoroutine(SuckAttack());
    }

    public override void Init()
    {
        base.Init();
        _isAttacking = false;
        _isCoolingDown = false;
        StopAllCoroutines();
    }

    //  ------------------ Protected ------------------

    /// <summary>
    /// Performs a range check and attacks if a BitGenerator is found.
    /// </summary>
    protected virtual void Check()
    {
        bool bitDetected = false;

        _colliderCheck = Physics2D.OverlapCircleAll(
            transform.position,
            attackStats.AttackRange,
            attackStats.AttackMask
        );

        foreach (Collider2D collider in _colliderCheck)
        {
            if (collider.CompareTag("BitGenerator"))
            {
                bitDetected = true;
                break;
            }
        }

        if (bitDetected) OnAttack();
    }

    /// <summary>
    /// Handles movement logic. Stops movement during an attack.
    /// </summary>
    protected override void Move()
    {
        if (_isAttacking ) return;
        base.Move();
    }

    /// <summary>
    /// Registers tick events and triggers on enable.
    /// </summary>
    protected override void PostEnable()
    {
        base.PostEnable();
        TickSystem.OnTickAction += Check;
        BitsController.AddTrigger(gameObject);
    }

    /// <summary>
    /// Unregisters tick events and triggers on disable.
    /// </summary>
    protected override void PostDisable()
    {
        base.PostDisable();
        TickSystem.OnTickAction -= Check;
    }

    /// <summary>
    /// Cleans up trigger registration on death.
    /// </summary>
    protected override void OnDeath()
    {
        _isAttacking = false;
        _isCoolingDown = false;
        forceField.gameObject.SetActive(false);
        suckField.gameObject.SetActive(false);
        _isAttacking = false;
        StopAllCoroutines();
        BitsController.RemoveTrigger(gameObject);
        PoolManager.Instance.GetObject(bitsParticleSystem, transform.position, quaternion.identity)
            .GetComponent<BitsController>().StartBits(stealBitTrigger.numOfBitsCollected / 4);
        base.OnDeath();
    }

    //  ------------------ Private ------------------

    private Collider2D[] _colliderCheck = new Collider2D[32];
    private bool _isAttacking;
    private bool _isCoolingDown;

    /// <summary>
    /// Performs the suck attack with visual effects and handles cooldown.
    /// </summary>
    private IEnumerator SuckAttack()
    {
        if (!IsBitGeneratorInRange())
        {
            _isAttacking = false;
            yield break;
        }

        forceField.gameObject.SetActive(true);
        suckField.gameObject.SetActive(true);

        yield return new WaitForSeconds(attackStats.AttackDuration);

        forceField.gameObject.SetActive(false);
        suckField.gameObject.SetActive(false);
        _isAttacking = false;

        StartCoroutine(StartCooldown());
    }

    /// <summary>
    /// Runs the cooldown after an attack to prevent spamming.
    /// </summary>
    private IEnumerator StartCooldown()
    {
        _isCoolingDown = true;
        yield return new WaitForSeconds(attackStats.AttackCooldownDuration);
        _isCoolingDown = false;
    }

    /// <summary>
    /// Updates enemy movement every frame.
    /// </summary>
    private void Update() => Move();


    /// <summary>
    /// Returns true if a BitGenerator is currently within attack range.
    /// </summary>
    private bool IsBitGeneratorInRange()
    {
        _colliderCheck = Physics2D.OverlapCircleAll(
            transform.position,
            attackStats.AttackRange,
            attackStats.AttackMask
        );

        foreach (Collider2D collider in _colliderCheck)
        {
            if (collider.CompareTag("BitGenerator"))
                return true;
        }

        return false;
    }
}
