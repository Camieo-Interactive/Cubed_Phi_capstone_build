using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages background music playback throughout the game.
/// </summary>
/// <remarks>
/// Dedicated solely to handling BGM.
/// </remarks>
public class BGMManager : SingletonBase<BGMManager>
{
    //  ------------------ Public ------------------

    [Header("BGM Configuration")]
    [Tooltip("BGM sound group")]
    public SoundGroup BGM;

    [Tooltip("Audio mixer for BGM control")]
    public AudioMixer audioMixer;

    [Tooltip("Audio source used for BGM playback")]
    public AudioSource audioSource;

    [Tooltip("Text UI displaying current BGM track")]
    public TextMeshProUGUI MusicText;

    [Tooltip("Animation curve used for fade transitions")]
    public AnimationCurve curve;

    public static bool IsMute = false;

    /// <summary>
    /// Sets the master volume and updates the audio mixer.
    /// </summary>
    /// <param name="volume">Linear volume value between 0.0 and 1.0</param>
    public void SetMasterVolume(float volume)
    {
        float clamped = Mathf.Clamp01(volume);
        float db = Mathf.Log10(Mathf.Max(clamped, 0.00001f)) * 20f;

        audioMixer.SetFloat("Master", db);
        PlayerPrefs.SetFloat("MasterVolume", clamped);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Sets the BGM volume and updates the audio mixer.
    /// </summary>
    /// <param name="volume">Linear volume value between 0.0 and 1.0</param>
    public void SetBGMVolume(float volume)
    {
        float clamped = Mathf.Clamp01(volume);
        float db = Mathf.Log10(Mathf.Max(clamped, 0.00001f)) * 20f;

        audioMixer.SetFloat("BGM", db);
        PlayerPrefs.SetFloat("BGMVolume", clamped);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Sets the SFX volume and updates the audio mixer.
    /// </summary>
    /// <param name="volume">Linear volume value between 0.0 and 1.0</param>
    public void SetSFXVolume(float volume)
    {
        float clamped = Mathf.Clamp01(volume);
        float db = Mathf.Log10(Mathf.Max(clamped, 0.00001f)) * 20f;

        audioMixer.SetFloat("SFX", db);
        PlayerPrefs.SetFloat("SFXVolume", clamped);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Initializes BGM manager after Awake lifecycle.
    /// </summary>
    public override void PostAwake()
    {
        DontDestroyOnLoad(gameObject);
        LoadSoundDictionary(BGM);
    }

    /// <summary>
    /// Begins playing a random BGM with fade-in.
    /// </summary>
    public void PlayBGM()
    {
        audioSource.Stop();
        StartCoroutine(FadeInto(0.5f));
        StartCoroutine(WaitForNextSong(audioSource.clip));
    }

    /// <summary>
    /// Queries a random BGM sound.
    /// </summary>
    #nullable enable
    public Sound? QueryRandomBGM() =>
        BGM.Sounds[UnityEngine.Random.Range(0, BGM.Sounds.Length)];

    /// <summary>
    /// Applies audio settings from a Sound to an AudioSource.
    /// </summary>
    public static void SetAudioSettings(AudioSource source, Sound? sound) =>
        _SetSourceSettings(source, sound);

    //  ------------------ Private ------------------
    private readonly Dictionary<int, int> _bgmDictionary = new();

    private void InitializeMixerVolumes()
    {
        float masterVol = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        float bgmVol = PlayerPrefs.GetFloat("BGMVolume", 1.0f);

        float masterDb = Mathf.Log10(Mathf.Max(masterVol, 0.00001f)) * 20f;
        float bgmDb = Mathf.Log10(Mathf.Max(bgmVol, 0.00001f)) * 20f;

        audioMixer.SetFloat("BGM", bgmDb);
        audioMixer.SetFloat("Master", masterDb);
    }

    private void LoadSoundDictionary(SoundGroup soundGroup)
    {
        for (int i = 0; i < soundGroup.Sounds.Length; i++)
        {
            soundGroup.Sounds[i].nameHash = Animator.StringToHash(soundGroup.Sounds[i].soundName);
            _bgmDictionary[soundGroup.Sounds[i].nameHash] = i;
        }
    }

    private static void _SetSourceSettings(AudioSource source, Sound? sound)
    {
        if (sound == null) return;

        source.clip = sound.clip;
        source.mute = IsMute;
        source.outputAudioMixerGroup = sound.audioMixerGroup;
        source.volume = sound.volume;
        source.pitch = sound.pitch;
    }

    private IEnumerator WaitForNextSong(AudioClip clip)
    {
        while (audioSource.time < clip.length)
        {
            yield return null;
        }

        PlayBGM();
    }

    private IEnumerator FadeInto(float fadeTime)
    {
        float t = 0f;
        Sound? sound = QueryRandomBGM();

        if (sound == null)
        {
            yield break;
        }

        SetAudioSettings(audioSource, sound);
        if (MusicText != null) MusicText.SetText($"Now Playing: {sound}");

        audioSource.Play();

        float targetVolume = audioSource.volume;
        audioSource.volume = 0f;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float normalized = t / fadeTime;
            audioSource.volume = Mathf.Clamp(curve.Evaluate(normalized) * targetVolume, 0f, targetVolume);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
    private void Start()
    {
        InitializeMixerVolumes();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGM();
    }
}
