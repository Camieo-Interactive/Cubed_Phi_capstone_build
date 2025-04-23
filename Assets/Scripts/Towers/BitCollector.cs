using UnityEngine;

public class BitCollector: BuildableUnit {
    public AudioSource AudioSrc;
    public BitCollectorTrigger BitCollectorTrigg;
    public ParticleSystemForceField ParticleSystemField; 

    public override void Check()
    {
        // Nothing implmented..
    }

    public override void OnBuildingDestroy()
    {
        BitsController.RemoveTrigger(ParticleSystemField.gameObject);
        base.OnBuildingDestroy();
    }

    public override void OnBuild()
    {
        base.OnBuild();
        BitCollectorTrigg.AudioSrc = AudioSrc; 
        BitCollectorTrigg.BitCollectorAnimatior = buildableAnimator;
        BitsController.AddTrigger(ParticleSystemField.gameObject);
    }
}