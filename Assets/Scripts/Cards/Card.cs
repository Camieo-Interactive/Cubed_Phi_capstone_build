using TMPro;
using Unity.Collections;
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
    [ReadOnly] public CardStats stats;

    [Header("UI Elements")]
    [Tooltip("The image component displaying the card sprite.")]
    public Image cardImage;

    [Tooltip("The text displaying the card name.")]
    public TextMeshProUGUI cardNameText;

    [Tooltip("The text displaying the card cost.")]
    public TextMeshProUGUI cardCostText;
    public Animator cardAnimator;

    public bool cardUsed = false;
    public Button cardButton;
    [Tooltip("Audio Source for the click sound effect.")]
    public AudioSource ClickAudioSource;

    /// <summary>
    /// Initializes the card with a random set of stats and assigns UI elements.
    /// </summary>
    public void Init() => Init(GameManager.Instance.currentCardList.PickRandomCard());

    public void Init(CardStats newStats)
    {
        stats = newStats;
        SetCardProperties();
        UICard iCard = GetComponent<UICard>();
        iCard.button.interactable = true;
        iCard.image.sprite = iCard.interactable;
        if (cardUsed)
        {
            cardUsed = false;
            cardButton.interactable = true;
        }
    }

    public void CardUsed()
    {
        cardButton.interactable = false;
        cardAnimator.Play("Normal");
        cardUsed = true;
    }

    public void SetCardProperties()
    {
        cardImage.sprite = stats.cardSprite;
        cardNameText.text = stats.cardName;
        cardCostText.text = $"Cost: {stats.cardCost}";
    }

    /// <summary>
    /// Selects this card.
    /// </summary>
    public void CardPressed()
    {
        if (!cardUsed) {
            ClickAudioSource.Play();
            GameManager.Instance.selectedCard = this;
        }
    }

    /// <summary>
    /// Deselects this card.
    /// </summary>
    public void CardDeselected() => GameManager.Instance.selectedCard = null;
}
