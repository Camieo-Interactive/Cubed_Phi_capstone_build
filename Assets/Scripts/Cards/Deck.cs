using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the deck of cards, including generation, folding, and removal.
/// </summary>
public class Deck : MonoBehaviour
{
    //  ------------------ Public ------------------

    [Header("Deck Configuration")]
    [Tooltip("Prefab used for card generation.")]
    public GameObject cardPrefab;

    [Tooltip("Number of cards in the deck.")]
    public int numberOfCards = 7;

    [Tooltip("Array holding the instantiated card objects.")]
    public GameObject[] cards;

    [Tooltip("Animator controlling the deck animations.")]
    public Animator animator;

    public TextMeshProUGUI rerollText;
    public AudioSource AudioSrc;

    public int numberOfDecks = 0;

    public CardSet starterDeck;
    /// <summary>
    /// Folds the deck, triggering the fold animation.
    /// </summary>
    public void FoldDeck()
    {
        // Get the manager.. 
        GameManager manager = GameManager.Instance;
        // If We can't fold
        if (manager.BitsCollected < _currentFoldAmount) return;
        // We already fold
        if (_isFolded) return;
        // Change the bits
        GameManager.RaiseBitChange(-_currentFoldAmount);
        AudioSrc.Play();
        // Increase the fold amount
        _currentFoldAmount = Math.Min(_currentFoldAmount + manager.reRollDelta, manager.reRollMax);
        rerollText.text = $"Reroll: {_currentFoldAmount} Bits";
        _isFolded = true;
        // Unflip the cards
        foreach (GameObject item in cards)
        {
            UICard card = item.GetComponent<UICard>();
            card.UnFlipCard();
        }
        if (_isFolded) animator.Play("DeckFold");
    }

    /// <summary>
    /// Removes all cards from the deck and returns them to the pool.
    /// </summary>
    public void RemoveDeck()
    {
        foreach (GameObject card in cards) PoolManager.Instance.ReturnObject(card);
    }

    /// <summary>
    /// Generates a new deck with the specified number of cards.
    /// </summary>
    public void GenerateDeck()
    {

        if (numberOfDecks == 0 && starterDeck != null)
        {
            CardStats[] collection = starterDeck.cards;
            cards = new GameObject[collection.Length];
            for (int i = 0; i < collection.Length; i++) CreateCard(i, collection[i]);
        }
        else
        {
            cards = new GameObject[numberOfCards];
            for (int i = 0; i < numberOfCards; i++) CreateCard(i);
        }
        numberOfDecks++;
        _isFolded = false;
    }

    public void FilpDeck()
    {
        bool shouldAutoOpen = PlayerPrefs.GetInt("AutoOpenDeck", 1) == 1;
        Debug.Log($"Should we flip the deck? {shouldAutoOpen}");
        if (!shouldAutoOpen) return;
        Debug.Log("Flip the deck!");
        foreach (GameObject item in cards)
        {
            UICard card = item.GetComponent<UICard>();
            card.FlipCard();
        }
    }

    /// <summary>
    /// Removes a specific card from the deck and returns it to the pool.
    /// </summary>
    /// <param name="cardToRemove">The card GameObject to remove.</param>
    public void RemoveCardFromDeck(GameObject cardToRemove)
    {
        if (cardToRemove == null || cards == null) return;

        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] != cardToRemove) continue;
            cards[i].GetComponent<Card>().CardUsed();
            UICard iCard = cards[i].GetComponent<UICard>();
            iCard.UnFlipCard();
            break;
        }
        cards = Array.FindAll(cards, card => card != null);
    }

    //  ------------------ Private ------------------
    private bool _isFolded = false;
    private int _currentFoldAmount = 0;
    private void Start() => rerollText.text =  $"Reroll: {_currentFoldAmount} Bits";
    private void CreateCard(int index, CardStats stats = null)
    {
        cards[index] = PoolManager.Instance.GetObject(cardPrefab, Vector3.zero, Quaternion.identity, transform);
        Card card = cards[index].GetComponent<Card>();
        if (stats == null) card.Init();
        else card.Init(stats);
    }
}