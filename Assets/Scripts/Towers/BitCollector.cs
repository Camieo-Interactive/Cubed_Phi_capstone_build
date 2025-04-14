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

    /// <summary>
    /// Initializes the BitGenerator when built.
    /// </summary>
    private void Start()
    {
        OnBuild();
        BitCollectorTrigg.AudioSrc = AudioSrc; 
        BitCollectorTrigg.BitCollectorAnimatior = buildableAnimator;
        BitsController.AddTrigger(ParticleSystemField.gameObject);
    }
    
}