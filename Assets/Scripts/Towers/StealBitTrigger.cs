using UnityEngine;

public class StealBitTrigger : MonoBehaviour, ITriggerEffect
{
    // Return true.. 
    public bool OnParticleTriggered(int particleCount) => true;
}