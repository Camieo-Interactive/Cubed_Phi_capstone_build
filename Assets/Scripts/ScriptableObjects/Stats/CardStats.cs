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

    [Tooltip("The probability of drawing or spawning this card (0.0 to 1.0). Lower the rarer.")]
    [Range(0f, 1f)]
    public float drawProbability;
}
