using UnityEngine;

public class WaveIncomingHandler : MonoBehaviour
{
    //  ------------------ Public ------------------
    
    [Header ("Wave Incoming SFX Settings")]
    [Tooltip ("Audio Source for the wave incoming sound effect.")]
    public AudioSource waveIncomingAudioSource;

    [Tooltip ("Audio Clip for the wave incoming sound effect.")]
    public AudioClip waveIncomingAudioClip;
    [Tooltip ("Audio Source for the final wave incoming sound effect.")]
    public AudioClip FinalWaveIncomingAudioClip;

    [Tooltip ("Animator for the wave incoming animation.")]
    public Animator waveIncomingAnimator;

    public void showWaveIncoming(bool isFinalWave = false)
    {
        _isFinalWave = isFinalWave;
        waveIncomingAnimator.Play("WaveIncoming");
    }
    public void playWaveIncomingSFX()
    {
        if(_isFinalWave) waveIncomingAudioSource.PlayOneShot(FinalWaveIncomingAudioClip);
        else waveIncomingAudioSource.PlayOneShot(waveIncomingAudioClip);
    }

    // ------------------ Private ------------------
    private bool _isFinalWave = false;
}
