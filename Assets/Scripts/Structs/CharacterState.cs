using UnityEngine;

/// <summary>
/// Represents the emotional state of a character with various attributes.
/// </summary>
[System.Serializable] // Makes the struct visible and editable in the Unity Inspector.
public struct CharacterState
{
    /// <summary>
    /// The level of affection the character feels.
    /// </summary>
    [Tooltip("The level of affection the character feels.")]
    public int Affection;

    /// <summary>
    /// The level of hatred the character feels.
    /// </summary>
    [Tooltip("The level of hatred the character feels.")]
    public int Hatred;

    /// <summary>
    /// The level of trust the character has.
    /// </summary>
    [Tooltip("The level of trust the character has.")]
    public int Trust;

    /// <summary>
    /// The level of fear the character experiences.
    /// </summary>
    [Tooltip("The level of fear the character experiences.")]
    public int Fear;

    /// <summary>
    /// The level of annoyance the character feels.
    /// </summary>
    [Tooltip("The level of annoyance the character feels.")]
    public int Annoyance;

    /// <summary>
    /// If the character has was unlocked for the player.
    /// </summary>
    [Tooltip("If the character has was unlocked for the player.")]
    public int Unlocked;

    /// <summary>
    /// If the character has blocked the player.
    /// </summary>
    [Tooltip("If the character has blocked the player.")]
    public int Blocked;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="conditional"></param>
    /// <param name="value"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void ChangeValue(KILMConditionals conditional, int value)
    {
        switch (conditional)
        {
            case KILMConditionals.Affection:
                Affection += value;
                break;
            case KILMConditionals.Trust:
                Trust += value;
                break;
            case KILMConditionals.Annoyance:
                Annoyance += value;
                break;
            case KILMConditionals.Hatred:
                Hatred += value;
                break;
            case KILMConditionals.Fear:
                Fear += value;
                break;
            case KILMConditionals.Unlocked:
                Unlocked += value;
                break;
            case KILMConditionals.Blocked:
                Blocked += value;
                break;
            default:
                throw new System.NotImplementedException($"Unsupported conditional: {conditional}");
        }
    }

    public void UpdateStatus() {
        // TODO: Update the status of the character
    }
}