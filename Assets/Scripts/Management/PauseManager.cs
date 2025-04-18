using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages game pause behavior and UI animations.
/// </summary>
/// <remarks>
/// Handles time scale, animator transitions, and button input for pausing and resuming the game.
/// </remarks>
public class PauseManager : MonoBehaviour
{
    //  ------------------ Public ------------------

    [Header("Pause UI")]
    [Tooltip("Animator controlling the pause panel UI.")]
    public Animator pauseAnimator;

    [Tooltip("CanvasGroup or root UI element for pause menu (optional).")]
    public GameObject pausePanel;

    public SettingsManager settingsManager;
    public FastFoward fastFoward;

    //  ------------------ Protected ------------------

    //  ------------------ Private ------------------

    private bool _isPaused = false;
    private bool _isAnimating = false;

    /// <summary>
    /// Called by the in-game pause button.
    /// Triggers pause and UI animation if not already paused.
    /// </summary>
    public void PauseButton()
    {
        if (_isPaused || _isAnimating)
            return;
        
        StartCoroutine(PlayPauseAnimation("PauseEnter", paused: true));
    }

    /// <summary>
    /// Called by the resume button inside the pause menu.
    /// Unpauses and plays exit animation.
    /// </summary>
    public void ResumeButton()
    {
        if (!_isPaused || _isAnimating)
            return;
        if (settingsManager.isSettingsVisible) settingsManager.SettingsToggle();
        StartCoroutine(PlayPauseAnimation("PauseExit", paused: false));
    }

    /// <summary>
    /// Handles the animation flow and time scale toggling.
    /// </summary>
    /// <param name="stateName">The animation state to play directly.</param>
    /// <param name="paused">Whether we're entering or exiting pause.</param>
    private IEnumerator PlayPauseAnimation(string stateName, bool paused)
    {
        _isAnimating = true;

        pausePanel?.SetActive(true);
        pauseAnimator.Play(stateName, 0, 0f);

        float duration = GetAnimationClipLength(stateName);
        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = paused ? 0f : fastFoward.isFastFoward ? 2f : 1f;
        _isPaused = paused;
        _isAnimating = false;

        if (!paused) pausePanel?.SetActive(false);
    }

    /// <summary>
    /// Returns the duration of the animation clip that matches the state name.
    /// </summary>
    /// <param name="stateName">The animation state name.</param>
    /// <returns>Length in seconds (real time).</returns>
    private float GetAnimationClipLength(string stateName)
    {
        if (pauseAnimator.runtimeAnimatorController == null)
            return 0.5f;

        foreach (var clip in pauseAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == stateName)
                return clip.length;
        }

        return 0.5f;
    }
}
