using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a card in the game, displaying its stats and visuals.
/// </summary>
public class Card : MonoBehaviour
{
    //  ------------------ Public ------------------
    
    [Header("Card Data")]
    [Tooltip("The stats associated with this card.")]
    public CardStats stats;

    [Header("UI Elements")]
    [Tooltip("The image component displaying the card sprite.")]
    public Image cardImage;

    [Tooltip("The text displaying the card name.")]
    public TextMeshProUGUI cardNameText;

    [Tooltip("The text displaying the card cost.")]
    public TextMeshProUGUI cardCostText;

    /// <summary>
    /// Initializes the card with a random set of stats from the current card list.
    /// </summary>
    public void Init()
    {
        stats = GameManager.Instance.currentCardList.PickRandomCard();
        cardImage.sprite = stats.cardSprite;
        cardNameText.text = stats.cardName;
        cardCostText.text = $"Cost: {stats.cardCost}";
    }
}
