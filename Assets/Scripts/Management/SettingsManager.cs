using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// Manages player settings including audio volume, screen resolution, and fullscreen toggle.
/// </summary>
/// <remarks>
/// Loads resolution options from a ScriptableObject asset.
/// </remarks>
public class SettingsManager : MonoBehaviour
{
    //  ------------------ Public ------------------

    [Header("Audio Sliders")]
    [Tooltip("Slider to control master volume.")]
    public Slider MasterVolumeSlider;

    [Tooltip("Slider to control BGM volume.")]
    public Slider BGMVolumeSlider;

    [Tooltip("Slider to control SFX volume.")]
    public Slider SFXVolumeSlider;

    [Header("Display Settings")]
    [Tooltip("Dropdown to select screen resolution.")]
    public TMP_Dropdown ResolutionDropdown;

    [Tooltip("Toggle to switch fullscreen mode (PC only).")]
    public Toggle FullscreenToggle;

    [Header("Gameplay Settings")]
    [Tooltip("Toggle to automatically open the deck at game start.")]
    public Toggle AutoOpenDeckToggle;

    [Header("Resolution Presets")]
    [Tooltip("Reference to the resolution preset asset.")]
    public ResolutionPresetAsset PresetAsset;
    [Tooltip("Reference to the resolution text asset.")]
    public TextMeshProUGUI ResolutionText;

    [Header("Settings Panel Animation")]
    [Tooltip("Animator controlling the settings panel.")]
    public Animator settingsAnimator;
    public MainMenu mainMenu;
    public bool isSettingsVisible = false;


    //  ------------------ Private ------------------

    private List<string> _resolutionOptions = new();

    private bool _isAnimating = false;


    private void Start()
    {
        ApplyStartupSettings();
        InitializeAudioSliders();
        InitializeResolutionSettings();
        InitializeFullscreenToggle();
        InitializeAutoOpenDeckToggle();
    }

    /// <summary>
    /// Initializes slider values and binds audio volume listeners.
    /// </summary>
    private void InitializeAudioSliders()
    {
        float master = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        float bgm = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        MasterVolumeSlider.SetValueWithoutNotify(master);
        BGMVolumeSlider.SetValueWithoutNotify(bgm);
        SFXVolumeSlider.SetValueWithoutNotify(sfx);

        MasterVolumeSlider.onValueChanged.AddListener(volume =>
        {
            BGMManager.Instance.SetMasterVolume(volume);
            PlayerPrefs.SetFloat("MasterVolume", volume);
        });

        BGMVolumeSlider.onValueChanged.AddListener(volume =>
        {
            BGMManager.Instance.SetBGMVolume(volume);
            PlayerPrefs.SetFloat("BGMVolume", volume);
        });

        SFXVolumeSlider.onValueChanged.AddListener(volume =>
        {
            BGMManager.Instance.SetSFXVolume(volume);
            PlayerPrefs.SetFloat("SFXVolume", volume);
        });
    }

    private void InitializeAutoOpenDeckToggle()
    {
        bool autoOpenDeck = PlayerPrefs.GetInt("AutoOpenDeck", 1) == 1;
        AutoOpenDeckToggle.SetIsOnWithoutNotify(autoOpenDeck);

        AutoOpenDeckToggle.onValueChanged.AddListener(state =>
        {
            PlayerPrefs.SetInt("AutoOpenDeck", state ? 1 : 0);
            PlayerPrefs.Save();
        });
    }

    /// <summary>
    /// Populates the resolution dropdown using preset values.
    /// </summary>
    private void InitializeResolutionSettings()
    {
#if UNITY_STANDALONE
        if (PresetAsset == null || PresetAsset.resolutions.Length == 0)
        {
            ResolutionDropdown.gameObject.SetActive(false);
            return;
        }

        ResolutionDropdown.ClearOptions();
        _resolutionOptions.Clear();

        // Default to highest resolution if no saved value
        int defaultIndex = 0;
        int maxPixels = 0;

        for (int i = 0; i < PresetAsset.resolutions.Length; i++)
        {
            ResolutionOption res = PresetAsset.resolutions[i];
            _resolutionOptions.Add($"{res.width} x {res.height}");

            int pixels = res.width * res.height;
            if (pixels > maxPixels)
            {
                maxPixels = pixels;
                defaultIndex = i;
            }
        }

        // Load saved index or fallback to highest res
        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", defaultIndex);
        savedIndex = Mathf.Clamp(savedIndex, 0, PresetAsset.resolutions.Length - 1);

        ResolutionDropdown.AddOptions(_resolutionOptions);
        ResolutionDropdown.SetValueWithoutNotify(savedIndex);
        ResolutionDropdown.onValueChanged.AddListener(index =>
        {
            SetResolution(index);
            PlayerPrefs.SetInt("ResolutionIndex", index);
        });

        // Apply resolution immediately
        SetResolution(savedIndex);
#else
        ResolutionText.gameObject.SetActive(false);
        ResolutionDropdown.gameObject.SetActive(false);
#endif
    }


