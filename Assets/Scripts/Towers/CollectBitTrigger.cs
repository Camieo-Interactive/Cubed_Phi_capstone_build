using UnityEngine;

public class CollectBitTrigger : MonoBehaviour, ITriggerEffect
{
    public bool OnParticleTriggered(int particleCount) {
        GameManager.RaiseBitChange(particleCount);
        return true;
    }
}