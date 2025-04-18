using UnityEngine;

/// <summary>
/// ScriptableObject that holds a list of predefined resolution options.
/// </summary>
[CreateAssetMenu(fileName = "ResolutionPresets", menuName = "Settings/Resolution Preset Asset")]
public class ResolutionPresetAsset : ScriptableObject
{
    [Tooltip("List of available screen resolutions.")]
    public ResolutionOption[] resolutions;
}
