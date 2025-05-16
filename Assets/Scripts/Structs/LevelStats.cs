using System;

[Serializable]
/// <summary>
/// A struct to hold statistics about a level in the game.
/// </summary>
/// <remarks>
/// This struct contains information about the number of bits collected,
/// the number of towers created, and the number of enemies killed during the level.
/// /// </remarks>
public struct LevelStats {

    /// <summary>
    /// The number of bits collected by the player during the level.
    /// </summary>
    public int NumberOfBitsCollected;

    /// <summary>
    /// The number of towers created in the level.
    /// </summary>
    public int NumberOfTowersCreated;

    /// <summary>
    /// The number of enemies killed by the player during the level.
    /// </summary>
    public int numberOfEnemiesKilled;
}
