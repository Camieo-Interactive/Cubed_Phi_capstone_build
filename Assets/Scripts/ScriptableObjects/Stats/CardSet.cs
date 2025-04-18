using System;
using UnityEngine;

/// <summary>
/// A scriptable object that holds a collection of card statistics.
/// </summary>
[CreateAssetMenu(fileName = "New Card Set", menuName = "Card/Card Set")]
[Serializable]
public class CardSet : ScriptableObject 
{
    //  ------------------ Public ------------------

    [Tooltip("Determines if the card set should loop through the cards.")]
    public bool loopThrough;

    [Tooltip("A array of CardStats, representing sets of cards in the collection.")]
    public CardStats[] cards;
}

