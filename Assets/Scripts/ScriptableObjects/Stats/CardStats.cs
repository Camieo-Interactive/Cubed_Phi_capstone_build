using System;
using UnityEngine;

/// <summary>
/// Defines the core statistics for a card.
/// </summary>
/// <remarks>
/// Used as a ScriptableObject to store and manage card attributes such as name, sprite, associated object, and cost.
/// </remarks>
[CreateAssetMenu(fileName = "New Card Stats", menuName = "Card/Card Stats")]
[Serializable]
public class CardStats : ScriptableObject
{
    //  ------------------ Public ------------------

    [Header("Card Attributes")]

    [Tooltip("The name of the card.")]
    public string cardName;

    [Tooltip("The sprite representing the card.")]
    public Sprite cardSprite;

    [Tooltip("The GameObject representation of the card.")]
    public GameObject cardObject;

    [Tooltip("The resource cost to play the card.")]
    public int cardCost;
    [Header("Card Probability")]

    [Tooltip("The relative weight for drawing this card. Higher values increase chances relative to other cards.")]
    [Range(0f, 100f)] // A wider range might be more appropriate
    public float drawProbability;

    [Tooltip("Tier which the card is avaliable for use. 0 Being avaliable at the start of game, and higher being later.")]
    public int cardTier;
}
