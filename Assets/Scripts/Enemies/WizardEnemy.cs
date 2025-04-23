using System.Collections;
using UnityEngine;

/// <summary>
/// A wizard enemy that teleports shortly after taking damage.
/// </summary>
public class WizardEnemy : BasicGunner
{
    //  ------------------ Public ------------------

    [SerializeField]
    [Tooltip("Delay after being damaged before teleport can trigger.")]
    public float _teleportDelay = 1.5f;

    [SerializeField]
    [Tooltip("Particle system played when the wizard teleports.")]
    public ParticleSystem teleportEffect;

    //  ------------------ Protected ------------------

    protected bool _canTeleport = false;

    /// <summary>
    /// Signals to the base class that movement should be replaced with teleport.
    /// </summary>
    protected override bool movementConditional()
    {
        if (_canTeleport && !teleportEffect.isPlaying)
        {
            teleportEffect?.Play();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Called when the wizard takes damage.
    /// </summary>
    protected void OnDamaged(int currentHealth)
    {
        if (_teleportDelayCoroutine != null)
            StopCoroutine(_teleportDelayCoroutine);

        _teleportDelayCoroutine = StartCoroutine(TimeSinceDamaged());
    }

    protected override void PostEnable()
    {
        base.PostEnable();
        healthComponent.OnHealthChanged += OnDamaged;
    }

    protected override void PostDisable()
    {
        base.PostDisable();
        healthComponent.OnHealthChanged -= OnDamaged;
    }

    protected override void OnDeath()
    {
        _canTeleport = false;
        teleportEffect?.Stop();
        base.OnDeath();
    }

    //  ------------------ Private ------------------

    private Coroutine _teleportDelayCoroutine;

    /// <summary>
    /// Waits before enabling teleportation.
    /// </summary>
    private IEnumerator TimeSinceDamaged()
    {
        _canTeleport = true;
        yield return new WaitForSeconds(_teleportDelay);
        _canTeleport = false;
    }
}
