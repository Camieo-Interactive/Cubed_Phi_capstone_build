using UnityEngine;

/// <summary>
/// Interface for buildable objects in the game.
/// </summary>
public interface IBuildable
{
    /// <summary>
    /// Called when the buildable object is constructed.
    /// </summary>
    void OnBuild();

    /// <summary>
    /// Called when the buildable object is destroyed.
    /// </summary>
    void OnDestroy();
}
