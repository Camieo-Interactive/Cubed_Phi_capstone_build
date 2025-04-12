using System.Collections;
using UnityEngine;

/// <summary>
/// A slime enemy that spawns additional enemies upon death.
/// </summary>
public class BitStealerEnemy : EnemyBase, IAttackable
{
    public EnemyAttackStats attackStats;
    public ParticleSystemForceField forceField;
    public ParticleSystem suckField;
    private Collider2D[] ColliderCheck = new Collider2D[32];
    private bool _isAttacking;
    public void OnAttack()
    {
        // Already attacking
        if (_isAttacking) return;
        _isAttacking = true;
        StartCoroutine(SuckAttack());
    }

    private IEnumerator SuckAttack()
    {
        forceField.gameObject.SetActive(true);
        suckField.gameObject.SetActive(true);
        yield return new WaitForSeconds(attackStats.AttackDuration);
        forceField.gameObject.SetActive(false);
        suckField.gameObject.SetActive(false);
        _isAttacking = false;
    }

    protected override void Move()
    {
        float speed = (healthComponent.currentStatus != DamageStatus.SLOW) ? stats.movementSpeed : (stats.movementSpeed * 0.25f);
        speed = _isAttacking ? 0 : speed;
        transform.position += (Vector3)(moveDirection.normalized * speed * Time.deltaTime);

        
    }

    protected virtual void Check() {
        ColliderCheck = Physics2D.OverlapCircleAll(transform.position, attackStats.AttackRange, attackStats.AttackMask);

        foreach (Collider2D collider in ColliderCheck)
        {
            if (collider.gameObject.tag == "BitGenerator") OnAttack();
        }
    }

    protected override void PostEnable()
    {
        base.PostEnable();
        TickSystem.OnTickAction += Check;
    }

    protected override void PostDisable()
    {
        base.PostDisable();
        TickSystem.OnTickAction -= Check;
    }

    //  ------------------ Private ------------------

    /// <summary>
    /// Updates the enemy's movement every frame.
    /// </summary>
    private void Update() => Move();
}
