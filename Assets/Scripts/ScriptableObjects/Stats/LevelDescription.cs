using UnityEngine;

/// <summary>
/// Represents the description and associated statistics for a level.
/// </summary>
[CreateAssetMenu(fileName = "New Level Description", menuName = "Levels/Level Description")]
public class LevelDescription : ScriptableObject
{
    [Header("Basic Information")]
    [Tooltip("The name of the level.")]
    public string levelName;

    [Tooltip("A brief description of the level.")]
    [TextArea]
    public string Description;

    [Tooltip("The name of the Scene")]
    public string SceneName;

    [Header("Cycle Statistics")]
    [Tooltip("The cycle statistics associated with the level.")]
    public CycleStats cycleStats;

    [Header("Cards and Enemies")]
    [Tooltip("The card set associated with the level.")]
    public CardSet cardSet;

    [Tooltip("The card list associated with the level.")]
    public CardList cardList;

    [Tooltip("The enemy waves for the level.")]
    public EnemyWave[] enemyWaves;

    [Tooltip("The enemy prefabs used in the level.")]
    public GameObject[] enemyPrefabs;
}