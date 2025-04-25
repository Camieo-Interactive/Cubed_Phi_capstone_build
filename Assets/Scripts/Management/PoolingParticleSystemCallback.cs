using UnityEngine;

/// <summary>
/// Callback handler for returning particle systems to the object pool when they stop playing.
/// </summary>
public class PoolingParticleSystemCallback : MonoBehaviour
{

    /// <summary>
    /// Called when the particle system stops, returning the object to the pool.
    /// </summary>
    public void OnParticleSystemStopped() => ReturnToPool();

    public void ReturnToPool() => PoolManager.Instance.ReturnObject(gameObject);
}