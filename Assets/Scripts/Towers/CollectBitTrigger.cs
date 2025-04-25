using UnityEngine;

public class CollectBitTrigger : MonoBehaviour, ITriggerEffect
{
    public AudioSource audioSource;
    public bool OnParticleTriggered(int particleCount)
    {
        GameManager.RaiseBitChange(particleCount);
        audioSource.Play();
        return true;
    }
}