using UnityEngine;

/// <summary>
/// Serializable representation of a screen resolution.
/// </summary>
[System.Serializable]
public struct ResolutionOption
{
    [Tooltip("Screen width in pixels.")]
    public int width;

    [Tooltip("Screen height in pixels.")]
    public int height;

    [Tooltip("Refresh rate in Hz.")]
    public int refreshRate;
}
