using UnityEngine;

public class BitCollectorTrigger : MonoBehaviour, ITriggerEffect
{
    public AudioSource AudioSrc;
    public Animator BitCollectorAnimatior;
    public bool OnParticleTriggered(int particleCount)
    {
        GameManager.RaiseBitChange(particleCount);
        AnimatorStateInfo stateInfo = BitCollectorAnimatior.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("GeneratedBits") || stateInfo.normalizedTime >= 1f)
        {
            AudioSrc.Play();
            BitCollectorAnimatior.Play("GeneratedBits", 0, 0f);
        }
        return true;
    }
}