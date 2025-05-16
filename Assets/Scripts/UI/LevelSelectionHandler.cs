using UnityEngine;

/// <summary>
/// Handles the level selection animations by controlling an Animator component.
/// </summary>
public class LevelSelectionHandler : MonoBehaviour
{
    /// <summary>
    /// The Animator component used to play level selection animations.
    /// </summary>
    [Header("Animator Settings")]
    [Tooltip("The Animator component used to play level selection animations.")]
    public Animation animator;

    /// <summary>
    /// Triggers the "LevelSelectionEnter" animation to indicate entering the level selection screen.
    /// </summary>
    public void LevelSelectionEnter() => animator.Play("LevelSelectionEnter");

    /// <summary>
    /// Triggers the "LevelSelectionQuit" animation to indicate exiting the level selection screen.
    /// </summary>
    public void LevelSelectionExit() => animator.Play("LevelSelectionExit");
}
