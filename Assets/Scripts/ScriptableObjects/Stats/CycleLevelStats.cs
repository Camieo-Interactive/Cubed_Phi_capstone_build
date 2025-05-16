using UnityEngine;

/// <summary>
/// Represents the statistics and descriptions for a cycle of levels in the game.
/// </summary>
[CreateAssetMenu(fileName = "New Cycle Level Stats", menuName = "Levels/Cycle Level Stats")]
public class CycleLevelStats : ScriptableObject
{
    /// <summary>
    /// The name of the cycle associated with these stats.
    /// </summary>
    public string cycleName;
    /// <summary>
    /// The statistics associated with the cycle.
    /// </summary>
    public CycleStats cycle;

    /// <summary>
    /// An array of level descriptions that belong to this cycle.
    /// </summary>
    public LevelDescription[] levels;
}