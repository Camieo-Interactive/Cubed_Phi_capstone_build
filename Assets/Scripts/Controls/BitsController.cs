// using System.Collections.Generic;
// using UnityEngine;

// /// <summary>
// /// Controller for handling the bits particle system behavior.
// /// </summary>
// /// <remarks>
// /// This script controls the number of bits emitted, processes trigger events, and assigns the trigger object
// /// for the particle system. It also returns the GameObject to the pool when the particle system stops.
// /// </remarks>
// public class BitsController : MonoBehaviour
// {
//     //  ------------------ Public ------------------
//     [Header("Particle System Settings")]
//     [Tooltip("Reference to the ParticleSystem that emits bits.")]
//     public ParticleSystem bitsParticleSystem;
//     public static GameObject[] triggers
//     /// <summary>
//     /// Starts the bits particle system by emitting the specified number of bits.
//     /// Also sets the trigger object for the particle system.
//     /// </summary>
//     /// <param name="bitsCount">The number of bits to emit.</param>
//     /// <param name="trigger">The GameObject to be used as the trigger.</param>
//     public void StartBits(int bitsCount, GameObject trigger)
//     {
//         Debug.Log($"Bits emitted: {bitsCount}");
//         bitsParticleSystem.Emit(bitsCount);
//         SetTriggerObject(trigger);
//     }

//     /// <summary>
//     /// Sets the trigger object for the particle system.
//     /// This assigns the Collider2D on the provided GameObject as the trigger for particle events.
//     /// </summary>
//     /// <param name="triggerObject">The GameObject that contains the Collider2D to be used as trigger.</param>
//     public void SetTriggerObject(GameObject triggerObject)
//     {
//         Collider2D triggerCollider = triggerObject.GetComponent<Collider2D>();
//         var triggerModule = bitsParticleSystem.trigger;
//         triggerModule.enabled = true;
//         triggerModule.SetCollider(0, triggerCollider);
//     }

//     //  ------------------ Private ------------------
//     private List<ParticleSystem.Particle> _particles = new();

//     /// <summary>
//     /// Called when the particle system stops playing.
//     /// Returns the GameObject to the object pool.
//     /// </summary>
//     private void OnParticleSystemStopped() => PoolManager.Instance.ReturnObject(gameObject);

//     // /// <summary>
//     // /// Processes trigger events for particles entering the trigger collider.
//     // /// Kills the particles, increments the BitsCollected counter, and raises a bit change event.
//     // /// </summary>
//     private void OnParticleTrigger()
//     {
//         var triggerModule = bitsParticleSystem.trigger;
//         int triggeredParticles = bitsParticleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);
//         for (int i = 0; i < triggeredParticles; i++)
//         {
//             ParticleSystem.Particle p = _particles[i];
//             p.remainingLifetime = 0;
//             _particles[i] = p;
//         }

//         GameManager.RaiseBitChange(triggeredParticles);

//         bitsParticleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);
//     }
// }

using System.Collections.Generic;
using UnityEngine;

public class BitsController : MonoBehaviour
{
    [Header("Particle System Settings")]
    public ParticleSystem bitsParticleSystem;
    public static List<GameObject> triggers = new();
    public int numberOfTriggers = 0;

    public static void AddTrigger(GameObject trigger)
    {
        if (!triggers.Contains(trigger)) triggers.Add(trigger);
    }

    public static void RemoveTrigger(GameObject trigger)
    {
        int index = triggers.IndexOf(trigger);
        if (index < 0) return;

        int lastIndex = triggers.Count - 1;
        triggers[index] = triggers[lastIndex];
        triggers.RemoveAt(lastIndex);
    }
    public void StartBits(int bitsCount)
    {
        bitsParticleSystem.Emit(bitsCount);

        _triggerIndices.Clear();
        numberOfTriggers = 0;
        foreach (GameObject triggerObj in triggers) SetTriggerObject(triggerObj);
    }

    public void SetTriggerObject(GameObject triggerObject)
    {
        Collider2D triggerCollider = triggerObject.GetComponent<Collider2D>();
        var triggerModule = bitsParticleSystem.trigger;
        triggerModule.enabled = true;
        triggerModule.SetCollider(numberOfTriggers, triggerCollider);
        _triggerIndices[triggerCollider] = numberOfTriggers;
        numberOfTriggers++;
    }

    // Dictionary to map trigger colliders to their indices
    private Dictionary<Collider2D, int> _triggerIndices = new Dictionary<Collider2D, int>();
    private List<ParticleSystem.Particle> _particles = new List<ParticleSystem.Particle>();

    private void OnParticleSystemStopped() => PoolManager.Instance.ReturnObject(gameObject);

    private void OnParticleTrigger()
    {
        // Get all particles that entered any trigger
        int enteredParticles = bitsParticleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);

        if (enteredParticles <= 0) return;
        
        // Track which particles hit which triggers
        Dictionary<int, List<int>> particleIndicesByTrigger = new Dictionary<int, List<int>>();

        // Determine which trigger each particle hit
        for (int i = 0; i < enteredParticles; i++)
        {
            int triggerIndex = GetClosestTriggerIndex(_particles[i].position);
            if (!particleIndicesByTrigger.ContainsKey(triggerIndex)) particleIndicesByTrigger[triggerIndex] = new List<int>();
            particleIndicesByTrigger[triggerIndex].Add(i);
        }

        // Apply effects for each trigger
        foreach (var kvp in particleIndicesByTrigger)
        {
            int triggerIndex = kvp.Key;
            List<int> particleIndices = kvp.Value;
            GameObject triggerObj = GetTriggerObjectByIndex(triggerIndex);

            ITriggerEffect effect = triggerObj?.GetComponent<ITriggerEffect>();
            bool shouldDeleteParticles = effect == null;

            if (effect != null) shouldDeleteParticles = effect.OnParticleTriggered(particleIndices.Count);

            // Delete particles if needed
            if (!shouldDeleteParticles) continue;

            foreach (int index in particleIndices)
            {
                ParticleSystem.Particle p = _particles[index];
                p.remainingLifetime = 0;
                _particles[index] = p;
            }

        }

        // Update all particles
        bitsParticleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);

    }

    private int GetClosestTriggerIndex(Vector3 position)
    {
        int closestIndex = 0;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < triggers.Count; i++)
        {
            if (triggers[i] == null) continue;

            float distance = Vector3.Distance(position, triggers[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    private GameObject GetTriggerObjectByIndex(int index)
    {
        if (index >= 0 && index < triggers.Count) return triggers[index];
        return null;
    }
}

