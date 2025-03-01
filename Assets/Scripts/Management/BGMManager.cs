using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;



public class BGMManager : SingletonBase<BGMManager>
{
    [Tooltip("BGM Sound group")]
    public SoundGroup BGM;

    [Tooltip("SFX Sound group")]
    public SoundGroup SFX;

    [Tooltip("Misc Sounds that don't fit ether group!")]
    public SoundGroup Misc;

    [Tooltip("Mixer")]
    public AudioMixer audioMixer;

    [Tooltip("Source of BGM and MISC audio")]
    public AudioSource audioSource;

    [Tooltip("Audio State for sound manager")]
    public SoundModes audioState;
    [Tooltip("Music Text in player UI")]
    public TextMeshProUGUI MusicText;
    [Tooltip("Animation curve for fading")]
    public AnimationCurve curve;
    public static bool isMute = false;

    [HideInInspector]
    public Dictionary<int, int> values = new();
    private void Start()
    {
        // var stats = Player.currentStats;

        // // Convert to decibels using logarithmic scaling
        // float masterDb = Mathf.Log10(Mathf.Max(stats.MasterVolume, 0.00001f)) * 20;
        // float sfxDb = Mathf.Log10(Mathf.Max(stats.SFXVolume, 0.00001f)) * 20;
        // float bgmDb = Mathf.Log10(Mathf.Max(stats.BGMVolume, 0.00001f)) * 20;

        // // Apply to the audio mixer
        // audioMixer.SetFloat("Master", masterDb);
        // audioMixer.SetFloat("SFX", sfxDb);
        // audioMixer.SetFloat("BGM", bgmDb);
        // Debug.Log(stats);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void LoadSoundDictionary(SoundGroup s)
    {
        for (int i = 0; i < s.Sounds.Length; i++)
        {
            s.Sounds[i].nameHash = Animator.StringToHash(s.Sounds[i].soundName);
            values[s.Sounds[i].nameHash] = i;
        }
    }

#nullable enable
    public Sound? Query(int ID, SoundEnums sound) => sound switch
    {
        SoundEnums.SFX => values.TryGetValue(ID, out int index) ? SFX.Sounds[index] : null,
        SoundEnums.BGM => values.TryGetValue(ID, out int index) ? BGM.Sounds[index] : null,
        _ => throw new NotImplementedException(),
    };

    // TODO: 
    // Add Bad luck protection.. 
    public Sound? QueryRandomBGM() => BGM.Sounds[UnityEngine.Random.Range(0, BGM.Sounds.Length)];
    public static void SetAudioSettings(AudioSource source, Sound? sound) => _SetSourceSettings(source, sound);
    private static void _SetSourceSettings(AudioSource source, Sound? sound)
    {
        if (sound == null) return;
        source.clip = sound.clip;
        source.mute = isMute;
        source.outputAudioMixerGroup = sound.audioMixerGroup;
        source.volume = sound.volume;
        source.pitch = sound.pitch;
    }

    // public void UpdateVolumeSettings(float masterVol, float sfxVol, float bgmVol)
    // {
    //     var stats = Player.currentStats;

    //     stats.MasterVolume = masterVol;
    //     stats.SFXVolume = sfxVol;
    //     stats.BGMVolume = bgmVol;

    //     // Convert to decibels using logarithmic scaling
    //     float masterDb = Mathf.Log10(Mathf.Max(masterVol, 0.00001f)) * 20;
    //     float sfxDb = Mathf.Log10(Mathf.Max(sfxVol, 0.00001f)) * 20;
    //     float bgmDb = Mathf.Log10(Mathf.Max(bgmVol, 0.00001f)) * 20;

    //     // Apply to the audio mixer
    //     audioMixer.SetFloat("Master", masterDb);
    //     audioMixer.SetFloat("SFX", sfxDb);
    //     audioMixer.SetFloat("BGM", bgmDb);
    // }

    public void startBGM()
    {
        audioSource.Stop();

        switch (audioState)
        {
            case SoundModes.UNKNOWN:
                break;
            case SoundModes.IN_LEVEL:
                if (audioSource.isPlaying) return;
                StartCoroutine(FadeInto(0.5f));
                StartCoroutine(WaitForNextSong(audioSource.clip));
                break;
            case SoundModes.IN_MAIN_MENU:
                // TODO:
                // Main menu theme
                break;
            case SoundModes.IN_PAUSE_MENU:
                // TODO: 
                // Pause Menu theme
                break;
            default:
                break;
        }
    }

    // public void togglePause() {
    //     if (PauseMenu.isActive) audioSource.Pause();
    //     else audioSource.UnPause();
    // }

    private IEnumerator WaitForNextSong(AudioClip clip)
    {
        while (audioSource.time < clip.length) yield return null;
        startBGM();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        audioState = SoundModes.IN_LEVEL;
        startBGM();
    }

    IEnumerator FadeInto(float fadeTime)
    {
        float t = 0;
        Sound? sound = QueryRandomBGM();
        if (sound == null) yield return 0;
        SetAudioSettings(audioSource, sound);
        if (MusicText != null) MusicText.text = $"Currently Playing: {sound}";
        audioSource.Play();
        float prevVolume = audioSource.volume;
        audioSource.volume = 0;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float normalizedTime = t / fadeTime;
            audioSource.volume = Mathf.Clamp(curve.Evaluate(normalizedTime) * prevVolume, 0, prevVolume);
            yield return 0;
        }
        audioSource.volume = prevVolume;
    }

    public override void PostAwake()
    {
        // Just initalize the BGM Manager.. 
        DontDestroyOnLoad(gameObject);
        LoadSoundDictionary(BGM);
        LoadSoundDictionary(SFX);
    }
};