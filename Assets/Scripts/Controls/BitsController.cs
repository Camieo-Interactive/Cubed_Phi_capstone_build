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

