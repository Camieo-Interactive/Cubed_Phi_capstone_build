using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// A collection of cards with methods to randomly select one based on probability.
/// </summary>
[CreateAssetMenu(fileName = "New Card List", menuName = "Card/Card List")]
[Serializable]
public class CardList : ScriptableObject
{
    //  ------------------ Public ------------------

    [Header("Card Collection")]

    [Tooltip("Array of available cards.")]
    public CardStats[] cards;

    /// <summary>
    /// Picks a random card from the list based on probability weights.
    /// </summary>
    /// <returns>A randomly selected CardStats object.</returns>
    public CardStats PickRandomCard()
    {
        float totalWeight = cards.Sum(card => card.drawProbability);
        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        foreach (var card in cards)
        {
            currentWeight += card.drawProbability;
            if (randomValue <= currentWeight) return card;
        }

        return cards.Last();
    }
}
