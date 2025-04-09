using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller for handling the bits particle system behavior.
/// </summary>
/// <remarks>
/// This script controls the number of bits emitted, processes trigger events, and assigns the trigger object
/// for the particle system. It also returns the GameObject to the pool when the particle system stops.
/// </remarks>
public class BitsController : MonoBehaviour
{
    //  ------------------ Public ------------------
    [Header("Particle System Settings")]
    [Tooltip("Reference to the ParticleSystem that emits bits.")]
    public ParticleSystem bitsParticleSystem;
    
    /// <summary>
    /// Starts the bits particle system by emitting the specified number of bits.
    /// Also sets the trigger object for the particle system.
    /// </summary>
    /// <param name="bitsCount">The number of bits to emit.</param>
    /// <param name="trigger">The GameObject to be used as the trigger.</param>
    public void StartBits(int bitsCount, GameObject trigger)
    {
        bitsParticleSystem.Emit(bitsCount);
        SetTriggerObject(trigger);
    }

    /// <summary>
    /// Sets the trigger object for the particle system.
    /// This assigns the Collider2D on the provided GameObject as the trigger for particle events.
    /// </summary>
    /// <param name="triggerObject">The GameObject that contains the Collider2D to be used as trigger.</param>
    public void SetTriggerObject(GameObject triggerObject)
    {
        Collider2D triggerCollider = triggerObject.GetComponent<Collider2D>();
        var triggerModule = bitsParticleSystem.trigger;
        triggerModule.enabled = true;
        triggerModule.SetCollider(0, triggerCollider);
    }

    //  ------------------ Private ------------------
    private List<ParticleSystem.Particle> _particles = new();

    /// <summary>
    /// Called when the particle system stops playing.
    /// Returns the GameObject to the object pool.
    /// </summary>
    private void OnParticleSystemStopped() => PoolManager.Instance.ReturnObject(gameObject);

    /// <summary>
    /// Processes trigger events for particles entering the trigger collider.
    /// Kills the particles, increments the BitsCollected counter, and raises a bit change event.
    /// </summary>
    private void OnParticleTrigger()
    {
        int triggeredParticles = bitsParticleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);
        for (int i = 0; i < triggeredParticles; i++)
        {
            ParticleSystem.Particle p = _particles[i];
            p.remainingLifetime = 0;
            GameManager.Instance.BitsCollected++;
            _particles[i] = p;
        }

        GameManager.RaiseBitChange(triggeredParticles);
        
        bitsParticleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);
    }
}
