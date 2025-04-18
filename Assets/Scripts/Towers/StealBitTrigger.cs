using UnityEngine;

public class StealBitTrigger : MonoBehaviour, ITriggerEffect
{
    public int numOfBitsCollected = 0; 
    // Return true.. 
    public bool OnParticleTriggered(int particleCount) {
        numOfBitsCollected += particleCount;
        return true;
    } 
}