using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller for handling the bits particle system behavior.
/// </summary>
/// <remarks>
/// This script allows you to control the number of bits emitted and provides functions to start the particle system,
/// process trigger events, and set the trigger object for the particle system's trigger module.
/// It also handles returning the GameObject to the pool when the particle system stops.
/// </remarks>
public class BitsController : MonoBehaviour
{
    [Header("Particle System Settings")]
    [Tooltip("Reference to the ParticleSystem that emits bits.")]
    public ParticleSystem bitsParticleSystem;
    
    private List<ParticleSystem.Particle> _particles = new();

    /// <summary>
    /// Starts the bits particle system by emitting the specified number of bits.
    /// This method acts as the emit trigger.
    /// </summary>
    /// <param name="bitsCount">The number of bits to emit.</param>
    public void StartBits(int bitsCount, GameObject trigger) {
        bitsParticleSystem.Emit(bitsCount);
        SetTriggerObject(trigger);
    }   
    
    /// <summary>
    /// Called when the particle system stops playing.
    /// Returns the GameObject to the object pool.
    /// </summary>
    private void OnParticleSystemStopped() => PoolManager.Instance.ReturnObject(gameObject);

    /// <summary>
    /// Processes trigger events for particles that enter the trigger collider.
    /// Kills the particles and increments the BitsCollected counter in the GameManager.
    /// </summary>
    private void OnParticleTrigger()
    {
        int triggeredParticles = bitsParticleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);
        
        for (int i = 0; i < triggeredParticles; i++) {
            ParticleSystem.Particle p = _particles[i];
            p.remainingLifetime = 0;
            GameManager.Instance.BitsCollected++;
            _particles[i] = p;
        }

        bitsParticleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);
    }
    
    /// <summary>
    /// Sets the trigger object for the particle system.
    /// This function assigns the collider on the provided GameObject as the trigger for particle events.
    /// </summary>
    /// <param name="triggerObject">The GameObject that contains the Collider to be used as trigger.</param>
    public void SetTriggerObject(GameObject triggerObject)
    {
        if (triggerObject == null)
        {
            Debug.LogWarning("Trigger object is null.");
            return;
        }
        
        Collider2D triggerCollider = triggerObject.GetComponent<Collider2D>();
        if (triggerCollider == null)
        {
            Debug.LogWarning("No Collider component found on the trigger object.");
            return;
        }
        
        var triggerModule = bitsParticleSystem.trigger;
        triggerModule.enabled = true;
        triggerModule.SetCollider(0, triggerCollider);
    }
}