    /// <summary>
    /// Initializes fullscreen toggle if supported on platform.
    /// </summary>
    private void InitializeFullscreenToggle()
    {
#if UNITY_STANDALONE || UNITY_WEBGL
#if UNITY_STANDALONE 
        bool isFullscreen = PlayerPrefs.GetInt("IsFullscreen", 1) == 1;
#endif
#if UNITY_WEBGL 
        bool isFullscreen = PlayerPrefs.GetInt("IsFullscreen", 0) == 1;
#endif
        FullscreenToggle.SetIsOnWithoutNotify(isFullscreen);
        Screen.fullScreen = isFullscreen;

        FullscreenToggle.onValueChanged.AddListener(state =>
        {
            SetFullscreen(state);
            PlayerPrefs.SetInt("IsFullscreen", state ? 1 : 0);
        });
#else
    FullscreenToggle.gameObject.SetActive(false);
#endif
    }


    /// <summary>
    /// Applies default or saved settings on scene start.
    /// </summary>
    private void ApplyStartupSettings()
    {
#if UNITY_STANDALONE
        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        savedIndex = Mathf.Clamp(savedIndex, 0, PresetAsset.resolutions.Length - 1);
        SetResolution(savedIndex);
#endif

#if UNITY_STANDALONE
        bool fullscreen = PlayerPrefs.GetInt("IsFullscreen", 1) == 1;
        SetFullscreen(fullscreen);
#endif

        PlayerPrefs.Save();
    }


    /// <summary>
    /// Sets screen resolution from dropdown selection.
    /// </summary>
    /// <param name="index">Index of the selected resolution.</param>
    private void SetResolution(int index)
    {
        if (PresetAsset == null || index < 0 || index >= PresetAsset.resolutions.Length)
            return;

        ResolutionOption selected = PresetAsset.resolutions[index];
        var refreshRate = new RefreshRate { numerator = (uint)selected.refreshRate, denominator = 1 };
        Screen.SetResolution(selected.width, selected.height, Screen.fullScreenMode, refreshRate);
    }

    /// <summary>
    /// Toggles fullscreen mode on supported platforms.
    /// </summary>
    /// <param name="isFullscreen">Whether fullscreen should be enabled.</param>
    private void SetFullscreen(bool isFullscreen) => Screen.fullScreen = isFullscreen;


    /// <summary>
    /// Toggles the settings panel open/closed using animation.
    /// </summary>
    public void SettingsToggle()
    {
        if (_isAnimating || settingsAnimator == null) return;

        StartCoroutine(HandleSettingsToggle());

        if (mainMenu == null) return;
        if (mainMenu.instructionsCanvas.activeSelf)
            mainMenu.instructionsCanvas.SetActive(!mainMenu.instructionsCanvas.activeSelf);
    }

    /// <summary>
    /// Coroutine to manage settings panel animation flow.
    /// </summary>
    private IEnumerator HandleSettingsToggle()
    {
        _isAnimating = true;

        string triggerName = isSettingsVisible ? "SettingsExit" : "SettingsEnter";
        settingsAnimator.Play(triggerName);

        // Optional: Adjust this to match the actual animation duration
        float animationLength = GetAnimationClipLength(triggerName);
        yield return new WaitForSecondsRealtime(animationLength);

        isSettingsVisible = !isSettingsVisible;
        _isAnimating = false;
    }

    /// <summary>
    /// Gets the length of the animation clip for the given trigger.
    /// </summary>
    /// <param name="triggerName">Trigger name that matches the animation state.</param>
    private float GetAnimationClipLength(string triggerName)
    {
        if (settingsAnimator.runtimeAnimatorController == null) return 0.5f;

        foreach (var clip in settingsAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == triggerName) return clip.length;
        }

        return 0.5f; // Default fallback duration
    }
}
