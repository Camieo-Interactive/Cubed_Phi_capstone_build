using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the background music (BGM) playback, including volume control, song fading, and random song selection.
/// </summary>
public class BGMManager : SingletonBase<BGMManager>
{
    //  ------------------ Public ------------------

    [Header("BGM Configuration")]
    [Tooltip("The sound group that contains all background music.")]
    public SoundGroup BGM;

    [Tooltip("The audio mixer for controlling volumes.")]
    public AudioMixer audioMixer;

    [Tooltip("The audio source responsible for playing the background music.")]
    public AudioSource audioSource;

    [Tooltip("Text UI element to display the currently playing music.")]
    public TextMeshProUGUI MusicText;

    [Tooltip("Animation curve for fading the music volume.")]
    public AnimationCurve fadeCurve;

    // Static property to control mute state
    public static bool IsMute { get; set; } = false;

    /// <summary>
    /// Called after the singleton instance is initialized.
    /// </summary>
    public override void PostAwake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        InitializeMixerVolumes();
    }

    /// <summary>
    /// Starts playing background music with fading in the volume.
    /// </summary>
    public void PlayBGM()
    {
        if (!_forcePlayNew && audioSource.isPlaying) 
            return;
        
        StopMusicCoroutines();
        audioSource.Stop();
        _fadeCoroutine = StartCoroutine(FadeInto(0.5f));
        _forcePlayNew = false;
    }

    /// <summary>
    /// Forces a new BGM to play regardless of current playback state.
    /// </summary>
    public void ForceNewBGM()
    {
        _forcePlayNew = true;
        PlayBGM();
    }

    /// <summary>
    /// Queries a random BGM sound from the list.
    /// </summary>
    public Sound QueryRandomBGM() =>
        BGM.Sounds[Random.Range(0, BGM.Sounds.Length)];

    /// <summary>
    /// Sets the master volume in the audio mixer.
    /// </summary>
    public void SetMasterVolume(float volume) => 
        SetMixerVolume("Master", volume, "MasterVolume");

    /// <summary>
    /// Sets the BGM volume in the audio mixer.
    /// </summary>
    public void SetBGMVolume(float volume) => 
        SetMixerVolume("BGM", volume, "BGMVolume");

    /// <summary>
    /// Sets the SFX volume in the audio mixer.
    /// </summary>
    public void SetSFXVolume(float volume) => 
        SetMixerVolume("SFX", volume, "SFXVolume");

    //  ------------------ Protected ------------------
    
    //  ------------------ Private ------------------
    
    private Coroutine _fadeCoroutine;
    private Coroutine _nextSongCoroutine;
    private bool _forcePlayNew = false;
    private string _currentSongName = string.Empty;
    private float _lastPlayPosition = 0f;
    private bool _wasPlayingBeforePause = false;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeMixerVolumes();
        _forcePlayNew = true;
        PlayBGM();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            // Store state when losing focus
            if (audioSource.clip != null)
            {
                _wasPlayingBeforePause = audioSource.isPlaying;
                _lastPlayPosition = audioSource.time;
            }
        }
        else
        {
            // Restore state when gaining focus
            RestoreMusicState();
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // Store state when pausing
            if (audioSource.clip != null)
            {
                _wasPlayingBeforePause = audioSource.isPlaying;
                _lastPlayPosition = audioSource.time;
            }
        }
        else
        {
            // Restore state when unpausing
            RestoreMusicState();
        }
    }

    private void RestoreMusicState()
    {
        if (audioSource.clip == null || !_wasPlayingBeforePause) 
            return;
        
        // Resume playback at the stored position
        if (!audioSource.isPlaying)
        {
            audioSource.time = _lastPlayPosition;
            audioSource.Play();
        }
    }

    private void StopMusicCoroutines()
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
        }
        
        if (_nextSongCoroutine != null)
        {
            StopCoroutine(_nextSongCoroutine);
            _nextSongCoroutine = null;
        }
    }

    /// <summary>
    /// Sets the volume for a specific audio mixer parameter and saves it to PlayerPrefs.
    /// </summary>
    private void SetMixerVolume(string mixerParam, float volume, string prefsKey)
    {
        float clamped = Mathf.Clamp01(volume);
        float db = Mathf.Log10(Mathf.Max(clamped, 0.00001f)) * 20f;

        audioMixer.SetFloat(mixerParam, db);
        PlayerPrefs.SetFloat(prefsKey, clamped);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Initializes the mixer volumes from saved player preferences.
    /// </summary>
    private void InitializeMixerVolumes()
    {
        SetMixerVolume("Master", PlayerPrefs.GetFloat("MasterVolume", 1.0f), "MasterVolume");
        SetMixerVolume("BGM", PlayerPrefs.GetFloat("BGMVolume", 1.0f), "BGMVolume");
        SetMixerVolume("SFX", PlayerPrefs.GetFloat("SFXVolume", 1.0f), "SFXVolume");
    }

    /// <summary>
    /// Fades the audio volume into the target volume over a specified time.
    /// </summary>
    private IEnumerator FadeInto(float fadeTime)
    {
        Sound sound = QueryRandomBGM();
        
        // Store current song information
        _currentSongName = sound.soundName;

        // Apply sound settings
        audioSource.clip = sound.clip;
        audioSource.mute = IsMute;
        audioSource.outputAudioMixerGroup = sound.audioMixerGroup;
        audioSource.volume = 0f;
        audioSource.pitch = sound.pitch;

        // Update UI with current song
        if (MusicText != null)
            MusicText.SetText($"Now Playing: {sound}");

        audioSource.Play();

        float targetVolume = sound.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / fadeTime;
            audioSource.volume = Mathf.Clamp(fadeCurve.Evaluate(normalizedTime) * targetVolume, 0f, targetVolume);
            yield return null;
        }

        audioSource.volume = targetVolume;
        _wasPlayingBeforePause = true;

        // Queue next song
        _nextSongCoroutine = StartCoroutine(WaitForNextSong());
    }

    /// <summary>
    /// Waits for the current song to finish before starting the next one.
    /// </summary>
    private IEnumerator WaitForNextSong()
    {
        if (audioSource.clip == null) 
            yield break;
        
        float clipLength = audioSource.clip.length;
        bool songFinished = false;
        
        while (!songFinished && audioSource.clip != null)
        {
            // Only check if we're actually playing
            if (audioSource.isPlaying)
            {
                // Consider the song finished when it's very close to the end
                songFinished = audioSource.time >= clipLength - 0.1f;
            }
            
            yield return null;
        }
        
        // Only play a new song if the current one finished properly
        if (songFinished)
        {
            _wasPlayingBeforePause = false; // Reset state before playing new song
            _forcePlayNew = true;
            PlayBGM();
        }
    }
}