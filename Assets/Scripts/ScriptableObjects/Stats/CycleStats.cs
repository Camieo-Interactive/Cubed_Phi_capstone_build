using TMPro;
using UnityEngine;

/// <summary>
/// Represents the statistics and visual elements associated with a cycle.
/// </summary>
[CreateAssetMenu(fileName = "New Cycle Stats", menuName = "Levels/Cycle Stats")]
public class CycleStats : ScriptableObject
{
    /// <summary>
    /// The color associated with the cycle.
    /// </summary>
    [Tooltip("The color associated with the cycle.")]
    public Color cycleColor;

    /// <summary>
    /// The text color used for the cycle.
    /// </summary>
    [Tooltip("The text color used for the cycle.")]
    public Color cycleTextColor;

    /// <summary>
    /// The sprite used for the back of the cycle card.
    /// </summary>
    [Tooltip("The sprite used for the back of the cycle card.")]
    public Sprite cycleCardBack;

    /// <summary>
    /// The sprite used for the front of the cycle card.
    /// </summary>
    [Tooltip("The sprite used for the front of the cycle card.")]
    public Sprite cycleCardFront;

    /// <summary>
    /// The font asset used for the cycle.
    /// </summary>
    [Tooltip("The font asset used for the cycle.")]
    public TMP_FontAsset cycleFont;
}
